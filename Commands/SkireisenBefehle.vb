Public Module SkireisenBefehle
    Private ReadOnly _importiereTeilnehmer As RoutedUICommand
    Private ReadOnly _teilnehmerKoennenstufeVergabe As RoutedUICommand

    Sub New()
        _importiereTeilnehmer = New RoutedUICommand("Teilnehmerliste importieren", "ImportTeilnehmerliste", GetType(SkireisenBefehle))
        _teilnehmerKoennenstufeVergabe = New RoutedUICommand("Levelbeurteilung Teilnehmer", "TeilnehmerKoennenstufeVergabe", GetType(SkireisenBefehle))
    End Sub

    Public ReadOnly Property ImportTeilnehmerliste As RoutedUICommand
        Get
            Return _importiereTeilnehmer
        End Get
    End Property

    Public ReadOnly Property TeilnehmerKoennenstufeVergabe As RoutedUICommand
        Get
            Return _teilnehmerKoennenstufeVergabe
        End Get
    End Property

End Module
