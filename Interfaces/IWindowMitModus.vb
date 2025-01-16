
Namespace Interfaces


    Public Interface IWindowMitModus
        Property Modus As Interfaces.IModus
        Property Dialog As Boolean
        Sub ModusEinstellen()
        Sub HandleSchliessenButton(sender As Object, e As RoutedEventArgs)
    End Interface

End Namespace
