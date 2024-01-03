
Namespace Commands

    Public Module SkiclubCommands
        Private ReadOnly _importSkiclub As RoutedUICommand
        Private ReadOnly _importParticipants As RoutedUICommand
        Private ReadOnly _importInstructors As RoutedUICommand
        Private ReadOnly _beurteileTeilnehmerlevel As RoutedUICommand
        Private ReadOnly _neueGruppe As RoutedUICommand
        Private ReadOnly _GruppeLoeschen As RoutedUICommand
        Private ReadOnly _neuerTeilnehmer As RoutedUICommand
        Private ReadOnly _teilnehmerLoeschen As RoutedUICommand
        Private ReadOnly _neuerUebungsleiter As RoutedUICommand
        Private ReadOnly _uebungsleiterLoeschen As RoutedUICommand
        Private ReadOnly _neuesLevel As RoutedUICommand
        Private ReadOnly _levelLoeschen As RoutedUICommand
        Private ReadOnly _neuerSkill As RoutedUICommand
        Private ReadOnly _skillLoeschen As RoutedUICommand
        Private ReadOnly _dialogOk As RoutedUICommand
        Private ReadOnly _dialogCancel As RoutedUICommand

        Sub New()
            _importSkiclub = New RoutedUICommand("Skiclub importieren", "ImportSkiclub", GetType(SkiclubCommands))
            _importParticipants = New RoutedUICommand("Teilnehmerliste importieren", "ImportParticipants", GetType(SkiclubCommands))
            _importInstructors = New RoutedUICommand("Skilehrer importieren", "ImportInstructors", GetType(SkiclubCommands))
            _beurteileTeilnehmerlevel = New RoutedUICommand("Teilnehmer beurteilen", "BeurteileTeilnehmerlevel", GetType(SkiclubCommands))
            _neueGruppe = New RoutedUICommand("Gruppen erstellen", "NeueGruppe", GetType(SkiclubCommands))
            _GruppeLoeschen = New RoutedUICommand("Gruppen löschen", "GruppeLoeschen", GetType(SkiclubCommands))
            _neuerTeilnehmer = New RoutedUICommand("Teilnehmer hinzufügen", "NeuerTeilnehmer", GetType(SkiclubCommands))
            _teilnehmerLoeschen = New RoutedUICommand("Teilnehmer löschen", "TeilnehmerLoeschen", GetType(SkiclubCommands))
            _neuerUebungsleiter = New RoutedUICommand("Skilehrer hinzufügen", "NeuerUebungsleiter", GetType(SkiclubCommands))
            _uebungsleiterLoeschen = New RoutedUICommand("Skilehrer löschen", "UebungsleiterLoeschen", GetType(SkiclubCommands))
            _neuesLevel = New RoutedUICommand("Level hinzufügen", "NeuesLevel", GetType(SkiclubCommands))
            _levelLoeschen = New RoutedUICommand("Level löschen", "LevelLoeschen", GetType(SkiclubCommands))
            _neuerSkill = New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))
            _skillLoeschen = New RoutedUICommand("Fähigkeit entfernen", "SkillLoeschen", GetType(SkiclubCommands))
            _dialogOk = New RoutedUICommand("Eingabe abschließen", NameOf(DialogOk), GetType(SkiclubCommands))
            _dialogCancel = New RoutedUICommand("Eingabe abbrechen", NameOf(DialogCancel), GetType(SkiclubCommands))
        End Sub

        Public ReadOnly Property ImportSkiclub As RoutedUICommand
            Get
                Return _importSkiclub
            End Get
        End Property

        Public ReadOnly Property ImportParticipants As RoutedUICommand
            Get
                Return _importParticipants
            End Get
        End Property

        Public ReadOnly Property ImportInstructors As RoutedUICommand
            Get
                Return _importInstructors
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

        Public ReadOnly Property NeuerSkill As RoutedUICommand
            Get
                Return _neuerSkill
            End Get
        End Property

        Public ReadOnly Property LoescheSkill As RoutedUICommand
            Get
                Return _skillLoeschen
            End Get
        End Property

        Public ReadOnly Property DialogOk As RoutedUICommand
            Get
                Return _dialogOk
            End Get
        End Property

        Public ReadOnly Property DialogCancel As RoutedUICommand
            Get
                Return _dialogCancel
            End Get
        End Property

    End Module
End Namespace
