Imports Microsoft.Win32
Imports Skischule.Entities
Imports System.Reflection
Imports System.ComponentModel
Imports System.IO
Imports System.IO.Compression
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports System.IO.IsolatedStorage
Imports System.Windows.Media.Animation
Imports Microsoft.Office.Core

Class MainWindow

#Region "Fields"
    Private _dummySpalteFuerLayer0 As ColumnDefinition
    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _skikursListFile As FileInfo
    Private _mRUSortedList As SortedList(Of Integer, String)
    Private _skischule As Entities.Skischule

#End Region

#Region "Constructor"

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ' Spalte initialisieren und in dieselbe Gruppe setzen,
        ' wie die Spalte mit dem Freunde Explorer im
        ' layer1-Grid
        _dummySpalteFuerLayer0 = New ColumnDefinition()
        _dummySpalteFuerLayer0.SharedSizeGroup = "pinSpalte"

        ' das Grid gleich zu Beginn pinnen
        layerTeilnehmerliste.Visibility = Visibility.Visible
        btnPinIt.IsChecked = True

        _teilnehmerListCollectionView = New ListCollectionView(New TeilnehmerCollection())
        AddHandler _teilnehmerListCollectionView.CurrentChanged, New EventHandler(AddressOf _teilnehmerListCollectionView_CurrentChanged)

    End Sub

#End Region

