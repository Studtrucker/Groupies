Imports System.Collections.ObjectModel
Imports Groupies.Entities
Imports Groupies.Entities.Generation1

Public Module MappingGeneration2

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club) As Generation4.Club
        Return MapSkiClub2Club(Skiclub, "Club Gen2")
    End Function

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club, Dateiname As String) As Generation4.Club

        Dim AlleLeistungsstufen = New LeistungsstufeCollection
        AlleLeistungsstufen = GetAlleLeistungsstufen(Skiclub)


        Dim Gruppen = New GruppeCollection
        Gruppen = GetAlleGruppen(Skiclub, AlleLeistungsstufen)


        Dim Teilnehmer = New TeilnehmerCollection
        Teilnehmer = GetAlleTeilnehmer(Skiclub, AlleLeistungsstufen)


        Dim NeuerClub = New Generation4.Club With {
            .Einteilungsliste = New EinteilungCollection,
            .ClubName = If(Skiclub.ClubName, Dateiname),
            .Trainerliste = GetAlleTrainer(Skiclub),
            .Teilnehmerliste = GetAlleTeilnehmer(Skiclub),
            .Leistungsstufenliste = AlleLeistungsstufen,
            .Faehigkeitenliste = GetAlleFaehigkeiten(Skiclub),
            .Gruppenliste = Gruppen}

        NeuerClub.Teilnehmerliste.KorrekturLeistungsstufen(NeuerClub.Leistungsstufenliste)
        NeuerClub.Gruppenliste.KorrekturLeistungsstufen(NeuerClub.Leistungsstufenliste)


        ' Einteilung wird neu erstellt
        NeuerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag 1", .Sortierung = 1})

        ' Erste Einteilung füllen
        NeuerClub.Einteilungsliste(0).VerfuegbareTrainerListe = Skiclub.GruppenloseTrainer
        NeuerClub.Einteilungsliste(0).VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTrainer Select T.TrainerID).ToList())
        NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerListe = Skiclub.GruppenloseTeilnehmer
        NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTeilnehmer Select T.TeilnehmerID).ToList())
        NeuerClub.Einteilungsliste(0).GruppenIDListe = New ObservableCollection(Of Guid)((From g In Skiclub.Gruppenliste Select g.Ident).ToList())
        NeuerClub.Einteilungsliste(0).Gruppenliste = Skiclub.Gruppenliste

        ' Todo: Statements lesbar optimieren
        Skiclub.Gruppenliste.Where(Function(Gl) Gl.Trainer IsNot Nothing).ToList.ForEach(Sub(Gl) Gl.TrainerID = Gl.Trainer.TrainerID)
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G) G.LeistungsstufeID = NeuerClub.Leistungsstufenliste.Where(Function(Ls) Ls.Benennung = G.Leistungsstufe.Benennung).Single.Ident)


        Return NeuerClub

    End Function

    ''' <summary>
    ''' Gruppen werden aus dem Skiclub extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation2.Club, Leistungsstufen As LeistungsstufeCollection) As GruppeCollection
        Dim Gruppen = New GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen

        For Each g In Skiclub.Gruppenliste
            g.LeistungsstufeID = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung).Ident, Guid.Empty)
            'g.Leistungsstufe.Ident = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung).Ident, Guid.Empty)
            g.Leistungsstufe = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung), Nothing)
            g.TrainerID = If(g.Trainer IsNot Nothing, g.Trainer.TrainerID, Guid.Empty)
            Gruppen.Add(g)
        Next

        'Skiclub.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g))
        'Gruppen.ToList.ForEach(Sub(G) G.LeistungsstufeID = Skiclub.Gruppenliste.First(Function(G2G) G2G.GruppenID = G.GruppenID).Leistungsstufe.Ident)
        Return Gruppen

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
            g.LeistungsstufeID = If(g.Leistungsstufe IsNot Nothing, Skiclub.Leistungsstufenliste.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung).Ident, Guid.Empty)
            g.TrainerID = If(g.Trainer IsNot Nothing, g.Trainer.TrainerID, Guid.Empty)
            Gruppen.Add(g)
        Next

        'Skiclub.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g))
        'Gruppen.ToList.ForEach(Sub(G) G.LeistungsstufeID = Skiclub.Gruppenliste.First(Function(G2G) G2G.GruppenID = G.GruppenID).Leistungsstufe.Ident)
        Return Gruppen

    End Function

    ''' <summary>
    ''' Leistungsstufen werden aus den Leistungsstufen gelesen
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
            Skiclub.Gruppenliste.ToList.ForEach(Sub(Gr) Leistungsstufen.Add(New Leistungsstufe With {
                                                                                            .Benennung = Gr.Leistungsstufe.Benennung,
                                                                                            .Beschreibung = Gr.Leistungsstufe.Beschreibung,
                                                                                            .Ident = Gr.Leistungsstufe.Ident,
                                                                                            .Faehigkeiten = Gr.Leistungsstufe.Faehigkeiten,
                                                                                            .Sortierung = Gr.Leistungsstufe.Sortierung}))
            'Es werden die Leistungsstufen aus den Gruppen von den Teilnehmern ergänzt
            Skiclub.Gruppenliste.ToList.ForEach(Sub(Gr) Gr.Mitgliederliste.ToList.ForEach(Sub(Tn) Leistungsstufen.Add(New Leistungsstufe With {
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
    Private Function GetAlleTeilnehmer(Skiclub As Generation2.Club, Leistungsstufen As LeistungsstufeCollection) As TeilnehmerCollection

        Dim Teilnehmer = New TeilnehmerCollection

        For Each t In Skiclub.AlleTeilnehmer
            t.LeistungsstandID = If(t.Leistungsstand IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstand.Benennung).Ident, Guid.Empty)
            t.Leistungsstand.Ident = If(t.Leistungsstand IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstand.Benennung).Ident, Guid.Empty)
            Teilnehmer.Add(t)
        Next

        Return Teilnehmer

    End Function

    ''' <summary>
    ''' Teilnehmer werden aus den Gruppen und der gruppenlose Teilnehmer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Generation2.Club) As TeilnehmerCollection

        Dim Teilnehmer = New TeilnehmerCollection

        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))

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
        Trainer = New TrainerCollection(Trainer.GroupBy(Of Guid)(Function(LS) LS.TrainerID).Select(Function(T) T.First).ToList)

        Return Trainer

    End Function

    ''' <summary>
    ''' Leistungsstufen werden aus den Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufenVonTeilnehmern(Skiclub As Generation2.Club) As LeistungsstufeCollection
        ' Eigene Collection initialisieren
        Dim Leistungsstufen = New LeistungsstufeCollection
        ' Leistungsstufen aus den Teilnehmern entnehmen und in die Collection einfügen
        Skiclub.Gruppenliste.ToList.ForEach(Sub(Gl) Gl.Mitgliederliste.ToList.ForEach(Sub(M) Leistungsstufen.Add(M.Leistungsstand)))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Leistungsstufen.Add(T.Leistungsstand))
        ' Entferne doppelte Leistungsstufen
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of String)(Function(LS) LS.Benennung).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Leistungsstufen

    End Function

End Module
