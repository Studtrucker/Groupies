Imports System.ComponentModel
Imports Groupies.Commands
Imports Groupies.Controller
Imports Groupies.Entities
Imports CDS = Groupies.Controller.AppController
Namespace UserControls

    Public Class GruppendetailUserControl
        'Private ReadOnly _LeistungsstufenListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerBearbeiten,
                                       AddressOf Handle_TrainerBearbeiten_Execute,
                                       AddressOf Handle_TrainerBearbeiten_CanExecuted))

            ' ListCollectionView für die Combobox erstellen

            '_LeistungsstufenListCollectionView = New CollectionView(AppController.CurrentClub.Leistungsstufenliste.Select(Function(LS) LS.Benennung))
            'GruppenleistungsstufeComboBox.ItemsSource = _LeistungsstufenListCollectionView

        End Sub

        Public Sub setView(sender As Object, e As RoutedEventArgs)
            If DataContext Is Nothing OrElse DirectCast(DataContext, CollectionView).CurrentItem Is Nothing Then Exit Sub
            Dim cv As ICollectionView = CollectionViewSource.GetDefaultView(DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Mitgliederliste)
            cv.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
            cv.SortDescriptions.Add(New SortDescription("Vorname", ListSortDirection.Ascending))
            cv.SortDescriptions.Add(New SortDescription("Leistungsstufe", ListSortDirection.Ascending))
        End Sub

        Private Sub UserControl_Loaded(sender As Object, e As RoutedEventArgs)
            setView(sender, e)
        End Sub



#Region "Teilnehmer"

        Private Sub Handle_TeilnehmerAusGruppeEntfernen(sender As Object, e As RoutedEventArgs)
            For i = MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
                CDS.CurrentClub.TeilnehmerAusGruppeEntfernen(MitgliederlisteDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
            Next
        End Sub

        'Private Sub Handle_TeilnehmerBearbeiten(sender As Object, e As RoutedEventArgs)
        '    For i = MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
        '        CDS.CurrentClub.TeilnehmerBearbeiten(MitgliederlisteDataGrid.SelectedItems.Item(i).CurrentItem)
        '    Next
        'End Sub

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
            If DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
                CDS.CurrentClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
            End If
        End Sub


        Private Sub Handle_TrainerBearbeiten_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
            e.CanExecute = DirectCast(DirectCast(DataContext, CollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing
        End Sub

        Private Sub Handle_TrainerBearbeiten_Execute(sender As Object, e As ExecutedRoutedEventArgs)

            Dim dlg = New TrainerDialog With {.Owner = Nothing, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

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
