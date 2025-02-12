Imports System.ComponentModel
Imports AppCon = Groupies.Controller.AppController


Public Class Traineruebersicht
    Private _TrainerCollectionView As ICollectionView

    Private Sub Handle_Traineruebersicht_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        _TrainerCollectionView = New ListCollectionView(AppCon.AktuellerClub.AlleTrainer)
        If _TrainerCollectionView.CanSort Then
            _TrainerCollectionView.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
        End If
        DataContext = _TrainerCollectionView

    End Sub


End Class
