Imports Groupies.Entities.Generation4

Namespace Services

    Public Class TrainerService

        Public Event TrainerInGruppeEingeteilt As EventHandler(Of TrainerServiceEventArgs)
        Public Event TrainerAusGruppeEntfernt As EventHandler(Of TrainerServiceEventArgs)

        ''' <summary>
        ''' Club-Instanz
        ''' </summary>
        Public Sub New()
        End Sub


        Protected Overridable Sub OnTrainerInGruppeEingeteilt(e As TrainerServiceEventArgs)
            RaiseEvent TrainerInGruppeEingeteilt(Me, e)
        End Sub
        Protected Overridable Sub OnTrainerAusGruppeEntfernt(e As TrainerServiceEventArgs)
            RaiseEvent TrainerAusGruppeEntfernt(Me, e)
        End Sub

        ''' <summary>
        ''' Trainer in Gruppe einteilen und aus VerfügbareTrainer entfernen
        ''' </summary>
        ''' <param name="e"></param>
        Public Sub TrainerInGruppeEinteilen(e As TrainerServiceEventArgs)
            'Public Sub TrainerInGruppeEinteilen(NeuerTrainerID As Guid, GruppenID As Guid, EinteilungID As Guid)
            Dim NeuerTrainerID = e.TrainerIDListe(0)
            Dim EinteilungID = e.EinteilungID.Value
            Dim GruppenID = e.GruppeID.Value

            ' Aktuellen Trainer auslesen
            Dim AktuellerTrainer = Club.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.Trainer
            ' Wenn bereits ein Trainer zugewiesen ist, diesen zuerst entfernen
            If AktuellerTrainer IsNot Nothing Then
                ' Aktueller Trainer ist nun entfernt und befindet sich in der VerfügbarenTrainerliste
                'TrainerAusGruppeEntfernen(AktuellerTrainer.TrainerID, GruppenID, EinteilungID)
            End If

            ' Neuer Trainer aus Trainerliste auslesen
            Dim NeuerTrainer = TrainerLesen(NeuerTrainerID)
            ' In TrainerID und Trainer in Gruppe schreiben ...
            Club.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.TrainerID = NeuerTrainerID
            Club.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.Trainer = NeuerTrainer

            ' ... aus VerfügbareTrainer entfernen
            Club.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Remove(NeuerTrainer)
            Club.Einteilungsliste.Where(Function(EL) EL.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Remove(NeuerTrainerID)

            OnTrainerInGruppeEingeteilt(e)

        End Sub

        ''' <summary>
        ''' Trainer aus Gruppe entfernen und in VerfügbareTrainer kopieren
        ''' </summary>
        ''' <param name="TrainerEventArgs"></param>
        Public Sub TrainerAusGruppeEntfernen(TrainerEventArgs As TrainerServiceEventArgs)
            'Public Sub TrainerAusGruppeEntfernen(AktuellerTrainerID As Guid, GruppenID As Guid, EinteilungID As Guid)

            Dim AktuellerTrainerID = TrainerEventArgs.TrainerIDListe(0)
            Dim EinteilungID = TrainerEventArgs.EinteilungID.Value
            Dim GruppenID = TrainerEventArgs.GruppeID.Value

            ' Trainer auslesen
            Dim AktuellerTrainer = Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.Trainer
            ' Trainer entfernen
            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.Trainer = Nothing
            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.Gruppenliste.Where(Function(G) G.Ident = GruppenID).Single.TrainerID = Nothing

            ' In VerfügbareTrainer kopieren
            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Add(AktuellerTrainer)
            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Add(AktuellerTrainerID)

            OnTrainerInGruppeEingeteilt(EventArgs.Empty)

        End Sub


        ''' <summary>
        ''' Trainer aus Einteilung und aus den Gruppen entfernen
        ''' </summary>
        ''' <param name="TrainerID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerAusEinteilungEntfernen(TrainerID As Guid, EinteilungID As Guid)

            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Remove(TrainerLesen(TrainerID))
            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Remove(TrainerID)
            Club.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(G)
                                                                                          If G.TrainerID = TrainerID Then
                                                                                              G.Trainer = Nothing
                                                                                              G.TrainerID = Nothing
                                                                                          End If
                                                                                      End Sub))

            OnTrainerInGruppeEingeteilt(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Trainer aus Trainerliste in Einteilung.VerfügbareTrainerListe hinzugefügt.
        ''' </summary>
        ''' <param name="TrainerID"></param>
        ''' <param name="EinteilungID"></param>
        Public Sub TrainerInEinteilungHinzufuegen(TrainerID As Guid, EinteilungID As Guid)

            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerListe.Add(TrainerLesen(TrainerID))
            Club.Einteilungsliste.Where(Function(E) E.Ident = EinteilungID).Single.VerfuegbareTrainerIDListe.Add(TrainerID)

            OnTrainerInGruppeEingeteilt(EventArgs.Empty)

        End Sub

        Private Function TrainerLesen(TrainerID As Guid) As Trainer
            Return Club.Trainerliste.Where(Function(T) T.TrainerID = TrainerID).Single
        End Function

    End Class
End Namespace
