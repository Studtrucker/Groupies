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
Imports System.Windows.Markup
Imports Skischule.UserControls
Imports System.Collections.ObjectModel
Imports Skischule.ExcelService
Imports DS = Skischule.DataService.CurrentDataService
Imports Skischule.DataService

Class MainWindow

#Region "Fields"
    Private _dummySpalteFuerLayerTeilnehmerDetails As ColumnDefinition
    Private _dummySpalteFuerLayerSkikurslisteDetails As ColumnDefinition
    Private _dummySpalteFuerLayerUebungsleiterDetails As ColumnDefinition
    Private _dummySpalteFuerLayerLevelDetails As ColumnDefinition

    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _skikursListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _uebungsleiterListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _levelListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _groupLevelListCollectionView As ICollectionView
    Private _groupLeaderListCollectionView As ICollectionView
    Private _participantLevelListCollectionView As ICollectionView
    Private _participantMemberOfGroupListCollectionView As ICollectionView


    Private _skischuleListFile As FileInfo
    Private _mRUSortedList As SortedList(Of Integer, String)
    '    Private _skischule As Entities.Skiclub

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
        _dummySpalteFuerLayerSkikurslisteDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinSkikursgruppenSpalte"}
        _dummySpalteFuerLayerUebungsleiterDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinUebungsleiterSpalte"}
        _dummySpalteFuerLayerLevelDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinLevelSpalte"}

        ' das Grid gleich zu Beginn pinnen
        layerTeilnehmerliste.Visibility = Visibility.Visible
        tabitemTeilnehmer_GotFocus(Me, New RoutedEventArgs())
        btnTeilnehmerPinIt.IsChecked = True

        layerLevelliste.Visibility = Visibility.Visible
        tabitemLevel_GotFocus(Me, New RoutedEventArgs())
        btnLevelPinIt.IsChecked = True

        layerSkikursdetails.Visibility = Visibility.Visible
        tabitemSkikurs_GotFocus(Me, New RoutedEventArgs())
        btnSkikursPinIt.IsChecked = True

        layerSkilehrerdetails.Visibility = Visibility.Visible
        tabitemSkilehrer_GotFocus(Me, New RoutedEventArgs())
        btnSkilehrerPinIt.IsChecked = True

        _teilnehmerListCollectionView = New ListCollectionView(New ParticipantCollection())
        AddHandler _teilnehmerListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _skikursListCollectionView = New ListCollectionView(New GroupCollection())
        AddHandler _skikursListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _uebungsleiterListCollectionView = New ListCollectionView(New InstructorCollection())
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
        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleNewExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleListOpenExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleListSaveExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleListSaveAsExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Help, AddressOf HandleHelpExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))

        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportSkiclub, AddressOf HandleImportSkiclubExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportParticipants, AddressOf HandleImportParticipantsExecuted, AddressOf HandleImportParticipantsCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.BeurteileTeilnehmerlevel, AddressOf HandleBeurteileTeilnehmerkoennenExecuted, AddressOf HandleBeurteileTeilnehmerkoennenCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NewParticipant, AddressOf HandleNeuerTeilnehmerExecuted, AddressOf HandleNeuerTeilnehmerCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerLoeschen, AddressOf HandleTeilnehmerLoeschenExecuted, AddressOf HandleTeilnehmerLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NeuerUebungsleiter, AddressOf HandleNeuerUebungsleiterExecuted, AddressOf HandleNeuerUebungsleiterCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.UebungsleiterLoeschen, AddressOf HandleUebungsleiterLoeschenExecuted, AddressOf HandleUebungsleiterLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NeueGruppe, AddressOf HandleNeueSkikursgruppeExecuted, AddressOf HandleNeueSkikursgruppeCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppeLoeschen, AddressOf HandleSkikursgruppeLoeschenExecuted, AddressOf HandleSkikursgruppeLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NeuesLevel, AddressOf HandleNeuesLevelExecuted, AddressOf HandleNeuesLevelCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.LevelLoeschen, AddressOf HandleLevelLoeschenExecuted, AddressOf HandleLevelLoeschenCanExecuted))

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

    Private Sub HandleNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If DS.Skiclub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        DS.Skiclub = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Skischule"
        Dim NeueSkischule = New Entities.Skiclub
        NeueSkischule.Levellist = CreateDefaultService.CreateLevels()
        Dim dlg = New CountOfGroupsDialog
        If dlg.ShowDialog Then
            NeueSkischule.Grouplist = CreateDefaultService.CreateGroups(dlg.Count.Text)
        End If
        SetView(NeueSkischule)

        If MessageBoxResult.Yes = MessageBox.Show("Auch gleich neue Teilnehmer hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
            SkiclubCommands.NewParticipant.Execute(Nothing, Me)
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
        e.CanExecute = DS.Skiclub IsNot Nothing
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

    Private Sub HandleImportParticipantsCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = DataService.Skiclub IsNot Nothing AndAlso DataService.Skiclub.Participantlist IsNot Nothing
    End Sub

    Private Sub HandleImportParticipantsExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim ImportParticipants = ImportService.ImportParticipants
        If ImportParticipants IsNot Nothing Then
            'DataService.Skiclub.Participantlist.ToList.AddRange(ImportParticipants)
            ImportParticipants.ToList.ForEach(Sub(x) DataService.Skiclub.Participantlist.Add(x))
            MessageBox.Show(String.Format("Es wurden {0} Teilnehmer erfolgreich importiert", ImportParticipants.Count))
        End If

    End Sub


    Private Sub HandleImportSkiclubExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If DS.Skiclub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        DS.Skiclub = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Skischule"

        Dim importSkiclub = ImportService.ImportSkiclub
        If importSkiclub IsNot Nothing Then
            SetView(importSkiclub)
            MessageBox.Show(String.Format("Daten aus {0} erfolgreich importiert", ImportService.Workbook.Name))
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
                    TryCast(_uebungsleiterListCollectionView.CurrentItem, Instructor).Picture = buffer
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

    Private Sub HandleListPrintExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New PrintDialog()
        If dlg.ShowDialog = True Then
            Dim printArea = New Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight)
            Dim pageMargin = New Thickness(30, 30, 30, 60)
            Dim doc As FixedDocument = GetListAsFixedDocument(printArea, pageMargin)
            dlg.PrintDocument(doc.DocumentPaginator, "Skischule")
        End If

    End Sub

    Private Sub HandleListPrintCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        ' Todo: Can execute festlegen
        e.CanExecute = True '_skischule IsNot Nothing OrElse _skischule.Skikursgruppenliste IsNot Nothing OrElse _skischule.Skikursgruppenliste.Count > 0
    End Sub



