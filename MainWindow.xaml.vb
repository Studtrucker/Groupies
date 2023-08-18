Imports Microsoft.Win32
Imports Skireisen.Entities
Imports System.Reflection
Imports System.ComponentModel
Imports System.IO
Imports System.IO.Compression
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports System.IO.IsolatedStorage
Imports Skireisen.BasicObjects
Imports System.Windows.Media.Animation

Class MainWindow

#Region "Fields"
    Private _dummySpalteFuerLayer0 As ColumnDefinition
    Private _teilnehmerList As TeilnehmerCollection
    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _skireiseListFile As FileInfo
    Private _mRUSortedList As SortedList(Of Integer, String)

#End Region

#Region "Events"

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
        layer1Teilnehmerliste.Visibility = Visibility.Visible
        btnPinIt.IsChecked = True

        _teilnehmerListCollectionView = New ListCollectionView(New TeilnehmerCollection())
        AddHandler _teilnehmerListCollectionView.CurrentChanged, New EventHandler(AddressOf _teilnehmerListCollectionView_CurrentChanged)

    End Sub


    Private Sub HandleMainWindowLoaded(sender As Object, e As RoutedEventArgs)

        ' 0. Zur InputBindings ein MouseBinding hinzufügen. Nur als Beispiel,
        '    um mit Strg und Doppelklick eine neue Liste anlegen zu können
        'Dim mg = New MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control)
        'Dim m = New MouseBinding(ApplicationCommands.[New], mg)
        'InputBindings.Add(m)


        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden
        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleListNewExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleListOpenExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleListSaveExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleListSaveAsExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Help, AddressOf HandleHelpExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))

        CommandBindings.Add(New CommandBinding(SkireisenBefehle.ImportTeilnehmerliste, AddressOf HandleImportTeilnehmerExecuted, AddressOf HandleImportTeilnehmerCanExecute))
        CommandBindings.Add(New CommandBinding(SkireisenBefehle.BeurteileTeilnehmerkoennen, AddressOf HandleBeurteileTeilnehmerkoennenExecuted, AddressOf HandleBeurteileTeilnehmerkoennenCanExecute))

        ' 2. SortedList für meist genutzte Freundeslisten (Most Recently Used) initialisieren
        _mRUSortedList = New SortedList(Of Integer, String)

        ' 3. SortedList für meist genutzte Freundeslisten befüllen
        LoadmRUSortedListMenu()

        ' 4. Die zuletzt verwendete Freundesliste laden, falls nicht eine .friends-Datei doppelgeklickt wurde
        If (Environment.GetCommandLineArgs().Length = 2) Then
            Dim args = Environment.GetCommandLineArgs
            Dim filename = args(1)
            OpenSkireiseList(filename)
        Else
            ' 5. JumpList in Windows Taskbar aktualisieren
            RefreshJumpListInWinTaskbar()
        End If

    End Sub

    Private Sub HandleMainWindowClosing(sender As Object, e As CancelEventArgs)
        Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schliessen?", "Achtung", MessageBoxButton.YesNo)
        e.Cancel = result = MessageBoxResult.No
    End Sub

    Private Sub HandleMainwindowClosed(sender As Object, e As EventArgs)

        ' 1. Den Pfad der letzen Liste ins IsolatedStorage speichern.
        If _skireiseListFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("LastSkireisenList", System.IO.FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(_skireiseListFile.FullName)
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

#Region "Methoden zum Laden der meist genutzten Listen und der letzten Skireisen"

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
#End Region

#Region "EventHandler der CommandBindings"

    Private Sub HandleListNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        If _teilnehmerList IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Liste noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        _teilnehmerList = Nothing

        Title = "Skireisen"
        SetView(New TeilnehmerCollection)
        If MessageBoxResult.Yes = MessageBox.Show("Neue Reise erstellt. Jetzt gleich die Bearbeitung beginnen?", "Achtung", MessageBoxButton.YesNo) Then
            ' Todo: wie soll die Bearbeitung der neuen Skireise beginnen?
            initializeStandardKoennenstufen()
        End If

    End Sub

    Private Sub HandleListOpenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        If dlg.ShowDialog = True Then
            OpenSkireiseList(dlg.FileName)
        End If
    End Sub

    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        OpenSkireiseList(TryCast(sender, MenuItem).Header.ToString())
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

    Private Sub HandleImportTeilnehmerExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim importTeilnehmer = DatenImport.ImportTeilnehmerListe()
        If importTeilnehmer IsNot Nothing Then
            SetView(importTeilnehmer)
            '            DataContext = importTeilnehmer
        End If

    End Sub

    Private Sub HandleImportTeilnehmerCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _teilnehmerList IsNot Nothing
    End Sub

    Private Sub HandleListSaveCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _teilnehmerList IsNot Nothing
    End Sub

    Private Sub HandleListSaveAsExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If _skireiseListFile IsNot Nothing Then
            dlg.FileName = _skireiseListFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveSkireise(dlg.FileName)
            _skireiseListFile = New FileInfo(dlg.FileName)
        End If
    End Sub

    Private Sub HandleListSaveExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        If _skireiseListFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveSkireise(_skireiseListFile.FullName)
        End If
    End Sub
    Private Sub HandleCloseExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
    End Sub

#End Region

#Region "Helper-Methoden"

    Private Sub OpenSkireiseList(fileName As String)
        If _skireiseListFile IsNot Nothing AndAlso fileName.Equals(_skireiseListFile.FullName) Then
            MessageBox.Show("Die Liste " & fileName & " ist bereits geöffnet")
            Exit Sub
        End If

        If Not File.Exists(fileName) Then
            MessageBox.Show("Die Datei existiert nicht")
            Exit Sub
        End If

        ' Datei enzippen und deserialisieren
        Dim serializer = New XmlSerializer(GetType(TeilnehmerCollection))
        Dim loadedFriendCollection As TeilnehmerCollection = Nothing
        Using fs = New FileStream(fileName, FileMode.Open)
            Using zipStream = New GZipStream(fs, CompressionMode.Decompress)
                Try
                    loadedFriendCollection = TryCast(serializer.Deserialize(zipStream), TeilnehmerCollection)
                Catch ex As InvalidDataException
                    MessageBox.Show("Datei ungültig: " & ex.Message)
                    Exit Sub
                End Try
            End Using
        End Using

        _teilnehmerList = Nothing

        _skireiseListFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)
        SetView(loadedFriendCollection)
        Title = "Skireisen - " & fileName

    End Sub


    Private Sub SaveSkireise_alt(fileName As String)
        ' 1. Freundeliste serialisieren und gezippt abspeichern
        Dim serializer = New XmlSerializer(GetType(TeilnehmerCollection))
        Using fs = New FileStream(fileName, FileMode.Create)
            Using zipStream = New GZipStream(fs, CompressionMode.Compress)
                serializer.Serialize(zipStream, _teilnehmerList)
            End Using
        End Using
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Skireise - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Skireise gespeichert!")
    End Sub

    Private Sub SaveSkireise(fileName As String)
        ' 1. Freundeliste serialisieren und gezippt abspeichern
        Dim serializer = New XmlSerializer(GetType(TeilnehmerCollection))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, _teilnehmerList)
        End Using

        Dim serializerLevels = New XmlSerializer(GetType(KoennenstufenCollection))
        Using fsLevels = New FileStream(fileName, FileMode.Append)
            serializerLevels.Serialize(fsLevels, Koennenstufen)
        End Using
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Skireise - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Skireise gespeichert!")
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
            .Title = "SkireiseReleaseNotes",
            .Description = "Zeigt die ReleaseNotes zu Skireise an",
            .ApplicationPath = "C:\Windows\notepad.exe",
            .IconResourcePath = "C:\Windows\notepad.exe",
            .WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            .Arguments = "SkireiseReleaseNotes.txt"}

        jumplist.JumpItems.Add(jumptask)

        ' Hinweis Die JumpPath - Elemente sind nur sichtbar, wenn die ".ski"-Dateiendung
        ' unter Windows mit Skireise assoziiert wird (kann durch Installation via Setup-Projekt erreicht werden,
        ' das auch in den Beispielen enthalten ist, welches die dafür benötigten Werte in die Registry schreibt)

        For i = _mRUSortedList.Values.Count - 1 To 0 Step -1
            Dim jumpPath = New JumpPath With {
                .CustomCategory = "Zuletzt geöffnet",
                .Path = _mRUSortedList.Values(i)}

            jumplist.JumpItems.Add(jumpPath)
        Next

        JumpList.SetJumpList(Application.Current, jumplist)

    End Sub

    Private Sub initializeStandardKoennenstufen()

        BasicObjects.erstelleKoennenstufen()

    End Sub

