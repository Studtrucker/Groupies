Imports System.ComponentModel
Imports Groupies.Entities
Imports CDS = Groupies.Services.CurrentDataService

Public Class GroupView
    'Private _group As Group
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

    Public Property Group As Group

    Private Sub GroupView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Group = DirectCast(DataContext, Group)
        SetView()

        _levelListCollectionView = New ListCollectionView(CDS.Skiclub.Levellist)
        If _levelListCollectionView.CanSort Then
            _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
        End If
        GroupLevelCombobox.ItemsSource = _levelListCollectionView

        '*****************************************************************************************************
        '* Darf nicht gefiltert werden, denn sonst werden die einegteilten Instructoren nicht mehr angezeigt *
        '* If _instructorListCollectionView.CanFilter Then                                                   *
        '*     _instructorListCollectionView.Filter = Function(f As Instructor) f.IsAvailable                *
        '* End If                                                                                            *
        '*****************************************************************************************************
        _instructorListCollectionView = New ListCollectionView(CDS.Skiclub.Instructorlist)
        If _instructorListCollectionView.CanSort Then
            _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorFirstName", ListSortDirection.Ascending))
            _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorLastName", ListSortDirection.Ascending))
        End If
        GroupLeaderCombobox.ItemsSource = _instructorListCollectionView

    End Sub

    Private Sub AddParticipant(Participant As Participant)

        Participant.SetAsGroupMember(Group.GroupID)
        Group.AddMember(Participant)

    End Sub

    Private Sub RemoveParticipant()

        For Each item In GroupMembersDataGrid.SelectedItems
            DirectCast(item, Participant).DeleteFromGroup()
        Next
        Group.RemoveMembers(GroupMembersDataGrid.SelectedItems)


    End Sub

    Private Sub SetView()

        _groupmemberListCollectionView = New ListCollectionView(DirectCast(DataContext, Group).GroupMembers)
        If _groupmemberListCollectionView.CanSort Then
            _groupmemberListCollectionView.SortDescriptions.Clear()
            _groupmemberListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFullName", ListSortDirection.Ascending))
        End If
        _groupmemberListCollectionView.MoveCurrentToFirst()
        GroupMembersDataGrid.ItemsSource = _groupmemberListCollectionView
    End Sub

    ' Für den Empfang von Objekten 
    Private Sub GroupMembersDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)
        ' Participants werden Groupmember
        Dim CorrectDataFormat = e.Data.GetDataPresent("Groupies.Entities.Participant")
        If CorrectDataFormat Then
            Dim TN = e.Data.GetData("Groupies.Entities.Participant")
            'For Each Participant As Participant In ic
            TN.SetAsGroupMember(Group.GroupID)
            Group.AddMember(TN)
            CDS.Skiclub.Participantlist.Remove(CDS.Skiclub.Participantlist.Where(Function(x) x.ParticipantID.Equals(TN.ParticipantID)).First)
            CDS.Skiclub.Participantlist.Add(TN)
        End If
    End Sub

    ' Für das Verschieben von Objekten 
    Private Sub GroupMembersDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)

        Dim Tn = TryCast(GroupMembersDataGrid.SelectedItem, Participant)

        If Tn IsNot Nothing Then
            Dim Data = New DataObject(GetType(Participant), Tn)
            DragDrop.DoDragDrop(GroupMembersDataGrid, Data, DragDropEffects.Move)
            Group.RemoveMember(Tn)
        End If

    End Sub

    Private Sub TextBlock_MouseDown(sender As Object, e As MouseButtonEventArgs)

        Dim Tn = TryCast(_instructorListCollectionView.CurrentItem, Instructor)

        If Tn IsNot Nothing Then
            Dim Data = New DataObject(GetType(Participant), Tn)
            'DragDrop.DoDragDrop(GroupMembersDataGrid, Data, DragDropEffects.Move)
            Group.GroupLeader = Nothing
        End If
    End Sub
End Class
