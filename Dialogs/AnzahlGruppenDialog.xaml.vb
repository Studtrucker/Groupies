Public Class AnzahlGruppenDialog
    Private Sub HandleButtonCancelClick(sender As Object, e As RoutedEventArgs)
        DialogResult = False
    End Sub
    Private Sub HandleButtonOKClick(sender As Object, e As RoutedEventArgs)
        DialogResult = True
    End Sub

    ' Todo: Binding für Slider.Value mit Property Anzahl erstellen

End Class
