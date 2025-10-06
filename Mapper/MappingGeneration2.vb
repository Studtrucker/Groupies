Imports Groupies.Entities

Public Module MappingGeneration2

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club) As Generation4.Club

        Dim Leistungsstufen = New LeistungsstufeCollection
        Leistungsstufen = GetAlleLeistungsstufenVonTeilnehmern(Skiclub)

        Dim Gruppen = New GruppeCollection
        Gruppen = GetAlleGruppen(Skiclub, Leistungsstufen)

        Dim Teilnehmer = New TeilnehmerCollection
        Teilnehmer = GetAlleTeilnehmer(Skiclub, Leistungsstufen)


        Dim NeuerClub = New Generation4.Club With {
            .AlleEinteilungen = New EinteilungCollection,
            .ClubName = If(Skiclub.ClubName, "Club"),
            .AlleTrainer = GetAlleTrainer(Skiclub),
            .AlleTeilnehmer = GetAlleTeilnehmer(Skiclub),
            .AlleLeistungsstufen = Leistungsstufen,
            .AlleFaehigkeiten = GetAlleFaehigkeiten(Skiclub),
            .AlleGruppen = Gruppen}

        ' Einteilung wird neu erstellt
        NeuerClub.AlleEinteilungen.Add(New Einteilung With {.Benennung = "Tag 1", .Sortierung = 1})

        ' Erste Einteilung füllen
        Skiclub.Gruppenliste.ToList.ForEach(Sub(Gl) NeuerClub.AlleEinteilungen(0).EinteilungAlleGruppen.Add(Gl))
        Skiclub.Gruppenliste.ToList.ForEach(Sub(Gl) Gl.TrainerID = Gl.Trainer.TrainerID)
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G) G.LeistungsstufeID = NeuerClub.AlleLeistungsstufen.Where(Function(Ls) Ls.Benennung = G.Leistungsstufe.Benennung).Single.Ident)
        NeuerClub.AlleEinteilungen(0).GruppenloseTrainer = Skiclub.GruppenloseTrainer
        NeuerClub.AlleEinteilungen(0).GruppenloseTeilnehmer = Skiclub.GruppenloseTeilnehmer

        NeuerClub.AlleTeilnehmer.KorrekturLeistungsstufen(NeuerClub.AlleLeistungsstufen)
        NeuerClub.AlleGruppen.KorrekturLeistungsstufen(NeuerClub.AlleLeistungsstufen)

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
            g.Leistungsstufe.Ident = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung).Ident, Guid.Empty)
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
