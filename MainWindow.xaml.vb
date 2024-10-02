Imports System.Reflection
Imports System.ComponentModel
Imports System.IO
Imports System.IO.Compression
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports System.IO.IsolatedStorage
Imports System.Windows.Media.Animation
Imports System.Windows.Controls.Primitives
Imports System.Text
Imports System.Windows.Markup
Imports System.Linq
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports Microsoft.Win32
Imports Microsoft.Office.Core
Imports Groupies.UserControls
Imports Groupies.Services
Imports Groupies.Entities
Imports Groupies.Commands
Imports Groupies.Interfaces
Imports Groupies.Controller.AppController

Class MainWindow

#Region "Fields"

    'Private _Skiclub As Entities.Skiclub

    Private ReadOnly _dummySpalteFuerLayerTeilnehmerDetails As ColumnDefinition
    Private ReadOnly _dummySpalteFuerLayerSkikurslisteDetails As ColumnDefinition
    Private ReadOnly _dummySpalteFuerLayerUebungsleiterDetails As ColumnDefinition
    Private ReadOnly _dummySpalteFuerLayerLevelDetails As ColumnDefinition

    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _skikursListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _uebungsleiterListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _levelListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _groupLevelListCollectionView As ICollectionView
    Private _groupLeaderListCollectionView As ICollectionView
    Private _participantLevelListCollectionView As ICollectionView
    Private _participantMemberOfGroupListCollectionView As ICollectionView
    Private _participantsToDistributeListCollectionView As ICollectionView
    Private _participantsInGroupMemberListCollectionView As ICollectionView
    Private _participantListOverviewCollectionView As ICollectionView

    Private _skischuleListFile As FileInfo
    Private _mRUSortedList As SortedList(Of Integer, String)

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

        '_Skiclub = CDS.Skiclubs


        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ' Spalte initialisieren und in dieselbe Gruppe setzen,
        ' wie die Spalte mit dem Freunde Explorer im
        ' layer1-Grid
        _dummySpalteFuerLayerTeilnehmerDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinTeilnehmerSpalte"}
        _dummySpalteFuerLayerSkikurslisteDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinSkikursgruppenSpalte"}
        _dummySpalteFuerLayerUebungsleiterDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinUebungsleiterSpalte"}
        _dummySpalteFuerLayerLevelDetails = New ColumnDefinition() With {.SharedSizeGroup = "pinLevelSpalte"}

        ' das Grid gleich zu Beginn pinnen

        tabitemSkikursuebersicht.Visibility = Visibility.Visible
        'OverviewTabItem_GotFocus(Me, New RoutedEventArgs())

        layerTeilnehmerliste.Visibility = Visibility.Visible
        layerLevelliste.Visibility = Visibility.Visible
        layerSkikursdetails.Visibility = Visibility.Visible
        layerSkilehrerdetails.Visibility = Visibility.Visible

        _participantListOverviewCollectionView = New ListCollectionView(New TeilnehmerCollection)
        AddHandler _participantListOverviewCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _teilnehmerListCollectionView = New ListCollectionView(New TeilnehmerCollection())
        AddHandler _teilnehmerListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _skikursListCollectionView = New ListCollectionView(New GruppeCollection())
        AddHandler _skikursListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _uebungsleiterListCollectionView = New ListCollectionView(New TrainerCollection())
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)
        _levelListCollectionView = New ListCollectionView(New LeistungsstufeCollection())
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, New EventHandler(AddressOf _listCollectionView_CurrentChanged)

        _participantsToDistributeListCollectionView = New ListCollectionView(New TeilnehmerCollection())
        _participantsInGroupMemberListCollectionView = New ListCollectionView(New TeilnehmerCollection())

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
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Help, AddressOf HandleHelpExecuted))

        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportSkiclub, AddressOf HandleImportSkiclubExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportInstructors, AddressOf HandleImportInstructorsExecuted, AddressOf HandleImportInstructorsCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportParticipants, AddressOf HandleImportParticipantsExecuted, AddressOf HandleImportParticipantsCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.BeurteileTeilnehmerlevel, AddressOf HandleBeurteileTeilnehmerkoennenExecuted, AddressOf HandleBeurteileTeilnehmerkoennenCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NewParticipant, AddressOf HandleNewParticipantExecuted, AddressOf HandleNewParticipantCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerLoeschen, AddressOf HandleTeilnehmerLoeschenExecuted, AddressOf HandleTeilnehmerLoeschenCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NewInstructor, AddressOf HandleNeuerUebungsleiterExecuted, AddressOf HandleNeuerUebungsleiterCanExecuted))
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

        If CurrentClub IsNot Nothing Then
            SetView(CurrentClub)
        End If

        ' 5. JumpList in Windows Taskbar aktualisieren
        RefreshJumpListInWinTaskbar()

    End Sub

    Private Sub loadLastSkischule()

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

    'Private Sub LoadmRUSortedListMenu()
    '    Try
    '        Using iso = IsolatedStorageFile.GetUserStoreForAssembly
    '            Using stream = New IsolatedStorageFileStream("mRUSortedList", System.IO.FileMode.Open, iso)
    '                Using reader = New StreamReader(stream)
    '                    Dim i = 0
    '                    While reader.Peek <> -1
    '                        Dim line = reader.ReadLine().Split(";")
    '                        If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not _mRUSortedList.ContainsKey(Integer.Parse(line(0))) Then
    '                            If File.Exists(line(1)) Then
    '                                i += 1
    '                                _mRUSortedList.Add(i, line(1))
    '                            End If
    '                        End If
    '                    End While
    '                End Using
    '            End Using
    '        End Using
    '        RefreshMostRecentMenu()
    '    Catch ex As FileNotFoundException
    '        'Throw ex
    '    End Try

    'End Sub

    'Private Sub LoadLastSkischule()
    '    ' Die letze Skischule aus dem IsolatedStorage holen.
    '    Try
    '        Dim x = ""
    '        Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
    '            Using stream = New IsolatedStorageFileStream("LastSkischule", FileMode.Open, iso)
    '                Using reader = New StreamReader(stream)
    '                    x = reader.ReadLine
    '                End Using
    '            End Using

    '        End Using
    '        If File.Exists(x) Then Me.OpenSkischule(x)
    '    Catch ex As FileNotFoundException
    '    End Try
    'End Sub

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
        If CurrentClub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        CurrentClub = Nothing
        ' Neues Skischulobjekt initialisieren
        Title = "Groupies"
        Dim NeueSkischule = New Entities.Club("Club") With {.Leistungsstufenliste = PresetService.StandardLeistungsstufenErstellen()}
        Dim dlg = New CountOfGroupsDialog
        If dlg.ShowDialog Then
            NeueSkischule.Gruppenliste = PresetService.StandardGruppenErstellen(dlg.Count.Text)
        End If
        GroupOverviewWrapPanel.Children.Clear()
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
        e.CanExecute = CurrentClub IsNot Nothing
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
        e.CanExecute = Services.Club IsNot Nothing AndAlso Services.Club.Teilnehmerliste IsNot Nothing
    End Sub

    Private Sub HandleImportParticipantsExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim ImportParticipants = ImportService.ImportParticipants
        If ImportParticipants IsNot Nothing Then
            'DataService.Skiclub.Participantlist.ToList.AddRange(ImportParticipants)
            ImportParticipants.ToList.ForEach(Sub(x) Services.Club.Teilnehmerliste.Add(x))
            MessageBox.Show(String.Format("Es wurden {0} Teilnehmer erfolgreich importiert", ImportParticipants.Count))
            setView(CurrentClub.Gruppenliste)
        End If

    End Sub

    Private Sub HandleImportInstructorsCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Services.Club IsNot Nothing AndAlso Services.Club.Trainerliste IsNot Nothing
    End Sub

    Private Sub HandleImportInstructorsExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim ImportInstructors = ImportService.ImportInstructors
        If ImportInstructors IsNot Nothing Then
            'DataService.Skiclub.Participantlist.ToList.AddRange(ImportParticipants)
            ImportInstructors.ToList.ForEach(Sub(x) Services.Club.Trainerliste.Add(x))
            MessageBox.Show(String.Format("Es wurden {0} Skilehrer erfolgreich importiert", ImportInstructors.Count))
            setView(CurrentClub.Gruppenliste)
        End If

    End Sub

    Private Sub HandleImportSkiclubExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If CurrentClub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuellen Groupies noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        CurrentClub = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Groupies"

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
                    TryCast(_uebungsleiterListCollectionView.CurrentItem, Trainer).Foto = buffer
                    RefreshTaskBarItemOverlay()
                    CommandManager.InvalidateRequerySuggested()
                    validPictureFile = True
                End Using
            Else
                Dim sb = New StringBuilder()
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
            Dim doc As FixedDocument
            Dim printArea = New Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight)
            Dim pageMargin = New Thickness(30, 30, 30, 60)
            If e.Parameter = "InstructorInfo" Then
                doc = PrintoutInfo(Printversion.Instructor, printArea, pageMargin)
            Else
                doc = PrintoutInfo(Printversion.Participant, printArea, pageMargin)
            End If
            dlg.PrintDocument(doc.DocumentPaginator, e.Parameter)
        End If

    End Sub

    Private Sub HandleListPrintCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        ' Todo: Can execute festlegen
        e.CanExecute = True '_skischule IsNot Nothing OrElse _skischule.Skikursgruppenliste IsNot Nothing OrElse _skischule.Skikursgruppenliste.Count > 0
    End Sub



