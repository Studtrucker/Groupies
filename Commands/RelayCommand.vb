Imports System.Windows.Input

Public Class RelayCommand(Of T)
    Implements ICommand

    Private ReadOnly _execute As Action(Of T)
    Private ReadOnly _canExecute As Predicate(Of T)

    Public Sub New(execute As Action(Of T), Optional canExecute As Predicate(Of T) = Nothing)
        _execute = execute
        _canExecute = If(canExecute, Function(x) True)
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _canExecute(CType(parameter, T))
    End Function

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        _execute(CType(parameter, T))
    End Sub
End Class
