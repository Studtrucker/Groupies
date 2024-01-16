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
Imports CDS = Groupies.Services.CurrentDataService

Public Class Window1

#Region "Fields"
    Private _participantListCollectionView As ICollectionView
    Private _groupListCollectionView As ICollectionView
    Private _intructorListCollectionView As ICollectionView

    Private Property _mRuSortedList As SortedList(Of Integer, String)

#End Region

#Region "Constructor"

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ' DataContext Window
        _groupListCollectionView = New ListCollectionView(New GroupCollection)
        ' DataContext participantDataGrid
        _participantListCollectionView = New ListCollectionView(New ParticipantCollection)
        ' DataContext groupleaderDataGrid
        _intructorListCollectionView = New ListCollectionView(New InstructorCollection)

    End Sub

#End Region

#Region "Window-Events"
    Private Sub Window1_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden

        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleNewExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleClubOpenExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleClubSaveExecuted, AddressOf HandleClubSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleClubSaveAsExecuted, AddressOf HandleClubSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleClubPrintExecuted, AddressOf HandleClubPrintCanExecute))

        ' 2. SortedList für meist genutzte Skischulen (Most Recently Used) initialisieren
        _mRuSortedList = New SortedList(Of Integer, String)

        ' 3. SortedList für meist genutzte Skischulen befüllen
        LoadmRUSortedListMenu()

        ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
        If (Environment.GetCommandLineArgs().Length = 2) Then
            Dim args = Environment.GetCommandLineArgs
            CDS.SkiclubFileName = args(1)
            Services.StartService.OpenSkischule(CDS.SkiclubFileName)
        Else
            Services.StartService.LoadLastSkischule()
        End If


        If CDS.Skiclub IsNot Nothing Then
            Title = String.Format("Groupies - {0}", CDS.SkiclubFileName)
            setView(CDS.Skiclub)
        End If

    End Sub

    Private Sub HandleMainWindowClosed(sender As Object, e As EventArgs)
        ' 1. Den Pfad der letzen Liste ins IsolatedStorage speichern.
        If StartService.SkiclubListFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(StartService.SkiclubListFile.FullName)
                    End Using
                End Using
            End Using
        End If

        ' 2. Die meist genutzen Listen ins Isolated Storage speichern
        If _mRuSortedList.Count > 0 Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("mRUSortedList", FileMode.OpenOrCreate, iso)
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

#Region "EventHandler der CommandBindings"

    Private Sub HandleNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If CDS.Skiclub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        ' Skischulobjekt löschen
        CDS.Skiclub = Nothing
        ' Alle DataContexte löschen
        DataContext = Nothing
        ParticipantDataGrid.DataContext = Nothing
        InstuctorDataGrid.DataContext = Nothing

        ' Neues Skischulobjekt initialisieren
        Title = "Groupies"

        CurrentDataService.CreateNewSkiclub()

        setView(CDS.Skiclub)

    End Sub

    Private Sub HandleCloseExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
    End Sub

    Private Sub HandleClubOpenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        OpenFile()
    End Sub

    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        OpenSkischule(TryCast(sender, MenuItem).Header.ToString())
    End Sub

    Private Sub HandleClubSaveCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = CDS.Skiclub IsNot Nothing
    End Sub

    Private Sub HandleClubSaveAsExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If StartService.SkiclubListFile IsNot Nothing Then
            dlg.FileName = StartService.SkiclubListFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveSkischule(dlg.FileName)
            StartService.SkiclubListFile = New FileInfo(dlg.FileName)
        End If
    End Sub

    Private Sub HandleClubSaveExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        If StartService.SkiclubListFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveSkischule(CDS.SkiclubFileName)
        End If
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

#End Region

#Region "Helper-Methoden"

    Public Sub LoadmRUSortedListMenu()
        Try
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("mRUSortedList", System.IO.FileMode.Open, iso)
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

    Private Sub OpenFile()

        StartService.OpenFile()

        setView(Services.Skiclub)
        Title = "Groupies - " & CDS.SkiclubFileName
    End Sub

    Private Sub OpenSkischule(fileName As String)

        'StartService.OpenSkischule(fileName)

        'setView(Services.Skiclub)
        'Title = "Groupies - " & CDS.SkiclubFileName

    End Sub

    Private Sub SaveSkischule(fileName As String)
        ' 1. Skischule serialisieren und gezippt abspeichern
        SaveXML(fileName)
        'SaveZIP(fileName)
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Groupies - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Groupies gespeichert!")
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Skiclub))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, CDS.Skiclub)
        End Using
    End Sub

    Private Sub QueueMostRecentFilename(fileName As String)
        QueueMostRecentFilename(fileName)
        RefreshMostRecentMenu()
    End Sub

    Private Sub RefreshMostRecentMenu()
        mostrecentlyUsedMenuItem.Items.Clear()
        For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
            Dim mi = New MenuItem With {.Header = _mRuSortedList.Values(i)}
            AddHandler mi.Click, AddressOf HandleMostRecentClick
            mostrecentlyUsedMenuItem.Items.Add(mi)
        Next
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

        For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
            Dim jumpPath = New JumpPath With {
                    .CustomCategory = "Zuletzt geöffnet",
                    .Path = _mRuSortedList.Values(i)}

            jumplist.JumpItems.Add(jumpPath)
        Next

        JumpList.SetJumpList(Application.Current, jumplist)

    End Sub

    Private Sub setView(Skikursliste As Skiclub)
        ' Neue ListCollectionView laden
        _groupListCollectionView = New ListCollectionView(CDS.Skiclub.Grouplist)
        If _groupListCollectionView.CanSort Then
            _groupListCollectionView.SortDescriptions.Add(New SortDescription("GroupSort", ListSortDirection.Ascending))
        End If
        DataContext = _groupListCollectionView

        _participantListCollectionView = New ListCollectionView(CDS.Skiclub.ParticipantsToDistribute)
        ParticipantDataGrid.DataContext = _participantListCollectionView

        _intructorListCollectionView = New ListCollectionView(CDS.Skiclub.Instructorlist)
        InstuctorDataGrid.DataContext = _intructorListCollectionView

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
        CDS.Skiclub = CDS.Skiclub.GetAktualisierungen()

        '_Skiclub.Grouplist.ToList.ForEach(Sub(GL) GL.GroupMembers.ToList.Sort(Function(P1, P2) P1.ParticipantFullName.CompareTo(P2.ParticipantFullName)))
        ' nach AngezeigterName sortierte Liste verwenden
        Dim sortedGroupView = New ListCollectionView(CDS.Skiclub.Grouplist)
        sortedGroupView.SortDescriptions.Add(New SortDescription("GroupNaming", ListSortDirection.Ascending))

        Dim skikursgruppe As Group
        Dim page As FixedPage = Nothing

        ' durch die Gruppen loopen und Seiten generieren
        For i As Integer = 0 To sortedGroupView.Count - 1
            sortedGroupView.MoveCurrentToPosition(i)
            skikursgruppe = CType(sortedGroupView.CurrentItem, Group)

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


#End Region

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)

    End Sub

    Enum Printversion
        Instructor
        Participant
    End Enum


End Class
