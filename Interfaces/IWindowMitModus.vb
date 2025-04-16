
Namespace Interfaces


    Public Interface IWindowMitModus

        ''' <summary>
        ''' Der aktuelle, eingestellte Modus
        ''' </summary>
        ''' <returns></returns>
        Property Modus As Interfaces.IModus


        ''' <summary>
        ''' Die Methode, um das Formular auf den aktuellen Modus einzustellen
        ''' </summary>
        Sub ModusEinstellen()
        Sub Bearbeiten(Of T)(Original As T)


    End Interface

End Namespace
