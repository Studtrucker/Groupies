Public Module SkischuleBefehle
    Private ReadOnly _importiereTeilnehmer As RoutedUICommand
    Private ReadOnly _beurteileTeilnehmerlevel As RoutedUICommand
    Private ReadOnly _neueGruppe As RoutedUICommand
    Private ReadOnly _GruppeLoeschen As RoutedUICommand
    Private ReadOnly _neuerTeilnehmer As RoutedUICommand
    Private ReadOnly _teilnehmerLoeschen As RoutedUICommand
    Private ReadOnly _neuerUebungsleiter As RoutedUICommand
    Private ReadOnly _uebungsleiterLoeschen As RoutedUICommand

    Sub New()
        _importiereTeilnehmer = New RoutedUICommand("Teilnehmerliste importieren", "ImportTeilnehmerliste", GetType(SkischuleBefehle))
        _beurteileTeilnehmerlevel = New RoutedUICommand("Teilnehmer beurteilen", "BeurteileTeilnehmerlevel", GetType(SkischuleBefehle))
        _neueGruppe = New RoutedUICommand("Gruppen erstellen", "NeueGruppe", GetType(SkischuleBefehle))
        _GruppeLoeschen = New RoutedUICommand("Gruppen löschen", "GruppeLoeschen", GetType(SkischuleBefehle))
        _neuerTeilnehmer = New RoutedUICommand("Teilnehmer hinzufügen", "NeuerTeilnehmer", GetType(SkischuleBefehle))
        _teilnehmerLoeschen = New RoutedUICommand("Teilnehmer löschen", "TeilnehmerLoeschen", GetType(SkischuleBefehle))
        _neuerUebungsleiter = New RoutedUICommand("Skilehrer hinzufügen", "NeuerUebungsleiter", GetType(SkischuleBefehle))
        _uebungsleiterLoeschen = New RoutedUICommand("Skilehrer löschen", "UebungsleiterLoeschen", GetType(SkischuleBefehle))
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

    Public ReadOnly Property NeuerTeilnehmer As RoutedUICommand
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
End Module
