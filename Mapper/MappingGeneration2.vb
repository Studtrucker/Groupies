Imports System.Collections.ObjectModel
Imports Groupies.Entities

Public Module MappingGeneration2

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club) As Generation4.Club
        Return MapSkiClub2Club(Skiclub, "Club aus Daten-Generation 2")
    End Function

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club, Dateiname As String) As Generation4.Club

        Dim EindeutigeFaehigkeiten = GetAlleFaehigkeiten(Skiclub)

        Dim EindeutigeTrainer = GetAlleTrainer(Skiclub)

        Dim EindeutigeLeistungsstufen = GetAlleLeistungsstufen(Skiclub, EindeutigeFaehigkeiten)

        Dim EindeutigeTeilnehmer = GetAlleTeilnehmer(Skiclub, EindeutigeLeistungsstufen)

        Dim EindeutigeGruppen = GetAlleGruppen(Skiclub, EindeutigeLeistungsstufen, EindeutigeTrainer, EindeutigeTeilnehmer)

        Dim NeuerClub = New Generation4.Club With {
            .Einteilungsliste = New Generation4.EinteilungCollection,
            .ClubName = If(Skiclub.ClubName, Dateiname),
            .Trainerliste = EindeutigeTrainer,
            .Teilnehmerliste = EindeutigeTeilnehmer,
            .Leistungsstufenliste = EindeutigeLeistungsstufen,
            .Faehigkeitenliste = GetAlleFaehigkeiten(Skiclub),
            .Gruppenliste = EindeutigeGruppen}

        ' Einteilung wird neu erstellt
        NeuerClub.Einteilungsliste.Add(New Generation4.Einteilung With {.Benennung = "Tag 1", .Sortierung = 1})

        ' Erste Einteilung füllen
        NeuerClub.Einteilungsliste(0).VerfuegbareTrainerListe = New Generation4.TrainerCollection(Skiclub.GruppenloseTrainer.Select(Of Generation4.Trainer)(Function(T)
                                                                                                                                                                Return New Generation4.Trainer With {
                                                                                                                                                                .TrainerID = T.TrainerID,
                                                                                                                                                                .Vorname = T.Vorname,
                                                                                                                                                                .Nachname = T.Nachname,
                                                                                                                                                                .Spitzname = T.Spitzname,
                                                                                                                                                                .Foto = T.Foto}
                                                                                                                                                            End Function).ToList())

        NeuerClub.Einteilungsliste(0).VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTrainer Select T.TrainerID).ToList())

        NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerListe = New Generation4.TeilnehmerCollection(Skiclub.GruppenloseTeilnehmer.Select(Of Generation4.Teilnehmer)(Function(T)
                                                                                                                                                                                 Return New Generation4.Teilnehmer With {
                                                                                                                                                                                 .Ident = T.TeilnehmerID,
                                                                                                                                                                                 .Vorname = T.Vorname,
                                                                                                                                                                                 .Nachname = T.Nachname}
                                                                                                                                                                             End Function).ToList())

        'NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTeilnehmer Select T.Ident).ToList())
        'NeuerClub.Einteilungsliste(0).GruppenIDListe = New ObservableCollection(Of Guid)((From g In Skiclub.Gruppenliste Select g.Ident).ToList())
        'NeuerClub.Einteilungsliste(0).Gruppenliste = Skiclub.Gruppenliste

        Return NeuerClub

    End Function

    ''' <summary>
    ''' Faehigkeiten werden aus dem Skiclub extrahiert.
    ''' Dazu werden die Fähigkeiten der Leistungsstufen, 
    ''' der Gruppen.Leistungsstufen 
    ''' und Teilnehmer.Leistungsstufen gesammelt.
    ''' Doppelte Fähigkeiten werden nach einer Gruppierung über die Benennung entfernt.
    ''' Die Faehigkeiten werden dann in Generation 4 Faehigkeiten umgewandelt.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Generation2.Club) As Generation4.FaehigkeitCollection

        Dim FaehigkeitenG2 = New Generation2.FaehigkeitCollection
        Skiclub.Gruppenliste.Where(Function(G) G.Leistungsstufe IsNot Nothing _
                                       AndAlso G.Leistungsstufe.Faehigkeiten IsNot Nothing _
                                       AndAlso G.Leistungsstufe.Faehigkeiten.Count > 0).SelectMany(Function(G) G.Leistungsstufe.Faehigkeiten).ToList.ForEach(Sub(F) FaehigkeitenG2.Add(F))
        Skiclub.AlleTeilnehmer.Where(Function(T) T.Leistungsstand IsNot Nothing _
                                         AndAlso T.Leistungsstand.Faehigkeiten IsNot Nothing _
                                         AndAlso T.Leistungsstand.Faehigkeiten.Count > 0).SelectMany(Function(T) T.Leistungsstand.Faehigkeiten).ToList.ForEach(Sub(F) FaehigkeitenG2.Add(F))
        Skiclub.Leistungsstufenliste.Where(Function(L) L.Faehigkeiten IsNot Nothing _
                                         AndAlso L.Faehigkeiten.Count > 0).SelectMany(Function(L) L.Faehigkeiten).ToList.ForEach(Sub(F) FaehigkeitenG2.Add(F))

        ' Entferne doppelte Fähigkeiten und wandle in Generation 4 Fähigkeiten um
        ' Erklärung:
        ' 1. GroupBy: Gruppiere die Fähigkeiten über die Benennung
        ' 2. Select: Wähle aus jeder Gruppe das erste Element (die erste Fähigkeit)
        ' 3. ToList: Konvertiere das Ergebnis in eine Liste
        ' 4. Select: Wandle jede Fähigkeit in eine Generation4.Fähigkeit um
        Dim FaehigkeitenG4 = New Generation4.FaehigkeitCollection(FaehigkeitenG2.GroupBy(Function(f)
                                                                                             Return New With {Key f.Benennung, Key f.Beschreibung}
                                                                                         End Function).Select(Function(G)
                                                                                                                  Return G.First
                                                                                                              End Function).ToList.Select(Function(f)
                                                                                                                                              Return New Generation4.Faehigkeit With {
                                                                                                                                              .FaehigkeitID = IIf(f.FaehigkeitID = Guid.Empty, Guid.NewGuid, f.FaehigkeitID),
                                                                                                                                              .Benennung = f.Benennung,
                                                                                                                                              .Beschreibung = f.Beschreibung,
                                                                                                                                              .Sortierung = f.Sortierung}
                                                                                                                                          End Function))

        Return FaehigkeitenG4

    End Function


    ''' <summary>
    ''' Trainer werden aus dem Skiclub extrahiert.
    ''' Dazu werden die Trainer der Gruppen, 
    ''' die gruppenlosen Trainer, 
    ''' die einegteilten Trainer 
    ''' und alle Trainer gesammelt.
    ''' Doppelte Trainer werden nach einer Gruppierung über den Namen entfernt.
    ''' Die Trainer werden dann in Generation 4 Trainer umgewandelt.   
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTrainer(Skiclub As Generation2.Club) As Generation4.TrainerCollection

        Dim TrainerG2 = New Generation2.TrainerCollection
        Skiclub.Gruppenliste.Where(Function(G) G.Trainer IsNot Nothing).Select(Function(G) G.Trainer).ToList.ForEach(Sub(T) TrainerG2.Add(T))
        Skiclub.GruppenloseTrainer.ToList.ForEach(Sub(T) TrainerG2.Add(T))
        Skiclub.EingeteilteTrainer.ToList.ForEach(Sub(T) TrainerG2.Add(T))
        Skiclub.AlleTrainer.ToList.ForEach(Sub(T) TrainerG2.Add(T))

        ' Entferne doppelte Trainer und wandle in Generation 4 Trainer um
        ' Erklärung:
        ' 1. GroupBy: Gruppiere die Trainer über die Namen
        ' 2. Select: Wähle aus jeder Gruppe das erste Element (der erste Trainer)
        ' 3. ToList: Konvertiere das Ergebnis in eine Liste
        ' 4. Select: Wandle jede Trainer in eine Generation4.Fähigkeit um
        Dim TrainerG4 = New Generation4.TrainerCollection(TrainerG2.GroupBy(Function(T)
                                                                                Return T.VorUndNachname
                                                                            End Function).Select(Function(G)
                                                                                                     Return G.First
                                                                                                 End Function).ToList.Select(Function(T)
                                                                                                                                 Return New Generation4.Trainer With {
                                                                                                                                 .TrainerID = T.TrainerID,
                                                                                                                                 .Vorname = T.Vorname,
                                                                                                                                 .Nachname = T.Nachname,
                                                                                                                                 .Spitzname = T.Spitzname,
                                                                                                                                 .Foto = T.Foto}
                                                                                                                             End Function))

        Return TrainerG4

    End Function

    ''' <summary>
    ''' Leistungsstufen werden aus dem Skiclub extrahiert.
    ''' Dazu werden die Leistungsstufen der Gruppen, 
    ''' die Leistungsstufen der Teilnehmer 
    ''' und alle Leistungsstufen aus der Leistungsstufenliste gesammelt.
    ''' Doppelte Leistungsstufen werden nach einer Gruppierung über die Benennung entfernt.
    ''' Die Leistungsstufen werden dann in Generation 4 Leistungsstufen umgewandelt.   
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufen(Skiclub As Generation2.Club, EindeutigeFaehigkeiten As Generation4.FaehigkeitCollection) As Generation4.LeistungsstufeCollection

        Dim LeistungsstufenG2 = New Generation2.LeistungsstufeCollection

        Skiclub.Gruppenliste.Where(Function(G) G.Leistungsstufe IsNot Nothing).ToList.ForEach(Sub(G) LeistungsstufenG2.Add(G.Leistungsstufe))
        Skiclub.AlleTeilnehmer.Where(Function(T) T.Leistungsstand IsNot Nothing).ToList.ForEach(Sub(T) LeistungsstufenG2.Add(T.Leistungsstand))
        Skiclub.GruppenloseTeilnehmer.Where(Function(T) T.Leistungsstand IsNot Nothing).ToList.ForEach(Sub(T) LeistungsstufenG2.Add(T.Leistungsstand))
        Skiclub.EingeteilteTeilnehmer.Where(Function(T) T.Leistungsstand IsNot Nothing).ToList.ForEach(Sub(T) LeistungsstufenG2.Add(T.Leistungsstand))
        Skiclub.Leistungsstufenliste.ToList.ForEach(Sub(L) LeistungsstufenG2.Add(L))

        ' Entferne doppelte Leistungsstufen und wandle in Generation 4 Leistungsstufen um
        ' Erklärung:
        ' 1. GroupBy: Gruppiere die Leistungsstufen über die Benennung
        ' 2. Select: Wähle aus jeder Gruppe das erste Element (die erste Leistungsstufe)
        ' 3. ToList: Konvertiere das Ergebnis in eine Liste
        ' 4. Select: Wandle jede Leistungsstufen in eine Generation4.Leistungsstufen um
        ' 5. Dabei werden die Fähigkeiten der Leistungsstufe durch die eindeutigen Fähigkeiten aus der übergebenen Fähigkeiten-Collection ersetzt.
        ' 6. Es wird auch eine Liste der Fähigkeiten-IDs erstellt.
        Dim LeistungsstufenG4 = New Generation4.LeistungsstufeCollection(LeistungsstufenG2.GroupBy(Function(L)
                                                                                                       Return L.Benennung
                                                                                                   End Function).Select(Function(G)
                                                                                                                            Return G.First
                                                                                                                        End Function).ToList.Select(Function(L)
                                                                                                                                                        ' Hier ist L eine Generation2.Leistungsstufe
                                                                                                                                                        Dim FL4 = New Generation4.FaehigkeitCollection(From T In L.Faehigkeiten.Select(Function(F)
                                                                                                                                                                                                                                           ' Suche die Fähigkeit in der übergebenen Fähigkeiten-Collection
                                                                                                                                                                                                                                           Return EindeutigeFaehigkeiten.FirstOrDefault(Function(EF) EF.Benennung = F.Benennung And EF.Beschreibung = F.Beschreibung)
                                                                                                                                                                                                                                       End Function).ToList)
                                                                                                                                                        Dim FID4Liste = New ObservableCollection(Of Guid)(From T In L.Faehigkeiten.Select(Function(F)
                                                                                                                                                                                                                                              ' Suche die Fähigkeit in der übergebenen Fähigkeiten-Collection
                                                                                                                                                                                                                                              Return EindeutigeFaehigkeiten.FirstOrDefault(Function(EF) EF.Benennung = F.Benennung And EF.Beschreibung = F.Beschreibung).FaehigkeitID
                                                                                                                                                                                                                                          End Function).ToList)
                                                                                                                                                        Dim L4 = New Generation4.Leistungsstufe With {
                                                                                                                                                        .Ident = L.LeistungsstufeID,
                                                                                                                                                        .Benennung = L.Benennung,
                                                                                                                                                        .Beschreibung = L.Beschreibung,
                                                                                                                                                        .Faehigkeiten = FL4,
                                                                                                                                                        .Sortierung = L.Sortierung,
                                                                                                                                                        .FaehigkeitenIDListe = FID4Liste}
                                                                                                                                                        Return L4
                                                                                                                                                    End Function))
        Return LeistungsstufenG4

    End Function


    ''' <summary>
    ''' Sammelt alle Teilnehmer
    ''' Teilnehmer werden aus den Gruppen
    ''' und den gruppenlosen Teilnehmern extrahiert
    ''' 
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Generation2.Club, Leistunggstufen As Generation4.LeistungsstufeCollection) As Generation4.TeilnehmerCollection

        Dim Teilnehmer = New Generation4.TeilnehmerCollection
        'Skiclub.AlleTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        'Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        'Skiclub.EingeteilteTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        'Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))

        '' Entferne doppelte Teilnehmer
        'Teilnehmer = New Generation4.TeilnehmerCollection(Teilnehmer.GroupBy(Of String)(Function(T) T.VorUndNachname).Select(Function(Gruppe) Gruppe.First).SelectMany(Of Generation2.Teilnehmer)(Function(T)
        '                                                                                                                                                                                              Return New Generation4.Teilnehmer With {
        '                                                                                                                                                                      .Ident = T.TeilnehmerID,
        '                                                                                                                                                                      .Vorname = T.Vorname,
        '                                                                                                                                                                      .Nachname = T.Nachname}
        '                                                                                                                                                                                          End Function).ToList)

        Return Teilnehmer

    End Function

    ''' <summary>
    ''' Gruppen werden aus dem Skiclub extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation2.Club, Leistungsstufen As Generation4.LeistungsstufeCollection, Trainer As Generation4.TrainerCollection, Teilnehmer As Generation4.TeilnehmerCollection) As Generation4.GruppeCollection

        Dim Gruppen = New Generation4.GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
        'Skiclub.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g))

        ' Entferne doppelte Gruppen
        'Gruppen = Gruppen.GroupBy(Of String)(Function(L) L.Benennung).Select(Function(L) L.First)

        Return Gruppen

    End Function









    '''' <summary>
    '''' Die Leistungsstufe des Teilnehmers wird durch die eindeutige Leistungsstufe aus der übergebenen Leistungsstufen-Collection ersetzt.
    '''' </summary>
    '''' <param name="Teilnehmer"></param>
    '''' <param name="Leistungsstufen"></param>
    '''' <returns></returns>
    'Private Function ErsetzeLeistungsstufeDurchEindeutigeLeistungsstufe(Teilnehmer As TeilnehmerCollection, Leistungsstufen As LeistungsstufeCollection) As TeilnehmerCollection
    '    For Each t In Teilnehmer
    '        ' Hier wird dem Teilnehmer die LeistungsstufenID aus der übergebenen Leistungsstufen-Collection zugewiesen oder Guid.Empty, wenn keine Leistungsstufe vorhanden ist.
    '        t.LeistungsstufeID = If(t.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstufe.Benennung).Ident, Guid.Empty)
    '        t.Leistungsstufe = If(t.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstufe.Benennung), Nothing)
    '    Next
    '    Return Teilnehmer
    'End Function

    '''' <summary>
    '''' Die Leistungsstufe der Gruppe wird durch die eindeutige Leistungsstufe aus der übergebenen Leistungsstufen-Collection ersetzt.
    '''' </summary>
    '''' <param name="Gruppen"></param>
    '''' <param name="Leistungsstufen"></param>
    '''' <returns></returns>
    'Private Function ErsetzeLeistungsstufeDurchEindeutigeLeistungsstufe(Gruppen As GruppeCollection, Leistungsstufen As LeistungsstufeCollection) As GruppeCollection
    '    For Each g In Gruppen
    '        ' Hier wird dem Teilnehmer die LeistungsstufenID aus der übergebenen Leistungsstufen-Collection zugewiesen oder Guid.Empty, wenn keine Leistungsstufe vorhanden ist.
    '        g.LeistungsstufeID = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung).Ident, Guid.Empty)
    '        g.Leistungsstufe = If(g.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = g.Leistungsstufe.Benennung), Nothing)
    '    Next
    '    Return Gruppen
    'End Function

    '''' <summary>
    '''' Die Trainer der Gruppe wird durch den eindeutige Trainer aus der übergebenen Trainer-Collection ersetzt.
    '''' </summary>
    '''' <param name="Gruppen"></param>
    '''' <param name="Trainer"></param>
    '''' <returns></returns>
    'Private Function ErsetzeTrainerDurchEindeutigeTrainer(Gruppen As GruppeCollection, Trainer As TrainerCollection) As GruppeCollection
    '    For Each g In Gruppen
    '        ' Hier wird dem Teilnehmer die LeistungsstufenID aus der übergebenen Leistungsstufen-Collection zugewiesen oder Guid.Empty, wenn keine Leistungsstufe vorhanden ist.
    '        g.TrainerID = If(g.Trainer IsNot Nothing, Trainer.First(Function(T) T.VorUndNachname = g.Trainer.VorUndNachname).TrainerID, Guid.Empty)
    '        g.Trainer = If(g.Trainer IsNot Nothing, Trainer.First(Function(T) T.VorUndNachname = g.Trainer.VorUndNachname), Nothing)
    '    Next
    '    Return Gruppen
    'End Function


End Module
