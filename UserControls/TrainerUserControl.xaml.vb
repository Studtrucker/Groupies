
Namespace UserControls
    Public Class TrainerUserControl

        Public Sub New()

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
            DataContext = New ViewModelTrainer()

        End Sub

        Public Sub New(ViewModel As ViewModelTrainer)

            ' Dieser Aufruf ist für den Designer erforderlich.
            InitializeComponent()

            ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

            DataContext = ViewModel

        End Sub

        Private Sub HandleDrop(sender As Object, e As DragEventArgs)
            Throw New NotImplementedException()
        End Sub

        Private Sub HandleDragOver(sender As Object, e As DragEventArgs)
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace