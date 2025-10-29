Imports System
Imports System.Windows.Input

Public Class RelayCommand(Of T)
    Implements ICommand

    Private ReadOnly _execute As Action(Of T)
    Private ReadOnly _canExecute As Func(Of Boolean)
    Private ReadOnly _canExecuteWithParam As Func(Of T, Boolean)

    ''' <summary>
    ''' Konstruktor mit parameterlosem CanExecute (bestehendes Verhalten).
    ''' </summary>
    Public Sub New(execute As Action(Of T), Optional canExecute As Func(Of Boolean) = Nothing)
        If execute Is Nothing Then Throw New ArgumentNullException(NameOf(execute))
        _execute = execute
        _canExecute = If(canExecute, Function() True)
    End Sub

    ''' <summary>
    ''' Konstruktor mit parameterisiertem CanExecute (wird verwendet, wenn CanExecute vom CommandParameter abhängen soll).
    ''' </summary>
    Public Sub New(execute As Action(Of T), canExecute As Func(Of T, Boolean))
        If execute Is Nothing Then Throw New ArgumentNullException(NameOf(execute))
        _execute = execute
        _canExecuteWithParam = canExecute
    End Sub

    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged

    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        ' Wenn ein parameterisiertes CanExecute existiert, verwende es (üblicher Fall für MenuItems mit CommandParameter)
        If _canExecuteWithParam IsNot Nothing Then
            Dim param As T = Nothing
            If parameter IsNot Nothing Then
                Try
                    param = CType(parameter, T)
                Catch ex As Exception
                    ' fallback: wenn Cast fehlschlägt, param bleibt Nothing
                End Try
            End If
            Return _canExecuteWithParam(param)
        End If

        ' sonst das parameterlose CanExecute
        If _canExecute IsNot Nothing Then
            Return _canExecute()
        End If

        Return True
    End Function

    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        _execute(CType(parameter, T))
    End Sub

    Public Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub
End Class