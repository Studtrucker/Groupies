Imports System
Imports System.Windows.Input

Public Class RelayCommand(Of T)
    Implements ICommand

    Private ReadOnly _execute As Action(Of T)
    Private ReadOnly _canExecute As Func(Of Boolean)


    ''' <summary>
    ''' Kontruktor mit der Möglichkeit, die Ausführbarkeit zu steuern.
    ''' </summary>
    ''' <param name="execute"></param>
    ''' <param name="canExecute"></param>
    Public Sub New(execute As Action(Of T), Optional canExecute As Func(Of Boolean) = Nothing)
        ' Hier wird der Delegate für die Ausführung des Befehls gesetzt.
        _execute = execute
        ' Hier wird der Delegate für die Ausführbarkeit gesetzt.
        _canExecute = If(canExecute, Function() True)
    End Sub

    ''' <summary>
    ''' Dieses Event wird ausgelöst, wenn sich die Ausführbarkeit des Befehls ändert.
    ''' </summary>
    ''' <remarks>Das Event muss in der ViewModel-Klasse aufgerufen werden, wenn sich die Ausführbarkeit ändert.</remarks>
    ''' <example>RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)</example>
    ''' <example>RaiseEvent CanExecuteChanged(Me, New EventArgs())</example>
    Public Event CanExecuteChanged As EventHandler Implements ICommand.CanExecuteChanged


    ''' <summary>
    ''' Diese Methode wird aufgerufen, um zu prüfen, ob der Befehl ausgeführt werden kann.
    ''' </summary>
    ''' <param name="parameter"></param>
    ''' <returns></returns>
    ''' <remarks>Hier wird die Ausführbarkeit des Befehls geprüft.</remarks>
    Public Function CanExecute(parameter As Object) As Boolean Implements ICommand.CanExecute
        Return _canExecute()
    End Function

    ''' <summary>
    ''' Diese Methode wird aufgerufen, um den Befehl auszuführen.
    ''' </summary>
    ''' <remarks>Hier wird der Befehl ausgeführt.</remarks>
    Public Sub Execute(parameter As Object) Implements ICommand.Execute
        _execute(CType(parameter, T))
    End Sub

    ''' <summary>
    ''' Diese Methode wird aufgerufen, um das CanExecuteChanged-Event auszulösen.
    ''' </summary>
    ''' <remarks>
    ''' Diese Methode sollte in der ViewModel-Klasse aufgerufen werden, 
    ''' wenn sich die Ausführbarkeit des Befehls ändert.
    ''' </remarks>
    Public Sub RaiseCanExecuteChanged()
        RaiseEvent CanExecuteChanged(Me, EventArgs.Empty)
    End Sub
End Class
