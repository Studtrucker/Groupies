Imports Groupies.Entities

Public Module MappingGeneration2

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club) As Generation4.Club

        Dim NeuerClub = New Generation4.Club With {
            .Einteilungsliste = New EinteilungCollection,
            .ClubName = If(Skiclub.ClubName, "Club"),
            .AlleTrainer = GetAlleTrainer(Skiclub),
            .AlleTeilnehmer = GetAlleTeilnehmer(Skiclub),
            .AlleLeistungsstufen = GetAlleLeistungsstufenVonTeilnehmern(Skiclub),
            .AlleFaehigkeiten = GetAlleFaehigkeiten(Skiclub),
            .AlleGruppen = GetAlleGruppen(Skiclub)}

        ' Einteilung wird neu erstellt
        NeuerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag 1", .Sortierung = 1})

        ' Erste Einteilung füllen
        Skiclub.Gruppenliste.ToList.ForEach(Sub(Gl) NeuerClub.Einteilungsliste(0).Gruppenliste.Add(Gl))
        NeuerClub.Einteilungsliste(0).GruppenloseTrainer = Skiclub.GruppenloseTrainer
        NeuerClub.Einteilungsliste(0).GruppenloseTeilnehmer = Skiclub.GruppenloseTeilnehmer

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
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g))

        Return Gruppen

    End Function

    ''' <summary>
    ''' Faehigkeiten werden aus den Teilnehmern extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Generation2.Club) As FaehigkeitCollection

        ' Leere Fähigkeit hinzufügen für Dropdowns
        Dim Faehigkeiten = New FaehigkeitCollection From {
            New Faehigkeit With {.FaehigkeitID = Guid.Empty, .Benennung = String.Empty, .Sortierung = -1}
        }

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
        ' Leistungsstufe mit ID Guid.Empty hinzufügen, damit die Dropdownlisten einen leeren Eintrag bekommen
        Dim Leistungsstufen = New LeistungsstufeCollection From {
            New Leistungsstufe With {.LeistungsstufeID = Guid.Empty, .Benennung = String.Empty, .Sortierung = -1}
        }
        ' Leistungsstufen aus den Teilnehmern entnehmen und in die Collection einfügen
        Skiclub.Gruppenliste.ToList.ForEach(Sub(Gl) Gl.Mitgliederliste.ToList.ForEach(Sub(M) Leistungsstufen.Add(M.Leistungsstand)))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Leistungsstufen.Add(T.Leistungsstand))
        ' Entferne doppelte Leistungsstufen
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.LeistungsstufeID).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Leistungsstufen

    End Function

End Module
