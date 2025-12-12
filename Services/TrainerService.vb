Imports Groupies.Entities.Generation4

Namespace Services

    Public Class TrainerService

        Public Shared Event TrainerGeaendert As EventHandler(Of TrainerEventArgs)

        Public Shared Event TrainerErstellt As EventHandler(Of TrainerEventArgs)

        Public Shared Event TrainerGeloescht As EventHandler(Of TrainerEventArgs)

        Public Sub New()
        End Sub

        Protected Overridable Sub OnTrainerGeaendert(e As TrainerEventArgs)
            RaiseEvent TrainerGeaendert(Me, e)
        End Sub
        Protected Overridable Sub OnTrainerErstellt(e As TrainerEventArgs)
            RaiseEvent TrainerErstellt(Me, e)
        End Sub
        Protected Overridable Sub OnTrainerGeloescht(e As TrainerEventArgs)
            RaiseEvent TrainerGeloescht(Me, e)
        End Sub

        Public Sub TrainerErstellen()

            Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Trainer),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

            mvw.AktuellesViewModel.Model = New Trainer()
            dialog.DataContext = mvw
            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                OnTrainerErstellt(New TrainerEventArgs(mvw.AktuellesViewModel.Model))
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Trainer).VorNachname} wurde gespeichert")
            End If

        End Sub


        Public Sub TrainerBearbeiten(TrainerToEdit As Trainer)

            ' Hier können Sie die Logik für den Neu-Button implementieren
            Dim dialog = New BasisDetailWindow() With {
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Trainer),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

            mvw.AktuellesViewModel.Model = New Trainer(TrainerToEdit)
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                Dim club = ServiceProvider.DateiService.AktuellerClub

                OnTrainerGeaendert(New TrainerEventArgs(mvw.AktuellesViewModel.Model))
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Trainer).VorNachname} wurde gespeichert")
            End If

        End Sub

        Public Sub TrainerLoeschen(TrainerToDelete As Trainer)
            Dim result = MessageBox.Show($"Möchten Sie {TrainerToDelete.VorNachname} wirklich aus dem gesamten Club - auch in den Gruppen - löschen?", "Trainer löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)
            If result = MessageBoxResult.Yes Then
                OnTrainerGeloescht(New TrainerEventArgs(TrainerToDelete))

                Dim club = ServiceProvider.DateiService.AktuellerClub

                ' Aus allen Einteilungen entfernen
                club.Einteilungsliste.ToList.ForEach(Sub(el)
                                                         el.VerfuegbareTrainerIDListe.Remove(TrainerToDelete.TrainerID)
                                                         el.VerfuegbareTrainerListe.Remove(TrainerAusListeLesen(el.VerfuegbareTrainerListe.ToList, TrainerToDelete.TrainerID))
                                                         el.Gruppenliste.Where(Function(GT) GT IsNot Nothing AndAlso GT.TrainerID = TrainerToDelete.TrainerID).ToList.ForEach(Sub(G)
                                                                                                                                                                                  G.TrainerID = Nothing
                                                                                                                                                                                  G.Trainer = Nothing
                                                                                                                                                                              End Sub)
                                                     End Sub)

                ' aus Club Trainerliste entfernen
                club.Trainerliste.Remove(TrainerAusListeLesen(club.Trainerliste.ToList, TrainerToDelete.TrainerID))

                MessageBox.Show($"{TrainerToDelete.VorNachname} wurde gelöscht")

            End If

        End Sub

        Private Function TrainerAusListeLesen(List As List(Of Trainer), TrainerID As Guid) As Trainer
            Return List.Where(Function(T) T.TrainerID = TrainerID).SingleOrDefault
        End Function

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

            Dim alleTrainer = If(ServiceProvider.DateiService.AktuellerClub, Nothing)?.Trainerliste
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

        Public Function TrainerInEinteilungVorhanden(TrainerToCheck As Trainer, EinteilungToCheck As Einteilung) As Boolean
            Dim VorhandenInEinteilung = EinteilungToCheck.VerfuegbareTrainerIDListe.Any(Function(T) T = TrainerToCheck.TrainerID)
            VorhandenInEinteilung = VorhandenInEinteilung OrElse EinteilungToCheck.Gruppenliste.Any(Function(G) G.TrainerID = TrainerToCheck.TrainerID)
            Return VorhandenInEinteilung
        End Function


        Public Sub TrainerCopyToEinteilung(TrainerToCopy As Trainer, Einteilung As Einteilung)
            ' 1. Prüfen, ob der Teilnehmer in der Einteilung schon vorhanden ist
            If TrainerInEinteilungVorhanden(TrainerToCopy, Einteilung) Then
                Return
            End If
            ' 2. Teilnehmer in die Einteilung einfügen
            Einteilung.VerfuegbareTrainerListe.Add(TrainerToCopy)
            Einteilung.VerfuegbareTrainerIDListe.Add(TrainerToCopy.TrainerID)
        End Sub

    End Class
End Namespace
