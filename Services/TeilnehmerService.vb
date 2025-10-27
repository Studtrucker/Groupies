Imports Groupies.Entities.Generation4

Namespace Services

    Public Class TeilnehmerService

        Public Shared Event TeilnehmerGeaendert As EventHandler(Of EventArgs)

        Public Sub New()
        End Sub

        Protected Overridable Sub OnTeilnehmerGeaendert(e As EventArgs)
            RaiseEvent TeilnehmerGeaendert(Me, e)
        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern in eine Gruppe einteilen und aus der Liste nicht zugewiesene Teilnehmer entfernen
        ''' </summary>
        ''' <param name="NeueTeilnehmerListe"></param>
        ''' <param name="Gruppe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerInGruppeEinteilen(NeueTeilnehmerListe As List(Of Teilnehmer), Gruppe As Gruppe, Einteilung As Einteilung)

            ' In Teilnehmerliste in Gruppe schreiben ...
            For Each Item In NeueTeilnehmerListe
                Gruppe.Mitgliederliste.Add(Item)
                Gruppe.MitgliederIDListe.Add(Item.Ident)
            Next

            ' ... aus NichtZugewieseneTeilnehmer entfernen
            For Each Teilnehmer In NeueTeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Remove(Teilnehmer.Ident)
                Einteilung.NichtZugewieseneTeilnehmerListe.Remove(Einteilung.NichtZugewieseneTeilnehmerListe.Where(Function(T) T.Ident = Teilnehmer.Ident).Single)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern aus einer Gruppe eintfernen und in die Liste nicht zugewiesene Teilnehmer hinzufügen.
        ''' </summary>
        ''' <param name="TeilnehmerListe"></param>
        ''' <param name="Gruppe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(TeilnehmerListe As List(Of Teilnehmer), Gruppe As Gruppe, Einteilung As Einteilung)

            ' Teilnehmerliste aus Gruppe entfernen ...
            For Each Tn In TeilnehmerListe
                Gruppe.Mitgliederliste.Remove(TeilnehmerAusListeLesen(Gruppe.Mitgliederliste.ToList, Tn))
                Gruppe.MitgliederIDListe.Remove(Tn.Ident)
            Next

            ' ... in NichtZugewieseneTeilnehmer schreiben
            For Each Tn In TeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerListe.Add(Tn)
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Add(Tn.Ident)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern wird aus der Einteilung entfernt
        ''' </summary>
        ''' <param name="TeilnehmerListe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerEinteilungHinzufuegen(TeilnehmerListe As List(Of Teilnehmer), Gruppe As Gruppe, Einteilung As Einteilung)

            For Each Tn In TeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Add(Tn.Ident)
                Einteilung.NichtZugewieseneTeilnehmerListe.Add(Tn)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        ''' <summary>
        ''' Eine Liste von Teilnehmern wird aus der Einteilung entfernt
        ''' </summary>
        ''' <param name="TeilnehmerListe"></param>
        ''' <param name="Einteilung"></param>
        Public Sub TeilnehmerAusEinteilungEntfernen(TeilnehmerListe As List(Of Teilnehmer), Einteilung As Einteilung)

            For Each Tn In TeilnehmerListe
                Einteilung.NichtZugewieseneTeilnehmerIDListe.Remove(Tn.Ident)
                Einteilung.NichtZugewieseneTeilnehmerListe.Remove(Einteilung.NichtZugewieseneTeilnehmerListe.Where(Function(NZT) NZT.Ident = Tn.Ident).Single)
            Next

            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub


        Public Sub TeilnehmerErstellen()

            Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Teilnehmer),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

            mvw.AktuellesViewModel.Model = New Teilnehmer
            dialog.DataContext = mvw
            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                Services.DateiService.AktuellerClub.Teilnehmerliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Teilnehmer).VorUndNachname} wurde gespeichert")
            End If
            OnTeilnehmerGeaendert(EventArgs.Empty)
        End Sub

        Public Sub TeilnehmerBearbeiten(TeilnehmerToEdit As Teilnehmer)


            ' Hier können Sie die Logik für den Neu-Button implementieren
            Dim dialog = New BasisDetailWindow() With {
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Teilnehmer),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

            mvw.AktuellesViewModel.Model = New Teilnehmer(TeilnehmerToEdit)
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                Dim club = DateiService.AktuellerClub

                Dim index = club.Teilnehmerliste.IndexOf(TeilnehmerAusListeLesen(club.Teilnehmerliste.ToList, TeilnehmerToEdit))
                ' 1) in Club-Teilnehmerliste austauschen
                club.Teilnehmerliste(index) = mvw.AktuellesViewModel.Model

                ' 2) in allen Einteilungen: NichtZugewieseneTeilnehmerListe austauschen
                For Each el In Club.Einteilungsliste
                    Dim toChangeNZ = el.NichtZugewieseneTeilnehmerListe.Where(Function(N) N.Ident = mvw.AktuellesViewModel.Model.Ident).ToList()
                    For Each n In toChangeNZ
                        index = el.NichtZugewieseneTeilnehmerListe.IndexOf(TeilnehmerAusListeLesen(el.NichtZugewieseneTeilnehmerListe.ToList, TeilnehmerToEdit))
                        el.NichtZugewieseneTeilnehmerListe(index) = mvw.AktuellesViewModel.Model
                    Next
                Next

                ' 3) in allen Gruppen: Mitgliederliste korrekt entfernen
                For Each E In Club.Einteilungsliste.Where(Function(el) el IsNot Nothing).ToList()
                    For Each G In E.Gruppenliste
                        Dim toChangeMembers = G.Mitgliederliste.Where(Function(m) m.Ident = mvw.AktuellesViewModel.Model.Ident).ToList()
                        For Each m In toChangeMembers
                            index = G.Mitgliederliste.IndexOf(TeilnehmerAusListeLesen(G.Mitgliederliste.ToList, TeilnehmerToEdit))
                            G.Mitgliederliste(index) = mvw.AktuellesViewModel.Model
                        Next
                    Next
                Next

                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Teilnehmer).VorUndNachname} wurde gespeichert")
            End If
            OnTeilnehmerGeaendert(EventArgs.Empty)

        End Sub

        Public Sub TeilnehmerLoeschen(TeilnehmerToDelete As Teilnehmer)

            Dim result = MessageBox.Show($"Möchten Sie {TeilnehmerToDelete.VorUndNachname} wirklich aus dem gesamten Club - auch in den Gruppen - löschen?", "Teilnehmer löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)
            If result = MessageBoxResult.Yes Then

                Dim club = DateiService.AktuellerClub

                ' 1) aus Club-Teilnehmerliste entfernen (sichere Suche)
                Dim foundInClub = TeilnehmerAusListeLesen(club.Teilnehmerliste.ToList(), TeilnehmerToDelete)
                If foundInClub IsNot Nothing Then
                    club.Teilnehmerliste.Remove(foundInClub)
                End If

                ' 2) in allen Einteilungen: NichtZugewieseneTeilnehmerListe entfernen (korrekt aus der Original-Collection)
                For Each el In club.Einteilungsliste
                    Dim toRemoveNZ = el.NichtZugewieseneTeilnehmerListe.Where(Function(N) N.Ident = TeilnehmerToDelete.Ident).ToList()
                    For Each n In toRemoveNZ
                        el.NichtZugewieseneTeilnehmerListe.Remove(n)
                        el.NichtZugewieseneTeilnehmerIDListe.Remove(n.Ident)
                    Next
                Next

                ' 3) in allen Gruppen: Mitgliederliste korrekt entfernen
                For Each E In club.Einteilungsliste.Where(Function(el) el IsNot Nothing).ToList()
                    For Each G In E.Gruppenliste
                        Dim toRemoveMembers = G.Mitgliederliste.Where(Function(m) m.Ident = TeilnehmerToDelete.Ident).ToList()
                        For Each m In toRemoveMembers
                            G.Mitgliederliste.Remove(m)
                            G.MitgliederIDListe.Remove(m.Ident)
                        Next
                    Next
                Next

                MessageBox.Show($"{TeilnehmerToDelete.VorUndNachname} wurde gelöscht")
                OnTeilnehmerGeaendert(EventArgs.Empty)
            End If

        End Sub

        Public Sub TeilnehmerSuchen(Name As String)
            Throw New NotImplementedException
        End Sub


        Private Function TeilnehmerAusListeLesen(Liste As List(Of Teilnehmer), TeilnehmerToFind As Teilnehmer) As Teilnehmer

            Return Liste.Where(Function(T) T.Ident = TeilnehmerToFind.Ident).SingleOrDefault()

        End Function
    End Class
End Namespace
