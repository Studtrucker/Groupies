Public Interface IWindowService
    Sub CloseWindow()
    Sub ShowWindow()
    Function ShowDialog() As Boolean

    Property DialogResult() As Nullable(Of Boolean)
    Property SizeToContent As SizeToContent
    Property Width As Double
    Property Height As Double
    Property Left As Double
    Property Top As Double
End Interface
