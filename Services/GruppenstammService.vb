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
            ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste.Add(mvw.AktuellesViewModel.Model)

            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppenstamm).Benennung} wurde gespeichert")
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(mvw.AktuellesViewModel.Model))
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
        Dim Gruppenstammliste = ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste

        If result = True Then
            ' 1) in Club-Gruppenstammliste austauschen
            Dim index = Gruppenstammliste.IndexOf(GruppenstammAusListeLesen(Gruppenstammliste.ToList, ItemToEdit.Ident))

            Dim Club = ServiceProvider.DateiService.AktuellerClub

            ' 2) in allen Einteilungen: Gruppe austauschen
            For Each el In Club.Einteilungsliste
                Dim toChangeGL = el.Gruppenliste.Where(Function(G) G.Gruppenstamm.Ident = mvw.AktuellesViewModel.Model.Ident).ToList()
                For Each n In toChangeGL
                    index = el.Gruppenliste.IndexOf(GruppenstammAusListeLesen(el.Gruppenliste.ToList, ItemToEdit.Ident))
                    el.Gruppenliste(index).Gruppenstamm = mvw.AktuellesViewModel.Model
                Next
            Next

            ServiceProvider.DateiService.AktuellerClub.Gruppenstammliste(index) = mvw.AktuellesViewModel.Model
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppenstamm).Benennung} wurde gespeichert")
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(DirectCast(mvw.AktuellesViewModel.Model, Gruppenstamm)))
        Else
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(ItemToEdit))
        End If
    End Sub

    Public Sub GruppenstammLoeschen(ItemToDelete As Gruppenstamm)

        Dim club = ServiceProvider.DateiService.AktuellerClub

        ' Prüfung, ob das ItemToDelete in irgendeiner Gruppe in Einteilungen verwendet.
        ' Dann darf das Item nicht gelöscht werden.
        For Each el In club.Einteilungsliste
            If GruppenstammInEinteilungVorhanden(ItemToDelete, el) Then
                MessageBox.Show($"{ItemToDelete.Benennung} kann nicht gelöscht werden, da es in einer oder mehreren Einteilungen verwendet wird.", "Löschen nicht möglich", MessageBoxButton.OK, MessageBoxImage.Exclamation)
                Return
            End If
        Next

        Dim result = MessageBox.Show($"Möchten Sie {ItemToDelete.Benennung} wirklich aus dem gesamten Club löschen?", "Gruppenstamm löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)

        If result = MessageBoxResult.Yes Then
            ' aus Club Gruppenliste entfernen
            club.Gruppenstammliste.Remove(GruppenstammAusListeLesen(club.Gruppenstammliste.ToList, ItemToDelete.Ident))
            OnGruppenstammBearbeitet(New GruppenstammEventArgs(ItemToDelete))
            MessageBox.Show($"{ItemToDelete.Benennung} wurde gelöscht")
        End If

    End Sub

    Public Sub GruppenstammEinteilen(Liste As List(Of Gruppenstamm))
        Debug.WriteLine($"GruppenstammEinteilen wurde für {Liste.Count} Gruppen aufgerufen")
    End Sub

    Private Function GruppenstammAusListeLesen(Liste As List(Of Gruppenstamm), Guid As Guid) As Gruppenstamm
        Return Liste.FirstOrDefault(Function(L) L.Ident = Guid)
    End Function
    Private Function GruppenstammAusListeLesen(Liste As List(Of Gruppe), Guid As Guid) As Gruppe
        Return Liste.FirstOrDefault(Function(L) L.Gruppenstamm.Ident = Guid)
    End Function

    Public Function GruppenstammInEinteilungVorhanden(GruppenstammToCheck As Gruppenstamm, EinteilungToCheck As Einteilung) As Boolean

        Dim VorhandenInEinteilung = EinteilungToCheck.Gruppenliste.Any(Function(G) G.GruppenstammID = GruppenstammToCheck.Ident)

        Return VorhandenInEinteilung

    End Function

End Class