#Region "Window-Events"

    Private Sub HandleMainWindowLoaded(sender As Object, e As RoutedEventArgs)

        ' 0. Zur InputBindings ein MouseBinding hinzufügen. Nur als Beispiel,
        '    um mit Strg und Doppelklick eine neue Liste anlegen zu können
        Dim mg = New MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control)
        Dim m = New MouseBinding(ApplicationCommands.[New], mg)
        InputBindings.Add(m)


        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden
        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleListNewExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleListOpenExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleListSaveExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleListSaveAsExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Help, AddressOf HandleHelpExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))

        CommandBindings.Add(New CommandBinding(SkischuleBefehle.ImportTeilnehmerliste, AddressOf HandleImportExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.BeurteileTeilnehmerkoennen, AddressOf HandleBeurteileTeilnehmerkoennenExecuted, AddressOf HandleBeurteileTeilnehmerkoennenCanExecute))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.NeuerTeilnehmer, AddressOf HandleNeuerTeilnehmerExecuted, AddressOf HandleNeuerTeilnehmerCanExecuted))

        ' 2. SortedList für meist genutzte Freundeslisten (Most Recently Used) initialisieren
        _mRUSortedList = New SortedList(Of Integer, String)

        ' 3. SortedList für meist genutzte Freundeslisten befüllen
        LoadmRUSortedListMenu()

        ' 4. Die zuletzt verwendete Freundesliste laden, falls nicht eine .ski-Datei doppelgeklickt wurde
        If (Environment.GetCommandLineArgs().Length = 2) Then
            Dim args = Environment.GetCommandLineArgs
            Dim filename = args(1)
            OpenSkischule(filename)
        Else
            LoadLastSkischule()
        End If
        ' 5. JumpList in Windows Taskbar aktualisieren
        RefreshJumpListInWinTaskbar()

    End Sub

    Private Sub HandleMainWindowClosing(sender As Object, e As CancelEventArgs)
        Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schliessen?", "Achtung", MessageBoxButton.YesNo)
        e.Cancel = result = MessageBoxResult.No
    End Sub

    Private Sub HandleMainwindowClosed(sender As Object, e As EventArgs)

        ' 1. Den Pfad der letzen Liste ins IsolatedStorage speichern.
        If _skikursListFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("LastSkischule", System.IO.FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(_skikursListFile.FullName)
                    End Using
                End Using
            End Using
        End If

        ' 2. Die meist genutzen Listen ins Isolated Storage speichern
        If _mRUSortedList.Count > 0 Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("mRUSortedList", System.IO.FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        For Each kvp As KeyValuePair(Of Integer, String) In _mRUSortedList
                            writer.WriteLine(kvp.Key & ";" & kvp.Value)
                        Next
                    End Using
                End Using
            End Using
        End If

    End Sub

#End Region

#Region "Methoden zum Laden der meist genutzten Listen und der letzten Skikurse"

    Private Sub LoadmRUSortedListMenu()
        Try
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("mRUSortedList", System.IO.FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        Dim i = 0
                        While reader.Peek <> -1
                            Dim line = reader.ReadLine().Split(";")
                            If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not _mRUSortedList.ContainsKey(Integer.Parse(line(0))) Then
                                If File.Exists(line(1)) Then
                                    i += 1
                                    _mRUSortedList.Add(i, line(1))
                                End If
                            End If
                        End While
                    End Using
                End Using
            End Using
            RefreshMostRecentMenu()
        Catch ex As FileNotFoundException
            'Throw ex
        End Try

    End Sub

    Private Sub LoadLastSkischule()
        ' Die letze Skischule aus dem IsolatedStorage holen.
        Try
            Dim x = ""
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastSkischule", FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        x = reader.ReadLine
                    End Using
                End Using

            End Using
            If File.Exists(x) Then Me.OpenSkischule(x)
        Catch ex As FileNotFoundException
        End Try
    End Sub

#End Region

#Region "Methoden zum Pinnen und Ein-/Ausblenden des Freunde-Explorers"

    Private Sub HandleListePinning(sender As Object, e As RoutedEventArgs)

        ' Pinnen

        ' 1. ColumnDefinition zum layer0-Grid hinzufügen
        layerTeilnehmerdetails.ColumnDefinitions.Add(_dummySpalteFuerLayer0)

        ' 2. Button "Freunde Explorer" ausblenden
        btnShowTeilnehmerExplorer.Visibility = Visibility.Collapsed

        ' 3. pinImage in layer1-Grid auf pinned setzen
        pinImage.Source = New BitmapImage(New Uri("Images\icons8-pin-48.png", UriKind.Relative))

    End Sub

    Private Sub HandleListeUnpinning(sender As Object, e As RoutedEventArgs)
        ' Unpinnen

        ' 1. ColumnDefinition von layer0-Grid entfernen
        layerTeilnehmerdetails.ColumnDefinitions.Remove(_dummySpalteFuerLayer0)

        ' 2. Button "Freunde Explorer" einblenden
        btnShowTeilnehmerExplorer.Visibility = Visibility.Visible

        ' 3. pinImage in layer1-Grid auf unpinned setzen
        pinImage.Source = New BitmapImage(New Uri("Images\icons8-unpin-2-48.png", UriKind.Relative))

    End Sub

    Private Sub HandleButtonExpMouseEnter(sender As Object, e As RoutedEventArgs)

        ' TeilnehmerDetails-Grid mit den Explorern einblenden
        If (layerTeilnehmerliste.Visibility <> Visibility.Visible) Then

            ' 1. Das layerDetails-Grid um die Breite der "Teilnehmer   
            ' Explorer"-Spalte nach rechts versetzen
            layerTeilnehmerlisteTrans.X = layerTeilnehmerliste.ColumnDefinitions(1).Width.Value

            ' 2. layer1-Grid sichtbar machen
            layerTeilnehmerliste.Visibility = Visibility.Visible

            ' 3. Die X-Property der layer1Trans vom aktuellen Wert
            ' hin zum Wert 0 animieren, Dauer 500 Millisek
            Dim ani = New DoubleAnimation(0, New Duration(TimeSpan.FromMilliseconds(500)))
            layerTeilnehmerlisteTrans.BeginAnimation(TranslateTransform.XProperty, ani)

        End If
    End Sub

    Private Sub HandleLayerdetailsMouseEnter(sender As Object, e As RoutedEventArgs)

        ' layer1-Grid ausblenden
        If (Not btnPinIt.IsChecked.GetValueOrDefault() AndAlso layerTeilnehmerliste.Visibility = Visibility.Visible) Then

            ' 1. Zielwert für die Animation setzen
            Dim [to] = layerTeilnehmerliste.ColumnDefinitions(1).Width.Value

            ' 2. layer1Trans.X zum ermittelten Zielwert animieren
            ' und EventHandler für Completed-Event installieren
            Dim ani = New DoubleAnimation([to], New Duration(TimeSpan.FromMilliseconds(500)))
            AddHandler ani.Completed, New EventHandler(AddressOf ani_Completed)
            layerTeilnehmerlisteTrans.BeginAnimation(TranslateTransform.XProperty, ani)

        End If

    End Sub

    Sub ani_Completed(sender As Object, e As EventArgs)
        ' 3. layer1-Grid ausblenden
        layerTeilnehmerliste.Visibility = Visibility.Collapsed
    End Sub

#End Region

#Region "EventHandler der CommandBindings"

    Private Sub HandleListNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If _skischule IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        _skischule = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Skischule"
        SetView(New Entities.Skischule)
        If MessageBoxResult.Yes = MessageBox.Show("Neuen Skikurs erstellt. Jetzt gleich einen Teilnehmer hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
            SkischuleBefehle.NeuerTeilnehmer.Execute(Nothing, Me)
        End If

    End Sub

    Private Sub HandleListOpenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        If dlg.ShowDialog = True Then
            OpenSkischule(dlg.FileName)
        End If
    End Sub

    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        OpenSkischule(TryCast(sender, MenuItem).Header.ToString())
    End Sub

    Private Sub HandleBeurteileTeilnehmerkoennenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        'Todo: Das Handle Beurteilung erstellen
        'For Each item In BasicObjects.Koennenstufenliste
        '    Debug.WriteLine(item.KoennenstufeID.ToString & "; " & item.Benennung & "; " & item.AngezeigteBenennung)
        'Next

    End Sub

    Private Sub HandleBeurteileTeilnehmerkoennenCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = teilnehmerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleImportExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If _skischule IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        _skischule = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Skischule"

        Dim importSkischule = DatenImport.ImportSkischule
        If importSkischule IsNot Nothing Then
            SetView(importSkischule)
            MessageBox.Show(String.Format("Daten aus {0} erfolgreich importiert", DatenImport.Workbook.Name))
        End If

    End Sub

    Private Sub HandleListSaveCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleListSaveAsExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If _skikursListFile IsNot Nothing Then
            dlg.FileName = _skikursListFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveSkischule(dlg.FileName)
            _skikursListFile = New FileInfo(dlg.FileName)
        End If
    End Sub

    Private Sub HandleListSaveExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        If _skikursListFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveSkischule(_skikursListFile.FullName)
        End If
    End Sub
    Private Sub HandleCloseExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
    End Sub

    Private Sub HandleNeuerTeilnehmerCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleNeuerTeilnehmerExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New NeuerTeilnehmerDialog
        dlg.Owner = Me
        dlg.WindowStartupLocation = WindowStartupLocation.CenterOwner

        If dlg.ShowDialog = True Then
            _skischule.Teilnehmerliste.Add(dlg.Teilnehmer)
            _teilnehmerListCollectionView.MoveCurrentTo(dlg.Teilnehmer)
            teilnehmerDataGrid.ScrollIntoView(dlg.Teilnehmer)
        End If

    End Sub

#End Region

#Region "Sonstige Eventhandler"

    Private Sub HandleLayerTeilnehmerMouseEnter(sender As Object, e As RoutedEventArgs)

    End Sub

    Sub _teilnehmerListCollectionView_CurrentChanged(sender As Object, e As EventArgs)
        RefreshTaskBarItemOverlay()
    End Sub

    Private Sub RefreshTaskBarItemOverlay()
        Dim currentTeilnehmer = DirectCast(_teilnehmerListCollectionView.CurrentItem, Teilnehmer)

        'Todo: Aufbereiten für die Skilehrer Bilder

        'If currentTeilnehmer IsNot Nothing AndAlso currentTeilnehmer.Image IsNot Nothing Then
        '    Dim bi As New BitmapImage
        '    bi.BeginInit()
        '    bi.StreamSource = New MemoryStream(currentFriend.Image)
        '    bi.EndInit()
        '    TaskbarItemInfo.Overlay = bi
        'Else
        '    TaskbarItemInfo.Overlay = Nothing
        'End If
    End Sub

#End Region

#Region "Helper-Methoden"

    Private Sub OpenSkischule(fileName As String)

        If _skikursListFile IsNot Nothing AndAlso fileName.Equals(_skikursListFile.FullName) Then
            MessageBox.Show("Die Skischule " & fileName & " ist bereits geöffnet")
            Exit Sub
        End If

        If Not File.Exists(fileName) Then
            MessageBox.Show("Die Datei existiert nicht")
            Exit Sub
        End If

        ' Datei enzippen und deserialisieren
        Dim serializer = New XmlSerializer(GetType(Entities.Skischule))
        Dim loadedSkischule As Entities.Skischule = Nothing
        Using fs = New FileStream(fileName, FileMode.Open)
            Using zipStream = New GZipStream(fs, CompressionMode.Decompress)
                Try
                    loadedSkischule = TryCast(serializer.Deserialize(zipStream), Entities.Skischule)
                Catch ex As InvalidDataException
                    MessageBox.Show("Datei ungültig: " & ex.Message)
                    Exit Sub
                End Try
            End Using
        End Using

        _skischule = Nothing

        _skikursListFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)
        SetView(loadedSkischule)
        Title = "Skischule - " & fileName

    End Sub

    Private Sub SaveSkischule(fileName As String)
        ' 1. Skischule serialisieren und gezippt abspeichern
        Dim serializer = New XmlSerializer(GetType(Entities.Skischule))
        Using fs = New FileStream(fileName, FileMode.Create)
            Using zipStream = New GZipStream(fs, CompressionMode.Compress)
                serializer.Serialize(zipStream, _skischule)
            End Using
        End Using
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Skischule - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Skischule gespeichert!")
    End Sub

    Private Sub QueueMostRecentFilename(fileName As String)

        Dim max As Integer = 0
        For Each i In _mRUSortedList.Keys
            If i > max Then max = i
        Next

        Dim keysToRemove = New List(Of Integer)
        For Each kvp As KeyValuePair(Of Integer, String) In _mRUSortedList
            If kvp.Value.Equals(fileName) Then keysToRemove.Add(kvp.Key)
        Next

        For Each i As Integer In keysToRemove
            _mRUSortedList.Remove(i)
        Next

        _mRUSortedList.Add(max + 1, fileName)

        If _mRUSortedList.Count > 5 Then
            Dim min As Integer = Integer.MaxValue
            For Each i As Integer In _mRUSortedList.Keys
                If i < min Then min = i
            Next
            _mRUSortedList.Remove(min)
        End If

        RefreshMostRecentMenu()

    End Sub

    Private Sub RefreshMostRecentMenu()
        mostrecentlyUsedMenuItem.Items.Clear()
        RefreshMenuInApplication()
        RefreshJumpListInWinTaskbar()
    End Sub

    Private Sub RefreshMenuInApplication()
        For i = _mRUSortedList.Values.Count - 1 To 0 Step -1
            Dim mi = New MenuItem With {.Header = _mRUSortedList.Values(i)}
            AddHandler mi.Click, AddressOf HandleMostRecentClick
            mostrecentlyUsedMenuItem.Items.Add(mi)
        Next

        If mostrecentlyUsedMenuItem.Items.Count = 0 Then
            Dim mi = New MenuItem With {.Header = "keine"}
            mostrecentlyUsedMenuItem.Items.Add(mi)
        End If
    End Sub

    Private Sub RefreshJumpListInWinTaskbar()

        Dim jumplist = New JumpList With {
            .ShowFrequentCategory = False,
            .ShowRecentCategory = False}

        Dim jumptask = New JumpTask With {
            .CustomCategory = "Release Notes",
            .Title = "SkikursReleaseNotes",
            .Description = "Zeigt die ReleaseNotes zu Skikurse an",
            .ApplicationPath = "C:\Windows\notepad.exe",
            .IconResourcePath = "C:\Windows\notepad.exe",
            .WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            .Arguments = "SkikursReleaseNotes.txt"}

        jumplist.JumpItems.Add(jumptask)

        ' Hinweis Die JumpPath - Elemente sind nur sichtbar, wenn die ".ski"-Dateiendung
        ' unter Windows mit Skikurs assoziiert wird (kann durch Installation via Setup-Projekt erreicht werden,
        ' das auch in den Beispielen enthalten ist, welches die dafür benötigten Werte in die Registry schreibt)

        For i = _mRUSortedList.Values.Count - 1 To 0 Step -1
            Dim jumpPath = New JumpPath With {
                .CustomCategory = "Zuletzt geöffnet",
                .Path = _mRUSortedList.Values(i)}

            jumplist.JumpItems.Add(jumpPath)
        Next

        JumpList.SetJumpList(Application.Current, jumplist)

    End Sub

    Private Sub SetView(Schule As Entities.Skischule)
        _skischule = Schule
        SetView(_skischule.Teilnehmerliste)

    End Sub

    Private Sub SetView(Teilnehmers As TeilnehmerCollection)
        _skischule.Teilnehmerliste = Teilnehmers
        _teilnehmerListCollectionView = New ListCollectionView(Teilnehmers)
        ' Hinweis AddHandler Seite 764
        AddHandler _teilnehmerListCollectionView.CurrentChanged, AddressOf _teilnehmerListCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemTeilnehmer.DataContext = _teilnehmerListCollectionView
    End Sub

#End Region

End Class
