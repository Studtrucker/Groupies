Imports System.Text
Imports Skischule.Entities

Public Class NeueGruppeDialog
    Public ReadOnly Property Skikursgruppe() As Skikurs

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Skikursgruppe = New Skikurs
        DataContext = _Skikursgruppe

    End Sub

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        txtAngezeigterGruppenname.Focus()
    End Sub

    Private Sub HandleButtonOKClick(sender As Object, e As RoutedEventArgs)
        If ValidateInput() Then
            DialogResult = True
        Else
            MessageBox.Show(GetErrors)
        End If
    End Sub

    Private Function ValidateInput() As Boolean
        Return Not Validation.GetHasError(txtAngezeigterGruppenname)
    End Function

    Private Function GetErrors() As String
        Dim sb = New StringBuilder

        For Each [Error] In Validation.GetErrors(txtAngezeigterGruppenname)
            sb.AppendLine([Error].ErrorContent.ToString)
        Next

        Return sb.ToString

    End Function

    Private Sub HandleButtonCancelClick(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub

End Class
