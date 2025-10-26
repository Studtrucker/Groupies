Imports Groupies.Entities.Generation4
Public Class LeistungsstufenService

    Public Shared Event LeistungsstufeBearbeitet As EventHandler(Of EventArgs)

    Public Sub New()
    End Sub

    Public Sub OnLeistungsstufeBearbeitet(e As EventArgs)
        RaiseEvent LeistungsstufeBearbeitet(Me, e)
    End Sub

    Public Sub LeistungsstufeErstellen()

        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Leistungsstufe),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

        mvw.AktuellesViewModel.Model = New Leistungsstufe
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.Leistungsstufenliste.Add(mvw.AktuellesViewModel.Model)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Leistungsstufe).Benennung} wurde gespeichert")
        End If

        ' Implementierung zum Erstellen einer Leistungsstufe
        OnLeistungsstufeBearbeitet(EventArgs.Empty)
    End Sub

    Public Sub LeistungsstufeBearbeiten(LeistungsstufeToEdit As Leistungsstufe)

        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Leistungsstufe),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Leistungsstufe(LeistungsstufeToEdit)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            Dim index = Services.DateiService.AktuellerClub.Leistungsstufenliste.IndexOf(LeistungsstufeToEdit)
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.Leistungsstufenliste(index) = mvw.AktuellesViewModel.Model
            Debug.WriteLine(Services.DateiService.AktuellerClub.Leistungsstufenliste(index).Beschreibung)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Leistungsstufe).Benennung} wurde gespeichert")
        End If

        ' Implementierung zum Bearbeiten einer Leistungsstufe
        OnLeistungsstufeBearbeitet(EventArgs.Empty)
    End Sub

    Public Sub LeistungsstufeLoeschen(LeistungsstufeToDelete As Leistungsstufe)

        Dim result = MessageBox.Show($"Möchten Sie die Leistungsstufe {LeistungsstufeToDelete.Benennung} wirklich aus dem gesamten Club - auch die Einstufung der Gruppen und Teilnehmer - löschen?", "Leistungsstufe löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)
        If result = MessageBoxResult.Yes Then

            Dim Club = Services.DateiService.AktuellerClub

            ' 1) in Club-Leistungsstufenliste löschen
            Dim index = Club.Leistungsstufenliste.IndexOf(LeistungsstufeAusListeLesen(Club.Leistungsstufenliste.ToList, LeistungsstufeToDelete.Ident))
            Club.Leistungsstufenliste.RemoveAt(index)
            ' 1a) in allen Gruppen löschen
            For Each Gruppe In Club.Gruppenliste.Where(Function(G) G.LeistungsstufeID = LeistungsstufeToDelete.Ident)
                Gruppe.Leistungsstufe = Nothing
                Gruppe.LeistungsstufeID = Guid.Empty
            Next

            ' 1b) in allen Teilnehmern löschen
            For Each Teilnehmer In Club.Teilnehmerliste.Where(Function(T) T.LeistungsstufeID = LeistungsstufeToDelete.Ident)
                Teilnehmer.Leistungsstufe = Nothing
                Teilnehmer.LeistungsstufeID = Guid.Empty
            Next

            OnLeistungsstufeBearbeitet(EventArgs.Empty)
            MessageBox.Show($"{LeistungsstufeToDelete.Benennung} wurde gelöscht")

        End If

    End Sub

    Private Function LeistungsstufeAusListeLesen(List As List(Of Leistungsstufe), LeistungsstufeID As Guid) As Leistungsstufe
        Return List.Where(Function(I) I.Ident = LeistungsstufeID).Single
    End Function

End Class
