Imports System.Windows.Controls.Primitives
Imports Groupies.Interfaces

Public Class BasisWindow
    'Implements Interfaces.IWindowMitModus
    Private ViewModel As New DialogViewModelBase()

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        DataContext = ViewModel

        AddHandler ViewModel.RequestClose, AddressOf HandleCloseRequest

    End Sub

    'Public Sub New(ViewModel As DialogViewModelBase)

    '    ' Dieser Aufruf ist für den Designer erforderlich.
    '    InitializeComponent()

    '    ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    '    DataContext = ViewModel

    '    ' Reagiere auf das Close-Signal aus dem ViewModel
    '    AddHandler ViewModel.RequestClose, AddressOf HandleCloseRequest

    'End Sub

    Private Sub HandleCloseRequest(sender As Object, result As Boolean)
        Me.DialogResult = result
        Me.Close()
    End Sub

    'Private Sub HandleButtonOKExecuted(sender As Object, e As RoutedEventArgs)
    '    Close()
    'End Sub

End Class
