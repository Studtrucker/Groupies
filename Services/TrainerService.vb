Imports Groupies.Entities.Generation4

Namespace Services

    Public Class TrainerService

        Public Shared Event TrainerGeaendert As EventHandler(Of TrainerEventArgs)

        Public Sub New()
        End Sub

        Protected Overridable Sub OnTrainerGeaendert(e As TrainerEventArgs)
            RaiseEvent TrainerGeaendert(Me, e)
        End Sub

        ''' <summary>
        ''' Trainer in Gruppe einteilen und aus VerfügbareTrainer entfernen
        ''' </summary>
        ''' <param name="NeuerTrainerID"></param>
        ''' <param name="GruppenID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerInGruppeEinteilen(NeuerTrainerID As Guid, GruppenID As Guid, EinteilungID As Guid)

            ' Aktuellen Trainer und Neuer Trainer auslesen
            Dim NeuerTrainer = TrainerLesen(NeuerTrainerID)
            Dim AktuellerTrainer = TrainerLesen(DateiService.AktuellerClub.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.TrainerID)

            ' Wenn bereits ein Trainer zugewiesen ist, diesen zuerst entfernen
            If AktuellerTrainer IsNot Nothing Then
                ' Aktueller Trainer ist nun entfernt und befindet sich in der VerfügbarenTrainerliste
                TrainerAusGruppeEntfernen(AktuellerTrainer.TrainerID, GruppenID, EinteilungID)
            End If

            ' In TrainerID und Trainer in Gruppe schreiben ...
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.TrainerID = NeuerTrainerID
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.Trainer = NeuerTrainer

            ' ... aus VerfügbareTrainer entfernen
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Remove(NeuerTrainer)
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Remove(NeuerTrainerID)

            OnTrainerGeaendert(New TrainerEventArgs(NeuerTrainer))

        End Sub

        ''' <summary>
        ''' Trainer aus Gruppe entfernen und in VerfügbareTrainer kopieren
        ''' </summary>
        ''' <param name="TrainerID"></param>
        ''' <param name="GruppenID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerAusGruppeEntfernen(TrainerID As Guid, GruppenID As Guid, EinteilungID As Guid)
            'Public Sub TrainerAusGruppeEntfernen(AktuellerTrainerID As Guid, GruppenID As Guid, EinteilungID As Guid)

            ' Trainer auslesen
            Dim AktuellerTrainer = TrainerLesen(TrainerID)
            ' Trainer entfernen

            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.Trainer = Nothing
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.TrainerID = Nothing

            ' In VerfügbareTrainer kopieren
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Add(AktuellerTrainer)
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Add(AktuellerTrainer.TrainerID)

            OnTrainerGeaendert(TrainerEventArgs.Empty)

        End Sub


        ''' <summary>
        ''' Trainer aus Einteilung und aus den Gruppen entfernen
        ''' </summary>
        ''' <param name="TrainerID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerAusEinteilungEntfernen(TrainerID As Guid, EinteilungID As Guid)

            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Remove(TrainerLesen(TrainerID))
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Remove(TrainerID)
            DateiService.AktuellerClub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(G)
                                                                                                                If G.TrainerID = TrainerID Then
                                                                                                                    G.Trainer = Nothing
                                                                                                                    G.TrainerID = Nothing
                                                                                                                End If
                                                                                                            End Sub))

            OnTrainerGeaendert(TrainerEventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Trainerliste aus Einteilung und aus den Gruppen entfernen
        ''' </summary>
        ''' <param name="TrainerListe"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerAusEinteilungEntfernen(TrainerListe As IList(Of Trainer), EinteilungID As Guid)
            ' Todo: warum Trainerliste beinhaltet keine Trainer
            Dim Einteilung = DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single

            Dim alleTrainer = If(DateiService.AktuellerClub, Nothing)?.Trainerliste
            If alleTrainer Is Nothing Then
                ' Wenn keine Teilnehmerliste vorhanden ist, die gleiche Anzahl an Nothing-Einträgen zurückgeben
                Dim empties As New List(Of Trainer)(TrainerListe.Count)
                For Each item In TrainerListe
                    empties.Add(Nothing)
                Next
            End If

            ' Dictionary einmal aufbauen für O(1)-Lookups
            Dim lookup As New Dictionary(Of Guid, Trainer)(alleTrainer.Count)
            For Each t In alleTrainer
                If Not lookup.ContainsKey(t.TrainerID) Then
                    lookup.Add(t.TrainerID, t)
                End If
            Next

            ' Aus der VerfuegbareTrainerListe entfernen
            For Each t In TrainerListe
                Einteilung.VerfuegbareTrainerIDListe.Remove(t.TrainerID)
                Einteilung.VerfuegbareTrainerListe.Remove(t)
                ' ... und aus allen Gruppen entfernen
                For Each g In Einteilung.Gruppenliste
                    g.TrainerID = Nothing
                    g.Trainer = Nothing
                Next
            Next

            OnTrainerGeaendert(TrainerEventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Trainer aus Trainerliste in Einteilung.VerfügbareTrainerListe hinzugefügt.
        ''' </summary>
        ''' <param name="TrainerID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerEinteilungHinzufuegen(TrainerID As Guid, EinteilungID As Guid)

            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Add(TrainerLesen(TrainerID))
            DateiService.AktuellerClub.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Add(TrainerID)

            OnTrainerGeaendert(New TrainerEventArgs(TrainerLesen(TrainerID)))

        End Sub

        '''' <summary>
        '''' Vorhandener Trainer wird bearbeitet
        '''' </summary>
        '''' <param name="OriginalTrainerID"></param>
        'Public Sub TrainerBearbeiten(OriginalTrainerID As Guid, NeueTrainerdaten As Trainer)
        '    Dim ZuBearbeitenderTrainer = TrainerLesen(OriginalTrainerID)
        '    ZuBearbeitenderTrainer.Vorname = NeueTrainerdaten.Vorname
        '    ZuBearbeitenderTrainer.Nachname = NeueTrainerdaten.Nachname
        '    ZuBearbeitenderTrainer.Telefonnummer = NeueTrainerdaten.Telefonnummer
        '    ZuBearbeitenderTrainer.EMail = NeueTrainerdaten.EMail
        '    ZuBearbeitenderTrainer.Alias = NeueTrainerdaten.Alias
        '    ZuBearbeitenderTrainer.Foto = NeueTrainerdaten.Foto


        '    OnTrainerGeaendert(New TrainerEventArgs(TrainerLesen(ZuBearbeitenderTrainer)))
        '    ' den Trainer in allen Gruppen austauschen
        'End Sub

        'Public Sub TrainerHinzufuegen(e As TrainerEventArgs)
        '    Club.Trainerliste.Add(e.NeueTrainerdaten)
        'End Sub

        'Public Sub TrainerLoeschen(e As TrainerEventArgs)
        '    Dim TrainerID = e.TrainerIDListe(0)
        '    Dim Trainer = TrainerLesen(TrainerID)
        '    ' Zuerst aus allen Einteilungen entfernen
        '    Club.Einteilungsliste.ToList.ForEach(Sub(El) El.VerfuegbareTrainerListe.Remove(Trainer))
        '    Club.Einteilungsliste.ToList.ForEach(Sub(El) El.Gruppenliste.ToList.ForEach(Sub(G)
        '                                                                                    If G.TrainerID = TrainerID Then
        '                                                                                        G.Trainer = Nothing
        '                                                                                        G.TrainerID = Nothing
        '                                                                                    End If
        '                                                                                End Sub))
        '    ' Dann aus der Trainerliste entfernen
        '    Club.Trainerliste.Remove(Trainer)
        'End Sub

        Private Function TrainerLesen(TrainerID As Guid) As Trainer
            Return DateiService.AktuellerClub.Trainerliste.Where(Function(T) T.TrainerID = TrainerID).SingleOrDefault
        End Function

    End Class
End Namespace
