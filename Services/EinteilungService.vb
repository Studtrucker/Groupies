Imports System.Collections.ObjectModel
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

    Public Sub EinteilungKopieren(source As Einteilung, target As Einteilung)

        If target Is Nothing OrElse target Is Nothing Then Return
        Dim club = DateiService.AktuellerClub
        If club Is Nothing Then Return

        If source.Ident = target.Ident Then Return

        ' Gruppen hinzufügen (Referenzen)
        If source.Gruppenliste IsNot Nothing Then
            If target.Gruppenliste Is Nothing Then target.Gruppenliste = New GruppeCollection()
            For Each g In source.Gruppenliste
                ' falls du echte Kopien brauchst, hier copy‑constructor aufrufen
                If Not target.Gruppenliste.Any(Function(x) x.Ident = g.Ident) Then
                    Dim clonedGruppe = New Gruppe(g)
                    target.Gruppenliste.Add(clonedGruppe)
                    If target.GruppenIDListe Is Nothing Then target.GruppenIDListe = New ObservableCollection(Of Guid)
                    If Not target.GruppenIDListe.Contains(clonedGruppe.Ident) Then target.GruppenIDListe.Add(clonedGruppe.Ident)
                    'club.Gruppenliste.Add(clonedGruppe)
                End If
            Next
        End If

        ' nicht zugewiesene Teilnehmer hinzufügen
        If source.NichtZugewieseneTeilnehmerListe IsNot Nothing Then
            If target.NichtZugewieseneTeilnehmerListe Is Nothing Then
                target.NichtZugewieseneTeilnehmerListe = New TeilnehmerCollection()
                target.NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)()
            End If
            For Each t In source.NichtZugewieseneTeilnehmerListe
                If Not target.NichtZugewieseneTeilnehmerListe.Any(Function(x) x.Ident = t.Ident) Then
                    target.NichtZugewieseneTeilnehmerListe.Add(New Teilnehmer(t))
                    If Not target.NichtZugewieseneTeilnehmerIDListe.Contains(t.Ident) Then target.NichtZugewieseneTeilnehmerIDListe.Add(t.Ident)
                End If
            Next
        End If

        ' verfügbare Trainer hinzufügen
        If source.VerfuegbareTrainerListe IsNot Nothing Then
            If target.VerfuegbareTrainerListe Is Nothing Then
                target.VerfuegbareTrainerListe = New TrainerCollection()
                target.VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)()
            End If
            For Each tr In source.VerfuegbareTrainerListe
                If Not target.VerfuegbareTrainerListe.Any(Function(x) x.TrainerID = tr.TrainerID) Then
                    target.VerfuegbareTrainerListe.Add(New Trainer(tr))
                    If Not target.VerfuegbareTrainerIDListe.Contains(tr.TrainerID) Then target.VerfuegbareTrainerIDListe.Add(tr.TrainerID)
                End If
            Next
        End If

        ' Ereignis signalisieren, damit UI/Views aktualisiert werden können
        OnEinteilungBearbeitet(EventArgs.Empty)

    End Sub

    Public Function CanEinteilungKopieren(source As Einteilung, target As Einteilung) As Boolean
        If target Is Nothing Then Return False
        If source Is Nothing Then Return False
        Dim club = DateiService.AktuellerClub
        If club Is Nothing OrElse club.Einteilungsliste Is Nothing Then Return False
        ' Quelle ermitteln (Einteilung, die die aktuell ausgewählte Gruppe enthält)
        If source Is Nothing Then Return False
        If target.GruppenIDListe.Count > 0 OrElse target.NichtZugewieseneTeilnehmerIDListe.Count > 0 OrElse target.VerfuegbareTrainerIDListe.Count > 0 Then Return False
        Return source.Ident <> target.Ident
    End Function


    Private Function EinteilungAusListeLesen(EinteilungListe As List(Of Einteilung), EinteilungID As Guid) As Einteilung
        For Each Einteilung In EinteilungListe
            If Einteilung.Ident = EinteilungID Then
                Return Einteilung
            End If
        Next
        Return Nothing
    End Function

End Class
