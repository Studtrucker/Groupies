Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Markup
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports Groupies.Commands
Imports Groupies.Entities
Imports Groupies.Interfaces
Imports Groupies.MainWindow
Imports Groupies.Services
Imports Groupies.UserControls
Imports Microsoft.Win32
Imports AppCon = Groupies.Controller.AppController

Public Class Window1

#Region "Fields"

    Private _participantListCollectionView As ICollectionView
    Private _groupListCollectionView As ICollectionView
    Private _instructorListCollectionView As ICollectionView
    Private _groupiesFile As FileInfo
    Private _mRuSortedList As SortedList(Of Integer, String)

#End Region

#Region "Constructor"

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ' DataContext Window
        _groupListCollectionView = New ListCollectionView(New GruppeCollection)
        ' DataContext participantDataGrid
        _participantListCollectionView = New ListCollectionView(New TeilnehmerCollection)
        ' DataContext groupleaderDataGrid
        _instructorListCollectionView = New ListCollectionView(New TrainerCollection)

    End Sub

#End Region

#Region "Window-Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs)

        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden

        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleNewExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleClubOpenExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleClubSaveExecuted, AddressOf HandleClubSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleClubSaveAsExecuted, AddressOf HandleClubSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleClubPrintExecuted, AddressOf HandleClubPrintCanExecute))

        ' Neue Version
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerInGruppeEinteilen, AddressOf Handle_TeilnehmerInGruppeEinteilen_Execute, AddressOf Handle_TeilnehmerInGruppeEinteilen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerAusGruppeEntfernen, AddressOf Handle_TeilnehmerAusGruppeEntfernen_Execute, AddressOf Handle_TeilnehmerAusGruppeEntfernen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppeEinenTrainerZuweisen, AddressOf Handle_GruppeEinenTrainerZuweisen_Execute, AddressOf Handle_GruppeEinenTrainerZuweisen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppentrainerEntfernen, AddressOf Handle_GruppentrainerEntfernen_Execute, AddressOf Handle_GruppentrainerEntfernen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportTeilnehmer, AddressOf HandleImportTeilnehmerExecute, AddressOf HandleImportTeilnehmerCanExecute))


        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportSkiclub, AddressOf HandleImportSkiclubExecuted, AddressOf HandleImportSkiclubCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.ImportInstructors, AddressOf HandleImportInstructorsExecuted, AddressOf HandleImportInstructorsCanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.NeuerUebungsleiter, AddressOf HandleNewInstructorExecuted, AddressOf HandleNewInstructorCanExecuted))


        ' 2. SortedList für meist genutzte Skischulen (Most Recently Used) initialisieren
        _mRuSortedList = New SortedList(Of Integer, String)

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


        RefreshJumpListInWinTaskbar()


    End Sub

    Private Sub HandleMainWindowClosing(sender As Object, e As CancelEventArgs)
        Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schliessen?", "Achtung", MessageBoxButton.YesNo)
        e.Cancel = result = MessageBoxResult.No
    End Sub

    Private Sub HandleMainWindowClosed(sender As Object, e As EventArgs)

        ' 1. Den Pfad der letzen Liste ins IsolatedStorage speichern.
        If _groupiesFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(_groupiesFile.FullName)
                    End Using
                End Using
            End Using
        End If

        ' 2. Die meist genutzen Listen ins Isolated Storage speichern
        If _mRuSortedList.Count > 0 Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("_mRUSortedList", FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        For Each kvp As KeyValuePair(Of Integer, String) In _mRuSortedList
                            writer.WriteLine(kvp.Key.ToString() & ";" & kvp.Value)
                        Next
                    End Using
                End Using
            End Using
        End If

    End Sub

#End Region

#Region "Methoden zum Laden der meist genutzten Groupies und der letzten Groupies Datei"
    Public Sub LoadmRUSortedListMenu()
        Try
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("LastGroupies", System.IO.FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        Dim i = 0
                        While reader.Peek <> -1
                            Dim line = reader.ReadLine().Split(";")
                            If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not _mRuSortedList.ContainsKey(Integer.Parse(line(0))) Then
                                If File.Exists(line(1)) Then
                                    i += 1
                                    _mRuSortedList.Add(i, line(1))
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

    Private Sub LoadLastSkischule()
        ' Die letze Skischule aus dem IsolatedStorage holen.
        Try
            Dim x = ""
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        x = reader.ReadLine
                    End Using
                End Using

            End Using
            If File.Exists(x) Then OpenSkischule(x)
        Catch ex As FileNotFoundException
        End Try
    End Sub

#End Region

#Region "Methoden zum Pinnen und Ein-/Ausblenden der Explorer"

#End Region

#Region "EventHandler der CommandBindings"

    Private Sub HandleNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If AppCon.CurrentClub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        AppCon.CurrentClub = Nothing
        ' Alle DataContexte löschen
        DataContext = Nothing
        ParticipantDataGrid.DataContext = Nothing
        InstuctorDataGrid.DataContext = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Groupies"

        AppCon.NeuenClubErstellen("Club")

        setView(AppCon.CurrentClub)

    End Sub

    Private Sub HandleClubOpenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        If dlg.ShowDialog = True Then
            unsetView()
            OpenSkischule(dlg.FileName)
        End If
    End Sub

    Private Sub HandleClubSaveExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        If _groupiesFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveSkischule(_groupiesFile.FullName)
        End If
    End Sub

    Private Sub HandleClubSaveCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing
    End Sub

    Private Sub HandleClubSaveAsExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If _groupiesFile IsNot Nothing Then
            dlg.FileName = _groupiesFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveSkischule(dlg.FileName)
            _groupiesFile = New FileInfo(dlg.FileName)
        End If
    End Sub

    Private Sub HandleCloseExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
    End Sub

    Private Sub HandleHelpExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Throw New NotImplementedException
    End Sub

    Private Sub HandleClubPrintExecuted(sender As Object, e As ExecutedRoutedEventArgs)

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

    Private Sub HandleClubPrintCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        ' Todo: Can execute festlegen
        e.CanExecute = True '_skischule IsNot Nothing OrElse _skischule.Skikursgruppenliste IsNot Nothing OrElse _skischule.Skikursgruppenliste.Count > 0
    End Sub

    Private Sub HandleImportTeilnehmerExecute(sender As Object, e As ExecutedRoutedEventArgs)

        ImportService.ImportTeilnehmer()
        'Dim ImportParticipants = ImportService.ImportParticipants
        'If ImportParticipants IsNot Nothing Then
        '    'DataService.Skiclub.Participantlist.ToList.AddRange(ImportParticipants)
        '    ImportParticipants.ToList.ForEach(Sub(x) Services.Club.GruppenloseTeilnehmer.Add(x))
        '    MessageBox.Show(String.Format("Es wurden {0} Teilnehmer erfolgreich importiert", ImportParticipants.Count))
        '    setView(AppCon.CurrentClub)
        'End If

    End Sub

    Private Sub HandleImportTeilnehmerCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing AndAlso AppCon.CurrentClub.GruppenloseTeilnehmer IsNot Nothing
    End Sub

    Private Sub HandleImportInstructorsExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        Dim ImportInstructors = ImportService.ImportInstructors
        If ImportInstructors IsNot Nothing Then
            'DataService.Skiclub.Participantlist.ToList.AddRange(ImportParticipants)
            ImportInstructors.ToList.ForEach(Sub(x) Services.Club.GruppenloseTrainer.Add(x))
            MessageBox.Show(String.Format("Es wurden {0} Skilehrer erfolgreich importiert", ImportInstructors.Count))
            setView(AppCon.CurrentClub)
        End If

    End Sub

    Private Sub HandleImportInstructorsCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Services.Club IsNot Nothing AndAlso Services.Club.GruppenloseTrainer IsNot Nothing
    End Sub

    Private Sub HandleImportSkiclubExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If AppCon.CurrentClub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie das aktuelle Groupies noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        AppCon.CurrentClub = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Groupies"

        Dim importSkiclub = ImportService.ImportSkiclub
        If importSkiclub IsNot Nothing Then
            setView(importSkiclub)
            MessageBox.Show(String.Format("Daten aus {0} erfolgreich importiert", ImportService.Workbook.Name))
        End If

    End Sub

    Private Sub HandleImportSkiclubCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub HandleNewInstructorExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NewInstructorDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            AppCon.CurrentClub.GruppenloseTrainer.Add(dlg.Instructor)
            '_uebungsleiterListCollectionView.MoveCurrentTo(dlg.Instructor)
            'uebungsleiterDataGrid.ScrollIntoView(dlg.Instructor)
        End If
    End Sub

    Private Sub HandleNewInstructorCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        'e.CanExecute = tabitemUebungsleiter.IsSelected And uebungsleiterDataGrid.SelectedItems.Count > 0
        e.CanExecute = True
    End Sub

    Private Sub Handle_GruppeEinenTrainerZuweisen_Execute(sender As Object, e As ExecutedRoutedEventArgs)

    End Sub

    Private Sub Handle_GruppeEinenTrainerZuweisen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub.GruppenloseTeilnehmer.Count > 0
    End Sub

    Private Sub Handle_GruppentrainerEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)

    End Sub

    Private Sub Handle_GruppentrainerEntfernen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        ' Todo: Regel einbauen: Die aktuelle Gruppe muß mehr als 0 Mitglieder haben und mindestens ein Mitglied markiert sein
        e.CanExecute = True
    End Sub

    Private Sub Handle_TeilnehmerInGruppeEinteilen_Execute(sender As Object, e As ExecutedRoutedEventArgs)

    End Sub

    Private Sub Handle_TeilnehmerInGruppeEinteilen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub.GruppenloseTeilnehmer.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerAusGruppeEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)

    End Sub

    Private Sub Handle_TeilnehmerAusGruppeEntfernen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        ' Todo: Regel einbauen: Die aktuelle Gruppe muß mehr als 0 Mitglieder haben und mindestens ein Mitglied markiert sein
        e.CanExecute = True
    End Sub

    ' Weitere Handles für die Commands

