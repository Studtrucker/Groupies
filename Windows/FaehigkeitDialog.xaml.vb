Imports System.Text
Imports Groupies.Entities
Imports Groupies.Commands

Public Class FaehigkeitDialog

    Public ReadOnly Property Faehigkeit() As Faehigkeit

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Faehigkeit = New Faehigkeit()
        DataContext = _Faehigkeit

    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))
        NamingField.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Faehigkeit.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = True
    End Sub

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub
End Class