#Region "Level"

    Private Sub HandleNeuesLevelCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = CurrentClub IsNot Nothing
    End Sub

    Private Sub HandleNeuesLevelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewLevelDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        If dlg.ShowDialog = True Then
            CurrentClub.Leistungsstufenliste.Add(dlg.Level)
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
        For Each item As Leistungsstufe In levelDataGrid.SelectedItems
            RemoveLevelFromSkikursgruppe(item)
            RemoveLevelFromTeilnehmer(item)
            index(i) = CurrentClub.Leistungsstufenliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(CurrentClub.Leistungsstufenliste, index)
    End Sub

#End Region

#Region "Skikursgruppe"

    Private Sub HandleNeueSkikursgruppeCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = CurrentClub IsNot Nothing
    End Sub

    Private Sub HandleNeueSkikursgruppeExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewGroupDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        If dlg.ShowDialog = True Then
            CurrentClub.Gruppenliste.Add(dlg.Group)
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
        For Each item As Gruppe In skikurseDataGrid.SelectedItems
            RemoveSkikursgruppeFromTeilnehmer(item)
            index(i) = CurrentClub.Gruppenliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(CurrentClub.Gruppenliste, index)
    End Sub

#End Region

#Region "Teilnehmer"

    Private Sub HandleNewParticipantCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = CurrentClub IsNot Nothing
    End Sub

    Private Sub HandleNewParticipantExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New NewParticipantDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            CurrentClub.Teilnehmerliste.Add(dlg.Teilnehmer)
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
            index(i) = CurrentClub.Teilnehmerliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(CurrentClub.Teilnehmerliste, index)
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
        e.CanExecute = CurrentClub IsNot Nothing
    End Sub

    Private Sub HandleNeuerUebungsleiterExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewInstructorDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            CurrentClub.Trainerliste.Add(dlg.Instructor)
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
        For Each item As Trainer In uebungsleiterDataGrid.SelectedItems
            RemoveUebungsleiterFromSkikursgruppe(item)
            index(i) = CurrentClub.Trainerliste.IndexOf(item)
            i += 1
        Next

        RemoveItemsAt(CurrentClub.Trainerliste, index)

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

