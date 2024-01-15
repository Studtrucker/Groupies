Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports Groupies.Commands
Imports Groupies.Entities
Imports Groupies.Services
Imports CDS = Groupies.Services.CurrentDataService

Public Class Window1

#Region "Fields"
    Private _participantListCollectionView As ICollectionView
    Private _groupListCollectionView As ICollectionView
    Private _intructorListCollectionView As ICollectionView

    Private _mRUSortedList As SortedList(Of Integer, String)

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

        CurrentDataService.CreateNewSkiclub

        setView(CDS.Skiclub)

    End Sub
    Private Sub HandleCloseExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
    End Sub

#End Region

#Region "Helper-Methoden"
    Private Sub OpenSkischule(fileName As String)


        '_skischuleListFile = New FileInfo(fileName)
        'QueueMostRecentFilename(fileName)
        setView(Services.Skiclub)
        Title = "Groupies - " & fileName

    End Sub

    Private Sub setView(Skikursliste As Skiclub)
        ' Neue ListCollectionView laden
        _groupListCollectionView = New ListCollectionView(CDS.Skiclub.Grouplist)
        If _groupListCollectionView.CanSort Then
            _groupListCollectionView.SortDescriptions.Add(New SortDescription("GroupSort", ListSortDirection.Ascending))
        End If
        DataContext = _groupListCollectionView

        _participantListCollectionView = New ListCollectionView(CDS.Skiclub.ParticipantsToDistribute)
        participantDataGrid.DataContext = _participantListCollectionView

        _intructorListCollectionView = New ListCollectionView(CDS.Skiclub.Instructorlist)
        instuctorDataGrid.DataContext = _intructorListCollectionView

    End Sub
#End Region

    Private Sub MenuItem_Click_1(sender As Object, e As RoutedEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)

    End Sub

    Private Sub ParticipantsToDistributeDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)

    End Sub


End Class
