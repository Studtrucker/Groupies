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
        ''' Ein Trainer in eine Gruppe einteilen und aus der Liste der verfügbaren Trainern entfernen
        ''' </summary>
        ''' <param name="NeuerTrainer"></param>
        ''' <param name="Gruppe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TrainerInGruppeEinteilen(NeuerTrainer As Trainer, Gruppe As Gruppe, Einteilung As Einteilung)

            ' Aktuellen Trainer und Neuer Trainer auslesen
            Dim AktuellerTrainer = Gruppe.Trainer

            ' Wenn die Gruppe einen aktuellen Trainer hatte,
            If AktuellerTrainer IsNot Nothing Then
                ' dann wird der aktueller Trainer der verfügbaren Trainerliste hinzugefügt.
                Einteilung.VerfuegbareTrainerListe.Add(AktuellerTrainer)
                Einteilung.VerfuegbareTrainerIDListe.Add(AktuellerTrainer.TrainerID)
            End If

            ' Neue TrainerID und Trainer der Gruppe zuweisen ...
            Gruppe.TrainerID = neuerTrainer.TrainerID
            Gruppe.Trainer = neuerTrainer

            ' ... neue TrainerID und Trainer aus der Liste Verfügbare Trainer entfernen.
            Einteilung.VerfuegbareTrainerListe.Remove(Einteilung.VerfuegbareTrainerListe.Where(Function(T) T.TrainerID = neuerTrainer.TrainerID).Single)
            Einteilung.VerfuegbareTrainerIDListe.Remove(neuerTrainer.TrainerID)

            OnTrainerGeaendert(New TrainerEventArgs(neuerTrainer))

        End Sub

        Public Sub TrainerInGruppeEinteilen(NeuerTrainerListe As List(Of Trainer), Gruppe As Gruppe, Einteilung As Einteilung)
            'For Each NeuerTrainer In NeuerTrainerListe
            TrainerInGruppeEinteilen(NeuerTrainerListe(0), Gruppe, Einteilung)
            'Next
        End Sub


        ''' <summary>
        ''' Ein Trainer aus einer Gruppe entfernen und den verfügbaren Trainern hinzufügen
        ''' </summary>
        ''' <param name="Gruppe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe, Einteilung As Einteilung)

            Dim Trainer = Gruppe.Trainer

            ' Trainer aus der Gruppe entfernen
            Gruppe.Trainer = Nothing
            Gruppe.TrainerID = Nothing

            ' In VerfügbareTrainer kopieren
            Einteilung.VerfuegbareTrainerListe.Add(Trainer)
            Einteilung.VerfuegbareTrainerIDListe.Add(Trainer.TrainerID)

            OnTrainerGeaendert(TrainerEventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Ein Trainer aus einer Einteilung entfernen
        ''' </summary>
        ''' <param name="Trainer"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TrainerAusEinteilungEntfernen(Trainer As Trainer, Einteilung As Einteilung)

            ' (Der ausgewählte Trainer kann sich ja nur in der verfügbaren Trainerliste befinden)
            ' (Sollte ein zugewiesener Gruppentrainer entfernt werden, dann erst aus der Gruppe entfernen)

            Einteilung.VerfuegbareTrainerListe.Remove(Einteilung.VerfuegbareTrainerListe.Where(Function(T) T.TrainerID = Trainer.TrainerID).Single)
            Einteilung.VerfuegbareTrainerIDListe.Remove(Trainer.TrainerID)

            OnTrainerGeaendert(TrainerEventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Trainern aus einer Einteilung entfernen
        ''' </summary>
        ''' <param name="TrainerListe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TrainerAusEinteilungEntfernen(TrainerListe As IList(Of Trainer), Einteilung As Einteilung)

            ' (Der ausgewählte Trainer kann sich ja nur in der verfügbaren Trainerliste befinden)
            ' (Sollte ein zugewiesener Gruppentrainer entfernt werden, dann erst aus der Gruppe entfernen)

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
                Einteilung.VerfuegbareTrainerListe.Remove(Einteilung.VerfuegbareTrainerListe.Where(Function(VTL) VTL.TrainerID = t.TrainerID).Single)
            Next

            OnTrainerGeaendert(TrainerEventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Ausgewählte Trainer zur Liste der verfügbaren Trainer einer Einteilung hinzufügen.
        ''' </summary>
        ''' <param name="Trainerliste"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TrainerEinteilungHinzufuegen(Trainerliste As IList(Of Trainer), Einteilung As Einteilung)

            For Each T In Trainerliste
                Einteilung.VerfuegbareTrainerListe.Add(T)
                Einteilung.VerfuegbareTrainerIDListe.Add(T.TrainerID)
            Next

            OnTrainerGeaendert(TrainerEventArgs.Empty)

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

    End Class
End Namespace