#Region "Level"

    Private Sub HandleNeuesLevelCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = DS.Skiclub IsNot Nothing
    End Sub

    Private Sub HandleNeuesLevelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewLevelDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        If dlg.ShowDialog = True Then
            DS.Skiclub.Levellist.Add(dlg.Level)
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
            index(i) = DS.Skiclub.Levellist.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(DS.Skiclub.Levellist, index)
    End Sub

#End Region

#Region "Skikursgruppe"

    Private Sub HandleNeueSkikursgruppeCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = DS.Skiclub IsNot Nothing
    End Sub

    Private Sub HandleNeueSkikursgruppeExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewGroupDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        If dlg.ShowDialog = True Then
            DS.Skiclub.Grouplist.Add(dlg.Group)
            _skikursListCollectionView.MoveCurrentTo(dlg.Group)
            skikurseDataGrid.ScrollIntoView(dlg.Group)
        End If
    End Sub

    Private Sub HandleSkikursgruppeLoeschenCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = tabitemSkikurse.IsSelected And skikurseDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleSkikursgruppeLoeschenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim i As Integer
        Dim index(skikurseDataGrid.SelectedItems.Count - 1) As Integer
        For Each item As Group In skikurseDataGrid.SelectedItems
            RemoveSkikursgruppeFromTeilnehmer(item)
            index(i) = DS.Skiclub.Grouplist.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(DS.Skiclub.Grouplist, index)
    End Sub

#End Region

