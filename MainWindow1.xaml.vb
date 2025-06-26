
Public Class MainWindow1

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        DataContext = New ViewModels.MainViewModel()

    End Sub

End Class
