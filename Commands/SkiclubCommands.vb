Public Module SkiclubCommands
    Private ReadOnly _importiereTeilnehmer As RoutedUICommand
    Private ReadOnly _beurteileTeilnehmerlevel As RoutedUICommand
    Private ReadOnly _neueGruppe As RoutedUICommand
    Private ReadOnly _GruppeLoeschen As RoutedUICommand
    Private ReadOnly _neuerTeilnehmer As RoutedUICommand
    Private ReadOnly _teilnehmerLoeschen As RoutedUICommand
    Private ReadOnly _neuerUebungsleiter As RoutedUICommand
    Private ReadOnly _uebungsleiterLoeschen As RoutedUICommand
    Private ReadOnly _neuesLevel As RoutedUICommand
    Private ReadOnly _levelLoeschen As RoutedUICommand

    Sub New()
        _importiereTeilnehmer = New RoutedUICommand("Teilnehmerliste importieren", "ImportTeilnehmerliste", GetType(SkiclubCommands))
        _beurteileTeilnehmerlevel = New RoutedUICommand("Teilnehmer beurteilen", "BeurteileTeilnehmerlevel", GetType(SkiclubCommands))
        _neueGruppe = New RoutedUICommand("Gruppen erstellen", "NeueGruppe", GetType(SkiclubCommands))
        _GruppeLoeschen = New RoutedUICommand("Gruppen löschen", "GruppeLoeschen", GetType(SkiclubCommands))
        _neuerTeilnehmer = New RoutedUICommand("Teilnehmer hinzufügen", "NeuerTeilnehmer", GetType(SkiclubCommands))
        _teilnehmerLoeschen = New RoutedUICommand("Teilnehmer löschen", "TeilnehmerLoeschen", GetType(SkiclubCommands))
        _neuerUebungsleiter = New RoutedUICommand("Skilehrer hinzufügen", "NeuerUebungsleiter", GetType(SkiclubCommands))
        _uebungsleiterLoeschen = New RoutedUICommand("Skilehrer löschen", "UebungsleiterLoeschen", GetType(SkiclubCommands))
        _neuesLevel = New RoutedUICommand("Level hinzufügen", "NeuesLevel", GetType(SkiclubCommands))
        _levelLoeschen = New RoutedUICommand("Level löschen", "LevelLoeschen", GetType(SkiclubCommands))
    End Sub

    Public ReadOnly Property ImportTeilnehmerliste As RoutedUICommand
        Get
            Return _importiereTeilnehmer
        End Get
    End Property

    Public ReadOnly Property BeurteileTeilnehmerlevel As RoutedUICommand
        Get
            Return _beurteileTeilnehmerlevel
        End Get
    End Property

    Public ReadOnly Property GruppeLoeschen As RoutedUICommand
        Get
            Return _GruppeLoeschen
        End Get
    End Property

    Public ReadOnly Property NeueGruppe As RoutedUICommand
        Get
            Return _neueGruppe
        End Get
    End Property

    Public ReadOnly Property NewParticipant As RoutedUICommand
        Get
            Return _neuerTeilnehmer
        End Get
    End Property

    Public ReadOnly Property TeilnehmerLoeschen As RoutedUICommand
        Get
            Return _teilnehmerLoeschen
        End Get
    End Property

    Public ReadOnly Property NeuerUebungsleiter As RoutedUICommand
        Get
            Return _neuerUebungsleiter
        End Get
    End Property

    Public ReadOnly Property UebungsleiterLoeschen As RoutedUICommand
        Get
            Return _uebungsleiterLoeschen
        End Get
    End Property

    Public ReadOnly Property NeuesLevel As RoutedUICommand
        Get
            Return _neuesLevel
        End Get
    End Property

    Public ReadOnly Property LevelLoeschen As RoutedUICommand
        Get
            Return _levelLoeschen
        End Get
    End Property
End Module
