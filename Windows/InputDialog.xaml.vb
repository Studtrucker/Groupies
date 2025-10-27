Imports System.Windows

Namespace UserControls

    Partial Public Class InputDialog
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub Ok_Click(sender As Object, e As RoutedEventArgs)
            Me.DialogResult = True
            Me.Close()
        End Sub

        Private Sub Cancel_Click(sender As Object, e As RoutedEventArgs)
            Me.DialogResult = False
            Me.Close()
        End Sub
    End Class
End Namespace
