Public Interface IWindowService
    Sub CloseWindow()
    Sub ShowWindow()
    Function ShowDialog() As Boolean

    Property DialogResult() As Nullable(Of Boolean)

End Interface
