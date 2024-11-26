Imports System.ComponentModel
Imports Groupies.Commands
Imports Groupies.Entities

Namespace UserControls
    Public Class LeistungsstufeView
        'Public Property Level
        'Private _skillListCollectionView As ICollectionView

        'Sub New()

        '    ' Dieser Aufruf ist für den Designer erforderlich.
        '    InitializeComponent()

        '    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        '    _skillListCollectionView = New ListCollectionView(New SkillCollection())

        'End Sub

        '' Zur Ausführung des Handles HandleNewSkillExecuted erstellen, kann auf dem Mutter Window ein RoutedEvent registriert werden.
        '' Siehe auch EventTrigger (= Ereignisauslöser) Kapitel 11 »Styles, Trigger und Templates«

        'Private Sub LevelView_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        '    CommandBindings.Add(New CommandBinding(SkiclubCommands.NeuerSkill, AddressOf HandleNewSkillExecuted, AddressOf HandleNewSkillCanExecute))
        'End Sub

        'Private Sub HandleNewSkillCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        '    e.CanExecute = True
        'End Sub

        'Private Sub HandleNewSkillExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        '    Dim dlg = New NewSkillDialog ' With {.Owner = Me.Parent, .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        '    If dlg.ShowDialog = True Then
        '        'CDS.Skiclub.Levellist(0).LevelSkills.Add(dlg.Skill)
        '        '_skillListCollectionView.MoveCurrentTo(dlg.Skill)
        '        'SkillsDataGrid.ScrollIntoView(dlg.Skill)
        '    End If
        'End Sub

    End Class
End Namespace
