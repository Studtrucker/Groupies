Namespace Services

    ''' <summary>
    ''' Einfacher Service-Provider / Singleton-Holder für zentrale Service-Instanzen.
    ''' Ermöglicht zentralen Zugriff und einfache Initialisierung der Dienste.
    ''' </summary>
    Public NotInheritable Class ServiceProvider

        Private Sub New()
        End Sub

        Private Shared _dateiService As DateiService

        ''' <summary>
        ''' Zentrale DateiService-Instanz. Wird lazy erzeugt, wenn nicht gesetzt.
        ''' Kann bei Programmstart mit einer bereits konfigurierten Instanz initialisiert werden.
        ''' </summary>
        Public Shared Property DateiService As DateiService
            Get
                If _dateiService Is Nothing Then
                    _dateiService = New DateiService()
                End If
                Return _dateiService
            End Get
            Set(value As DateiService)
                _dateiService = value
            End Set
        End Property

    End Class
End Namespace