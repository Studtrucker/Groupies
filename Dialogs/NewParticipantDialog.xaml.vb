Imports System.Text
Imports System.Windows.Forms
Imports Skischule.Entities

Public Class NewParticipantDialog

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Teilnehmer = New Participant
        DataContext = _Teilnehmer

    End Sub

    Public ReadOnly Property Teilnehmer() As Participant

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded

        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogOk, AddressOf HandleButtonOKExecuted, AddressOf HandleButtonOKCanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.DialogCancel, AddressOf HandleButtonCancelExecuted))

        txtVorname.Focus()

    End Sub

    Private Sub HandleButtonOKCanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = Teilnehmer.IsOk
    End Sub

    Private Sub HandleButtonOKExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        If ValidateInput() Then
            DialogResult = True
        Else
            MessageBox.Show(GetErrors)
        End If
    End Sub

    Private Function ValidateInput() As Boolean
        Return Not Validation.GetHasError(txtVorname)
    End Function

    Private Function GetErrors() As String
        Dim sb = New StringBuilder

        For Each [Error] In Validation.GetErrors(txtVorname)
            sb.AppendLine([Error].ErrorContent.ToString)
        Next

        Return sb.ToString

    End Function

    Private Sub HandleButtonCancelExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        DialogResult = False
    End Sub


End Class
