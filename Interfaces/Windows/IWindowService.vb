Public Interface IWindowService
    Sub CloseWindow()
    Sub ShowWindow()
    Function ShowDialog() As Boolean

    Property DialogResult() As Nullable(Of Boolean)
    Property SizeToContent As SizeToContent
    Property WindowStartupLocation As WindowStartupLocation
    'Property MaxWidth As Double
    'Property MaxHeight As Double
    Property Width As Double
    Property Height As Double
    Property Left As Double
    Property Top As Double
    ReadOnly Property ActualWidth As Double
    ReadOnly Property ActualHeight As Double

End Interface
