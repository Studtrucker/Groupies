Imports System.Windows.Input

Public Class RelayCommand
    Implements ICommand

    Private ReadOnly _execute As Action(Of Object)
    Private ReadOnly _canExecute As Func(Of Object, Boolean)

    Public Sub New(execute As Action(Of Object), Optional canExecute As Func(Of Object, Boolean) = Nothing)
        _execute = execute
        _canExecute = If(canExecute, Function(p) True)
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _canExecute(parameter)
    End Function

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        _execute(parameter)
    End Sub
End Class

