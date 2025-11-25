Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class GruppenService


    Public Shared Event GruppeBearbeitet As EventHandler(Of EventArgs)

    Public Sub New()
    End Sub

    Protected Overridable Sub OnGruppeBearbeitet(e As EventArgs)
        RaiseEvent GruppeBearbeitet(Me, e)
    End Sub

    Public Sub GruppeErstellen()
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppe),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

        mvw.AktuellesViewModel.Model = New Gruppe
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste.Add(mvw.AktuellesViewModel.Model)

            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppe).Benennung} wurde gespeichert")
            OnGruppeBearbeitet(EventArgs.Empty)
        End If

    End Sub

    Public Sub GruppeBearbeiten(GruppeToEdit As Gruppe)
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppe),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Gruppe(GruppeToEdit)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()
        Dim Gruppenliste = ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste

        If result = True Then
            ' 1) in Club-Gruppenliste austauschen
            Dim index = Gruppenliste.IndexOf(GruppeAusListeLesen(Gruppenliste.ToList, GruppeToEdit.Ident))

            Dim Club = ServiceProvider.DateiService.AktuellerClub

            ' 2) in allen Einteilungen: Gruppe austauschen
            For Each el In Club.Einteilungsliste
                Dim toChangeGL = el.Gruppenliste.Where(Function(G) G.Ident = mvw.AktuellesViewModel.Model.Ident).ToList()
                For Each n In toChangeGL
                    index = el.Gruppenliste.IndexOf(GruppeAusListeLesen(el.Gruppenliste.ToList, GruppeToEdit.Ident))
                    el.Gruppenliste(index) = mvw.AktuellesViewModel.Model
                Next
            Next

            ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste(index) = mvw.AktuellesViewModel.Model
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppe).Benennung} wurde gespeichert")
            OnGruppeBearbeitet(EventArgs.Empty)
        End If
    End Sub

    Public Sub GruppeLoeschen(GruppeToDelete As Gruppe)

        Dim result = MessageBox.Show($"Möchten Sie {GruppeToDelete.Benennung} wirklich aus dem gesamten Club - auch in den Einteilungen - löschen?", "Gruppe löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)

        If result = MessageBoxResult.Yes Then


            Dim club = ServiceProvider.DateiService.AktuellerClub
            ' aus Club Gruppenliste entfernen
            ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste.Remove(GruppeAusListeLesen(club.Gruppenstammliste.ToList, GruppeToDelete.TrainerID))
            ' in allen Einteilungen aus Gruppenliste entfernen
            For Each el In club.Einteilungsliste
                For Each Gruppe In el.Gruppenliste.Where(Function(G) G.Ident = GruppeToDelete.Ident)
                    el.Gruppenliste.Remove(Gruppe)
                    el.GruppenIDListe.Remove(Gruppe.Ident)
                Next
            Next

            MessageBox.Show($"{GruppeToDelete.Benennung} wurde gelöscht")
            OnGruppeBearbeitet(EventArgs.Empty)
        End If

    End Sub

    Public Sub GruppeAusEinteilungEntfernen(GruppenToRemove As List(Of Gruppe), Einteilung As Einteilung)

        For Each gruppe In GruppenToRemove
            GruppeAusEinteilungEntfernen(gruppe, Einteilung)
        Next

    End Sub

    Public Sub GruppeAusEinteilungEntfernen(GruppeToRemove As Gruppe, Einteilung As Einteilung)

        Einteilung.Gruppenliste.Remove(GruppeAusListeLesen(Einteilung.Gruppenliste.ToList, GruppeToRemove.Ident))
        Einteilung.GruppenIDListe.Remove(GruppeToRemove.Ident)

        ServiceProvider.DateiService.AktuellerClub.Gruppenliste.Remove(GruppeAusListeLesen(ServiceProvider.DateiService.AktuellerClub.Gruppenliste.ToList, GruppeToRemove.Ident))

    End Sub

    Private Function GruppeAusListeLesen(Liste As List(Of Gruppe), Ident As Guid) As Gruppe
        Return Liste.Where(Function(g) g.Ident = Ident).SingleOrDefault
    End Function

    Private Function GruppeAusListeLesen(Liste As List(Of Gruppenstamm), Ident As Guid) As Gruppenstamm
        Return Liste.Where(Function(g) g.Ident = Ident).SingleOrDefault
    End Function

    Public Function GruppeInEinteilungVorhanden(GruppeToCheck As Gruppe, Einteilung As Einteilung) As Boolean
        Return Einteilung.Gruppenliste.Any(Function(g) g.GruppenstammID = GruppeToCheck.GruppenstammID)
    End Function

    Public Sub GruppeCopyToEinteilung(GruppeToCopy As Gruppe, Einteilung As Einteilung)

        ' 1. Prüfen, ob die Gruppe in der Einteilung schon vorhanden ist
        If GruppeInEinteilungVorhanden(GruppeToCopy, Einteilung) Then
            Return
        End If

        Dim NeueGruppe As New Gruppe(GruppeToCopy)


        ' 2. Prüfen, ob Gruppenmitglieder in der Einteilung vorhanden sind
        If GruppeToCopy.Mitgliederliste IsNot Nothing Then
            Dim TnService As New TeilnehmerService
            For Each tn In GruppeToCopy.Mitgliederliste
                If TnService.TeilnehmerInEinteilungVorhanden(tn, Einteilung) Then
                    NeueGruppe.Mitgliederliste.Remove(tn)
                    NeueGruppe.MitgliederIDListe.Remove(tn.Ident)
                End If
            Next
        End If

        ' 3. Prüfen, ob der Trainer in der Einteilung vorhanden ist
        Dim TrService As New TrainerService
        If GruppeToCopy.Trainer IsNot Nothing Then
            If TrService.TrainerInEinteilungVorhanden(GruppeToCopy.Trainer, Einteilung) Then
                NeueGruppe.Trainer = Nothing
                NeueGruppe.TrainerID = Guid.Empty
            End If
        End If

        ' 4. Gruppe kopieren und in die Einteilung einfügen
        Einteilung.Gruppenliste.Add(neueGruppe)
        Einteilung.GruppenIDListe.Add(neueGruppe.Ident)


        ' 5. Gruppe in die Gruppenliste schreiben
        ServiceProvider.DateiService.AktuellerClub.Gruppenliste.Add(NeueGruppe)

    End Sub

End Class
