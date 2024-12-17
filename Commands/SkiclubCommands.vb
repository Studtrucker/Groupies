
Namespace Commands

    Public Module SkiclubCommands

        Private ReadOnly _LeistungsstufeLoeschen As RoutedUICommand

        Sub New()
            _LeistungsstufeLoeschen = New RoutedUICommand("Leistungsstufe löschen", NameOf(LeistungsstufeLoeschen), GetType(SkiclubCommands))
            _LeistungsstufeLoeschen.InputGestures.Add(New KeyGesture(Key.Delete, ModifierKeys.Alt))
            LeistungsstufeNeuErstellen.InputGestures.Add(New KeyGesture(Key.N, ModifierKeys.Alt))
        End Sub



        ' TeilnehmerCommands
        Public ReadOnly Property TeilnehmerlisteImportieren As New RoutedUICommand("Teilnehmerliste importieren",
                                                                                         NameOf(TeilnehmerlisteImportieren),
                                                                                         GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerlisteExportierenXl As New RoutedUICommand("Teilnehmerliste exportieren",
                                                                                           NameOf(TeilnehmerlisteExportierenXl),
                                                                                           GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerInGruppeEinteilen As New RoutedUICommand("Teilnehmer in Gruppe einteilen",
                                                                                          NameOf(TeilnehmerInGruppeEinteilen),
                                                                                          GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerAusGruppeEntfernen As New RoutedUICommand("Teilnehmer aus Gruppe entfernen",
                                                                                           NameOf(TeilnehmerAusGruppeEntfernen),
                                                                                           GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerLeistungsbeurteilung As New RoutedUICommand("Teilnehmer beurteilen",
                                                                                             NameOf(TeilnehmerLeistungsbeurteilung),
                                                                                             GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerNeuErstellen As New RoutedUICommand("Teilnehmer hinzufügen",
                                                                                     NameOf(TeilnehmerNeuErstellen),
                                                                                     GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerSuchen As New RoutedUICommand("Teilnehmer suchen",
                                                                                     NameOf(TeilnehmerSuchen),
                                                                                     GetType(SkiclubCommands))
        Public ReadOnly Property TeilnehmerArchivieren As New RoutedUICommand("Teilnehmer archivieren",
                                                                                     NameOf(TeilnehmerArchivieren),
                                                                                     GetType(SkiclubCommands))


        ' TrainerCommands
        Public ReadOnly Property TrainerlisteImportieren As New RoutedUICommand("Trainer importieren",
                                                                                      NameOf(TrainerlisteImportieren),
                                                                                      GetType(SkiclubCommands))
        Public ReadOnly Property TrainerlisteExportierenXl As New RoutedUICommand("Trainer exportieren",
                                                                                        NameOf(TrainerlisteExportierenXl),
                                                                                        GetType(SkiclubCommands))
        Public ReadOnly Property TrainerInGruppeEinteilen As New RoutedUICommand("Gruppe einen Trainer zuweisen",
                                                                                       NameOf(TrainerInGruppeEinteilen),
                                                                                       GetType(SkiclubCommands))
        Public ReadOnly Property TrainerAusGruppeEntfernen As New RoutedUICommand("Trainer aus Gruppe entfernen",
                                                                                        NameOf(TrainerAusGruppeEntfernen),
                                                                                        GetType(SkiclubCommands))
        Public ReadOnly Property TrainerNeuErstellen As New RoutedUICommand("Trainer hinzufügen",
                                                                                   NameOf(TrainerNeuErstellen),
                                                                                   GetType(SkiclubCommands))
        Public ReadOnly Property TrainerBearbeiten As New RoutedUICommand("Trainer bearbeiten",
                                                                                  NameOf(TrainerBearbeiten),
                                                                                  GetType(SkiclubCommands))

        Public ReadOnly Property TrainerArchivieren As New RoutedUICommand("Trainer archivieren",
                                                                                  NameOf(TrainerArchivieren),
                                                                                  GetType(SkiclubCommands))



        ' GruppenCommands
        Public ReadOnly Property GruppeNeuErstellen As New RoutedUICommand("Gruppe erstellen",
                                                                                 NameOf(GruppeNeuErstellen),
                                                                                 GetType(SkiclubCommands))
        Public ReadOnly Property GruppeLoeschen As New RoutedUICommand("Gruppe löschen",
                                                                                 NameOf(GruppeLoeschen),
                                                                                 GetType(SkiclubCommands))
        Public ReadOnly Property GruppeSortieren As New RoutedUICommand("Gruppen sortieren",
                                                                                 NameOf(GruppeSortieren),
                                                                                 GetType(SkiclubCommands))

        ' FähigkeitenCommands
        Public ReadOnly Property FaehigkeitNeuErstellen As New RoutedUICommand("Neue Fähigkeit hinzufügen",
                                                                                 NameOf(FaehigkeitNeuErstellen),
                                                                                 GetType(SkiclubCommands))

        ' AllgemeineCommands

        Public ReadOnly Property DialogOk As New RoutedUICommand("Eingabe abschließen",
                                                                        NameOf(DialogOk),
                                                                        GetType(SkiclubCommands))

        Public ReadOnly Property DialogCancel As New RoutedUICommand("Eingabe abbrechen",
                                                                            NameOf(DialogCancel),
                                                                            GetType(SkiclubCommands))

        Public ReadOnly Property LeistungsstufeNeuErstellen As New RoutedUICommand("Leistungsstufe erstellen",
                                                                                         NameOf(LeistungsstufeNeuErstellen),
                                                                                         GetType(SkiclubCommands))

        Public ReadOnly Property LeistungsstufeLoeschen As RoutedUICommand
            Get
                Return _LeistungsstufeLoeschen
            End Get
        End Property

        Public ReadOnly Property NeuerSkill As New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))


    End Module
End Namespace
