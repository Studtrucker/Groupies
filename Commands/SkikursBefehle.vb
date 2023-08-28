Public Module SkikursBefehle
    Private ReadOnly _importiereTeilnehmer As RoutedUICommand
    Private ReadOnly _beurteileTeilnehmerKoennen As RoutedUICommand
    Private ReadOnly _erstelleGruppen As RoutedUICommand

    Sub New()
        _importiereTeilnehmer = New RoutedUICommand("Teilnehmerliste importieren", "ImportTeilnehmerliste", GetType(SkikursBefehle))
        _beurteileTeilnehmerKoennen = New RoutedUICommand("Teilnehmer beurteilen", "BeurteileTeilnehmerkoennen", GetType(SkikursBefehle))
        _erstelleGruppen = New RoutedUICommand("Gruppen erstellen", "ErstelleGruppen", GetType(SkikursBefehle))
    End Sub

    Public ReadOnly Property ImportTeilnehmerliste As RoutedUICommand
        Get
            Return _importiereTeilnehmer
        End Get
    End Property

    Public ReadOnly Property BeurteileTeilnehmerkoennen As RoutedUICommand
        Get
            Return _beurteileTeilnehmerKoennen
        End Get
    End Property

    Public ReadOnly Property ErstelleGruppen As RoutedUICommand
        Get
            Return _erstelleGruppen
        End Get
    End Property

End Module
