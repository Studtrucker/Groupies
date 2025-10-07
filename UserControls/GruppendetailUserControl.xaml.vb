Imports System.ComponentModel
Imports Groupies.Commands
Imports Groupies.Controller
Imports Groupies.Entities

Namespace UserControls

    Public Class GruppendetailUserControl

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        End Sub

        'Private _LeistungsstufenListCollectionView As ICollectionView

        '        Sub New()

        '            ' Dieser Aufruf ist für den Designer erforderlich.
        '            InitializeComponent()

        '            CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerBearbeiten,
        '                                       AddressOf Handle_TrainerBearbeiten_Execute,
        '                                       AddressOf Handle_TrainerBearbeiten_CanExecute))


        '            ' Wird im MainWindow verwendet. Beim Laden dieser Form ist das Objekt CurrentClub noch nicht bereit

        '        End Sub

        '#Region "Teilnehmer"

        '        Private Sub Handle_TeilnehmerAusGruppeEntfernen(sender As Object, e As RoutedEventArgs)
        '            For i = MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
        '                DirectCast(DataContext, ViewModels.MainViewModel).SelectedEinteilung.TeilnehmerAusGruppeEntfernen(MitgliederlisteDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ViewModels.MainViewModel).SelectedGruppe)
        '            Next
        '        End Sub

        '#End Region

        '#Region "Trainer"

        '        Private Sub Handle_TrainerAusGruppeEntfernen(sender As Object, e As MouseButtonEventArgs)
        '            If DirectCast(DataContext, Generation3.Club).SelectedGruppe.Trainer IsNot Nothing Then
        '                AppController.AktuellerClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, Generation3.Club).SelectedGruppe)
        '            End If
        '        End Sub


        '        Private Sub Handle_TrainerBearbeiten_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        '            e.CanExecute = AppController.AktuellerClub.SelectedGruppe IsNot Nothing AndAlso AppController.AktuellerClub.SelectedGruppe.Trainer IsNot Nothing
        '        End Sub

        '        Private Sub Handle_TrainerBearbeiten_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        '            Throw New NotImplementedException("Die Funktion zum Erstellen einer neuen Trainers ist noch nicht implementiert.")

        '            'Dim dlg = New TrainerDialog With {
        '            '    .Owner = Nothing,
        '            '    .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        '            ''.Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten),
        '            ''dlg.ModusEinstellen()
        '            '' Trainer aus der Gruppenliste holen
        '            'Dim Trainer = AppController.AktuellerClub.SelectedGruppe.Trainer
        '            'dlg.Bearbeiten(Trainer)

        '            'If dlg.ShowDialog = True Then
        '            '    Trainer = dlg.Trainer
        '            'End If
        '        End Sub

        '#End Region

    End Class
End Namespace
