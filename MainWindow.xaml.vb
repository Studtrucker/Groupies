
Public Class MainWindow

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        Dim ws As IWindowService = New WindowService(Me)

        DataContext = New ViewModels.MainViewModel(ws)

    End Sub

End Class
