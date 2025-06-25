Imports Groupies.Entities

Public Module MappingGeneration4

    Private NeuerClub As Generation4.Club

    Public Function MapSkiClub2Club(Skiclub As Generation4.Club) As Generation4.Club

        NeuerClub = Skiclub
        'NeuerClub.ClubName = If(Skiclub.ClubName, "Club")

        '' Trainer laden
        'NeuerClub.AlleTrainer = GetAlleTrainer(Skiclub)

        '' Teilnehmer laden
        'NeuerClub.AlleTeilnehmer = GetAlleTeilnehmer(Skiclub)

        '' Leistungsstufen laden
        'NeuerClub.AlleLeistungsstufen = GetAlleLeistungsstufenVonTeilnehmern(Skiclub)

        '' Fähigkeiten laden
        'NeuerClub.AlleFaehigkeiten = GetAlleFaehigkeiten(Skiclub)

        '' Gruppen laden
        'NeuerClub.AlleGruppen = GetAlleGruppen(Skiclub)

        'Einteilungen laden
        'NeuerClub.Einteilungsliste = GetAlleEinteilungen(Skiclub)

        'NeuerClub = Skiclub
        Return NeuerClub

    End Function

    ''' <summary>
    ''' Einteilungen werden aus dem Skiclub extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleEinteilungen(Skiclub As Generation4.Club) As EinteilungCollection
        Dim Einteilungen As EinteilungCollection

        Einteilungen = Skiclub.AlleEinteilungen

        Return Einteilungen
    End Function

    ''' <summary>
    ''' Gruppen werden aus den Einteilungen des Skiclubs extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation4.Club) As GruppeCollection
        Dim Gruppen = New GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.EinteilungAlleGruppen.ToList.ForEach(Sub(g) Gruppen.Add(g)))

        ' Entferne doppelte Gruppen
        Gruppen = New GruppeCollection(Gruppen.GroupBy(Of Guid)(Function(G) G.GruppenID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Gruppen

    End Function

    ''' <summary>
    ''' Faehigkeiten werden aus den Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Generation4.Club) As FaehigkeitCollection

        Dim Faehigkeiten = New FaehigkeitCollection
        ' Fähigkeiten aus den Leistungsstufen der Teilnehmer entnehmen und in die Collection einfügen
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.EinteilungAlleGruppen.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(M) M.Leistungsstand.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f)))))
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(e) e.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) T.Leistungsstand.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f))))
        ' Leere Fähigkeit hinzufügen für Dropdowns
        Faehigkeiten.Add(New Faehigkeit With {.FaehigkeitID = Guid.Empty, .Benennung = String.Empty, .Sortierung = -1})

        ' Entferne doppelte Fähigkeiten
        Faehigkeiten = New FaehigkeitCollection(Faehigkeiten.GroupBy(Of Guid)(Function(f) f.FaehigkeitID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Faehigkeiten

    End Function

    ''' <summary>
    ''' Teilnehmer werden aus den Gruppen und der gruppenlose Teilnehmer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Generation4.Club) As TeilnehmerCollection

        Dim Teilnehmer = New TeilnehmerCollection

        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.EinteilungAlleGruppen.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T))))
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))

        Return Teilnehmer

    End Function

    ''' <summary>
    ''' Trainer werden aus den Gruppen und der gruppenlose Trainer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    Private Function GetAlleTrainer(Skiclub As Generation4.Club) As TrainerCollection

        Dim Trainer = New TrainerCollection

        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.EinteilungAlleGruppen.ToList.ForEach(Sub(g) Trainer.Add(g.Trainer)))
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.GruppenloseTrainer.ToList.ForEach(Sub(T) Trainer.Add(T)))

        Return Trainer

    End Function

    ''' <summary>
    ''' Leistungsstufen werden aus den Gruppen extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    <Obsolete>
    Private Function GetAlleLeistungsstufenVonGruppen(Skiclub As Generation4.Club) As LeistungsstufeCollection
        ' Eigene Collection initialisieren
        Dim Leistungsstufen = New LeistungsstufeCollection
        ' Leistungsstufen aus den Gruppen entnehmen und in die Collection einfügen
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.EinteilungAlleGruppen.ToList.ForEach(Sub(g) Leistungsstufen.Add(g.Leistungsstufe)))
        ' Entferne doppelte Leistungsstufen
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.LeistungsstufeID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Leistungsstufen

    End Function

    ''' <summary>
    ''' Leistungsstufen werden aus den Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufenVonTeilnehmern(Skiclub As Generation4.Club) As LeistungsstufeCollection
        ' Eigene Collection initialisieren
        Dim Leistungsstufen = New LeistungsstufeCollection
        ' Leistungsstufen aus den Teilnehmern entnehmen und in die Collection einfügen
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.EinteilungAlleGruppen.ToList.ForEach(Sub(Gl) Gl.Mitgliederliste.ToList.ForEach(Sub(M) Leistungsstufen.Add(M.Leistungsstand))))
        Skiclub.AlleEinteilungen.ToList.ForEach(Sub(E) E.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Leistungsstufen.Add(T.Leistungsstand)))
        ' Leistungsstufe mit ID Guid.Empty hinzufügen, damit die Dropdownlisten einen leeren Eintrag bekommen
        Leistungsstufen.Add(New Leistungsstufe With {.LeistungsstufeID = Guid.Empty, .Benennung = String.Empty, .Sortierung = -1})
        ' Entferne doppelte Leistungsstufen
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.LeistungsstufeID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Leistungsstufen

    End Function


End Module
