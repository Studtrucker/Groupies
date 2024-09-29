Imports System.ComponentModel
Imports Groupies.Entities
Imports PropertyChanged
Imports CDS = Groupies.Services.CurrentDataService

Namespace UserControls

    Public Class GroupView

        Public Property Group As Group
        Private _levelListCollectionView As ICollectionView
        Private _instructorListCollectionView As ICollectionView
        Private _groupmemberListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

        End Sub


        Private Sub GroupView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

            If CDS.Skiclub IsNot Nothing AndAlso CDS.Skiclub.Levellist IsNot Nothing Then

                _levelListCollectionView = New ListCollectionView(CDS.Skiclub.Levellist)
                If _levelListCollectionView.CanSort Then
                    _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
                End If
                GroupLevelCombobox.ItemsSource = _levelListCollectionView

                '    '*****************************************************************************************************
                '    '* Darf nicht gefiltert werden, denn sonst werden die einegteilten Instructoren nicht mehr angezeigt *
                '    '* If _instructorListCollectionView.CanFilter Then                                                   *
                '    '*     _instructorListCollectionView.Filter = Function(f As Instructor) f.IsAvailable                *
                '    '* End If                                                                                            *
                '    '*****************************************************************************************************
                '    _instructorListCollectionView = New ListCollectionView(CDS.Skiclub.Instructorlist)
                '    If _instructorListCollectionView.CanSort Then
                '        _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorFirstName", ListSortDirection.Ascending))
                '        _instructorListCollectionView.SortDescriptions.Add(New SortDescription("InstructorLastName", ListSortDirection.Ascending))
                '    End If
                '    'GroupLeaderCombobox.ItemsSource = _instructorListCollectionView
            End If

        End Sub

        Private Sub SetView()

            '_groupmemberListCollectionView = New ListCollectionView(DirectCast(DataContext, Group).GroupMembers)
            'If _groupmemberListCollectionView.CanSort Then
            '    _groupmemberListCollectionView.SortDescriptions.Clear()
            '    _groupmemberListCollectionView.SortDescriptions.Add(New SortDescription("ParticipantFullName", ListSortDirection.Ascending))
            'End If
            '_groupmemberListCollectionView.MoveCurrentToFirst()
            'GroupMembersDataGrid.ItemsSource = _groupmemberListCollectionView

        End Sub


        ' Für den Empfang von ParticipantObjekten zu Groupmembern, jetzt okay 
        Private Sub GroupMembersDataGrid_Drop(sender As Object, e As DragEventArgs)
            ' Participants werden Groupmember
            Dim CorrectDataFormat = e.Data.GetDataPresent(GetType(IList))
            If CorrectDataFormat Then
                Dim TN = e.Data.GetData(GetType(IList))
                Dim CurrentGroup = DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Group)
                For Each Participant As Teilnehmer In TN
                    If Participant.IsNotInGroup Then
                        Participant.SetAsGroupMember(CurrentGroup.GroupID)
                        CurrentGroup.AddMember(Participant)
                    End If
                Next
            End If
        End Sub


        ' Für den Empfang von InstructorObjekten als Groupinstructor, jetzt okay 
        Private Sub GroupLeaderTextblock_Drop(sender As Object, e As DragEventArgs)
            '' Instructor wird Groupleader
            Dim CorrectDataFormat = e.Data.GetDataPresent(GetType(Instructor))
            If CorrectDataFormat Then
                Dim TN = e.Data.GetData(GetType(Instructor))
                Dim CurrentGroup = DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Group)
                ' Hat es schon einen Skilehrer gegeben?
                If CurrentGroup.GroupLeader IsNot Nothing Then
                    ' Alten Skilehrer wieder frei setzen
                    '                    CurrentGroup.GroupLeader.IsAvailable = True
                    CDS.Skiclub.Instructorlist.Where(Function(x) x.InstructorID = CurrentGroup.GroupLeader.InstructorID).Single.IsAvailable = True
                End If
                CurrentGroup.GroupLeader = TN
                ' Neuer Skilehrer ist nicht mehr frei
                DirectCast(TN, Instructor).IsAvailable = False
            End If
        End Sub

        Private Sub TextBlock_MouseDown(sender As Object, e As MouseButtonEventArgs)

            'Dim Tn = TryCast(GroupLeaderTextblock., Instructor)

            'If Tn IsNot Nothing Then
            '    Dim Data = New DataObject(GetType(Participant), Tn)
            '    Group.GroupLeader = Nothing
            'End If
        End Sub

        ' Für das Verschieben von ParticipantObjekten aus der Group, werden NotInAGroup 
        Private Sub GroupMembersDataGrid_MouseDown(sender As Object, e As MouseButtonEventArgs)

            Dim Tn = TryCast(GroupMembersDataGrid.SelectedItems, IList)

            If Tn IsNot Nothing Then
                'For Each item As Participant In Tn
                Dim Data = New DataObject(GetType(IList), Tn)
                DragDrop.DoDragDrop(GroupMembersDataGrid, Data, DragDropEffects.Move)
                'Next
            End If

        End Sub



        Private Sub MenuItemDeleteGroupMember_Click(sender As Object, e As RoutedEventArgs)
            For Each item As Teilnehmer In GroupMembersDataGrid.SelectedItems
                CDS.Skiclub.Participantlist.Where(Function(x) x.TeilnehmerID = item.TeilnehmerID).Single.RemoveFromGroup()
            Next
            SetView()
        End Sub

        Private Sub HandleDragOver(sender As Object, e As DragEventArgs)
            If e.Data.GetDataPresent(GetType(IList)) Then
                e.Effects = DragDropEffects.Copy
            Else
                e.Effects = DragDropEffects.None
            End If
        End Sub


        'Private Sub gridSkikursdetails_Drop(sender As Object, e As DragEventArgs)
        '    ' Participants werden aus Group entfernt
        '    Dim CorrectDataFormat = e.Data.GetDataPresent(GetType(IList))
        '    If CorrectDataFormat Then
        '        Dim TN = e.Data.GetData(GetType(IList))
        '        Dim CurrentGroup = DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Group)
        '        For Each Participant As Participant In TN
        '            CDS.Skiclub.Participantlist.Where(Function(x) x.ParticipantID = Participant.ParticipantID).Single.DeleteFromGroup()
        '        Next
        '    End If
        'End Sub
    End Class
End Namespace
