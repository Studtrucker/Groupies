
Public Class MainWindow1

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        Dim ws As IWindowService = New WindowService(Me)

        DataContext = New ViewModels.MainViewModel(ws)

    End Sub

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim dc = New TestWindowViewModel
        Dim tw = New TestWindow With {
            .DataContext = dc
        }
        tw.Show()
    End Sub
End Class
