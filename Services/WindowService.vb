Public Class WindowService
    Implements IWindowService

    Private ReadOnly _window As Window

    Public Sub New(window As Window)
        Me._window = window
    End Sub


    Public Sub ShowWindow() Implements IWindowService.ShowWindow
        If _window IsNot Nothing Then
            _window.Show()
        End If
    End Sub
    Public Sub CloseWindow() Implements IWindowService.CloseWindow
        If _window IsNot Nothing Then
            _window.Close()
        End If
    End Sub
    Public Function ShowDialog() As Boolean Implements IWindowService.ShowDialog
        If _window IsNot Nothing Then
            Return _window.ShowDialog() = True
        End If
        Return False
    End Function
End Class
