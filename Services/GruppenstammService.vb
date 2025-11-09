Imports Groupies.Entities.Generation4
Imports Groupies.Services
Public Class GruppenstammService
    Public Shared Event GruppenstammBearbeitet As EventHandler(Of EventArgs)

    Public Sub New()
    End Sub

    Protected Overridable Sub OnGruppenstammBearbeitet(e As GruppenstammEventArgs)
        RaiseEvent GruppenstammBearbeitet(Me, e)
    End Sub

    Public Sub GruppenstammErstellen()
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}
        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppenstamm),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}
        mvw.AktuellesViewModel.Model = New Gruppenstamm
        dialog.DataContext = mvw
        Dim result As Boolean = dialog.ShowDialog()
        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.Gruppenstammliste.Add(mvw.AktuellesViewModel.Model)

            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppenstamm).Benennung} wurde gespeichert")
            OnGruppenstammBearbeitet(EventArgs.Empty)
        End If
    End Sub

    Public Sub GruppenstammBearbeiten(ItemToEdit As Gruppenstamm)
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppenstamm),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Gruppenstamm(ItemToEdit)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()
        Dim Gruppenstammliste = Services.DateiService.AktuellerClub.Gruppenstammliste

        If result = True Then
            ' 1) in Club-Gruppenstammliste austauschen
            Dim index = Gruppenstammliste.IndexOf(GruppenstammAusListeLesen(Gruppenstammliste.ToList, ItemToEdit.Ident))

            Dim Club = Services.DateiService.AktuellerClub

            ' 2) in allen Einteilungen: Gruppe austauschen
            For Each el In Club.Einteilungsliste
                Dim toChangeGL = el.Gruppenliste.Where(Function(G) G.Gruppenstamm.Ident = mvw.AktuellesViewModel.Model.Ident).ToList()
                For Each n In toChangeGL
                    index = el.Gruppenliste.IndexOf(GruppenstammAusListeLesen(el.Gruppenliste.ToList, ItemToEdit.Ident))
                    el.Gruppenliste(index).Gruppenstamm = mvw.AktuellesViewModel.Model
                Next
            Next

            Services.DateiService.AktuellerClub.Gruppenstammliste(index) = mvw.AktuellesViewModel.Model
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppenstamm).Benennung} wurde gespeichert")
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(DirectCast(mvw.AktuellesViewModel.Model, Gruppenstamm)))
        Else
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(ItemToEdit))
        End If
    End Sub

    Public Sub GruppenstammLoeschen(ItemToDelete As Gruppenstamm)

        ' Prüfung, ob das ItemToDelete in irgendeiner Gruppe in Einteilungen verwendet.
        ' Dann darf das Item nicht gelöscht werden.
        Dim Gruppenliste = DateiService.AktuellerClub.Gruppenliste.Where(Function(G) G.GruppenstammID = ItemToDelete.Ident)
        'Dim istEnthalten = DateiService.AktuellerClub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.Contains(Gruppenliste))


        Dim result = MessageBox.Show($"Möchten Sie {ItemToDelete.Benennung} wirklich aus dem gesamten Club - auch in den Einteilungen - löschen?", "Gruppe löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)

        If result = MessageBoxResult.Yes Then


            'Dim club = DateiService.AktuellerClub
            '' aus Club Gruppenliste entfernen
            'DateiService.AktuellerClub.Gruppenstammliste.Remove(GruppeAusListeLesen(club.Gruppenstammliste.ToList, ItemToDelete.TrainerID))
            '' in allen Einteilungen aus Gruppenliste entfernen
            'For Each el In club.Einteilungsliste
            '    For Each Gruppe In el.Gruppenliste.Where(Function(G) G.Ident = ItemToDelete.Ident)
            '        el.Gruppenliste.Remove(Gruppe)
            '        el.GruppenIDListe.Remove(Gruppe.Ident)
            '    Next
            'Next

            MessageBox.Show($"{ItemToDelete.Benennung} wurde gelöscht")
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(ItemToDelete))
        End If

    End Sub

    Private Function GruppenstammAusListeLesen(Liste As List(Of Gruppenstamm), Guid As Guid) As Gruppenstamm
        Return Liste.FirstOrDefault(Function(L) L.LeistungsstufeID = Guid)
    End Function
    Private Function GruppenstammAusListeLesen(Liste As List(Of Gruppe), Guid As Guid) As Gruppe
        Return Liste.FirstOrDefault(Function(L) L.Gruppenstamm.Ident = Guid)
    End Function

End Class
