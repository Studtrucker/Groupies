Public Class BasisUebersichtWindow
    Private Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

    End Sub

    Public Sub New(ViewModel As UebersichtViewModel)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        DataContext = ViewModel

        ' Reagiere auf das Close-Signal aus dem ViewModel
        AddHandler ViewModel.Close, AddressOf HandleClose
        AddHandler ViewModel.Ansehen, AddressOf HandleAnsehen
        AddHandler ViewModel.Neu, AddressOf HandleNeu
        AddHandler ViewModel.Bearbeiten, AddressOf HandleBearbeiten
        AddHandler ViewModel.Loeschen, AddressOf HandleLoeschen
        AddHandler ViewModel.Vor, AddressOf HandleVor
        AddHandler ViewModel.Zurueck, AddressOf HandleZurueck

    End Sub

    Private Sub HandleClose(sender As Object, e As EventArgs)
        Close()
    End Sub
    Private Sub HandleAnsehen(sender As Object, e As EventArgs)
        MessageBox.Show("Ansehen")
    End Sub
    Private Sub HandleNeu(sender As Object, e As EventArgs)
        MessageBox.Show("Neu")
    End Sub
    Private Sub HandleBearbeiten(sender As Object, e As EventArgs)
        MessageBox.Show("Bearbeiten")
    End Sub
    Private Sub HandleLoeschen(sender As Object, e As EventArgs)
        MessageBox.Show("Loeschen")
    End Sub
    Private Sub HandleVor(sender As Object, e As EventArgs)
        MessageBox.Show("Vor")
    End Sub
    Private Sub HandleZurueck(sender As Object, e As EventArgs)
        MessageBox.Show("Zurück")
    End Sub

End Class
