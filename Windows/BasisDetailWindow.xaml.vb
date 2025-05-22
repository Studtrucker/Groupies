Imports System.Windows.Controls.Primitives
Imports Groupies.Interfaces

Public Class BasisWindow

    Private ViewModel As ViewModelWindow


    Private Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

    End Sub

    Public Sub New(ViewModel As ViewModelWindow)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        DataContext = ViewModel

        ' Reagiere auf das Close-Signal aus dem ViewModel
        AddHandler ViewModel.RequestClose, AddressOf HandleCloseRequest
        AddHandler ViewModel.Close, AddressOf HandleClose

    End Sub

    Private Sub HandleCloseRequest(sender As Object, result As Boolean)
        DialogResult = result
        Close()
    End Sub

    Private Sub HandleClose(sender As Object, e As EventArgs)
        Close()
    End Sub

End Class