Public Class WindowService
    Implements IWindowService

    Private ReadOnly _window As Window

    Public Sub New(window As Window)
        _window = window
    End Sub

    Public Property DialogResult As Nullable(Of Boolean) Implements IWindowService.DialogResult
        Get
            Return _window.DialogResult
        End Get
        Set(value As Nullable(Of Boolean))
            _window.DialogResult = value
        End Set
    End Property

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

    Public Property SizeToContent As SizeToContent Implements IWindowService.SizeToContent
        Get
            Return _window.SizeToContent
        End Get
        Set(value As SizeToContent)
            _window.SizeToContent = value
        End Set
    End Property

End Class
