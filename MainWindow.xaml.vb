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
Imports System.Windows.Controls.Primitives
Imports System.Text
Imports System.Linq
Imports System.Collections.Generic

Class MainWindow

#Region "Fields"
    Private _dummySpalteFuerLayerTeilnehmerDetails As ColumnDefinition
    Private _dummySpalteFuerLayerSkikursgruppenDetails As ColumnDefinition
    Private _dummySpalteFuerLayerUebungsleiterDetails As ColumnDefinition
    Private _dummySpalteFuerLayerLevelDetails As ColumnDefinition

    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _skikursgruppenListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _uebungsleiterListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _levelListCollectionView As ICollectionView '... DataContext für das MainWindow

    Private _skischuleListFile As FileInfo
    Private _mRUSortedList As SortedList(Of Integer, String)
    Private _skischule As Entities.Skischule

    Private _schalterLayerDetails As Grid
    Private _schalterBtnShowEplorer As Button
    Private _schalterPinImage As Image
    Private _schalterLayerListe As Grid
    Private _schalterLayerListeTransform As TranslateTransform
    Private _schalterDummySpalteFuerLayerDetails As ColumnDefinition
    Private _schalterBtnPinit As ToggleButton

#End Region

#Region "Constructor"

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ' Spalte initialisieren und in dieselbe Gruppe setzen,
        ' wie die Spalte mit dem Freunde Explorer im
        ' layer1-Grid
        _dummySpalteFuerLayerTeilnehmerDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinTeilnehmerSpalte"}
        _dummySpalteFuerLayerSkikursgruppenDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinSkikursgruppenSpalte"}
        _dummySpalteFuerLayerUebungsleiterDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinUebungsleiterSpalte"}
        _dummySpalteFuerLayerLevelDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinLevelSpalte"}

        ' das Grid gleich zu Beginn pinnen
        layerTeilnehmerliste.Visibility = Visibility.Visible
        tabitemTeilnehmer_GotFocus(Me, New RoutedEventArgs())
        btnTeilnehmerPinIt.IsChecked = True

        _teilnehmerListCollectionView = New ListCollectionView(New TeilnehmerCollection())
        AddHandler _teilnehmerListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _skikursgruppenListCollectionView = New ListCollectionView(New SkikursgruppeCollection())
        AddHandler _skikursgruppenListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _uebungsleiterListCollectionView = New ListCollectionView(New UebungsleiterCollection())
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _levelListCollectionView = New ListCollectionView(New LevelCollection())
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)

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
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.BeurteileTeilnehmerlevel, AddressOf HandleBeurteileTeilnehmerkoennenExecuted, AddressOf HandleBeurteileTeilnehmerkoennenCanExecute))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.NeuerTeilnehmer, AddressOf HandleNeuerTeilnehmerExecuted, AddressOf HandleNeuerTeilnehmerCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.TeilnehmerLoeschen, AddressOf HandleTeilnehmerLoeschenExecuted, AddressOf HandleTeilnehmerLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.NeuerUebungsleiter, AddressOf HandleNeuerUebungsleiterExecuted, AddressOf HandleNeuerUebungsleiterCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.UebungsleiterLoeschen, AddressOf HandleUebungsleiterLoeschenExecuted, AddressOf HandleUebungsleiterLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.NeueGruppe, AddressOf HandleNeueSkikursgruppeExecuted, AddressOf HandleNeueSkikursgruppeCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.GruppeLoeschen, AddressOf HandleSkikursgruppeLoeschenExecuted, AddressOf HandleSkikursgruppeLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.NeuesLevel, AddressOf HandleNeuesLevelExecuted, AddressOf HandleNeuesLevelCanExecuted))
        CommandBindings.Add(New CommandBinding(SkischuleBefehle.LevelLoeschen, AddressOf HandleLevelLoeschenExecuted, AddressOf HandleLevelLoeschenCanExecuted))

        ' 2. SortedList für meist genutzte Skischulen (Most Recently Used) initialisieren
        _mRUSortedList = New SortedList(Of Integer, String)

        ' 3. SortedList für meist genutzte Skischulen befüllen
        LoadmRUSortedListMenu()

        ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
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
        If _skischuleListFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("LastSkischule", System.IO.FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(_skischuleListFile.FullName)
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
        _schalterLayerDetails.ColumnDefinitions.Add(_schalterDummySpalteFuerLayerDetails)
        '        layerTeilnehmerdetails.ColumnDefinitions.Add(_dummySpalteFuerLayer0)

        ' 2. Button "Freunde Explorer" ausblenden
        _schalterBtnShowEplorer.Visibility = Visibility.Collapsed

        ' 3. pinImage in layer1-Grid auf pinned setzen
        _schalterPinImage.Source = New BitmapImage(New Uri("Images\icons8-pin-48.png", UriKind.Relative))

    End Sub

    Private Sub HandleListeUnpinning(sender As Object, e As RoutedEventArgs)

        ' Unpinnen
        ' 1. ColumnDefinition von layer0-Grid entfernen
        _schalterLayerDetails.ColumnDefinitions.Remove(_schalterDummySpalteFuerLayerDetails)

        ' 2. Button "Freunde Explorer" einblenden
        _schalterBtnShowEplorer.Visibility = Visibility.Visible

        ' 3. pinImage in layer1-Grid auf unpinned setzen
        _schalterPinImage.Source = New BitmapImage(New Uri("Images\icons8-unpin-2-48.png", UriKind.Relative))

    End Sub

    Private Sub HandleButtonExpMouseEnter(sender As Object, e As RoutedEventArgs)

        ' TeilnehmerDetails-Grid mit den Explorern einblenden
        If (_schalterLayerListe.Visibility <> Visibility.Visible) Then

            ' 1. Das layerDetails-Grid um die Breite der "Teilnehmer   
            ' Explorer"-Spalte nach rechts versetzen
            _schalterLayerListeTransform.X = _schalterLayerListe.ColumnDefinitions(1).Width.Value

            ' 2. layer1-Grid sichtbar machen
            _schalterLayerListe.Visibility = Visibility.Visible

            ' 3. Die X-Property der layer1Trans vom aktuellen Wert
            ' hin zum Wert 0 animieren, Dauer 500 Millisek
            Dim ani = New DoubleAnimation(0, New Duration(TimeSpan.FromMilliseconds(500)))
            _schalterLayerListeTransform.BeginAnimation(TranslateTransform.XProperty, ani)

        End If

    End Sub

    Private Sub HandleLayerdetailsMouseEnter(sender As Object, e As RoutedEventArgs)

        ' layer1-Grid ausblenden
        If (Not _schalterBtnPinit.IsChecked.GetValueOrDefault() AndAlso layerTeilnehmerliste.Visibility = Visibility.Visible) Then

            ' 1. Zielwert für die Animation setzen
            Dim [to] = _schalterLayerListe.ColumnDefinitions(1).Width.Value

            ' 2. layer1Trans.X zum ermittelten Zielwert animieren
            ' und EventHandler für Completed-Event installieren
            Dim ani = New DoubleAnimation([to], New Duration(TimeSpan.FromMilliseconds(500)))
            AddHandler ani.Completed, New EventHandler(AddressOf ani_Completed)
            _schalterLayerListeTransform.BeginAnimation(TranslateTransform.XProperty, ani)

        End If

    End Sub

    Sub ani_Completed(sender As Object, e As EventArgs)
        ' 3. layer1-Grid ausblenden
        _schalterLayerListe.Visibility = Visibility.Collapsed
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
        Dim NeueSkischule = New Entities.Skischule
        If MessageBoxResult.Yes = MessageBox.Show("Neue Skischule erstellt. Sollen Skikursgruppen angelegt werden?", "Achtung", MessageBoxButton.YesNo) Then
            NeueSkischule.Levelliste = Standardelemente.erstelleLevels()
            Dim dlg = New AnzahlGruppenDialog
            If dlg.ShowDialog Then
                NeueSkischule.Skikursgruppenliste = Standardelemente.erstelleGruppen(dlg.Anzahl, NeueSkischule.Levelliste)
            End If
        End If
        SetView(NeueSkischule)

        If MessageBoxResult.Yes = MessageBox.Show("Auch gleich neue Teilnehmer hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
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

    Private Sub HandleListSaveCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleListSaveAsExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If _skischuleListFile IsNot Nothing Then
            dlg.FileName = _skischuleListFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveSkischule(dlg.FileName)
            _skischuleListFile = New FileInfo(dlg.FileName)
        End If
    End Sub

    Private Sub HandleListSaveExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        If _skischuleListFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveSkischule(_skischuleListFile.FullName)
        End If
    End Sub
    Private Sub HandleCloseExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
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

    Private Sub HandleDrop(sender As Object, e As DragEventArgs)

        Dim filepath As String() = TryCast(e.Data.GetData(DataFormats.FileDrop, True), String())
        Dim validPictureFile = False

        If filepath.Length > 0 Then
            Dim extension As String = Path.GetExtension(filepath(0)).ToLower()

            If ImageTypes.AllImageTypes.Contains(extension) Then
                Using filestream = New FileStream(filepath(0), FileMode.Open)
                    Dim buffer = New Byte(filestream.Length - 1) {}
                    filestream.Read(buffer, 0, filestream.Length)
                    TryCast(_uebungsleiterListCollectionView.CurrentItem, Uebungsleiter).Foto = buffer
                    RefreshTaskBarItemOverlay()
                    CommandManager.InvalidateRequerySuggested()
                    validPictureFile = True
                End Using
            Else
                Dim sb As StringBuilder = New StringBuilder()
                sb.AppendLine("Es werden nur die folgenden Dateiformate")
                sb.Append("unterstützt: ")

                For Each fileformat As String In ImageTypes.AllImageTypes
                    sb.Append(fileformat)
                    sb.Append(", ")
                Next
                ' Das letzte ", " entfernen und Zeilenumbruch einfügen
                sb.Remove(sb.Length - 2, 1)
                MessageBox.Show(sb.ToString)
            End If
        End If

        If filepath.Length > 1 AndAlso validPictureFile Then
            MessageBox.Show("Sie haben mehr als eine Datei gedroppt, es wird nur die erste verwendet.")
        End If

    End Sub

    Private Sub HandleDragOver(sender As Object, e As DragEventArgs)

        e.Effects = DragDropEffects.None
        Dim filepath As String() = TryCast(e.Data.GetData(DataFormats.FileDrop, True), String())

        If filepath.Length > 0 Then
            Dim extension As String = Path.GetExtension(filepath(0)).ToLower()

            If ImageTypes.AllImageTypes.Contains(extension) Then
                e.Effects = DragDropEffects.Copy
            End If
        End If

        e.Handled = True
    End Sub

#Region "Level"

    Private Sub HandleNeuesLevelCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleNeuesLevelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NeuesLevelDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        If dlg.ShowDialog = True Then
            _skischule.Levelliste.Add(dlg.Level)
            _levelListCollectionView.MoveCurrentTo(dlg.Level)
            levelDataGrid.ScrollIntoView(dlg.Level)
        End If
    End Sub

    Private Sub HandleLevelLoeschenCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = tabitemLevels.IsSelected And levelDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleLevelLoeschenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim i As Integer
        Dim index(levelDataGrid.SelectedItems.Count - 1) As Integer
        For Each item As Level In levelDataGrid.SelectedItems
            RemoveLevelFromSkikursgruppe(item)
            RemoveLevelFromTeilnehmer(item)
            index(i) = _skischule.Levelliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(_skischule.Levelliste, index)
    End Sub

#End Region

#Region "Skikursgruppe"

    Private Sub HandleNeueSkikursgruppeCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleNeueSkikursgruppeExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NeueGruppeDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        If dlg.ShowDialog = True Then
            _skischule.Skikursgruppenliste.Add(dlg.Skikursgruppe)
            _skikursgruppenListCollectionView.MoveCurrentTo(dlg.Skikursgruppe)
            skikursgruppenDataGrid.ScrollIntoView(dlg.Skikursgruppe)
        End If
    End Sub

    Private Sub HandleSkikursgruppeLoeschenCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = tabitemSkikursgruppen.IsSelected And skikursgruppenDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleSkikursgruppeLoeschenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim i As Integer
        Dim index(skikursgruppenDataGrid.SelectedItems.Count - 1) As Integer
        For Each item As Skikursgruppe In skikursgruppenDataGrid.SelectedItems
            RemoveSkikursgruppeFromTeilnehmer(item.Gruppenname)
            index(i) = _skischule.Skikursgruppenliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(_skischule.Skikursgruppenliste, index)
    End Sub

#End Region

#Region "Teilnehmer"

    Private Sub HandleNeuerTeilnehmerCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleNeuerTeilnehmerExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New NeuerTeilnehmerDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            _skischule.Teilnehmerliste.Add(dlg.Teilnehmer)
            _teilnehmerListCollectionView.MoveCurrentTo(dlg.Teilnehmer)
            teilnehmerDataGrid.ScrollIntoView(dlg.Teilnehmer)
        End If

    End Sub

    Private Sub HandleTeilnehmerLoeschenCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = tabitemTeilnehmer.IsSelected And teilnehmerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleTeilnehmerLoeschenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim i As Integer
        Dim index(teilnehmerDataGrid.SelectedItems.Count - 1) As Integer
        For Each item As Teilnehmer In teilnehmerDataGrid.SelectedItems
            index(i) = _skischule.Teilnehmerliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(_skischule.Teilnehmerliste, index)
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

#End Region

#Region "Uebungsleiter"

    Private Sub HandleNeuerUebungsleiterCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _skischule IsNot Nothing
    End Sub

    Private Sub HandleNeuerUebungsleiterExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NeuerUebungsleiterDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            _skischule.Skilehrerliste.Add(dlg.Skilehrer)
            _uebungsleiterListCollectionView.MoveCurrentTo(dlg.Skilehrer)
            uebungsleiterDataGrid.ScrollIntoView(dlg.Skilehrer)
        End If
    End Sub

    Private Sub HandleUebungsleiterLoeschenCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = tabitemUebungsleiter.IsSelected And uebungsleiterDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleUebungsleiterLoeschenExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim i As Integer
        Dim index(uebungsleiterDataGrid.SelectedItems.Count - 1) As Integer
        For Each item As Uebungsleiter In uebungsleiterDataGrid.SelectedItems
            RemoveUebungsleiterFromSkikursgruppe(item)
            index(i) = _skischule.Skilehrerliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(_skischule.Skilehrerliste, index)

    End Sub

#End Region

#End Region

#Region "Sonstige Eventhandler"

    Sub _listCollectionView_CurrentChanged(sender As Object, e As EventArgs)
        RefreshTaskBarItemOverlay()
    End Sub

    Private Sub RefreshTaskBarItemOverlay()
        ' Dim currentTeilnehmer = DirectCast(_teilnehmerListCollectionView.CurrentItem, Teilnehmer)

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
    Private Sub AddLevelToTeilnehmer(Teilnehmerliste As TeilnehmerCollection, Level As Level)
        Teilnehmerliste.ToList.ForEach(Sub(x) x.PersoenlichesLevel = Level)
    End Sub

    Private Sub AddLevelToSkikursgruppe(Skikursgruppenliste As SkikursgruppeCollection, Level As Level)
        Skikursgruppenliste.ToList.ForEach(Sub(x) x.Gruppenlevel = Level)
    End Sub

    Private Sub AddUebungsleiterToSkikursgruppe(Skikursgruppe As Skikursgruppe, Uebungsleiter As Uebungsleiter)
        Skikursgruppe.Skilehrer = Uebungsleiter
    End Sub

    Private Sub AddSkikursgruppeToTeilnehmer(Teilnehmerliste As TeilnehmerCollection, Skikursgruppe As String)
        Teilnehmerliste.ToList.ForEach(Sub(x) x.Skikursgruppe = Skikursgruppe)
    End Sub

    Private Sub RemoveLevelFromTeilnehmer(level As Level)
        Dim liste = _skischule.Teilnehmerliste.TakeWhile(Function(x) x.PersoenlichesLevel.LevelID = level.LevelID)
        liste.ToList.ForEach(Sub(x) x.PersoenlichesLevel = Nothing)
    End Sub

    Private Sub RemoveLevelFromSkikursgruppe(level As Level)
        Dim liste = _skischule.Skikursgruppenliste.TakeWhile(Function(x) x.Gruppenlevel.LevelID = level.LevelID)
        liste.ToList.ForEach(Sub(x) x.Gruppenlevel = Nothing)
    End Sub

    Private Sub RemoveUebungsleiterFromSkikursgruppe(Uebungsleiter As Uebungsleiter)
        Dim liste = _skischule.Skikursgruppenliste.TakeWhile(Function(x) x.Skilehrer.SkilehrerID = Uebungsleiter.SkilehrerID)
        liste.ToList.ForEach(Sub(x) x.Skilehrer = Nothing)
    End Sub

    Private Sub RemoveSkikursgruppeFromTeilnehmer(Skikursgruppe As String)
        Dim liste = _skischule.Teilnehmerliste.TakeWhile(Function(x) x.Skikursgruppe = Skikursgruppe)
        liste.ToList.ForEach(Sub(x) x.Skikursgruppe = Nothing)
    End Sub

    Private Sub RemoveItemsAt(source As IList, ParamArray itemIndices As Integer())
        If source Is Nothing Then Throw New ArgumentNullException("Source")
        If itemIndices Is Nothing Then Throw New ArgumentNullException("itemIndices")
        For Each itemIndex In itemIndices.OrderByDescending(Function(x) x)
            source.RemoveAt(itemIndex)
        Next
    End Sub

    Private Sub OpenSkischule(fileName As String)

        If _skischuleListFile IsNot Nothing AndAlso fileName.Equals(_skischuleListFile.FullName) Then
            MessageBox.Show("Die Skischule " & fileName & " ist bereits geöffnet")
            Exit Sub
        End If

        If Not File.Exists(fileName) Then
            MessageBox.Show("Die Datei existiert nicht")
            Exit Sub
        End If

        Dim loadedSkischule = OpenXML(fileName)
        'Dim loadedSkischule = OpenZIP(fileName)
        If loadedSkischule Is Nothing Then Exit Sub

        _skischule = Nothing

        _skischuleListFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)
        SetView(loadedSkischule)
        Title = "Skischule - " & fileName

    End Sub

    Private Function OpenXML(fileName As String) As Entities.Skischule
        Dim serializer = New XmlSerializer(GetType(Entities.Skischule))
        Dim loadedSkischule As Entities.Skischule = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                loadedSkischule = TryCast(serializer.Deserialize(fs), Entities.Skischule)
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedSkischule
    End Function

    Private Function OpenZIP(fileName As String) As Entities.Skischule
        Dim serializer = New XmlSerializer(GetType(Entities.Skischule))
        Dim loadedSkischule As Entities.Skischule = Nothing

        ' Datei entzippen und deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Using zipStream = New GZipStream(fs, CompressionMode.Decompress)
                Try
                    loadedSkischule = TryCast(serializer.Deserialize(zipStream), Entities.Skischule)
                Catch ex As InvalidDataException
                    MessageBox.Show("Datei ungültig: " & ex.Message)
                    Return Nothing
                End Try
            End Using
        End Using
        Return loadedSkischule
    End Function


    Private Sub SaveSkischule(fileName As String)
        ' 1. Skischule serialisieren und gezippt abspeichern
        SaveXML(fileName)
        'SaveZIP(fileName)
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Skischule - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Skischule gespeichert!")
    End Sub

    Private Sub SaveZIP(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Skischule))
        Using fs = New FileStream(fileName, FileMode.Create)
            Using zipStream = New GZipStream(fs, CompressionMode.Compress)
                serializer.Serialize(zipStream, _skischule)
            End Using
        End Using
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Skischule))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, _skischule)
        End Using
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
        SetView(_skischule.Skikursgruppenliste)
        SetView(_skischule.Skilehrerliste)
        SetView(_skischule.Levelliste)
    End Sub

    Private Sub SetView(Teilnehmers As TeilnehmerCollection)
        _skischule.Teilnehmerliste = Teilnehmers
        _teilnehmerListCollectionView = New ListCollectionView(Teilnehmers)
        ' Hinweis AddHandler Seite 764
        AddHandler _teilnehmerListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemTeilnehmer.DataContext = _teilnehmerListCollectionView
    End Sub
    Private Sub SetView(Skikursgruppen As SkikursgruppeCollection)
        _skischule.Skikursgruppenliste = Skikursgruppen
        _skikursgruppenListCollectionView = New ListCollectionView(Skikursgruppen)
        ' Hinweis AddHandler Seite 764
        AddHandler _skikursgruppenListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemSkikursgruppen.DataContext = _skikursgruppenListCollectionView
    End Sub

    Private Sub SetView(Skilehrer As UebungsleiterCollection)
        _skischule.Skilehrerliste = Skilehrer
        _uebungsleiterListCollectionView = New ListCollectionView(Skilehrer)
        ' Hinweis AddHandler Seite 764
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemUebungsleiter.DataContext = _uebungsleiterListCollectionView
    End Sub

    Private Sub SetView(Level As LevelCollection)
        _skischule.Levelliste = Level
        _levelListCollectionView = New ListCollectionView(Level)
        ' Hinweis AddHandler Seite 764
        AddHandler _levelListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemLevels.DataContext = _levelListCollectionView
    End Sub

    Private Sub tabitemTeilnehmer_GotFocus(sender As Object, e As RoutedEventArgs)
        _schalterLayerDetails = layerTeilnehmerdetails
        _schalterBtnShowEplorer = btnShowTeilnehmerExplorer
        _schalterPinImage = pinTeilnehmerImage
        _schalterLayerListe = layerTeilnehmerliste
        _schalterLayerListeTransform = layerTeilnehmerlisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerTeilnehmerDetails
        _schalterBtnPinit = btnTeilnehmerPinIt
    End Sub

    Private Sub tabitemSkikursgruppen_GotFocus(sender As Object, e As RoutedEventArgs)
        _schalterLayerDetails = layerSkikursgruppendetails
        _schalterBtnShowEplorer = btnShowSkikursgruppenExplorer
        _schalterPinImage = pinSkikursgruppenImage
        _schalterLayerListe = layerSkikursgruppenliste
        _schalterLayerListeTransform = layerSkikursgruppenlisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerSkikursgruppenDetails
        _schalterBtnPinit = btnSkikursgruppenPinIt
    End Sub

    Private Sub tabitemSkilehrer_GotFocus(sender As Object, e As RoutedEventArgs)
        _schalterLayerDetails = layerSkilehrerdetails
        _schalterBtnShowEplorer = btnShowSkilehrerExplorer
        _schalterPinImage = pinSkilehrerImage
        _schalterLayerListe = layerSkilehrerliste
        _schalterLayerListeTransform = layerSkilehrerlisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerUebungsleiterDetails
        _schalterBtnPinit = btnSkilehrerPinIt
    End Sub

    Private Sub tabitemLevel_GotFocus(sender As Object, e As RoutedEventArgs)
        _schalterLayerDetails = layerLeveldetails
        _schalterBtnShowEplorer = btnShowLevelExplorer
        _schalterPinImage = pinLevelImage
        _schalterLayerListe = layerLevelliste
        _schalterLayerListeTransform = layerLevellisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerLevelDetails
        _schalterBtnPinit = btnLevelPinIt
    End Sub
#End Region

End Class
