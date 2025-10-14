Imports System.Collections.ObjectModel
Imports Groupies.Entities
Imports Groupies.Entities.Generation1

Public Module MappingGeneration2

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club) As Generation4.Club
        Return MapSkiClub2Club(Skiclub, "Club aus Daten-Generation 2")
    End Function

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club, Dateiname As String) As Generation4.Club

        Dim EindeutigeLeistungsstufen = New LeistungsstufeCollection
        EindeutigeLeistungsstufen = GetAlleLeistungsstufen(Skiclub)

        Dim EindeutigeTrainer = New TrainerCollection
        EindeutigeTrainer = GetAlleTrainer(Skiclub)

        Dim EindeutigeTeilnehmer = New TeilnehmerCollection
        EindeutigeTeilnehmer = GetAlleTeilnehmer(Skiclub)
        EindeutigeTeilnehmer = ErsetzeLeistungsstufeDurchEindeutigeLeistungsstufe(EindeutigeTeilnehmer, EindeutigeLeistungsstufen)

        Dim EindeutigeGruppen = New GruppeCollection
        EindeutigeGruppen = GetAlleGruppen(Skiclub)
        EindeutigeGruppen = ErsetzeLeistungsstufeDurchEindeutigeLeistungsstufe(EindeutigeGruppen, EindeutigeLeistungsstufen)
        EindeutigeGruppen = ErsetzeTrainerDurchEindeutigeTrainer(EindeutigeGruppen, EindeutigeTrainer)

        Dim NeuerClub = New Generation4.Club With {
            .Einteilungsliste = New EinteilungCollection,
            .ClubName = If(Skiclub.ClubName, Dateiname),
            .Trainerliste = EindeutigeTrainer,
            .Teilnehmerliste = EindeutigeTeilnehmer,
            .Leistungsstufenliste = EindeutigeLeistungsstufen,
            .Faehigkeitenliste = GetAlleFaehigkeiten(Skiclub),
            .Gruppenliste = EindeutigeGruppen}

        ' Einteilung wird neu erstellt
        NeuerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag 1", .Sortierung = 1})

        ' Erste Einteilung füllen
        NeuerClub.Einteilungsliste(0).VerfuegbareTrainerListe = Skiclub.GruppenloseTrainer
        NeuerClub.Einteilungsliste(0).VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTrainer Select T.TrainerID).ToList())
        NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerListe = Skiclub.GruppenloseTeilnehmer
        NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTeilnehmer Select T.TeilnehmerID).ToList())
        NeuerClub.Einteilungsliste(0).GruppenIDListe = New ObservableCollection(Of Guid)((From g In Skiclub.Gruppenliste Select g.Ident).ToList())
        NeuerClub.Einteilungsliste(0).Gruppenliste = Skiclub.Gruppenliste

        Return NeuerClub

    End Function

    ''' <summary>
    ''' Gruppen werden aus dem Skiclub extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation2.Club) As GruppeCollection

        Dim Gruppen = New GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
        For Each g In Skiclub.Gruppenliste
            Gruppen.Add(g)
        Next

        ' Entferne doppelte Gruppen
        Gruppen = New GruppeCollection(Gruppen.GroupBy(Of String)(Function(L) L.Benennung).Select(Function(L) L.First).ToList)

        Return Gruppen

    End Function

    ''' <summary>
    ''' Sammelt alle Leistungsstufen aus den Leistungsstufen des Skiclubs, den Leistungsstufen der Teilnehmer und den Leistungsstufen der Gruppen.
    ''' Gruppiert über die Benennungen und entfernt doppelte Leistungsstufen.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufen(Skiclub As Generation2.Club) As LeistungsstufeCollection

        Dim Leistungsstufen = New LeistungsstufeCollection

        If Skiclub.Leistungsstufenliste Is Nothing Then
        Else
            'Es wird die Liste Skiclub.Leistungsstufen eingelesen
            Skiclub.Leistungsstufenliste.ToList.ForEach(Sub(Ls) Leistungsstufen.Add(New Leistungsstufe With {
                                                                                    .Benennung = Ls.Benennung,
                                                                                    .Beschreibung = Ls.Beschreibung,
                                                                                    .Ident = Ls.Ident,
                                                                                    .Faehigkeiten = Ls.Faehigkeiten,
                                                                                    .Sortierung = Ls.Sortierung}))
        End If

        If Skiclub.AlleTeilnehmer Is Nothing Then
        Else
            'Es werden die Leistungsstufen von den Teilnehmern ergänzt
            Skiclub.AlleTeilnehmer.ToList.ForEach(Sub(Tn) Leistungsstufen.Add(New Leistungsstufe With {
                                                                                            .Benennung = Tn.Leistungsstand.Benennung,
                                                                                            .Beschreibung = Tn.Leistungsstand.Beschreibung,
                                                                                            .Ident = Tn.Leistungsstand.Ident,
                                                                                            .Faehigkeiten = Tn.Leistungsstand.Faehigkeiten,
                                                                                            .Sortierung = Tn.Leistungsstand.Sortierung}))
        End If

        If Skiclub.Gruppenliste IsNot Nothing Then
            'Es werden die Leistungsstufen aus den Gruppenleistungsstufe ergänzt
            Skiclub.Gruppenliste.Where(Function(G) G.Leistungsstufe IsNot Nothing).ToList.ForEach(Sub(Gr) Leistungsstufen.Add(New Leistungsstufe With {
                                                                                                                              .Benennung = Gr.Leistungsstufe.Benennung,
                                                                                                                              .Beschreibung = Gr.Leistungsstufe.Beschreibung,
                                                                                                                              .Ident = Gr.Leistungsstufe.Ident,
                                                                                                                              .Faehigkeiten = Gr.Leistungsstufe.Faehigkeiten,
                                                                                                                              .Sortierung = Gr.Leistungsstufe.Sortierung}))

            'Es werden die Leistungsstufen aus den Gruppen von den Teilnehmern ergänzt
            Skiclub.Gruppenliste.ToList.ForEach(Sub(Gr) Gr.Mitgliederliste.Where(Function(M) M.Leistungsstand IsNot Nothing).ToList.ForEach(Sub(Tn) Leistungsstufen.Add(New Leistungsstufe With {
                                                                                                                                                                        .Benennung = Tn.Leistungsstand.Benennung,
                                                                                                                                                                        .Beschreibung = Tn.Leistungsstand.Beschreibung,
                                                                                                                                                                        .Ident = Tn.Leistungsstand.Ident,
                                                                                                                                                                        .Faehigkeiten = Tn.Leistungsstand.Faehigkeiten,
                                                                                                                                                                        .Sortierung = Tn.Leistungsstand.Sortierung})))
        End If


        ' Entferne doppelte Fähigkeiten
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of String)(Function(L) L.Benennung).Select(Function(L) L.First).ToList)

        Return Leistungsstufen

    End Function

    ''' <summary>
    ''' Faehigkeiten werden aus den Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Generation2.Club) As FaehigkeitCollection

        Dim Faehigkeiten = New FaehigkeitCollection

        ' Fähigkeiten aus den Leistungsstufen der Teilnehmer entnehmen und in die Collection einfügen
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(M) M.Leistungsstand.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f))))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) T.Leistungsstand.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f)))

        ' Entferne doppelte Fähigkeiten
        Faehigkeiten = New FaehigkeitCollection(Faehigkeiten.GroupBy(Of Guid)(Function(f) f.FaehigkeitID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Faehigkeiten

    End Function

    ''' <summary>
    ''' Teilnehmer werden aus den Gruppen und der gruppenlose Teilnehmer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Generation2.Club) As TeilnehmerCollection

        Dim Teilnehmer = New TeilnehmerCollection
        Skiclub.AlleTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))

        ' Entferne doppelte Teilnehmer
        Teilnehmer = New TeilnehmerCollection(Teilnehmer.GroupBy(Of String)(Function(T) T.VorUndNachname).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Teilnehmer

    End Function


    ''' <summary>
    ''' Trainer werden aus den Gruppen und der gruppenlose Trainer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    Private Function GetAlleTrainer(Skiclub As Generation2.Club) As TrainerCollection

        Dim Trainer = New TrainerCollection

        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) Trainer.Add(g.Trainer))
        Skiclub.GruppenloseTrainer.ToList.ForEach(Sub(T) Trainer.Add(T))

        'Entferne leere Trainer-Objekte 
        Trainer.Where(Function(T) T Is Nothing).ToList.ForEach(Function(O) Trainer.Remove(O))

        ' Entferne doppelte Trainer
        Trainer = New TrainerCollection(Trainer.GroupBy(Of String)(Function(LS) LS.VorUndNachname).Select(Function(T) T.First).ToList)

        Return Trainer

    End Function

    ''' <summary>
    ''' Die Leistungsstufe des Teilnehmers wird durch die eindeutige Leistungsstufe aus der übergebenen Leistungsstufen-Collection ersetzt.
    ''' </summary>
    ''' <param name="Teilnehmer"></param>
    ''' <param name="Leistungsstufen"></param>
    ''' <returns></returns>
    Private Function ErsetzeLeistungsstufeDurchEindeutigeLeistungsstufe(Teilnehmer As TeilnehmerCollection, Leistungsstufen As LeistungsstufeCollection) As TeilnehmerCollection
        For Each t In Teilnehmer
            ' Hier wird dem Teilnehmer die LeistungsstufenID aus der übergebenen Leistungsstufen-Collection zugewiesen oder Guid.Empty, wenn keine Leistungsstufe vorhanden ist.
            t.LeistungsstandID = If(t.Leistungsstand IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstand.Benennung).Ident, Guid.Empty)
            t.Leistungsstand = If(t.Leistungsstand IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstand.Benennung), Nothing)
        Next
        Return Teilnehmer
    End Function

    ''' <summary>
    ''' Die Leistungsstufe der Gruppe wird durch die eindeutige Leistungsstufe aus der übergebenen Leistungsstufen-Collection ersetzt.
    ''' </summary>
    ''' <param name="Gruppen"></param>
    ''' <param name="Leistungsstufen"></param>
    ''' <returns></returns>
    Private Function ErsetzeLeistungsstufeDurchEindeutigeLeistungsstufe(Gruppen As GruppeCollection, Leistungsstufen As LeistungsstufeCollection) As GruppeCollection
        For Each g In Gruppen
            ' Hier wird dem Teilnehmer die LeistungsstufenID aus der übergebenen Leistungsstufen-Collection zugewiesen oder Guid.Empty, wenn keine Leistungsstufe vorhanden ist.
            g.LeistungsstufeID = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung).Ident, Guid.Empty)
            g.Leistungsstufe = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung), Nothing)
        Next
        Return Gruppen
    End Function

    ''' <summary>
    ''' Die Trainer der Gruppe wird durch den eindeutige Trainer aus der übergebenen Trainer-Collection ersetzt.
    ''' </summary>
    ''' <param name="Gruppen"></param>
    ''' <param name="Trainer"></param>
    ''' <returns></returns>
    Private Function ErsetzeTrainerDurchEindeutigeTrainer(Gruppen As GruppeCollection, Trainer As TrainerCollection) As GruppeCollection
        For Each g In Gruppen
            ' Hier wird dem Teilnehmer die LeistungsstufenID aus der übergebenen Leistungsstufen-Collection zugewiesen oder Guid.Empty, wenn keine Leistungsstufe vorhanden ist.
            g.TrainerID = If(g.Trainer IsNot Nothing, Trainer.First(Function(T) T.VorUndNachname = g.Trainer.VorUndNachname).TrainerID, Guid.Empty)
            g.Trainer = If(g.Trainer IsNot Nothing, Trainer.First(Function(T) T.VorUndNachname = g.Trainer.VorUndNachname), Nothing)
        Next
        Return Gruppen
    End Function


End Module
