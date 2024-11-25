
Namespace Commands

    Public Class SkiclubCommands

        ' TeilnehmerCommands
        Public Shared ReadOnly Property TeilnehmerlisteImportieren As New RoutedUICommand("Teilnehmerliste importieren",
                                                                                         NameOf(TeilnehmerlisteImportieren),
                                                                                         GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerlisteExportierenXl As New RoutedUICommand("Teilnehmerliste exportieren",
                                                                                           NameOf(TeilnehmerlisteExportierenXl),
                                                                                           GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerInGruppeEinteilen As New RoutedUICommand("Teilnehmer in Gruppe einteilen",
                                                                                          NameOf(TeilnehmerInGruppeEinteilen),
                                                                                          GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerAusGruppeEntfernen As New RoutedUICommand("Teilnehmer aus Gruppe entfernen",
                                                                                           NameOf(TeilnehmerAusGruppeEntfernen),
                                                                                           GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerLeistungsbeurteilung As New RoutedUICommand("Teilnehmer beurteilen",
                                                                                             NameOf(TeilnehmerLeistungsbeurteilung),
                                                                                             GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerNeuErstellen As New RoutedUICommand("Teilnehmer hinzufügen",
                                                                                     NameOf(TeilnehmerNeuErstellen),
                                                                                     GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerSuchen As New RoutedUICommand("Teilnehmer suchen",
                                                                                     NameOf(TeilnehmerSuchen),
                                                                                     GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerArchivieren As New RoutedUICommand("Teilnehmer archivieren",
                                                                                     NameOf(TeilnehmerArchivieren),
                                                                                     GetType(SkiclubCommands))


        ' TrainerCommands
        Public Shared ReadOnly Property TrainerlisteImportieren As New RoutedUICommand("Trainer importieren",
                                                                                      NameOf(TrainerlisteImportieren),
                                                                                      GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerlisteExportierenXl As New RoutedUICommand("Trainer exportieren",
                                                                                        NameOf(TrainerlisteExportierenXl),
                                                                                        GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerInGruppeEinteilen As New RoutedUICommand("Gruppe einen Trainer zuweisen",
                                                                                       NameOf(TrainerInGruppeEinteilen),
                                                                                       GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerAusGruppeEntfernen As New RoutedUICommand("Trainer aus Gruppe entfernen",
                                                                                        NameOf(TrainerAusGruppeEntfernen),
                                                                                        GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerNeuErstellen As New RoutedUICommand("Trainer hinzufügen",
                                                                                   NameOf(TrainerNeuErstellen),
                                                                                   GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerBearbeiten As New RoutedUICommand("Trainer bearbeiten",
                                                                                  NameOf(TrainerBearbeiten),
                                                                                  GetType(SkiclubCommands))

        Public Shared ReadOnly Property TrainerArchivieren As New RoutedUICommand("Trainer archivieren",
                                                                                  NameOf(TrainerArchivieren),
                                                                                  GetType(SkiclubCommands))



        ' GruppenCommands
        Public Shared ReadOnly Property GruppeNeuErstellen As New RoutedUICommand("Gruppe erstellen",
                                                                                 NameOf(GruppeNeuErstellen),
                                                                                 GetType(SkiclubCommands))
        Public Shared ReadOnly Property GruppeLoeschen As New RoutedUICommand("Gruppe löschen",
                                                                                 NameOf(GruppeLoeschen),
                                                                                 GetType(SkiclubCommands))
        Public Shared ReadOnly Property GruppeSortieren As New RoutedUICommand("Gruppen sortieren",
                                                                                 NameOf(GruppeSortieren),
                                                                                 GetType(SkiclubCommands))

        ' FähigkeitenCommands
        Public Shared ReadOnly Property FaehigkeitNeuErstellen As New RoutedUICommand("Fähigkeit erstellen",
                                                                                 NameOf(FaehigkeitNeuErstellen),
                                                                                 GetType(SkiclubCommands))

        ' AllgemeineCommands

        Public Shared ReadOnly Property DialogOk As New RoutedUICommand("Eingabe abschließen",
                                                                        NameOf(DialogOk),
                                                                        GetType(SkiclubCommands))

        Public Shared ReadOnly Property DialogCancel As New RoutedUICommand("Eingabe abbrechen",
                                                                            NameOf(DialogCancel),
                                                                            GetType(SkiclubCommands))

        Public Shared ReadOnly Property LeistungsstufeNeuErstellen As New RoutedUICommand("Leistungsstufe erstellen",
                                                                                         NameOf(LeistungsstufeNeuErstellen),
                                                                                         GetType(SkiclubCommands))
        Public Shared ReadOnly Property LeistungsstufeLoeschen As New RoutedUICommand("Leistungsstufe löschen",
                                                                                         NameOf(LeistungsstufeLoeschen),
                                                                                         GetType(SkiclubCommands))



        Public Shared ReadOnly Property NeuerSkill As New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))


    End Class
End Namespace
