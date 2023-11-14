Imports System.ComponentModel
Imports Skiclub.Entities


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
        SetView()
        Group = DirectCast(DataContext, Group)
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

        'If _participantsInGroupMemberListCollectionView.CurrentItem IsNot Nothing Then
        '    Dim tn = CDS.Skiclub.Participantlist.Where(Function(x) x.ParticipantID = DirectCast(_participantsInGroupMemberListCollectionView.CurrentItem, Participant).ParticipantID).Single
        '    DirectCast(_skikursListCollectionView.CurrentItem, Group).RemoveMember(_participantsInGroupMemberListCollectionView.CurrentItem)
        '    tn.MemberOfGroup = Nothing
        'End If
        'SetView()
    End Sub

    Private Sub SetView()

        _levelListCollectionView = New ListCollectionView(Skiclub.Services.CurrentDataService.Skiclub.Levellist)
        If _levelListCollectionView.CanSort Then
            _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
        End If
        GroupLevelCombobox.ItemsSource = _levelListCollectionView

        _instructorListCollectionView = New ListCollectionView(Skiclub.Services.CurrentDataService.Skiclub.Instructorlist)
        If _instructorListCollectionView.CanSort Then
            _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorFirstName", ListSortDirection.Ascending))
            _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorLastName", ListSortDirection.Ascending))
        End If

        '*****************************************************************************************************
        '* Darf nicht gefiltert werden, denn sonst werden die einegteilten Instructoren nicht mehr angezeigt *
        '* If _instructorListCollectionView.CanFilter Then                                                   *
        '*     _instructorListCollectionView.Filter = Function(f As Instructor) f.IsAvailable                *
        '* End If                                                                                            *
        '*****************************************************************************************************

        GroupLeaderCombobox.ItemsSource = _instructorListCollectionView

        _groupmemberListCollectionView = New ListCollectionView(DirectCast(DataContext, Group).GroupMembers)
        If _groupmemberListCollectionView.CanSort Then
            _groupmemberListCollectionView.SortDescriptions.Clear()
            _groupmemberListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFullName", ListSortDirection.Ascending))
        End If
        _groupmemberListCollectionView.MoveCurrentToFirst()
        GroupMembersDataGrid.ItemsSource = _groupmemberListCollectionView
    End Sub

    Private Sub GroupMembersDataGrid_ReceiveByDrop(sender As Object, e As DragEventArgs)
        ' Participants werden Groupmember
        Dim CorrectDataFormat = e.Data.GetDataPresent("System.Windows.Controls.SelectedItemCollection")
        If CorrectDataFormat Then
            Dim ic = e.Data.GetData("System.Windows.Controls.SelectedItemCollection")
            For Each Participant As Participant In ic
                Participant.SetAsGroupMember(Group.GroupID)
                Group.AddMember(Participant)
            Next
        End If
    End Sub

    Private Sub GroupMembersDataGrid_SendByMouseDown(sender As Object, e As MouseButtonEventArgs)
        ' Participants verlassen die Gruppe
        Dim datagrid = TryCast(sender, DataGrid)
        If datagrid IsNot Nothing AndAlso e.LeftButton = MouseButtonState.Pressed Then
            Dim x = DragDrop.DoDragDrop(datagrid, datagrid.SelectedItems, DragDropEffects.Move)
            If x = DragDropEffects.Move Then
                HasDropped()
            End If
        End If
    End Sub

    Private Sub HasDropped()
        Dim Pl = New ParticipantCollection
        GroupMembersDataGrid.SelectedItems.Cast(Of Participant).ToList.ForEach(Sub(x) x.DeleteFromGroup())
        GroupMembersDataGrid.SelectedItems.Cast(Of Participant).ToList.ForEach(Sub(x) Pl.Add(x))
        Group.RemoveMembers(Pl)
    End Sub

End Class
