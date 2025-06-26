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
        If _window Is Nothing Then
            Return
        End If
        _window.Show()
    End Sub

    Public Sub CloseWindow() Implements IWindowService.CloseWindow
        If _window Is Nothing Then
            Return
        End If
        _window.Close()
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

    'Public Property MaxWidth As Double Implements IWindowService.MaxWidth
    '    Get
    '        Return _window.MaxWidth
    '    End Get
    '    Set(value As Double)
    '        _window.MaxWidth = value
    '    End Set
    'End Property

    Public Property Width As Double Implements IWindowService.Width
        Get
            Return _window.Width
        End Get
        Set(value As Double)
            _window.Width = value
        End Set
    End Property
    Public Property Height As Double Implements IWindowService.Height
        Get
            Return _window.Height
        End Get
        Set(value As Double)
            _window.Height = value
        End Set
    End Property

    'Public Property MaxHeight As Double Implements IWindowService.MaxHeight
    '    Get
    '        Return _window.MaxHeight
    '    End Get
    '    Set(value As Double)
    '        _window.MaxHeight = value
    '    End Set
    'End Property
    Public Property Left As Double Implements IWindowService.Left
        Get
            Return _window.Left
        End Get
        Set(value As Double)
            _window.Left = value
        End Set
    End Property

    Public Property Top As Double Implements IWindowService.Top
        Get
            Return _window.Top
        End Get
        Set(value As Double)
            _window.Top = value
        End Set
    End Property

    Public ReadOnly Property ActualHeight As Double Implements IWindowService.ActualHeight
        Get
            Return _window.ActualHeight
        End Get
    End Property
    Public ReadOnly Property ActualWidth As Double Implements IWindowService.ActualWidth
        Get
            Return _window.ActualWidth
        End Get
    End Property

    Public Property WindowStartupLocation As WindowStartupLocation Implements IWindowService.WindowStartupLocation
        Get
            Return _window.WindowStartupLocation
        End Get
        Set(value As WindowStartupLocation)
            _window.WindowStartupLocation = value
        End Set
    End Property
End Class
