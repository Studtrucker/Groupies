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


        Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
            SetView()
        End Sub

        Private Sub SetView()
            Dim cv As ICollectionView = CollectionViewSource.GetDefaultView(DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Mitgliederliste)
            cv.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
            cv.SortDescriptions.Add(New SortDescription("Vorname", ListSortDirection.Ascending))
            cv.SortDescriptions.Add(New SortDescription("Leistungsstufe", ListSortDirection.Ascending))
        End Sub



#Region "Teilnehmer"

        Private Sub Handle_TeilnehmerAusGruppeEntfernen(sender As Object, e As RoutedEventArgs)
            For i = MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
                CDS.CurrentClub.TeilnehmerAusGruppeEntfernen(MitgliederlisteDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
            Next
        End Sub

#End Region

#Region "Trainer"

        Private Sub Handle_TrainerAusGruppeEntfernen(sender As Object, e As MouseButtonEventArgs)
            If DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
                CDS.CurrentClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
            End If
        End Sub

        Private Sub GroupView_DataContextChanged(sender As Object, e As DependencyPropertyChangedEventArgs) Handles Me.DataContextChanged
            SetView()
        End Sub

#End Region


    End Class
End Namespace
