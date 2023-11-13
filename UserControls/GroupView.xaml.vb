Imports System.ComponentModel
Imports System.Text
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
    'Private Sub GroupView_GotFocus(sender As Object, e As RoutedEventArgs) Handles Me.GotFocus
    '    SetView()
    'End Sub

    Private Sub AddParticipant(Participant As Participant)

        Participant.SetAsGroupMember(Group.GroupID)
        Group.AddMember(Participant)

        'Dim Layer = DirectCast(Me.Parent, WrapPanel).FindName("LayerParticipantToDistributeOverview")

        'Dim Grid = DirectCast(Layer, Grid)
        'Dim GridCollectionView = DirectCast(Grid.DataContext, CollectionView)
        'If GridCollectionView.CurrentItem IsNot Nothing Then

        '    Dim item = GridCollectionView.CurrentItem
        '    DirectCast(item, Participant).SetAsGroupMember(_group.GroupID)
        '    Group.AddMember(item)
        '    Dim ie = LogicalTreeHelper.GetChildren(Grid)
        '    Dim DataGrid As DataGrid
        '    For Each item In ie
        '        If item.name = "participantlistOverviewDataGrid" Then
        '            DataGrid = item
        '            DataGrid.Items.Remove(item)
        '        End If
        '    Next
        'End If

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

    Private Sub GroupMembersDataGrid_Drop(sender As Object, e As DragEventArgs)

        Dim CorrectDataFormat = e.Data.GetDataPresent("System.Windows.Controls.SelectedItemCollection")
        If CorrectDataFormat Then
            Dim ic = e.Data.GetData("System.Windows.Controls.SelectedItemCollection")
            For Each item In ic
                AddParticipant(DirectCast(item, Participant))
            Next
        End If
        CorrectDataFormat = e.Data.GetDataPresent("Skiclub.Entities.Participant")
        If CorrectDataFormat Then
            Dim ic = e.Data.GetData("Skiclub.Eintities.Participant")
            AddParticipant(DirectCast(ic, Participant))
        End If

    End Sub


    Private Sub GroupMembersDataGrid_MouseDown(sender As Object, e As MouseButtonEventArgs) Handles GroupMembersDataGrid.MouseDown
        Dim Tn = TryCast(GroupMembersDataGrid.SelectedItem, Participant)

        If Tn IsNot Nothing Then
            Dim Data = New DataObject(GetType(Participant), Tn)
            DragDrop.DoDragDrop(GroupMembersDataGrid, Data, DragDropEffects.Copy)
        End If

    End Sub
End Class
