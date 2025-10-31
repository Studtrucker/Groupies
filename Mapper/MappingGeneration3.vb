Imports Groupies.Entities

Public Module MappingGeneration3

    Private NeuerClub As Generation4.Club

    Public Function MapSkiClub2Club(Skiclub As Generation3.Club) As Generation4.Club

        Return MapSkiClub2Club(Skiclub, "Club Gen3")
    End Function

    Public Function MapSkiClub2Club(Skiclub As Generation3.Club, Dateiname As String) As Generation4.Club
        MsgBox("Achtung: Ein Skiclub der Generation 3 kann zur Zeit nicht geöffnet werden!", MsgBoxStyle.Exclamation, "Warten Sie auf ein Update")
        Return Nothing

        'Dim Leistungsstufen = GetAlleLeistungsstufen(Skiclub)

        'Dim Gruppen = GetAlleGruppen(Skiclub, Leistungsstufen)

        'Dim Teilnehmer = GetAlleTeilnehmer(Skiclub, Leistungsstufen)

        '' Trainer laden
        '' Teilnehmer laden
        '' Leistungsstufen laden
        '' Fähigkeiten laden
        '' Gruppen laden
        ''Einteilungen laden
        'NeuerClub = New Generation4.Club With {
        '    .ClubName = If(Skiclub.ClubName, Dateiname),
        '    .Trainerliste = GetAlleTrainer(Skiclub),
        '    .Teilnehmerliste = Teilnehmer,
        '    .Leistungsstufenliste = Leistungsstufen,
        '    .Faehigkeitenliste = GetAlleFaehigkeiten(Skiclub),
        '    .Gruppenliste = Gruppen,
        '    .Einteilungsliste = GetAlleEinteilungen(Skiclub)}

        ''NeuerClub = Skiclub
        'Return NeuerClub

    End Function

    '    ''' <summary>
    '    ''' Einteilungen werden aus dem Skiclub extrahiert
    '    ''' </summary>
    '    ''' <param name="Skiclub"></param>
    '    ''' <returns></returns>
    '    Private Function GetAlleEinteilungen(Skiclub As Generation3.Club) As Generation4.EinteilungCollection
    '        Dim Einteilungen As Generation4.EinteilungCollection

    '        ' Todo: Gruppen, Teilnehmer und Trainer den Einteilungen zuordnen
    '        Einteilungen = Skiclub.Einteilungsliste.Select(Of Generation4.Einteilung)(Function(E) New Generation4.Einteilung With {
    '                                                                                      .Benennung = E.Benennung,
    '                                                                                      .Ident = E.Ident,
    '                                                                                      .Gruppenliste = New Generation4.GruppeCollection,
    '                                                                                      .NichtZugewieseneTeilnehmerListe = New Generation4.TeilnehmerCollection,
    '                                                                                      .VerfuegbareTrainerListe = New Generation4.TrainerCollection
    '                                                                                      })

    '        Return Einteilungen
    '    End Function

    '    ''' <summary>
    '    ''' Gruppen werden aus den Einteilungen des Skiclubs extrahiert
    '    ''' </summary>
    '    ''' <param name="Skiclub"></param>
    '    ''' <returns></returns>
    '    Private Function GetAlleGruppen(Skiclub As Generation3.Club, Leistungsstufenliste As Generation4.LeistungsstufeCollection) As Generation4.GruppeCollection
    '        Dim Gruppen = New Generation4.GruppeCollection
    '        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g))

    '        ' Entferne doppelte Gruppen
    '        Gruppen = New Generation4.GruppeCollection(Gruppen.GroupBy(Of Guid)(Function(G) G.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

    '        Return Gruppen

    '    End Function

    '    ''' <summary>
    '    ''' Faehigkeiten werden aus den Teilnehmern extrahiert
    '    ''' </summary>
    '    ''' <param name="Skiclub"></param>
    '    ''' <returns></returns>
    '    Private Function GetAlleFaehigkeiten(Skiclub As Generation3.Club) As Generation4.FaehigkeitCollection

    '        Dim Faehigkeiten = New Generation4.FaehigkeitCollection
    '        ' Fähigkeiten aus den Leistungsstufen der Teilnehmer entnehmen und in die Collection einfügen
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(M) M.Leistungsstufe.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f)))))
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(e) e.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(T) T.Leistungsstufe.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f))))

    '        ' Entferne doppelte Fähigkeiten
    '        Faehigkeiten = New Generation4.FaehigkeitCollection(Faehigkeiten.GroupBy(Of Guid)(Function(f) f.FaehigkeitID).Select(Function(Gruppe) Gruppe.First).ToList)

    '        Return Faehigkeiten

    '    End Function

    '    ''' <summary>
    '    ''' Teilnehmer werden aus den Gruppen und der gruppenlose Teilnehmer-Liste extrahiert
    '    ''' </summary>
    '    ''' <param name="Skiclub"></param>
    '    ''' <returns></returns>
    '    Private Function GetAlleTeilnehmer(Skiclub As Generation3.Club, Leistungsstufenliste As Generation4.LeistungsstufeCollection) As Generation4.TeilnehmerCollection

    '        Dim Teilnehmer = New Generation4.TeilnehmerCollection

    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T))))
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))

    '        ' Entferne doppelte Teilnehmer
    '        Teilnehmer = New Generation4.TeilnehmerCollection(Teilnehmer.GroupBy(Of Guid)(Function(f) f.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

    '        Return Teilnehmer

    '    End Function

    '    ''' <summary>
    '    ''' Trainer werden aus den Gruppen und der gruppenlose Trainer-Liste extrahiert
    '    ''' </summary>
    '    ''' <param name="Skiclub"></param>
    '    Private Function GetAlleTrainer(Skiclub As Generation3.Club) As Generation4.TrainerCollection

    '        Dim Trainer = New Generation4.TrainerCollection

    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) Trainer.Add(g.Trainer)))
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.VerfuegbareTrainerListe.ToList.ForEach(Sub(T) Trainer.Add(T)))

    '        'Entferne leere Trainer-Objekte 
    '        Trainer.Where(Function(T) T Is Nothing).ToList.ForEach(Function(O) Trainer.Remove(O))

    '        ' Entferne doppelte Trainer
    '        Trainer = New Generation4.TrainerCollection(Trainer.GroupBy(Of Guid)(Function(LS) LS.TrainerID).Select(Function(T) T.First).ToList)

    '        Return Trainer

    '    End Function

    '    ''' <summary>
    '    ''' Leistungsstufen werden aus den Teilnehmern extrahiert
    '    ''' </summary>
    '    ''' <param name="Skiclub"></param>
    '    ''' <returns></returns>
    '    Private Function GetAlleLeistungsstufenVonTeilnehmern(Skiclub As Generation3.Club) As Generation4.LeistungsstufeCollection
    '        ' Eigene Collection initialisieren
    '        Dim Leistungsstufen = New Generation4.LeistungsstufeCollection
    '        ' Leistungsstufen aus den Teilnehmern entnehmen und in die Collection einfügen
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(Gl) Gl.Mitgliederliste.ToList.ForEach(Sub(M) Leistungsstufen.Add(M.Leistungsstufe))))
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(T) Leistungsstufen.Add(T.Leistungsstufe)))

    '        ' Entferne doppelte Leistungsstufen
    '        Leistungsstufen = New Generation4.LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

    '        Return Leistungsstufen

    '    End Function

    '    Private Function GetAlleLeistungsstufenVonGruppen(Skiclub As Generation3.Club) As Generation4.LeistungsstufeCollection
    '        ' Eigene Collection initialisieren
    '        Dim Leistungsstufen = New Generation4.LeistungsstufeCollection
    '        ' Leistungsstufen aus den Gruppen entnehmen und in die Collection einfügen
    '        'Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(Gl) Leistungsstufen.Add(Gl.Leistungsstufe)))

    '        ' Entferne doppelte Leistungsstufen
    '        Leistungsstufen = New Generation4.LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

    '        Return Leistungsstufen

    '    End Function

    '    Private Function GetAlleLeistungsstufen(Skiclub As Generation3.Club) As Generation4.LeistungsstufeCollection
    '        ' Eigene Collection initialisieren
    '        Dim Leistungsstufen = Skiclub.Leistungsstufenliste
    '        ' Leistungsstufen aus den Teilnehmern entnehmen und in die Collection einfügen
    '        Leistungsstufen.ToList.AddRange(GetAlleLeistungsstufenVonGruppen(Skiclub))
    '        Leistungsstufen.ToList.AddRange(GetAlleLeistungsstufenVonTeilnehmern(Skiclub))

    '        ' Entferne doppelte Leistungsstufen
    '        Dim EindeutigeLeistungsstufen = New Generation4.LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

    '        Return EindeutigeLeistungsstufen

    '    End Function

End Module
