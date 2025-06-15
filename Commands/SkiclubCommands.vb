
Namespace Commands

    Public Class SkiclubCommands

        Private Shared _LeistungsstufeLoeschen As New RoutedUICommand("Leistungsstufe löschen", NameOf(LeistungsstufeLoeschen), GetType(SkiclubCommands))
        Private Shared _LeistungsstufeNeuErstellen As New RoutedUICommand("Leistungsstufe erstellen", NameOf(LeistungsstufeNeuErstellen), GetType(SkiclubCommands))
        Private Shared _LeistungsstufeEinlesen As New RoutedUICommand("Leistungsstufe einlesen", NameOf(LeistungsstufeEinlesen), GetType(SkiclubCommands))
        Private Shared _EinteilungNeuErstellen As New RoutedUICommand("Einteilung erstellen", NameOf(EinteilungNeuErstellen), GetType(SkiclubCommands))
        Private Shared _EinteilungEinlesen As New RoutedUICommand("Einteilung einlesen", NameOf(EinteilungEinlesen), GetType(SkiclubCommands))
        Private Shared _EinteilungLoeschen As New RoutedUICommand("Einteilung löschen", NameOf(EinteilungLoeschen), GetType(SkiclubCommands))
        Private Shared _EinteilungKopieren As New RoutedUICommand("Einteilung Kopieren", NameOf(EinteilungKopieren), GetType(SkiclubCommands))


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
        Public Shared ReadOnly Property TeilnehmerNeuErstellen As New RoutedUICommand("Teilnehmer erstellen",
                                                                                     NameOf(TeilnehmerNeuErstellen),
                                                                                     GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerBearbeiten As New RoutedUICommand("Teilnehmer bearbeiten",
                                                                                     NameOf(TeilnehmerBearbeiten),
                                                                                     GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerLoeschen As New RoutedUICommand("Teilnehmer löschen",
                                                                                     NameOf(TeilnehmerLoeschen),
                                                                                     GetType(SkiclubCommands))
        Public Shared ReadOnly Property TeilnehmerEinlesen As New RoutedUICommand("Teilnehmer einlesen",
                                                                                     NameOf(TeilnehmerEinlesen),
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
        Public Shared ReadOnly Property TrainerInGruppeEinteilen As New RoutedUICommand("Trainer der Gruppe zuweisen",
                                                                                       NameOf(TrainerInGruppeEinteilen),
                                                                                       GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerAusGruppeEntfernen As New RoutedUICommand("Trainer aus Gruppe entfernen",
                                                                                        NameOf(TrainerAusGruppeEntfernen),
                                                                                        GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerNeuErstellen As New RoutedUICommand("Trainer erstellen",
                                                                                   NameOf(TrainerNeuErstellen),
                                                                                   GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerBearbeiten As New RoutedUICommand("Trainer bearbeiten",
                                                                                  NameOf(TrainerBearbeiten),
                                                                                  GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerLoeschen As New RoutedUICommand("Trainer löschen",
                                                                                  NameOf(TrainerLoeschen),
                                                                                  GetType(SkiclubCommands))
        Public Shared ReadOnly Property TrainerEinlesen As New RoutedUICommand("Trainer einlesen",
                                                                                  NameOf(TrainerEinlesen),
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
        Public Shared ReadOnly Property FaehigkeitNeuErstellen As New RoutedUICommand("Neue Fähigkeit hinzufügen",
                                                                                 NameOf(FaehigkeitNeuErstellen),
                                                                                 GetType(SkiclubCommands))
        Public Shared ReadOnly Property FaehigkeitEinlesen As New RoutedUICommand("Fähigkeit einlesen",
                                                                                 NameOf(FaehigkeitEinlesen),
                                                                                 GetType(SkiclubCommands))

        ' AllgemeineCommands

        Public Shared ReadOnly Property DialogOk As New RoutedUICommand("Eingabe abschließen",
                                                                        NameOf(DialogOk),
                                                                        GetType(SkiclubCommands))

        Public Shared ReadOnly Property DialogCancel As New RoutedUICommand("Eingabe abbrechen",
                                                                            NameOf(DialogCancel),
                                                                            GetType(SkiclubCommands))

        Public Shared ReadOnly Property EinteilungNeuErstellen As RoutedUICommand
            Get
                _EinteilungNeuErstellen.InputGestures.Add(New KeyGesture(Key.E, ModifierKeys.Alt))
                Return _EinteilungNeuErstellen
            End Get
        End Property

        Public Shared ReadOnly Property EinteilungLoeschen As RoutedUICommand
            Get
                Return _EinteilungLoeschen
            End Get
        End Property
        Public Shared ReadOnly Property EinteilungKopieren As RoutedUICommand
            Get
                Return _EinteilungKopieren
            End Get
        End Property


        Public Shared ReadOnly Property LeistungsstufeNeuErstellen As RoutedUICommand
            Get
                _LeistungsstufeNeuErstellen.InputGestures.Add(New KeyGesture(Key.N, ModifierKeys.Alt))
                Return _LeistungsstufeNeuErstellen
            End Get
        End Property


        Public Shared ReadOnly Property LeistungsstufeLoeschen As RoutedUICommand
            Get
                _LeistungsstufeLoeschen.InputGestures.Add(New KeyGesture(Key.Delete, ModifierKeys.Alt))
                Return _LeistungsstufeLoeschen
            End Get
        End Property
        Public Shared ReadOnly Property LeistungsstufeEinlesen As RoutedUICommand
            Get
                Return _LeistungsstufeEinlesen
            End Get
        End Property

        Public Shared ReadOnly Property EinteilungEinlesen As RoutedUICommand
            Get
                Return _EinteilungEinlesen
            End Get
        End Property

        Public Shared ReadOnly Property NeuerSkill As New RoutedUICommand("Neue Fähigkeit hinzufügen", "NeuerSkill", GetType(SkiclubCommands))


    End Class
End Namespace
