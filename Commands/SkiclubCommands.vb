
Namespace Commands

    Public Class SkiclubCommands

        ' TeilnehmerCommands
        Public Shared ReadOnly Property TeilnehmerlisteImportieren = New RoutedUICommand("Teilnehmerliste importieren",
                                                                                         NameOf(TeilnehmerlisteImportieren),
                                                                                         GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerlisteExportierenXl = New RoutedUICommand("Teilnehmerliste exportieren",
                                                                                           NameOf(TeilnehmerlisteExportierenXl),
                                                                                           GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerInGruppeEinteilen = New RoutedUICommand("Teilnehmer in Gruppe einteilen",
                                                                                          NameOf(TeilnehmerInGruppeEinteilen),
                                                                                          GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerAusGruppeEntfernen = New RoutedUICommand("Teilnehmer aus Gruppe entfernen",
                                                                                           NameOf(TeilnehmerAusGruppeEntfernen),
                                                                                           GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerLeistungsbeurteilung = New RoutedUICommand("Teilnehmer beurteilen",
                                                                                             NameOf(TeilnehmerLeistungsbeurteilung),
                                                                                             GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerNeuErstellen = New RoutedUICommand("Teilnehmer hinzufügen",
                                                                                     NameOf(TeilnehmerNeuErstellen),
                                                                                     GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerArchivieren = New RoutedUICommand("Teilnehmer archivieren",
                                                                                     NameOf(TeilnehmerArchivieren),
                                                                                     GetType(SkiclubCommands))


        ' TrainerCommands
        Public Shared ReadOnly Property TrainerlisteImportieren = New RoutedUICommand("Trainer importieren",
                                                                                      NameOf(TrainerlisteImportieren),
                                                                                      GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerlisteExportierenXl = New RoutedUICommand("Trainer exportieren",
                                                                                        NameOf(TrainerlisteExportierenXl),
                                                                                        GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerInGruppeEinteilen = New RoutedUICommand("Gruppe einen Trainer zuweisen",
                                                                                       NameOf(TrainerInGruppeEinteilen),
                                                                                       GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerAusGruppeEntfernen = New RoutedUICommand("Trainer aus Gruppe entfernen",
                                                                                        NameOf(TrainerAusGruppeEntfernen),
                                                                                        GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerNeuErstellen = New RoutedUICommand("Trainer hinzufügen",
                                                                                  NameOf(TrainerNeuErstellen),
                                                                                  GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerArchivieren = New RoutedUICommand("Trainer archivieren",
                                                                                  NameOf(TrainerArchivieren),
                                                                                  GetType(SkiclubCommands))



        ' GruppenCommands
        Public Shared ReadOnly Property GruppeNeuErstellen = New RoutedUICommand("Gruppen erstellen",
                                                                                 NameOf(GruppeNeuErstellen),
                                                                                 GetType(SkiclubCommands))


        ' AllgemeineCommands

        Public Shared ReadOnly Property DialogOk As New RoutedUICommand("Eingabe abschließen",
                                                                        NameOf(DialogOk),
                                                                        GetType(SkiclubCommands))

        Public Shared ReadOnly Property DialogCancel As New RoutedUICommand("Eingabe abbrechen",
                                                                            NameOf(DialogCancel),
                                                                            GetType(SkiclubCommands))






        ' Alte Commands
        Public Shared ReadOnly Property ImportSkiclub As New RoutedUICommand("Skiclub importieren", "ImportSkiclub", GetType(SkiclubCommands))
        Public Shared ReadOnly Property GruppeLoeschen As New RoutedUICommand("Gruppen löschen", "GruppeLoeschen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerLoeschen As New RoutedUICommand("Teilnehmer löschen", "TeilnehmerLoeschen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerLoeschen As New RoutedUICommand("Trainer löschen", "TrainerLoeschen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property NeuesLevel As New RoutedUICommand("Level hinzufügen", "NeuesLevel", GetType(SkiclubCommands))
        Public Shared ReadOnly Property LevelLoeschen As New RoutedUICommand("Level löschen", "LevelLoeschen", GetType(SkiclubCommands))
        Public Shared ReadOnly Property NeuerSkill As New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))


    End Class
End Namespace
