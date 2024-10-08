
Namespace Commands

    Public Class SkiclubCommands
        Private Shared _importSkiclub = New RoutedUICommand("Skiclub importieren", "ImportSkiclub", GetType(SkiclubCommands))
        Private Shared _importParticipants = New RoutedUICommand("Teilnehmerliste importieren", "ImportParticipants", GetType(SkiclubCommands))
        Private Shared _importInstructors = New RoutedUICommand("Skilehrer importieren", "ImportInstructors", GetType(SkiclubCommands))
        Private Shared _beurteileTeilnehmerlevel = New RoutedUICommand("Teilnehmer beurteilen", "BeurteileTeilnehmerlevel", GetType(SkiclubCommands))
        Private Shared _neueGruppe = New RoutedUICommand("Gruppen erstellen", "NeueGruppe", GetType(SkiclubCommands))
        Private Shared _GruppeLoeschen = New RoutedUICommand("Gruppen löschen", "GruppeLoeschen", GetType(SkiclubCommands))
        Private Shared _neuerTeilnehmer = New RoutedUICommand("Teilnehmer hinzufügen", "NeuerTeilnehmer", GetType(SkiclubCommands))
        Private Shared _teilnehmerLoeschen = New RoutedUICommand("Teilnehmer löschen", "TeilnehmerLoeschen", GetType(SkiclubCommands))
        Private Shared _newInstructor = New RoutedUICommand("Skilehrer hinzufügen", "NeuerUebungsleiter", GetType(SkiclubCommands))
        Private Shared _uebungsleiterLoeschen = New RoutedUICommand("Skilehrer löschen", "UebungsleiterLoeschen", GetType(SkiclubCommands))
        Private Shared _neuesLevel = New RoutedUICommand("Level hinzufügen", "NeuesLevel", GetType(SkiclubCommands))
        Private Shared _levelLoeschen = New RoutedUICommand("Level löschen", "LevelLoeschen", GetType(SkiclubCommands))
        Private Shared _neuerSkill = New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))
        Private Shared _skillLoeschen = New RoutedUICommand("Fähigkeit entfernen", "SkillLoeschen", GetType(SkiclubCommands))
        Private Shared _dialogOk = New RoutedUICommand("Eingabe abschließen", NameOf(DialogOk), GetType(SkiclubCommands))
        Private Shared _dialogCancel = New RoutedUICommand("Eingabe abbrechen", NameOf(DialogCancel), GetType(SkiclubCommands))

        Sub New()
        End Sub

        Public Shared ReadOnly Property ImportSkiclub As RoutedUICommand
            Get
                Return _importSkiclub
            End Get
        End Property

        Public Shared ReadOnly Property ImportParticipants As RoutedUICommand
            Get
                Return _importParticipants
            End Get
        End Property

        Public Shared ReadOnly Property ImportInstructors As RoutedUICommand
            Get
                Return _importInstructors
            End Get
        End Property

        Public Shared ReadOnly Property BeurteileTeilnehmerlevel As RoutedUICommand
            Get
                Return _beurteileTeilnehmerlevel
            End Get
        End Property

        Public Shared ReadOnly Property GruppeLoeschen As RoutedUICommand
            Get
                Return _GruppeLoeschen
            End Get
        End Property

        Public Shared ReadOnly Property NeueGruppe As RoutedUICommand
            Get
                Return _neueGruppe
            End Get
        End Property

        Public Shared ReadOnly Property NewParticipant As RoutedUICommand
            Get
                Return _neuerTeilnehmer
            End Get
        End Property

        Public Shared ReadOnly Property TeilnehmerLoeschen As RoutedUICommand
            Get
                Return _teilnehmerLoeschen
            End Get
        End Property

        Public Shared ReadOnly Property NewInstructor As RoutedUICommand
            Get
                Return _newInstructor
            End Get
        End Property

        Public Shared ReadOnly Property UebungsleiterLoeschen As RoutedUICommand
            Get
                Return _uebungsleiterLoeschen
            End Get
        End Property

        Public Shared ReadOnly Property NeuesLevel As RoutedUICommand
            Get
                Return _neuesLevel
            End Get
        End Property

        Public Shared ReadOnly Property LevelLoeschen As RoutedUICommand
            Get
                Return _levelLoeschen
            End Get
        End Property

        Public Shared ReadOnly Property NeuerSkill As RoutedUICommand
            Get
                Return _neuerSkill
            End Get
        End Property

        Public Shared ReadOnly Property LoescheSkill As RoutedUICommand
            Get
                Return _skillLoeschen
            End Get
        End Property

        Public Shared ReadOnly Property DialogOk As RoutedUICommand
            Get
                Return _dialogOk
            End Get
        End Property

        Public Shared ReadOnly Property DialogCancel As RoutedUICommand
            Get
                Return _dialogCancel
            End Get
        End Property

    End Class
End Namespace