#Region "Teilnehmer"

    Private Sub HandleNeuerTeilnehmerCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = DS.Skiclub IsNot Nothing
    End Sub

    Private Sub HandleNeuerTeilnehmerExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New NewParticipantDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            DS.Skiclub.Participantlist.Add(dlg.Teilnehmer)
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
        For Each item As Participant In teilnehmerDataGrid.SelectedItems
            index(i) = DS.Skiclub.Participantlist.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(DS.Skiclub.Participantlist, index)
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
        e.CanExecute = DS.Skiclub IsNot Nothing
    End Sub

    Private Sub HandleNeuerUebungsleiterExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewInstructorDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            DS.Skiclub.Instructorlist.Add(dlg.Instructor)
            _uebungsleiterListCollectionView.MoveCurrentTo(dlg.Instructor)
            uebungsleiterDataGrid.ScrollIntoView(dlg.Instructor)
        End If
    End Sub

    Private Sub HandleUebungsleiterLoeschenCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = tabitemUebungsleiter.IsSelected And uebungsleiterDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub HandleUebungsleiterLoeschenExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim i As Integer
        Dim index(uebungsleiterDataGrid.SelectedItems.Count - 1) As Integer
        For Each item As Instructor In uebungsleiterDataGrid.SelectedItems
            RemoveUebungsleiterFromSkikursgruppe(item)
            index(i) = DS.Skiclub.Instructorlist.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(DS.Skiclub.Instructorlist, index)

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
    Private Sub AddLevelToTeilnehmer(Teilnehmerliste As ParticipantCollection, Level As Level)
        Teilnehmerliste.ToList.ForEach(Sub(x) x.ParticipantLevel = Level)
    End Sub

    Private Sub AddLevelToSkikursgruppe(Skikursgruppenliste As GroupCollection, Level As Level)
        Skikursgruppenliste.ToList.ForEach(Sub(x) x.Grouplevel = Level)
    End Sub

    Private Sub AddUebungsleiterToSkikursgruppe(Skikursgruppe As Group, Uebungsleiter As Instructor)
        Skikursgruppe.Groupleader = Uebungsleiter
    End Sub

    Private Sub AddSkikursgruppeToTeilnehmer(Teilnehmerliste As ParticipantCollection, Skikursgruppe As Group)
        Teilnehmerliste.ToList.ForEach(Sub(x) x.ParticipantMemberOfGroup = Skikursgruppe)
    End Sub

    Private Sub RemoveLevelFromTeilnehmer(level As Level)
        Dim liste = DS.Skiclub.Participantlist.TakeWhile(Function(x) x.ParticipantLevel.LevelID = level.LevelID)
        liste.ToList.ForEach(Sub(x) x.ParticipantLevel = Nothing)
    End Sub

    Private Sub RemoveLevelFromSkikursgruppe(level As Level)
        Dim liste = DS.Skiclub.Grouplist.TakeWhile(Function(x) x.Grouplevel Is level)
        liste.ToList.ForEach(Sub(x) x.Grouplevel = Nothing)
    End Sub

    Private Sub RemoveUebungsleiterFromSkikursgruppe(Uebungsleiter As Instructor)
        Dim liste = DS.Skiclub.Grouplist.TakeWhile(Function(x) x.Groupleader Is Uebungsleiter)
        liste.ToList.ForEach(Sub(x) x.Groupleader = Nothing)
    End Sub

    Private Sub RemoveSkikursgruppeFromTeilnehmer(Skikursgruppe As Group)
        Dim liste = DS.Skiclub.Participantlist.TakeWhile(Function(x) x.ParticipantMemberOfGroup Is Skikursgruppe)
        liste.ToList.ForEach(Sub(x) x.ParticipantMemberOfGroup = Nothing)
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

        DS.Skiclub = Nothing

        _skischuleListFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)
        SetView(loadedSkischule)
        Title = "Skischule - " & fileName

    End Sub

    Private Function OpenXML(fileName As String) As Entities.Skiclub
        Dim serializer = New XmlSerializer(GetType(Entities.Skiclub))
        Dim loadedSkiclub As Entities.Skiclub = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(fs), Entities.Skiclub)
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedSkiclub
    End Function

    Private Function OpenZIP(fileName As String) As Entities.Skiclub
        Dim serializer = New XmlSerializer(GetType(Entities.Skiclub))
        Dim loadedSkischule As Entities.Skiclub = Nothing

        ' Datei entzippen und deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Using zipStream = New GZipStream(fs, CompressionMode.Decompress)
                Try
                    loadedSkischule = TryCast(serializer.Deserialize(zipStream), Entities.Skiclub)
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
        Dim serializer = New XmlSerializer(GetType(Entities.Skiclub))
        Using fs = New FileStream(fileName, FileMode.Create)
            Using zipStream = New GZipStream(fs, CompressionMode.Compress)
                serializer.Serialize(zipStream, DS.Skiclub)
            End Using
        End Using
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Skiclub))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, DS.Skiclub)
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

    Private Sub SetView(Schule As Entities.Skiclub)

        DS.Skiclub = Schule

        If DS.Skiclub IsNot Nothing Then
            _groupLevelListCollectionView = New CollectionView(DS.Skiclub.Levellist)
            _groupLeaderListCollectionView = New CollectionView(DS.Skiclub.Instructorlist)
            _participantMemberOfGroupListCollectionView = New CollectionView(DS.Skiclub.Grouplist)
            _participantLevelListCollectionView = New CollectionView(DS.Skiclub.Levellist)
        End If

        GroupLevelCombobox.ItemsSource = _groupLevelListCollectionView
        GroupLeaderCombobox.ItemsSource = _groupLeaderListCollectionView
        ParticipantLevelCombobox.ItemsSource = _participantLevelListCollectionView
        ParticipantMemberOfGroupCombobox.ItemsSource = _participantMemberOfGroupListCollectionView


        ' Uebersicht erstellen
        'CDS.Skiclub.Grouplist.ToList.ForEach(Sub(x) wrpSkikursübersicht.Children.Add(New VisibleSkikurs With {.DataContext = x}))

        SetView(DS.Skiclub.Participantlist)
        SetView(DS.Skiclub.Grouplist)
        SetView(DS.Skiclub.Instructorlist)
        SetView(DS.Skiclub.Levellist)
    End Sub

    Private Sub SetView(Teilnehmers As ParticipantCollection)
        _teilnehmerListCollectionView = New ListCollectionView(Teilnehmers)
        ' Hinweis AddHandler Seite 764
        AddHandler _teilnehmerListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemTeilnehmer.DataContext = _teilnehmerListCollectionView
    End Sub
    Private Sub SetView(Skikursliste As GroupCollection)
        _skikursListCollectionView = New ListCollectionView(Skikursliste)
        ' Hinweis AddHandler Seite 764
        AddHandler _skikursListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemSkikurse.DataContext = _skikursListCollectionView

    End Sub

    Private Sub SetView(Skilehrer As InstructorCollection)
        _uebungsleiterListCollectionView = New ListCollectionView(Skilehrer)
        ' Hinweis AddHandler Seite 764
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemUebungsleiter.DataContext = _uebungsleiterListCollectionView
    End Sub

    Private Sub SetView(Level As LevelCollection)
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

    Private Sub tabitemSkikurs_GotFocus(sender As Object, e As RoutedEventArgs)
        _schalterLayerDetails = layerSkikursdetails
        _schalterBtnShowEplorer = btnShowSkikursExplorer
        _schalterPinImage = pinSkikursImage
        _schalterLayerListe = layerSkikursliste
        _schalterLayerListeTransform = layerSkikurslisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerSkikurslisteDetails
        _schalterBtnPinit = btnSkikursPinIt
    End Sub
    Private Sub tabitemSkikursuebersicht_GotFocus(sender As Object, e As RoutedEventArgs)
        '_schalterLayerDetails = layerSkikursdetails
        '_schalterBtnShowEplorer = btnShowSkikursExplorer
        '_schalterPinImage = pinSkikursImage
        '_schalterLayerListe = layerSkikursliste
        '_schalterLayerListeTransform = layerSkikurslisteTrans
        '_schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerSkikurslisteDetails
        '_schalterBtnPinit = btnSkikursPinIt
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

    Private Function GetListAsFixedDocument(pageSize As Size, pageMargin As Thickness) As FixedDocument

        ' ein paar Variablen setzen
        Dim printFriendHeight As Double = 1000 ' Breite einer Gruppe
        Dim printFriendWidth As Double = 700 '  Höhe einer Gruppe

        ' ermitteln der tatsächlich verfügbaren Seitengrösse
        Dim availablePageHeight As Double = pageSize.Height - pageMargin.Top - pageMargin.Bottom
        Dim availablePageWidth As Double = pageSize.Width - pageMargin.Left - pageMargin.Right

        ' ermitteln der Anzahl Spalten und Zeilen
        Dim rowsPerPage As Integer = CType(Math.Floor(availablePageHeight / printFriendHeight), Integer)
        Dim columnsPerPage As Integer = CType(Math.Floor(availablePageWidth / printFriendWidth), Integer)

        ' mindestens eine Zeile und Spalte verwenden, damit beim späteren Loop keine Endlos-Schleife entsteht
        If rowsPerPage = 0 Then rowsPerPage = 1
        If columnsPerPage = 0 Then columnsPerPage = 1

        Dim friendsPerPage As Integer = rowsPerPage * columnsPerPage


        ' ermitteln der vertikalen und horizontalen Abstände zwischen Freunden
        Dim vMarginBetweenFriends As Double = 0
        If rowsPerPage > 1 Then
            Dim vLeftOverSpace As Double = availablePageHeight - (printFriendHeight * rowsPerPage)
            vMarginBetweenFriends = vLeftOverSpace / (rowsPerPage - 1)
        End If

        Dim hMarginBetweenFriends As Double = 0
        If columnsPerPage > 1 Then
            Dim hLeftOverSpace As Double = availablePageWidth - (printFriendWidth * columnsPerPage)
            hMarginBetweenFriends = hLeftOverSpace / (columnsPerPage - 1)
        End If

        ' das eigentliche Erstellen des FixDocuments starten
        Dim doc = New FixedDocument()
        doc.DocumentPaginator.PageSize = pageSize

        ' Objekte in der Skischule neu lesen, falls etwas geändert wurde
        DS.Skiclub = DS.Skiclub.GetAktualisierungen()

        ' nach AngezeigterName sortierte Liste verwenden
        Dim sortedView As ListCollectionView = New ListCollectionView(DS.Skiclub.Grouplist)
        sortedView.SortDescriptions.Add(New SortDescription("AngezeigterName", ListSortDirection.Ascending))

        Dim skikursgruppe As Group = Nothing
        Dim page As FixedPage = Nothing

        ' durch die Gruppen loopen und Seiten generieren
        For i As Integer = 0 To sortedView.Count - 1
            sortedView.MoveCurrentToPosition(i)
            skikursgruppe = CType(sortedView.CurrentItem, Group)

            If i Mod friendsPerPage = 0 Then
                If page IsNot Nothing Then
                    Dim content As PageContent = New PageContent()
                    TryCast(content, IAddChild).AddChild(page)
                    doc.Pages.Add(content)
                End If
                page = New FixedPage
            End If

            ' PrintableFriend-Control mit Friend-Objekt initialisieren und zur Page hinzufügen
            Dim pSkikursgruppe As PrintableSkikursgruppe = New PrintableSkikursgruppe
            pSkikursgruppe.Height = printFriendHeight
            pSkikursgruppe.Width = printFriendWidth

            pSkikursgruppe.InitPropsFromSkikursgruppe(skikursgruppe, DS.Skiclub.Instructorlist)
            Dim currentRow As Integer = (i Mod friendsPerPage) / columnsPerPage
            Dim currentColumn As Integer = i Mod columnsPerPage

            FixedPage.SetTop(pSkikursgruppe, pageMargin.Top + ((pSkikursgruppe.Height + vMarginBetweenFriends) * currentRow))
            FixedPage.SetLeft(pSkikursgruppe, pageMargin.Left + ((pSkikursgruppe.Width + hMarginBetweenFriends) * currentColumn))
            page.Children.Add(pSkikursgruppe)
        Next

        ' letzte Page zum Dokument hinzufügen, falls diese Kinder hat
        If page.Children.Count > 0 Then
            Dim Content As PageContent = New PageContent()
            TryCast(Content, IAddChild).AddChild(page)
            doc.Pages.Add(Content)
        End If

        Return doc

    End Function

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        For i = 0 To DS.Skiclub.Grouplist.Count - 1
            DS.Skiclub.Grouplist(i).Groupleader = DS.Skiclub.Instructorlist.Item(i)
        Next

    End Sub

#End Region

End Class
