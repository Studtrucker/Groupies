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
            .Faehigkeitenliste = EindeutigeFaehigkeiten,
            .Gruppenliste = EindeutigeGruppen}

        ' Einteilung wird neu erstellt
        NeuerClub.Einteilungsliste.Add(New Generation4.Einteilung With {
                                       .Benennung = "Tag 1",
                                       .Sortierung = 1,
                                       .Ident = Guid.NewGuid,
                                       .GruppenIDListe = New ObservableCollection(Of Guid)(From G In EindeutigeGruppen Select G.Ident),
                                       .Gruppenliste = EindeutigeGruppen,
                                       .VerfuegbareTrainerListe = GetVerfuegbareTrainer(Skiclub),
                                       .VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)(From T In GetVerfuegbareTrainer(Skiclub) Select T.TrainerID),
                                       .NichtZugewieseneTeilnehmerListe = NichtZugewieseneTeilnehmerListe(Skiclub, EindeutigeLeistungsstufen),
                                       .NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)(From T In NichtZugewieseneTeilnehmerListe(Skiclub, EindeutigeLeistungsstufen) Select T.Ident)})

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

        Return GetGeneration4TrainerListe(TrainerG2)

    End Function

    ''' <summary>
    ''' Trainer werden aus dem Skiclub extrahiert.
    ''' Dazu werden die gruppenlosen Trainer gesammelt.
    ''' Doppelte Trainer werden nach einer Gruppierung über den Namen entfernt.
    ''' Die Trainer werden dann in Generation 4 Trainer umgewandelt.   
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetVerfuegbareTrainer(Skiclub As Generation2.Club) As Generation4.TrainerCollection
        Dim TrainerG2 = New Generation2.TrainerCollection
        Skiclub.GruppenloseTrainer.ToList.ForEach(Sub(T) TrainerG2.Add(T))

        Return GetGeneration4TrainerListe(TrainerG2)

    End Function

    Private Function GetGeneration4TrainerListe(Trainer As Generation2.TrainerCollection) As Generation4.TrainerCollection

        ' Entferne doppelte Trainer und wandle in Generation 4 Trainer um
        ' Erklärung:
        ' 1. GroupBy: Gruppiere die Trainer über die Namen
        ' 2. Select: Wähle aus jeder Gruppe das erste Element (der erste Trainer)
        ' 3. ToList: Konvertiere das Ergebnis in eine Liste
        ' 4. Select: Wandle jede Trainer in eine Generation4.Fähigkeit um

        Dim TrainerG4 = New Generation4.TrainerCollection(Trainer.GroupBy(Function(T)
                                                                              Return T.VorUndNachname
                                                                          End Function).Select(Function(G)
                                                                                                   Return G.First
                                                                                               End Function).ToList.Select(Function(T)
                                                                                                                               Return New Generation4.Trainer With {
                                                                                                                                 .TrainerID = T.TrainerID,
                                                                                                                                 .Vorname = T.Vorname,
                                                                                                                                 .Nachname = T.Nachname,
                                                                                                                                 .[Alias] = T.Spitzname,
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
    ''' Teilnehmer werden aus dem Skiclub extrahiert.
    ''' Dazu werden die Teilnehmer der Gruppen, 
    ''' die Teilnehmer aus allenTeilnehmer,
    ''' die gruppenlosen Teilnehmer, 
    ''' und die eingeteilten Teilnehmer gesammelt.
    ''' Doppelte Teilnehmer werden nach einer Gruppierung über die Vor, Nachname und Geburtsdatum entfernt.
    ''' Die Teilnehmer werden dann in Generation 4 Teilnehmer umgewandelt.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Generation2.Club, EindeutigeLeistunggstufen As Generation4.LeistungsstufeCollection) As Generation4.TeilnehmerCollection

        Dim TeilnehmerG2 = New Generation2.TeilnehmerCollection
        Skiclub.AlleTeilnehmer.ToList.ForEach(Sub(T) TeilnehmerG2.Add(T))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) TeilnehmerG2.Add(T))
        Skiclub.EingeteilteTeilnehmer.ToList.ForEach(Sub(T) TeilnehmerG2.Add(T))
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) TeilnehmerG2.Add(T)))

        Return GetGeneration4TeilnehmerListe(TeilnehmerG2, EindeutigeLeistunggstufen)

    End Function

    ''' <summary>
    ''' Teilnehmer werden aus dem Skiclub extrahiert.
    ''' Dazu werden die Teilnehmer der gruppenlosen Teilnehmer gesammelt.
    ''' Doppelte Teilnehmer werden nach einer Gruppierung über die Vor, Nachname und Geburtsdatum entfernt.
    ''' Die Teilnehmer werden dann in Generation 4 Teilnehmer umgewandelt.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function NichtZugewieseneTeilnehmerListe(Skiclub As Generation2.Club, EindeutigeLeistunggstufen As Generation4.LeistungsstufeCollection) As Generation4.TeilnehmerCollection

        Dim TeilnehmerG2 = New Generation2.TeilnehmerCollection
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) TeilnehmerG2.Add(T))

        Return GetGeneration4TeilnehmerListe(TeilnehmerG2, EindeutigeLeistunggstufen)

    End Function

    ''' <summary>
    ''' Die Generation2 Teilnehmer werden in Generation 4 Teilnehmer umgewandelt.
    ''' </summary>
    ''' <param name="Teilnehmer"></param>
    ''' <param name="EindeutigeLeistunggstufen"></param>
    ''' <returns></returns>
    Private Function GetGeneration4TeilnehmerListe(Teilnehmer As Generation2.TeilnehmerCollection, EindeutigeLeistunggstufen As Generation4.LeistungsstufeCollection) As Generation4.TeilnehmerCollection
        ' Entferne doppelte Teilnehmer
        ' Entferne doppelte Teilnehmer und wandle in Generation 4 Teilnehmer um
        ' Erklärung:
        ' 1. GroupBy: Gruppiere die Teilnehmer über den Vor, Nachname und Geburtsdatum
        ' 2. Select: Wähle aus jeder Gruppe das erste Element (den ersten Teilnehmer)
        ' 3. ToList: Konvertiere das Ergebnis in eine Liste
        ' 4. Select: Wandle jede Teilnehmer in eine Generation4.Teilnehmer um
        ' 5. Dabei werden die Leistungsstufe durch die eindeutige Leistungsstufe aus der übergebenen Leistungsstufen-Collection ersetzt.
        Dim TeilnehmerG4 = New Generation4.TeilnehmerCollection(Teilnehmer.GroupBy(Function(T)
                                                                                       Return New With {Key T.Nachname, Key T.Vorname, Key T.Geburtsdatum}
                                                                                   End Function).Select(Function(Gruppe)
                                                                                                            Return Gruppe.First
                                                                                                        End Function).ToList.Select(Function(T)
                                                                                                                                        Dim T4L4 = If(T.Leistungsstand IsNot Nothing, EindeutigeLeistunggstufen.FirstOrDefault(Function(L)
                                                                                                                                                                                                                                   Return L.Benennung = T.Leistungsstand.Benennung
                                                                                                                                                                                                                               End Function), Nothing)
                                                                                                                                        Dim T4L4ID = If(T4L4 IsNot Nothing, T4L4.Ident, Guid.Empty)
                                                                                                                                        Dim T4 = New Generation4.Teilnehmer With {
                                                                                                                                        .Ident = T.TeilnehmerID,
                                                                                                                                        .Vorname = T.Vorname,
                                                                                                                                        .Nachname = T.Nachname,
                                                                                                                                        .Geburtsdatum = T.Geburtsdatum,
                                                                                                                                        .Leistungsstufe = T4L4,
                                                                                                                                        .LeistungsstufeID = T4L4ID}
                                                                                                                                        Return T4
                                                                                                                                    End Function).ToList)

        Return TeilnehmerG4

    End Function

    ''' <summary>
    ''' Gruppen werden aus dem Skiclub extrahiert
    ''' Dazu werden die Gruppen der Gruppenliste gesammelt.
    ''' Doppelte Gruppen werden nach einer Gruppierung über die Benennung entfernt.
    ''' Die Gruppen werden dann in Generation 4 Gruppen umgewandelt.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation2.Club, EindeutigeLeistunggstufen As Generation4.LeistungsstufeCollection, EindeutigeTrainer As Generation4.TrainerCollection, EindeutigeTeilnehmer As Generation4.TeilnehmerCollection) As Generation4.GruppeCollection

        Dim GruppenG2 = New Generation2.GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G) GruppenG2.Add(G))

        ' Entferne doppelte Gruppen und wandle in Generation 4 Gruppen um
        ' Erklärung:
        ' 1. GroupBy: Gruppiere die Gruppen über die Benennung
        ' 2. Select: Wähle aus jeder Gruppe das erste Element (den ersten Teilnehmer)
        ' 3. ToList: Konvertiere das Ergebnis in eine Liste
        ' 4. Select: Wandle jede Teilnehmer in eine Generation4.Teilnehmer um
        ' 5. Dabei werden die Leistungsstufe durch die eindeutige Leistungsstufe aus der übergebenen Leistungsstufen-Collection ersetzt.
        Dim GruppenG4 = New Generation4.GruppeCollection(GruppenG2.GroupBy(Function(G)
                                                                               Return G.Benennung
                                                                           End Function).Select(Function(G) G.First).ToList.Select(Function(G2)
                                                                                                                                       Dim G4LS4 = If(G2.Leistungsstufe IsNot Nothing, EindeutigeLeistunggstufen.FirstOrDefault(Function(L) L.Benennung = G2.Leistungsstufe.Benennung), Nothing)
                                                                                                                                       Dim G4L4ID = If(G2.Leistungsstufe IsNot Nothing, EindeutigeLeistunggstufen.FirstOrDefault(Function(L) L.Benennung = G2.Leistungsstufe.Benennung).Ident, Nothing)
                                                                                                                                       Dim G4Tr4 = If(G2.Trainer IsNot Nothing, EindeutigeTrainer.FirstOrDefault(Function(L) String.Format("{0} {1}", L.Vorname, L.Nachname) = G2.Trainer.VorUndNachname), Nothing)
                                                                                                                                       Dim G4Tr4ID = If(G2.Trainer IsNot Nothing, EindeutigeTrainer.FirstOrDefault(Function(L) String.Format("{0} {1}", L.Vorname, L.Nachname) = G2.Trainer.VorUndNachname).TrainerID, Nothing)

                                                                                                                                       Dim M4IDListe = New ObservableCollection(Of Guid)(From T In G2.Mitgliederliste.Select(Function(T2)
                                                                                                                                                                                                                                 Return EindeutigeTeilnehmer.FirstOrDefault(Function(T4) T4.Vorname = T2.Vorname And T4.Nachname = T2.Nachname And T4.Geburtsdatum = T2.Geburtsdatum)
                                                                                                                                                                                                                             End Function).ToList.Select(Function(T)
                                                                                                                                                                                                                                                             Return If(T IsNot Nothing, T.Ident, Guid.Empty)
                                                                                                                                                                                                                                                         End Function).ToList)
                                                                                                                                       Dim M4Liste = New Generation4.TeilnehmerCollection(From T In G2.Mitgliederliste.Select(Function(T2)
                                                                                                                                                                                                                                  Return EindeutigeTeilnehmer.FirstOrDefault(Function(T4) T4.Vorname = T2.Vorname And T4.Nachname = T2.Nachname And T4.Geburtsdatum = T2.Geburtsdatum)
                                                                                                                                                                                                                              End Function).ToList.Select(Function(T)
                                                                                                                                                                                                                                                              Return If(T IsNot Nothing, T, Nothing)
                                                                                                                                                                                                                                                          End Function).Where(Function(T) T IsNot Nothing).ToList)
                                                                                                                                       Dim G4 = New Generation4.Gruppe With {
                                                                                                                                       .Ident = G2.GruppenID,
                                                                                                                                       .Benennung = G2.Benennung,
                                                                                                                                       .MitgliederIDListe = M4IDListe,
                                                                                                                                       .Mitgliederliste = M4Liste,
                                                                                                                                       .Leistungsstufe = G4LS4,
                                                                                                                                       .LeistungsstufeID = G4L4ID,
                                                                                                                                       .Sortierung = G2.Sortierung,
                                                                                                                                       .Trainer = G4Tr4,
                                                                                                                                       .TrainerID = G4Tr4ID}
                                                                                                                                       Return G4
                                                                                                                                   End Function))

        Return GruppenG4

    End Function

End Module
