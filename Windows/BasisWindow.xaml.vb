Imports System.Windows.Controls.Primitives
Imports Groupies.Interfaces

Public Class BasisWindow
    Implements Interfaces.IWindowMitModus

    Private ViewModel As DialogViewModelBase


    Public Sub New()

        ViewModel = New DialogViewModelBase

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        DataContext = ViewModel

        AddHandler ViewModel.RequestClose, AddressOf HandleCloseRequest

    End Sub

    Public Sub New(ViewModel As DialogViewModelBase)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        DataContext = ViewModel

        ' Reagiere auf das Close-Signal aus dem ViewModel
        AddHandler ViewModel.RequestClose, AddressOf HandleCloseRequest
        AddHandler ViewModel.Close, AddressOf HandleClose

    End Sub

    Public Property Modus As IModus Implements Interfaces.IWindowMitModus.Modus


    Public Sub ModusEinstellen() Implements Interfaces.IWindowMitModus.ModusEinstellen
        Title &= Modus.Titel
        Icon = New BitmapImage(New Uri(Modus.IconString))
        CancelButton.Visibility = Modus.CancelButtonVisibility
        OkButton.Visibility = Modus.OkButtonVisibility
        CloseButton.Visibility = Modus.CloseButtonVisibility
    End Sub

    Public Sub Bearbeiten(Objekt As BaseModel) Implements Interfaces.IWindowMitModus.Bearbeiten
        Throw New NotImplementedException()
    End Sub

    Private Sub HandleCloseRequest(sender As Object, result As Boolean)
        DialogResult = result
        Close()
    End Sub
    Private Sub HandleClose(sender As Object, e As EventArgs)
        Close()
    End Sub

End Class