#End Region

#Region "Sonstige Eventhandler"

    ' Handles für Drag and Drop

    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        OpenSkischule(TryCast(sender, MenuItem).Header.ToString())
    End Sub

    Sub GroupiesCollectionView_CurrentChanged(sender As Object, e As EventArgs)
        Throw New NotImplementedException
    End Sub

    Private Sub RefreshTaskBarItemOverlay()
        Throw New NotImplementedException
    End Sub

    Private Sub HandleMenuOptionsClick(sender As Object, e As RoutedEventArgs)
        Throw New NotImplementedException
    End Sub

#End Region

#Region "Helper-Methoden"



    Private Sub OpenSkischule(fileName As String)

        If _groupiesFile IsNot Nothing AndAlso fileName.Equals(_groupiesFile.FullName) Then
            MessageBox.Show("Groupies " & fileName & " ist bereits geöffnet")
            Exit Sub
        End If

        If Not File.Exists(fileName) Then
            MessageBox.Show("Die Datei existiert nicht")
            Exit Sub
        End If

        Dim loadedClub = OpenXML(fileName)
        Dim loadedSkiclub = New Veraltert.Skiclub
        If loadedClub Is Nothing Then
            loadedSkiclub = OpenAltesXML(fileName)
            If loadedSkiclub Is Nothing Then
                Exit Sub
            End If
        End If

        _groupiesFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)

        AppCon.CurrentClub = Nothing

        ' Eintrag in CurrentDataService
        If loadedClub Is Nothing Then
            AppCon.CurrentClub = MapSkiClub2Club(loadedSkiclub)
        Else
            AppCon.CurrentClub = loadedClub
        End If

        setView(AppCon.CurrentClub)

        Title = "Groupies - " & fileName

    End Sub

    Private Function OpenAltesXML(fileName As String) As Veraltert.Skiclub

        Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
        Dim loadedSkiclub As Veraltert.Skiclub = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(fs), Veraltert.Skiclub)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedSkiclub

    End Function

    Private Function OpenXML(fileName As String) As Club

        Dim serializer = New XmlSerializer(GetType(Club))
        Dim loadedClub As Club = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                ' Todo: Doppelte Teilnehmer und Skilehrer - siehe Mapping altes Format!
                loadedClub = TryCast(serializer.Deserialize(fs), Club)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedClub
    End Function


    Private Sub SaveSkischule(fileName As String)
        ' Ewige Liste schreiben

        'AppCon.CurrentClub.AlleTeilnehmer.ToList.ForEach(Sub(Tn) AppCon.CurrentClub.EwigeTeilnehmerliste.Add(Tn, Now.Date))

        ' 1. Skischule serialisieren und gezippt abspeichern
        SaveXML(fileName)
        'SaveZIP(fileName)
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Groupies - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Groupies gespeichert!")
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Club))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, AppCon.CurrentClub)
        End Using
    End Sub

    Private Sub QueueMostRecentFilename(fileName As String)

        Dim max As Integer = 0
        For Each i In _mRuSortedList.Keys
            If i > max Then max = i
        Next

        Dim keysToRemove As List(Of Integer) = New List(Of Integer)()
        For Each kvp In _mRuSortedList
            If kvp.Value.Equals(fileName) Then keysToRemove.Add(kvp.Key)
        Next
        For Each i In keysToRemove
            _mRuSortedList.Remove(i)
        Next

        _mRuSortedList.Add(max + 1, fileName)

        If _mRuSortedList.Count > 5 Then
            Dim min = Integer.MaxValue
            For Each i In _mRuSortedList.Keys
                If i < min Then min = i
            Next

            _mRuSortedList.Remove(min)
        End If

        RefreshMostRecentMenu()
    End Sub

    Private Sub RefreshMostRecentMenu()
        mostrecentlyUsedMenuItem.Items.Clear()

        RefreshMenuInApplication()
        RefreshJumpListInWinTaskbar()
    End Sub

    Private Sub RefreshMenuInApplication()

        For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
            Dim mi As MenuItem = New MenuItem()
            mi.Header = _mRuSortedList.Values(i)
            AddHandler mi.Click, AddressOf HandleMostRecentClick
            mostrecentlyUsedMenuItem.Items.Add(mi)
        Next

        If mostrecentlyUsedMenuItem.Items.Count = 0 Then
            Dim mi = New MenuItem With {.Header = "keine"}
            mostrecentlyUsedMenuItem.Items.Add(mi)
        End If
    End Sub

    Private Sub RefreshJumpListInWinTaskbar()
        'JumpList Klasse
        'Stellt eine Liste von Elementen und Aufgaben dar, die auf einer Windows 7-Taskleistenschaltfläche als Menü angezeigt werden.
        Dim jumplist = New JumpList With {
                .ShowFrequentCategory = False,
                .ShowRecentCategory = False}

        'JumpTask Klasse
        'Stellt eine Verknüpfung zu einer Anwendung in der Taskleisten-Sprungliste unter Windows 7 dar.
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

        For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
            Dim jumpPath = New JumpPath With {
                    .CustomCategory = "Zuletzt geöffnet",
                    .Path = _mRuSortedList.Values(i)}

            jumplist.JumpItems.Add(jumpPath)
        Next

        JumpList.SetJumpList(Application.Current, jumplist)

    End Sub

    Private Sub setView(Skikursliste As Club)

        ' Hier wird der DataContext gesetzt!

        unsetView()

        ' Neue ListCollectionView laden
        _groupListCollectionView = New ListCollectionView(AppCon.CurrentClub.Gruppenliste.GruppeGeordnet.ToList)
        If _groupListCollectionView.CanSort Then
            _groupListCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Descending))
        End If
        DataContext = _groupListCollectionView

        setView(AppCon.CurrentClub.GruppenloseTeilnehmer)
        setView(AppCon.CurrentClub.GruppenloseTrainer)
    End Sub

    Private Sub setView(FreieTeilnehmer As TeilnehmerCollection)
        _participantListCollectionView = New ListCollectionView(FreieTeilnehmer)
        If _participantListCollectionView.CanSort Then
            _participantListCollectionView.SortDescriptions.Add(New SortDescription("Leistungsstufe", ListSortDirection.Ascending))
            _participantListCollectionView.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
            _participantListCollectionView.SortDescriptions.Add(New SortDescription("Vorname", ListSortDirection.Ascending))
        End If
        ParticipantDataGrid.DataContext = _participantListCollectionView
    End Sub

    'Private Sub setView(FreieTeilnehmer As IEnumerable(Of Teilnehmer))
    '    _participantListCollectionView = New ListCollectionView(FreieTeilnehmer)
    '    ParticipantDataGrid.DataContext = _participantListCollectionView
    'End Sub

    Private Sub setView(FreieTrainer As TrainerCollection)
        _instructorListCollectionView = New ListCollectionView(FreieTrainer)
        InstuctorDataGrid.DataContext = _instructorListCollectionView
    End Sub

    Private Sub setView(FreieTrainer As IEnumerable(Of Trainer))
        _instructorListCollectionView = New ListCollectionView(FreieTrainer.ToList)
        InstuctorDataGrid.DataContext = _instructorListCollectionView
    End Sub

    Private Sub unsetView()

        DataContext = Nothing
        ParticipantDataGrid.DataContext = Nothing
        InstuctorDataGrid.DataContext = Nothing

        _groupListCollectionView = New ListCollectionView(New GruppeCollection)
        _participantListCollectionView = New ListCollectionView(New TeilnehmerCollection)
        _instructorListCollectionView = New ListCollectionView(New TrainerCollection)

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


        '_Skiclub.Grouplist.ToList.ForEach(Sub(GL) GL.GroupMembers.ToList.Sort(Function(P1, P2) P1.ParticipantFullName.CompareTo(P2.ParticipantFullName)))
        ' nach AngezeigterName sortierte Liste verwenden
        Dim sortedGroupView = New ListCollectionView(AppCon.CurrentClub.Gruppenliste)
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
                pSkikursgruppe = New TeilnehmerAusdruck
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


#End Region

#Region "Enum für Printversion"

    Enum Printversion
        Instructor
        Participant
    End Enum

    Private Sub HandleInstructorMenuItemClick(sender As Object, e As RoutedEventArgs)
        Dim InstructorWindow = New InstructorsWindow
        InstructorWindow.Show()
    End Sub

    Private Sub ParticipantDataGrid_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        For i = ParticipantDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppCon.CurrentClub.TeilnehmerInGruppeEinteilen(ParticipantDataGrid.SelectedItems.Item(i), DirectCast(GroupDataGrid.DataContext, ICollectionView).CurrentItem)
        Next
    End Sub

    Private Sub InstuctorDataGrid_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If DirectCast(DirectCast(GroupDataGrid.DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
            MessageBox.Show("Es muss zuerst der aktuelle Trainer aus der Gruppe entfernt werden")
            Exit Sub
        End If
        AppCon.CurrentClub.TrainerEinerGruppeZuweisen(InstuctorDataGrid.SelectedItems.Item(0), DirectCast(GroupDataGrid.DataContext, ICollectionView).CurrentItem)
    End Sub


#End Region

End Class
