Public Class AnzahlGruppenDialog
    Public Property Anzahl() As Integer
    Private Sub HandleButtonCancelClick(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub
    Private Sub HandleButtonOKClick(sender As Object, e As RoutedEventArgs)
        If radioButton5.IsChecked Or radioButton10.IsChecked Or radioButton15.IsChecked Then
            DialogResult = True
        End If
    End Sub

    Private Sub radioButton5_Checked(sender As Object, e As RoutedEventArgs)
        _Anzahl = 5
    End Sub

    Private Sub radioButton10_Checked(sender As Object, e As RoutedEventArgs)
        _Anzahl = 10
    End Sub

    Private Sub radioButton15_Checked(sender As Object, e As RoutedEventArgs)
        _Anzahl = 15
    End Sub

End Class
