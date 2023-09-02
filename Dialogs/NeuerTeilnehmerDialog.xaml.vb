Imports System.Text
Imports System.Windows.Forms
Imports Skischule.Entities

Public Class NeuerTeilnehmerDialog

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        _Teilnehmer = New Teilnehmer
        DataContext = _Teilnehmer

    End Sub

    Public ReadOnly Property Teilnehmer() As Teilnehmer

    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        txtVorname.Focus()
    End Sub

    Private Sub HandleButtonOKClick(sender As Object, e As RoutedEventArgs)
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

    Private Sub HandleButtonCancelClick(sender As Object, e As RoutedEventArgs)
        DialogResult = False

    End Sub


End Class
