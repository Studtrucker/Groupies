Public Class GroupView
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

    Private Sub SetView(sender As Object, e As RoutedEventArgs)

    End Sub
End Class
