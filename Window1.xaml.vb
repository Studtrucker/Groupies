Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports Groupies.Commands
Imports Groupies.Entities
Imports Groupies.Services
Imports Microsoft.Win32
Imports CDS = Groupies.Services.CurrentDataService

Public Class Window1

#Region "Fields"
    Private _participantListCollectionView As ICollectionView
    Private _groupListCollectionView As ICollectionView
    Private _intructorListCollectionView As ICollectionView

    Private _mRUSortedList As SortedList(Of Integer, String)
    Private _skischuleListFile As FileInfo


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
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleListSaveExecuted, AddressOf HandleListSaveCanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleListSaveAsExecuted, AddressOf HandleListSaveCanExecute))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))

        ' 2. SortedList für meist genutzte Skischulen (Most Recently Used) initialisieren
        _mRUSortedList = New SortedList(Of Integer, String)

        ' 3. SortedList für meist genutzte Skischulen befüllen
        Services.StartService.LoadmRUSortedListMenu()

        ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
        If (Environment.GetCommandLineArgs().Length = 2) Then
            Dim args = Environment.GetCommandLineArgs
            CDS.SkiclubFileName = args(1)
            Services.StartService.OpenSkischule(CDS.SkiclubFileName)
        Else
            Services.StartService.LoadLastSkischule()
        End If

        Title = String.Format("Groupies - {0}", CDS.SkiclubFileName)
        setView(CDS.Skiclub)

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
        e.CanExecute = CDS.Skiclub IsNot Nothing
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

#End Region

#Region "Helper-Methoden"
    Private Sub OpenSkischule(fileName As String)


        '_skischuleListFile = New FileInfo(fileName)
        'QueueMostRecentFilename(fileName)
        setView(Services.Skiclub)
        Title = "Groupies - " & fileName

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

        For i = Services.StartService.RuSortedList.Values.Count - 1 To 0 Step -1
            Dim jumpPath = New JumpPath With {
                .CustomCategory = "Zuletzt geöffnet",
                .Path = _mRUSortedList.Values(i)}

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
#End Region

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)

    End Sub


End Class
