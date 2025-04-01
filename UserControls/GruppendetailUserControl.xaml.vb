Imports System.ComponentModel
Imports Groupies.Commands
Imports Groupies.Controller
Imports Groupies.Entities
Imports CDS = Groupies.Controller.AppController

Namespace UserControls

    Public Class GruppendetailUserControl

        'Private _LeistungsstufenListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerBearbeiten,
                                       AddressOf Handle_TrainerBearbeiten_Execute,
                                       AddressOf Handle_TrainerBearbeiten_CanExecuted))


            ' Wird im MainWindow verwendet. Beim Laden dieser Form ist das Objekt CurrentClub noch nicht bereit

        End Sub

#Region "Teilnehmer"

        Private Sub Handle_TeilnehmerAusGruppeEntfernen(sender As Object, e As RoutedEventArgs)
            For i = MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
                DirectCast(DataContext, Generation3.Club).SelectedEinteilung.TeilnehmerAusGruppeEntfernen(MitgliederlisteDataGrid.SelectedItems.Item(i), DirectCast(DataContext, Generation3.Club).SelectedGruppe)
            Next
        End Sub

        Private Sub Handle_TrainerBearbeiten_Execute(sender As Object, e As CanExecuteRoutedEventArgs)
            e.CanExecute = DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing
        End Sub

        Private Sub Handle_TrainerBearbeiten_CanExecuted(sender As Object, e As ExecutedRoutedEventArgs)

            Dim dlg = New TrainerDialog With {.Owner = Nothing, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            ' Trainer aus der Gruppenliste holen
            Dim Trainer = DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Trainer
            dlg.Bearbeiten(Trainer)

            If dlg.ShowDialog = True Then
                Trainer = dlg.Trainer
            End If
        End Sub
#End Region

#Region "Trainer"

        Private Sub Handle_TrainerAusGruppeEntfernen(sender As Object, e As MouseButtonEventArgs)

            'If DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
            '    AppController.AktuellerClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
            'End If
            If DirectCast(DataContext, Generation3.Club).SelectedGruppe.Trainer IsNot Nothing Then
                AppController.AktuellerClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, Generation3.Club).SelectedGruppe)
            End If
        End Sub


        Private Sub Handle_TrainerBearbeiten_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
            e.CanExecute = DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing
        End Sub

        Private Sub Handle_TrainerBearbeiten_Execute(sender As Object, e As ExecutedRoutedEventArgs)

            Dim dlg = New TrainerDialog With {
                .Owner = Nothing,
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten),
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            dlg.ModusEinstellen()
            ' Trainer aus der Gruppenliste holen
            Dim Trainer = DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Trainer
            dlg.Bearbeiten(Trainer)

            If dlg.ShowDialog = True Then
                Trainer = dlg.Trainer
            End If
        End Sub

#End Region


    End Class
End Namespace
