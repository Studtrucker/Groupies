Imports System.ComponentModel
Imports Groupies.Commands
Imports CDS = Groupies.Services.CurrentDataService
Imports Groupies.Entities

Namespace UserControls
    Public Class LevelView
        Public Property Level
        Private _skillListCollectionView As ICollectionView

        Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
            _skillListCollectionView = New ListCollectionView(New SkillCollection())

        End Sub

        ' Zur Ausführung des Handles HandleNewSkillExecuted erstellen, kann auf dem Mutter Window ein RoutedEvent registriert werden.
        ' Siehe auch EventTrigger (= Ereignisauslöser) Kapitel 11 »Styles, Trigger und Templates«

        Private Sub LevelView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
            CommandBindings.Add(New CommandBinding(SkiclubCommands.NewSkill, AddressOf HandleNewSkillExecuted))
            CommandBindings.Add(New CommandBinding(SkiclubCommands.DeleteSkill, AddressOf HandleDeleteSkillExecuted, AddressOf HandleHandleDeleteSkillExecutedCanExecute))

        End Sub

        Private Sub HandleHandleDeleteSkillExecutedCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
            ' e.CanExecute = SkillsDataGrid.SelectedItems.Count > 0
            e.CanExecute = True
        End Sub

        Private Sub HandleDeleteSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
            Throw New NotImplementedException()

        End Sub

        Private Sub HandleNewSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
            Dim dlg = New NewSkillDialog ' With {.Owner = Me.Parent, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
            If dlg.ShowDialog = True Then
                CDS.Skiclub.Levellist(0).LevelSkills.Add(dlg.Skill)
                _skillListCollectionView.MoveCurrentTo(dlg.Skill)
                SkillsDataGrid.ScrollIntoView(dlg.Skill)
            End If
        End Sub

        Private Sub SkillsDataGrid_AddingNewItem(sender As Object, e As AddingNewItemEventArgs)

        End Sub
    End Class
End Namespace
