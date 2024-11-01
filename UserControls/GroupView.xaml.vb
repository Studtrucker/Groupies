Imports System.ComponentModel
Imports Groupies.Commands
Imports Groupies.Entities
Imports PropertyChanged
Imports CDS = Groupies.Controller.AppController
Namespace UserControls

    Public Class GroupView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

        End Sub



#Region "Teilnehmer"

        Private Sub Handle_TeilnehmerAusGruppeEntfernen(sender As Object, e As RoutedEventArgs)
            For i = GroupMembersDataGrid.SelectedItems.Count - 1 To 0 Step -1
                CDS.CurrentClub.TeilnehmerAusGruppeEntfernen(GroupMembersDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
            Next
        End Sub

#End Region

#Region "Trainer"

        Private Sub Handle_TrainerAusGruppeEntfernen(sender As Object, e As MouseButtonEventArgs)
            If DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
                CDS.CurrentClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
            End If
        End Sub

#End Region


    End Class
End Namespace
