Imports System.ComponentModel
Imports Skiclub.Entities

Public Class GroupView
    Private _group As Group
    Private _levelListCollectionView As ICollectionView
    Private _instructorListCollectionView As ICollectionView
    Private _groupmemberListCollectionView As ICollectionView

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _levelListCollectionView = New ListCollectionView(New LevelCollection())
        _instructorListCollectionView = New ListCollectionView(New InstructorCollection())
        _groupmemberListCollectionView = New ListCollectionView(New ParticipantCollection())

    End Sub

    Private Sub GroupView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        SetView()
    End Sub
    'Private Sub GroupView_GotFocus(sender As Object, e As RoutedEventArgs) Handles Me.GotFocus
    '    SetView()
    'End Sub

    Private Sub AddParticipant(sender As Object, e As RoutedEventArgs)
        'DirectCast(_skikursListCollectionView.CurrentItem, Group).AddMember(_participantsToDistributeListCollectionView.CurrentItem)
        'DirectCast(_participantsToDistributeListCollectionView.CurrentItem, Participant).MemberOfGroup = DirectCast(_skikursListCollectionView.CurrentItem, Group).GroupID
        'SetView()
    End Sub

    Private Sub RemoveParticipant(sender As Object, e As RoutedEventArgs)
        'If _participantsInGroupMemberListCollectionView.CurrentItem IsNot Nothing Then
        '    Dim tn = CDS.Skiclub.Participantlist.Where(Function(x) x.ParticipantID = DirectCast(_participantsInGroupMemberListCollectionView.CurrentItem, Participant).ParticipantID).Single
        '    DirectCast(_skikursListCollectionView.CurrentItem, Group).RemoveMember(_participantsInGroupMemberListCollectionView.CurrentItem)
        '    tn.MemberOfGroup = Nothing
        'End If
        'SetView()
    End Sub

    Private Sub SetView()

        _group = DirectCast(DataContext, Group)
        _levelListCollectionView = New CollectionView(Skiclub.Services.CurrentDataService.SortedLevels)
        GroupLevelCombobox.ItemsSource = _levelListCollectionView

        _instructorListCollectionView = Nothing
        _instructorListCollectionView = New CollectionView(Skiclub.Services.CurrentDataService.SortedInstructorsAvailable)
        GroupLeaderCombobox.ItemsSource = _instructorListCollectionView

        _groupmemberListCollectionView = New ListCollectionView(DirectCast(DataContext, Group).GroupMembers)
        If _groupmemberListCollectionView.CanSort Then
            _groupmemberListCollectionView.SortDescriptions.Clear()
            _groupmemberListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFullName", ListSortDirection.Ascending))
        End If
        _groupmemberListCollectionView.MoveCurrentToFirst()
        GroupMembersDataGrid.ItemsSource = _groupmemberListCollectionView
    End Sub


End Class
