
Namespace Commands

    Public Class SkiclubCommands


        ' Neue Commands
        Public Shared ReadOnly Property TeilnehmerInGruppeEinteilen As New RoutedUICommand("Teilnehmer in Gruppe einteilen", "TeilnehmerInGruppeEinteilen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerAusGruppeEntfernen As New RoutedUICommand("Teilnehmer aus Gruppe entfernen", "TeilnehmerAusGruppeEntfernen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property GruppeEinenTrainerZuweisen As New RoutedUICommand("Gruppe einen Trainer zuweisen", "GruppeEinenTrainerZuweisen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property GruppentrainerEntfernen As New RoutedUICommand("Trainer aus Gruppe entfernen", "GruppentrainerEntfernen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property ImportTeilnehmer As New RoutedUICommand("Teilnehmerliste importieren", "ImportTeilnehmer", GetType(SkiclubCommands))
        Public Shared ReadOnly Property ImportTrainer As New RoutedUICommand("Trainer importieren", "ImportTrainer", GetType(SkiclubCommands))
        Public Shared ReadOnly Property ExportXlTeilnehmer As New RoutedUICommand("Teilnehmerliste exportieren", "ExportXlTeilnehmer", GetType(SkiclubCommands))
        Public Shared ReadOnly Property ExportXlTrainer As New RoutedUICommand("Trainer exportieren", "ExportXlTrainer", GetType(SkiclubCommands))


        ' Alte Commands
        Public Shared ReadOnly Property ImportSkiclub As New RoutedUICommand("Skiclub importieren", "ImportSkiclub", GetType(SkiclubCommands))


        Public Shared ReadOnly Property BeurteileTeilnehmerlevel As New RoutedUICommand("Teilnehmer beurteilen", "BeurteileTeilnehmerlevel", GetType(SkiclubCommands))

        Public Shared ReadOnly Property GruppeLoeschen As New RoutedUICommand("Gruppen löschen", "GruppeLoeschen", GetType(SkiclubCommands))

        Public Shared ReadOnly Property NeueGruppe As New RoutedUICommand("Gruppen erstellen", "NeueGruppe", GetType(SkiclubCommands))

        Public Shared ReadOnly Property NeuerTeilnehmer As New RoutedUICommand("Teilnehmer hinzufügen", "NeuerTeilnehmer", GetType(SkiclubCommands))

        Public Shared ReadOnly Property TeilnehmerLoeschen As New RoutedUICommand("Teilnehmer löschen", "TeilnehmerLoeschen", GetType(SkiclubCommands))

        Public Shared ReadOnly Property NeuerTrainer As New RoutedUICommand("Trainer hinzufügen", "NeuerTrainer", GetType(SkiclubCommands))

        Public Shared ReadOnly Property TrainerLoeschen As New RoutedUICommand("Trainer löschen", "TrainerLoeschen", GetType(SkiclubCommands))

        Public Shared ReadOnly Property NeuesLevel As New RoutedUICommand("Level hinzufügen", "NeuesLevel", GetType(SkiclubCommands))

        Public Shared ReadOnly Property LevelLoeschen As New RoutedUICommand("Level löschen", "LevelLoeschen", GetType(SkiclubCommands))

        Public Shared ReadOnly Property NeuerSkill As New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))

        Public Shared ReadOnly Property LoescheSkill As New RoutedUICommand("Fähigkeit entfernen", "SkillLoeschen", GetType(SkiclubCommands))

        Public Shared ReadOnly Property DialogOk As New RoutedUICommand("Eingabe abschließen", NameOf(DialogOk), GetType(SkiclubCommands))

        Public Shared ReadOnly Property DialogCancel As New RoutedUICommand("Eingabe abbrechen", NameOf(DialogCancel), GetType(SkiclubCommands))

    End Class
End Namespace