#End Region

#Region "Methoden zum Pinnen und Ein-/Ausblenden des Freunde-Explorers"

    Private Sub HandlePinning(sender As Object, e As RoutedEventArgs)

        ' Pinnen

        ' 1. ColumnDefinition zum layer0-Grid hinzufügen
        layer0Teilnehmerdetails.ColumnDefinitions.Add(_dummySpalteFuerLayer0)

        ' 2. Button "Freunde Explorer" ausblenden
        btnShowTeilnehmerExplorer.Visibility = Visibility.Collapsed

        ' 3. pinImage in layer1-Grid auf pinned setzen
        pinImage.Source = New BitmapImage(New Uri("Images\icons8-pin-48.png", UriKind.Relative))

    End Sub

    Private Sub HandleUnpinning(sender As Object, e As RoutedEventArgs)
        ' Unpinnen

        ' 1. ColumnDefinition von layer0-Grid entfernen
        layer0Teilnehmerdetails.ColumnDefinitions.Remove(_dummySpalteFuerLayer0)

        ' 2. Button "Freunde Explorer" einblenden
        btnShowTeilnehmerExplorer.Visibility = Visibility.Visible

        ' 3. pinImage in layer1-Grid auf unpinned setzen
        pinImage.Source = New BitmapImage(New Uri("Images\icons8-unpin-2-48.png", UriKind.Relative))

    End Sub


    Private Sub HandleButtonTNExpMouseEnter(sender As Object, e As RoutedEventArgs)

        ' layerDetails-Grid mit den Explorern einblenden
        If (layer1Teilnehmerliste.Visibility <> Visibility.Visible) Then

            ' 1. Das layerDetails-Grid um die Breite der "Teilnehmer   
            ' Explorer"-Spalte nach rechts versetzen
            layer1TeilnehmerlisteTrans.X = layer1Teilnehmerliste.ColumnDefinitions(1).Width.Value

            ' 2. layer1-Grid sichtbar machen
            layer1Teilnehmerliste.Visibility = Visibility.Visible

            ' 3. Die X-Property der layer1Trans vom aktuellen Wert
            ' hin zum Wert 0 animieren, Dauer 500 Millisek
            Dim ani = New DoubleAnimation(0, New Duration(TimeSpan.FromMilliseconds(500)))
            layer1TeilnehmerlisteTrans.BeginAnimation(TranslateTransform.XProperty, ani)

        End If
    End Sub

    Private Sub HandleLayer0MouseEnter(sender As Object, e As RoutedEventArgs)

        ' layer1-Grid ausblenden
        If (Not btnPinIt.IsChecked.GetValueOrDefault() AndAlso layer1Teilnehmerliste.Visibility = Visibility.Visible) Then

            ' 1. Zielwert für die Animation setzen
            Dim [to] = layer1Teilnehmerliste.ColumnDefinitions(1).Width.Value

            ' 2. layer1Trans.X zum ermittelten Zielwert animieren
            ' und EventHandler für Completed-Event installieren
            Dim ani = New DoubleAnimation([to], New Duration(TimeSpan.FromMilliseconds(500)))
            AddHandler ani.Completed, New EventHandler(AddressOf ani_Completed)
            layer1TeilnehmerlisteTrans.BeginAnimation(TranslateTransform.XProperty, ani)

        End If

    End Sub

    Sub ani_Completed(sender As Object, e As EventArgs)
        ' 3. layer1-Grid ausblenden
        layer1Teilnehmerliste.Visibility = Visibility.Collapsed
    End Sub

    'Private Sub HandleButtonSLExpMouseEnter(sender As Object, e As RoutedEventArgs)
    '    ' layerDetails-Grid mit den Explorern einblenden
    '    If (layer1Skilehrerliste.Visibility <> Visibility.Visible) Then

    '        ' 1. Das layerDetails-Grid um die Breite der "Teilnehmer   
    '        ' Explorer"-Spalte nach rechts versetzen
    '        layer1SkilehrerlisteTrans.X = layer1Skilehrerliste.ColumnDefinitions(1).Width.Value

    '        ' 2. layer1-Grid sichtbar machen
    '        layer1Skilehrerliste.Visibility = Visibility.Visible

    '        ' 3. Die X-Property der layer1Trans vom aktuellen Wert
    '        ' hin zum Wert 0 animieren, Dauer 500 Millisek
    '        Dim ani = New DoubleAnimation(0, New Duration(TimeSpan.FromMilliseconds(500)))
    '        layer1SkilehrerlisteTrans.BeginAnimation(TranslateTransform.XProperty, ani)

    '    End If
    'End Sub

    'Private Sub HandleButtonSGExpMouseEnter(sender As Object, e As MouseEventArgs)
    '    ' layerDetails-Grid mit den Explorern einblenden
    '    If (layer1Skigruppenliste.Visibility <> Visibility.Visible) Then

    '        ' 1. Das layerDetails-Grid um die Breite der "Teilnehmer   
    '        ' Explorer"-Spalte nach rechts versetzen
    '        layer1SkigruppenlisteTrans.X = layer1Skigruppenliste.ColumnDefinitions(1).Width.Value

    '        ' 2. layer1-Grid sichtbar machen
    '        layer1Skigruppenliste.Visibility = Visibility.Visible

    '        ' 3. Die X-Property der layer1Trans vom aktuellen Wert
    '        ' hin zum Wert 0 animieren, Dauer 500 Millisek
    '        Dim ani = New DoubleAnimation(0, New Duration(TimeSpan.FromMilliseconds(500)))
    '        layer1SkigruppenlisteTrans.BeginAnimation(TranslateTransform.XProperty, ani)

    '    End If
    'End Sub

#End Region

#Region "weitere Eventhandler"

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

    Private Sub SetView(TeilnehmerListe As TeilnehmerCollection)
        _teilnehmerList = TeilnehmerListe
        _teilnehmerListCollectionView = New ListCollectionView(TeilnehmerListe)
        ' Hinweis AddHandler Seite 764
        AddHandler _teilnehmerListCollectionView.CurrentChanged, AddressOf _teilnehmerListCollectionView_CurrentChanged
        ' DataContext wird gesetzt Inhalt = CollectionView, diese kennt sein CurrentItem
        DataContext = _teilnehmerListCollectionView
    End Sub

End Class
