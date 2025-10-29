Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class EinteilungService

    Public Event EinteilungBearbeitet As EventHandler

    Public Sub New()
        ' Initialisierungscode hier einfügen, falls erforderlich
    End Sub

    Protected Overridable Sub OnEinteilungBearbeitet(e As EventArgs)
        RaiseEvent EinteilungBearbeitet(Me, e)
    End Sub

    Public Sub EinteilungErstellen()
        ' Implementierung zum Erstellen einer Einteilung

        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Einteilung),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

        mvw.AktuellesViewModel.Model = New Einteilung
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.Einteilungsliste.Add(mvw.AktuellesViewModel.Model)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Einteilung).Benennung} wurde gespeichert")
            OnEinteilungBearbeitet(EventArgs.Empty)
        End If

    End Sub

    Public Sub EinteilungBearbeiten(EinteilungToEdit As Einteilung)
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Einteilung),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Einteilung(EinteilungToEdit)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            Dim club = DateiService.AktuellerClub

            Dim index = club.Einteilungsliste.IndexOf(EinteilungAusListeLesen(club.Einteilungsliste.ToList, EinteilungToEdit.Ident))
            ' 1) in Club-Einteilungsliste austauschen
            club.Einteilungsliste(index) = mvw.AktuellesViewModel.Model
            MessageBox.Show($"Die Änderung {DirectCast(mvw.AktuellesViewModel.Model, Einteilung).Benennung} wurde übernommen")
            OnEinteilungBearbeitet(EventArgs.Empty)

        End If

    End Sub

    Public Sub EinteilungLoeschen(EinteilungToDelete As Einteilung)
        Dim club = DateiService.AktuellerClub
        If MessageBox.Show($"Wollen Sie die Einteilung {EinteilungToDelete.Benennung} wirklich löschen?", "Einteilung löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.Yes Then
            club.Einteilungsliste.Remove(EinteilungAusListeLesen(club.Einteilungsliste.ToList, EinteilungToDelete.Ident))
            MessageBox.Show($"Einteilung {EinteilungToDelete.Benennung} wurde gelöscht.")
            OnEinteilungBearbeitet(EventArgs.Empty)
        End If
    End Sub

    Public Sub EinteilungKopieren(EinteilungToCopy As Integer)
        ' Implementierung zum Kopieren einer Einteilung
    End Sub

    Private Function EinteilungAusListeLesen(EinteilungListe As List(Of Einteilung), EinteilungID As Guid) As Einteilung
        For Each Einteilung In EinteilungListe
            If Einteilung.Ident = EinteilungID Then
                Return Einteilung
            End If
        Next
        Return Nothing
    End Function

End Class
