Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Commands

Namespace UserControls
    Public Class Leistungsstufenuebersicht
        Private _levelListCollectionView As ICollectionView
        Private _skillListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

            _levelListCollectionView = New ListCollectionView(Controller.AppController.CurrentClub.Leistungsstufenliste)
            _levelListCollectionView.SortDescriptions.Add(New SortDescription("SortNumber", ListSortDirection.Ascending))
            DataContext = _levelListCollectionView

            _skillListCollectionView = New ListCollectionView(New FaehigkeitCollection())

        End Sub

        ' Zur Ausführung des Handles HandleNewSkillExecuted erstellen, kann auf dem Mutter Window ein RoutedEvent registriert werden.
        ' Siehe auch EventTrigger (= Ereignisauslöser) Kapitel 11 »Styles, Trigger und Templates«

        Private Sub LevelView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            CommandBindings.Add(New CommandBinding(SkiclubCommands.NeuerSkill, AddressOf HandleNewSkillExecuted, AddressOf HandleNewSkillCanExecute))
        End Sub

        Private Sub HandleNewSkillCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
            e.CanExecute = True
        End Sub

        Private Sub HandleNewSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
            Dim dlg = New NeueFaehigkeitDialog ' With {.Owner = Me.Parent, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
            If dlg.ShowDialog = True Then
                Dim s = dlg.Faehigkeit
                Dim i = Controller.AppController.CurrentClub.Leistungsstufenliste.IndexOf(_levelListCollectionView.CurrentItem)
                Controller.AppController.CurrentClub.Leistungsstufenliste(i).Faehigkeiten.Add(s)
                _skillListCollectionView.MoveCurrentTo(s)
                Leistungsstufe.SkillsDataGrid.ScrollIntoView(s)
            End If
        End Sub
        Private Sub Handle_LeistungsstufeLoeschen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
            Throw New NotImplementedException
            'wer ist hier der sender?
            'Dim wirdverwendet = AppCon.CurrentClub.AlleTeilnehmer.ToList.TrueForAll(Function(Tn) Tn.Leistungsstand.Equals(sender))
            'e.CanExecute = DirectCast(_gruppenlisteCollectionView.CurrentItem, Gruppe).Trainer Is Nothing AndAlso DirectCast(_gruppenlisteCollectionView.CurrentItem, Gruppe).Mitgliederliste.Count = 0
        End Sub

        Private Sub Handle_LeistungsstufeLoeschen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
            Throw New NotImplementedException
        End Sub
    End Class

End Namespace
