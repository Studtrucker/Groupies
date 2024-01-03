Imports Groupies.UserControls
Imports Groupies.Commands
Imports Groupies.Entities

Public Class NewDialog
    Private View As SkillView
    Sub New(Entity As EntityType)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        View = New SkillView With {.Name = "View"}
        UserControlBorder.Child = View
        Me.Title = "Neue Fähigkeit"
        View.Skill = New Skill


    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

    End Sub


    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = View.Skill.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub

End Class

Public Enum EntityType
    Group
    Instructor
    Participant
    Level
    Skill
End Enum
