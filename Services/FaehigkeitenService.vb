Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class FaehigkeitenService
    Public Shared Event FaehigkeitBearbeitet As EventHandler(Of EventArgs)

    Public Sub New()
    End Sub

    Public Sub OnFaehigkeitBearbeitet(e As EventArgs)
        RaiseEvent FaehigkeitBearbeitet(Me, e)
    End Sub

    Public Sub FaehigkeitErstellen()

        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Faehigkeit),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

        mvw.AktuellesViewModel.Model = New Faehigkeit
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            ServiceProvider.DateiService.AktuellerClub.Faehigkeitenliste.Add(mvw.AktuellesViewModel.Model)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Faehigkeit).Benennung} wurde gespeichert")
            OnFaehigkeitBearbeitet(EventArgs.Empty)
        End If

    End Sub
    Public Sub FaehigkeitBearbeiten(FaehigkeitToEdit As Faehigkeit)
        ' Implementierung zum Erstellen einer Fähigkeit
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Faehigkeit),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Faehigkeit(FaehigkeitToEdit)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            Dim Club = ServiceProvider.DateiService.AktuellerClub
            Dim index = Club.Faehigkeitenliste.IndexOf(FaehigkeitToEdit)
            Club.Faehigkeitenliste(index) = mvw.AktuellesViewModel.Model

            Club.Leistungsstufenliste.ToList.ForEach(Sub(ls)
                                                         Dim Aenderungen = ls.Faehigkeiten.Where(Function(F) F.FaehigkeitID = FaehigkeitToEdit.FaehigkeitID).ToList
                                                         For Each Faehigkeit In Aenderungen
                                                             Dim faeIndex = ls.Faehigkeiten.IndexOf(Faehigkeit)
                                                             ls.Faehigkeiten(faeIndex) = mvw.AktuellesViewModel.Model
                                                         Next
                                                     End Sub)

            OnFaehigkeitBearbeitet(EventArgs.Empty)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Faehigkeit).Benennung} wurde gespeichert")
        End If

    End Sub
    Public Sub FaehigkeitLoeschen(FaehigkeitToDelete As Faehigkeit)
        Dim result = MessageBox.Show($"Möchten Sie die Fähigkeit {FaehigkeitToDelete.Benennung} wirklich aus dem gesamten Club - auch in den Leistungsstufen - löschen?", "Fähigkeit löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning)
        If result = MessageBoxResult.Yes Then
            Dim Club = ServiceProvider.DateiService.AktuellerClub
            Club.Faehigkeitenliste.Remove(FaehigkeitAusListeLesen(Club.Faehigkeitenliste.ToList, FaehigkeitToDelete.FaehigkeitID))
            Club.Leistungsstufenliste.ToList.ForEach(Sub(ls)
                                                         ls.Faehigkeiten.Remove(FaehigkeitAusListeLesen(Club.Faehigkeitenliste.ToList, FaehigkeitToDelete.FaehigkeitID))
                                                         ls.FaehigkeitenIDListe.Remove(FaehigkeitToDelete.FaehigkeitID)
                                                     End Sub)
            OnFaehigkeitBearbeitet(EventArgs.Empty)
        End If

        ' Implementierung zum Erstellen einer Fähigkeit
    End Sub

    Public Sub FaehigkeitAusLeistungsstufeEntfernen(FaehikeitToRemove As Faehigkeit, Leistungsstufe As Leistungsstufe)
        ' Implementierung zum Erstellen einer Fähigkeit
        OnFaehigkeitBearbeitet(EventArgs.Empty)
    End Sub
    Public Sub FaehigkeitLeistungsstufeHinzufuegen(FaehikeitToAdd As Faehigkeit, Leistungsstufe As Leistungsstufe)
        ' Implementierung zum Erstellen einer Fähigkeit
        OnFaehigkeitBearbeitet(EventArgs.Empty)
    End Sub

    Private Function FaehigkeitAusListeLesen(Liste As List(Of Faehigkeit), FaehigkeitID As Guid) As Faehigkeit
        Return Liste.Where(Function(I) I.FaehigkeitID = FaehigkeitID).SingleOrDefault
    End Function

End Class

