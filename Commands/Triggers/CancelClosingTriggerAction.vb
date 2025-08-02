Imports Microsoft.Xaml.Behaviors

Public Class CancelClosingTriggerAction
    Inherits TriggerAction(Of Window)

    Public Shared ReadOnly CommandProperty As DependencyProperty =
        DependencyProperty.Register("Command", GetType(ICommand), GetType(CancelClosingTriggerAction), New PropertyMetadata(Nothing))

    Public Property Command As ICommand
        Get
            Return CType(GetValue(CommandProperty), ICommand)
        End Get
        Set(value As ICommand)
            SetValue(CommandProperty, value)
        End Set
    End Property

    Protected Overrides Sub Invoke(parameter As Object)
        Dim args = TryCast(parameter, System.ComponentModel.CancelEventArgs)
        If Command IsNot Nothing AndAlso Command.CanExecute(args) Then
            Command.Execute(args)
        End If
    End Sub
End Class