#Region "Methoden zum Laden der meist genutzten Listen und der letzten Freundesliste"

    Public Sub LoadmRUSortedListMenu()

        _mRUSortedList = New SortedList(Of Integer, String)
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
            Throw ex
        End Try
    End Sub

    Private Sub RefreshMostRecentMenu()

        mostrecentlyUsedMenuItem.Items.Clear()

        RefreshMenuInApplication()
        RefreshJumpListInWinTaskbar()
    End Sub

    Private Sub RefreshMenuInApplication()
        For i = _mRUSortedList.Values.Count - 1 To 0 Step -1
            Dim mi As MenuItem = New MenuItem With {.Header = _mRUSortedList.Values(i)}
            AddHandler mi.Click, AddressOf HandleMostRecentClick
            mostrecentlyUsedMenuItem.Items.Add(mi)
        Next

        If mostrecentlyUsedMenuItem.Items.Count = 0 Then
            Dim mi = New MenuItem With {.Header = "keine"}
            mostrecentlyUsedMenuItem.Items.Add(mi)
        End If
    End Sub

#End Region


    Private Sub AddLevelToTeilnehmer(Teilnehmerliste As TeilnehmerCollection, Level As Leistungsstufe)
        Teilnehmerliste.ToList.ForEach(Sub(x) x.Leistungsstand = Level)
    End Sub

    Private Sub AddLevelToSkikursgruppe(Skikursgruppenliste As GruppeCollection, Level As Leistungsstufe)
        Skikursgruppenliste.ToList.ForEach(Sub(x) x.Leistungsstufe = Level)
    End Sub

    Private Sub AddUebungsleiterToSkikursgruppe(Skikursgruppe As Gruppe, Uebungsleiter As Trainer)
        Skikursgruppe.Trainer = Uebungsleiter
    End Sub

    Private Sub AddSkikursgruppeToTeilnehmer(Teilnehmerliste As TeilnehmerCollection, Skikursgruppe As Gruppe)
        ' Todo: Zuweisung umschreiben
        'Teilnehmerliste.ToList.ForEach(Sub(x) x.MemberOfGroup = Skikursgruppe.GruppenID)
    End Sub

    Private Sub RemoveLevelFromTeilnehmer(level As Leistungsstufe)
        Dim liste = CurrentClub.Teilnehmerliste.TakeWhile(Function(x) x.Leistungsstand.LeistungsstufeID = level.LeistungsstufeID)
        liste.ToList.ForEach(Sub(x) x.Leistungsstand = Nothing)
    End Sub

    Private Sub RemoveLevelFromSkikursgruppe(level As Leistungsstufe)
        Dim liste = CurrentClub.Gruppenliste.TakeWhile(Function(x) x.Leistungsstufe Is level)
        liste.ToList.ForEach(Sub(x) x.Leistungsstufe = Nothing)
    End Sub

    Private Sub RemoveUebungsleiterFromSkikursgruppe(Uebungsleiter As Trainer)
        Dim liste = CurrentClub.Gruppenliste.TakeWhile(Function(x) x.Trainer Is Uebungsleiter)
        liste.ToList.ForEach(Sub(x) x.Trainer = Nothing)
    End Sub

    Private Sub RemoveSkikursgruppeFromTeilnehmer(Skikursgruppe As Gruppe)
        'Dim liste = CDS.Club.Teilnehmerliste.TakeWhile(Function(x) x.MemberOfGroup.Equals(Skikursgruppe.GruppenID))
        'liste.ToList.ForEach(Sub(x) x.MemberOfGroup = Nothing)
    End Sub

    Private Sub RemoveItemsAt(source As IList, ParamArray itemIndices As Integer())
        If source Is Nothing Then Throw New ArgumentNullException("Source")
        If itemIndices Is Nothing Then Throw New ArgumentNullException("itemIndices")
        For Each itemIndex In itemIndices.OrderByDescending(Function(x) x)
            source.RemoveAt(itemIndex)
        Next
    End Sub

    Private Sub OpenSkischule(fileName As String)

        'OpenSkischule(fileName)

        _skischuleListFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)
        SetView(Services.Club)
        Title = "Groupies - " & fileName

    End Sub

    Public Sub QueueMostRecentFilename(fileName As String)

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

    Private Function OpenXML(fileName As String) As Entities.Club
        Dim serializer = New XmlSerializer(GetType(Entities.Club))
        Dim loadedSkiclub As Entities.Club = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(fs), Entities.Club)
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedSkiclub
    End Function

    Private Function OpenZIP(fileName As String) As Entities.Club
        Dim serializer = New XmlSerializer(GetType(Entities.Club))
        Dim loadedSkischule As Entities.Club = Nothing

        ' Datei entzippen und deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Using zipStream = New GZipStream(fs, CompressionMode.Decompress)
                Try
                    loadedSkischule = TryCast(serializer.Deserialize(zipStream), Entities.Club)
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
        Title = "Groupies - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Groupies gespeichert!")
    End Sub

    Private Sub SaveZIP(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Club))
        Using fs = New FileStream(fileName, FileMode.Create)
            Using zipStream = New GZipStream(fs, CompressionMode.Compress)
                serializer.Serialize(zipStream, CurrentClub)
            End Using
        End Using
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Club))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, CurrentClub)
        End Using
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

    Private Sub SetView(Schule As Entities.Club)

        CurrentClub = Schule

        If CurrentClub IsNot Nothing Then
            _groupLevelListCollectionView = New ListCollectionView(CurrentClub.Leistungsstufenliste)
            _groupLeaderListCollectionView = New ListCollectionView(CurrentClub.Trainerliste)
            _participantMemberOfGroupListCollectionView = New ListCollectionView(CurrentClub.Gruppenliste)
            _participantLevelListCollectionView = New ListCollectionView(CurrentClub.Leistungsstufenliste)
        End If

        If _participantLevelListCollectionView.CanSort Then
            _participantLevelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
        End If

        GroupLevelComboBox.ItemsSource = _groupLevelListCollectionView
        GroupLeaderCombobox.ItemsSource = _groupLeaderListCollectionView
        ParticipantLevelComboBox.ItemsSource = _participantLevelListCollectionView
        CurrentClub.Gruppenliste.GruppenListeSortierungNachName.ToList.ForEach(Sub(x) GroupOverviewWrapPanel.Children.Add(New GroupView With {.DataContext = x}))

        setView(CurrentClub.Gruppenliste)
        SetView(CurrentClub.Teilnehmerliste)
        SetView(CurrentClub.Trainerliste)
        SetView(CurrentClub.Leistungsstufenliste)

    End Sub

    Private Sub setView(Skikursliste As GruppeCollection)

        ' Neue ListCollectionView laden
        _participantListOverviewCollectionView = New ListCollectionView(CurrentClub.Teilnehmerliste)
        ' ListCollectionView nach Teilnehmer in Gruppen filtern
        If _participantListOverviewCollectionView.CanFilter Then
            _participantListOverviewCollectionView.Filter = Function(x As Teilnehmer) x.IstGruppenmitglied = False
        End If

        ' ListCollectionView sortieren
        If _participantListOverviewCollectionView.CanSort Then
            _participantListOverviewCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFirstName", ListSortDirection.Ascending))
            _participantListOverviewCollectionView.SortDescriptions.Add(New SortDescription("ParticipantLastName", ListSortDirection.Ascending))
        End If
        participantlistOverviewDataGrid.ItemsSource = _participantListOverviewCollectionView
        'End If

        _skikursListCollectionView = New ListCollectionView(Skikursliste)
        ' Hinweis AddHandler Seite 764
        AddHandler _skikursListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemSkikurse.DataContext = _skikursListCollectionView
        If _skikursListCollectionView.CanSort Then
            _skikursListCollectionView.SortDescriptions.Add(New SortDescription("GroupNaming", ListSortDirection.Ascending))
        End If
        _skikursListCollectionView.MoveCurrentToFirst()

    End Sub

    Private Sub SetView(Teilnehmers As TeilnehmerCollection)
        _teilnehmerListCollectionView = New ListCollectionView(Teilnehmers)
        _teilnehmerListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFirstName", ListSortDirection.Ascending))
        _teilnehmerListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantLastName", ListSortDirection.Ascending))

        ' Hinweis AddHandler Seite 764
        AddHandler _teilnehmerListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemTeilnehmer.DataContext = _teilnehmerListCollectionView

    End Sub

    Private Sub SetView(Skilehrer As TrainerCollection)
        _uebungsleiterListCollectionView = New ListCollectionView(Skilehrer)
        ' Hinweis AddHandler Seite 764
        AddHandler _uebungsleiterListCollectionView.CurrentChanged, AddressOf _listCollectionView_CurrentChanged
        ' DataContext wird gesetzt
        ' Inhalt = CollectionView, diese kennt sein CurrentItem
        tabitemUebungsleiter.DataContext = _uebungsleiterListCollectionView
    End Sub

    Private Sub SetView(Level As LeistungsstufeCollection)
        _levelListCollectionView = New ListCollectionView(Level)
        '_levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Descending))
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

        btnTeilnehmerPinIt.IsChecked = True

    End Sub

    Private Sub tabitemSkikurs_GotFocus(sender As Object, e As RoutedEventArgs)

        _schalterLayerDetails = layerSkikursdetails
        _schalterBtnShowEplorer = btnShowSkikursExplorer
        _schalterPinImage = pinSkikursImage
        _schalterLayerListe = layerSkikursliste
        _schalterLayerListeTransform = layerSkikurslisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerSkikurslisteDetails
        _schalterBtnPinit = btnSkikursPinIt

        btnSkikursPinIt.IsChecked = True

    End Sub

    Private Sub OverviewTabItem_GotFocus(sender As Object, e As RoutedEventArgs)
        GroupOverviewWrapPanel.Children.Clear()
        CurrentClub.Gruppenliste.GruppenListeSortierungNachName.ToList.ForEach(Sub(x) GroupOverviewWrapPanel.Children.Add(New GroupView With {.DataContext = x}))
    End Sub

    Private Sub tabitemSkilehrer_GotFocus(sender As Object, e As RoutedEventArgs)

        _schalterLayerDetails = layerSkilehrerdetails
        _schalterBtnShowEplorer = btnShowSkilehrerExplorer
        _schalterPinImage = pinSkilehrerImage
        _schalterLayerListe = layerSkilehrerliste
        _schalterLayerListeTransform = layerSkilehrerlisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerUebungsleiterDetails
        _schalterBtnPinit = btnSkilehrerPinIt

        btnSkilehrerPinIt.IsChecked = True

    End Sub

    Private Sub tabitemLevel_GotFocus(sender As Object, e As RoutedEventArgs)

        _schalterLayerDetails = layerLeveldetails
        _schalterBtnShowEplorer = btnShowLevelExplorer
        _schalterPinImage = pinLevelImage
        _schalterLayerListe = layerLevelliste
        _schalterLayerListeTransform = layerLevellisteTrans
        _schalterDummySpalteFuerLayerDetails = _dummySpalteFuerLayerLevelDetails
        _schalterBtnPinit = btnLevelPinIt

        btnLevelPinIt.IsChecked = True

    End Sub

    Private Function PrintoutInfo(Printversion As Printversion, pageSize As Size, pageMargin As Thickness) As FixedDocument

        ' ein paar Variablen setzen
        Dim printFriendHeight As Double = 1000 ' Breite einer Gruppe
        Dim printFriendWidth As Double = 730 '  Höhe einer Gruppe

        ' ermitteln der tatsächlich verfügbaren Seitengrösse
        Dim availablePageHeight As Double = pageSize.Height - pageMargin.Top - pageMargin.Bottom
        Dim availablePageWidth As Double = pageSize.Width - pageMargin.Left - pageMargin.Right

        ' ermitteln der Anzahl Spalten und Zeilen
        Dim rowsPerPage As Integer = CType(Math.Floor(availablePageHeight / printFriendHeight), Integer)
        Dim columnsPerPage As Integer = CType(Math.Floor(availablePageWidth / printFriendWidth), Integer)

        ' mindestens eine Zeile und Spalte verwenden, damit beim späteren Loop keine Endlos-Schleife entsteht
        If rowsPerPage = 0 Then rowsPerPage = 1
        If columnsPerPage = 0 Then columnsPerPage = 1

        Dim participantsPerPage As Integer = rowsPerPage * columnsPerPage


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

        'Todo: Berechnen, wieviele Teilnehmer auf einer Seite gedruckt werden können
        Dim doc = New FixedDocument()
        doc.DocumentPaginator.PageSize = pageSize
        ' Objekte in der Skischule neu lesen, falls etwas geändert wurde
        CurrentClub = CurrentClub.GetAktualisierungen()

        '_Skiclub.Grouplist.ToList.ForEach(Sub(GL) GL.GroupMembers.ToList.Sort(Function(P1, P2) P1.ParticipantFullName.CompareTo(P2.ParticipantFullName)))
        ' nach AngezeigterName sortierte Liste verwenden
        Dim sortedGroupView = New ListCollectionView(CurrentClub.Gruppenliste)
        sortedGroupView.SortDescriptions.Add(New SortDescription("GroupNaming", ListSortDirection.Ascending))

        Dim skikursgruppe As Gruppe
        Dim page As FixedPage = Nothing

        ' durch die Gruppen loopen und Seiten generieren
        For i As Integer = 0 To sortedGroupView.Count - 1
            sortedGroupView.MoveCurrentToPosition(i)
            skikursgruppe = CType(sortedGroupView.CurrentItem, Gruppe)

            If i Mod participantsPerPage = 0 Then
                page = New FixedPage
                If page IsNot Nothing Then
                    Dim content = New PageContent()
                    TryCast(content, IAddChild).AddChild(page)
                    doc.Pages.Add(content)
                End If
            End If


            ' Printable-Control mit Group-Objekt initialisieren und zur Page hinzufügen
            Dim pSkikursgruppe As IPrintableNotice
            If Printversion = Printversion.Participant Then
                pSkikursgruppe = New PrintableNoticeForParticipants
            Else
                pSkikursgruppe = New PrintableNoticeForInstructors
            End If

            DirectCast(pSkikursgruppe, UserControl).Height = printFriendHeight
            DirectCast(pSkikursgruppe, UserControl).Width = printFriendWidth

            pSkikursgruppe.InitPropsFromGroup(skikursgruppe)
            Dim currentRow As Integer = (i Mod participantsPerPage) / columnsPerPage
            Dim currentColumn As Integer = i Mod columnsPerPage

            FixedPage.SetTop(pSkikursgruppe, pageMargin.Top + ((DirectCast(pSkikursgruppe, UserControl).Height + vMarginBetweenFriends) * currentRow))
            FixedPage.SetLeft(pSkikursgruppe, pageMargin.Left + ((DirectCast(pSkikursgruppe, UserControl).Width + hMarginBetweenFriends) * currentColumn))
            page.Children.Add(pSkikursgruppe)
        Next

        Return doc

    End Function

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        For i = 0 To CurrentClub.Gruppenliste.Count - 1
            CurrentClub.Gruppenliste(i).Trainer = CurrentClub.Trainerliste.Item(i)
        Next
    End Sub

    Private Sub AddParticipant(sender As Object, e As RoutedEventArgs)
        DirectCast(_skikursListCollectionView.CurrentItem, Gruppe).TeilnehmerHinzufuegen(_participantsToDistributeListCollectionView.CurrentItem)
        'DirectCast(_participantsToDistributeListCollectionView.CurrentItem, Teilnehmer).MemberOfGroup = DirectCast(_skikursListCollectionView.CurrentItem, Gruppe).GruppenID
        DirectCast(_participantsToDistributeListCollectionView.CurrentItem, Teilnehmer).IstGruppenmitglied = True
        setView(CurrentClub.Gruppenliste)
    End Sub

    Private Sub RemoveParticipant(sender As Object, e As RoutedEventArgs)
        If _participantsInGroupMemberListCollectionView.CurrentItem IsNot Nothing Then
            Dim tn = CurrentClub.Teilnehmerliste.Where(Function(x) x.TeilnehmerID = DirectCast(_participantsInGroupMemberListCollectionView.CurrentItem, Teilnehmer).TeilnehmerID).Single
            DirectCast(_skikursListCollectionView.CurrentItem, Gruppe).TeilnehmerEntfernen(_participantsInGroupMemberListCollectionView.CurrentItem)
            'tn.MemberOfGroup = Nothing
        End If
        setView(CurrentClub.Gruppenliste)
    End Sub


    ' Für das Verschieben von Objekten 
    Private Sub ParticipantsToDistributeDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)
        Dim Tn = TryCast(participantlistOverviewDataGrid.SelectedItem, Teilnehmer)

        If Tn IsNot Nothing Then
            Dim Data = New DataObject(GetType(Teilnehmer), Tn)
            DragDrop.DoDragDrop(participantlistOverviewDataGrid, Data, DragDropEffects.Move)
        End If

    End Sub



    ' Für den Empfang von Objekten 
    Private Sub ParticipantsToDistributeDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)
        Dim CorrectDataFormat = e.Data.GetDataPresent("Groupies.Entities.Participant")
        If CorrectDataFormat Then
            'Dim TN As Teilnehmer = e.Data.GetData("Groupies.Entities.Participant")
            ''For Each Participant As Participant In ic
            'TN.RemoveFromGroup()
            'CDS.Club.Teilnehmerliste.Remove(CDS.Club.Teilnehmerliste.Where(Function(x) x.TeilnehmerID.Equals(TN.TeilnehmerID)).First)
            'CDS.Club.Teilnehmerliste.Add(TN)
            'Next
        End If
    End Sub

#End Region

    Enum Printversion
        Instructor
        Participant
    End Enum

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)
        Dim win = New Window1
        win.ShowDialog()
    End Sub
End Class
