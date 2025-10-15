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

        NeuerClub.Einteilungsliste(0).NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)((From T In Skiclub.GruppenloseTeilnehmer Select T.Ident).ToList())
        NeuerClub.Einteilungsliste(0).GruppenIDListe = New ObservableCollection(Of Guid)((From g In Skiclub.Gruppenliste Select g.Ident).ToList())
        NeuerClub.Einteilungsliste(0).Gruppenliste = Skiclub.Gruppenliste

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

        Dim Faehigkeiten = New Generation4.FaehigkeitCollection
        Skiclub.Gruppenliste.Where(Function(g) g.Leistungsstufe IsNot Nothing AndAlso g.Leistungsstufe.Faehigkeiten IsNot Nothing).SelectMany(Function(g) g.Leistungsstufe.Faehigkeiten).ToList.ForEach(Sub(f) Faehigkeiten.Add(f))
        Skiclub.AlleTeilnehmer.Where(Function(T) T.Leistungsstufe IsNot Nothing AndAlso T.Leistungsstufe.Faehigkeiten IsNot Nothing).SelectMany(Function(T) T.Leistungsstufe.Faehigkeiten).ToList.ForEach(Sub(f) Faehigkeiten.Add(f))
        Skiclub.Leistungsstufenliste.Where(Function(Ls) Ls.Faehigkeiten IsNot Nothing).SelectMany(Function(Ls) Ls.Faehigkeiten).ToList.ForEach(Sub(f) Faehigkeiten.Add(f))

        ' Entferne doppelte Fähigkeiten
        Dim EindeutigeFaehigkeiten = New Generation4.FaehigkeitCollection(Faehigkeiten.GroupBy(Function(f)
                                                                                                   Return f.Benennung
                                                                                               End Function).Select(Function(FGruppierung)
                                                                                                                        Return FGruppierung.First
                                                                                                                    End Function).SelectMany(Of Faehigkeit)(Function(f)
                                                                                                                                                                Return New Faehigkeit With {
                                                                                                                                                    .Benennung = f.Benennung,
                                                                                                                                                    .Sortierung = f.Sortierung,
                                                                                                                                                    .Beschreibung = f.Beschreibung,
                                                                                                                                                    .FaehigkeitID = f.FaehigkeitID
                                                                                                                                                    }
                                                                                                                                                            End Function).ToList)

        Return Faehigkeiten

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

        Dim Trainer = New Generation4.TrainerCollection
        Skiclub.Gruppenliste.Where(Function(G) G.Trainer IsNot Nothing).Select(Function(G) G.Trainer).ToList.ForEach(Sub(T) Trainer.Add(T))
        Skiclub.GruppenloseTrainer.ToList.ForEach(Sub(T) Trainer.Add(T))

        ' Entferne doppelte Trainer
        Trainer = New Generation4.TrainerCollection(Trainer.GroupBy(Of String)(Function(LS) LS.VorUndNachname).Select(Function(T) T.First).SelectMany(Of Trainer)(Function(T)
                                                                                                                                                                      Return New Trainer With {
                                                                                                                                                          .TrainerID = T.TrainerID,
                                                                                                                                                          .Vorname = T.Vorname,
                                                                                                                                                          .Nachname = T.Nachname,
                                                                                                                                                          .Spitzname = T.Spitzname,
                                                                                                                                                          .Foto = T.Foto,
                                                                                                                                                          .EMail = T.EMail
                                                                                                                                                          }
                                                                                                                                                                  End Function).ToList)

        Return Trainer

    End Function

    ''' <summary>
    ''' Sammelt alle Leistungsstufen.
    ''' Leistungsstufen des Skiclubs, 
    ''' den Leistungsstufen der Teilnehmer 
    ''' und die Leistungsstufen der Gruppen.
    ''' Gruppiert über die Benennungen und entfernt doppelte Leistungsstufen.
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufen(Skiclub As Generation2.Club, EindeutigeFaehigkeiten As Generation4.FaehigkeitCollection) As Generation4.LeistungsstufeCollection

        Dim Leistungsstufen = New LeistungsstufeCollection

        ' Erklärung:
        ' 1. Where: Leistungsstufe ist nicht Nothing, FähigkeitenListe ist nicht Nothing und FähigkeitenListe hat mindestens ein Element = Liste<Leistungsstufe>
        ' 2. ForEach: Für jede Leistungsstufe in Skiclub.Leistungsstufenliste
        ' 3. ForEach: Für jede Fähigkeit in der Leistungsstufe.Fähigkeitenliste
        ' 4. Zuweisung: Weise der Fähigkeit die eindeutige Fähigkeit aus der eindeutigen Fähigkeitenliste zu, die den gleichen Namen hat.
        Skiclub.Leistungsstufenliste.Where(Function(G2L)
                                               Return G2L IsNot Nothing AndAlso G2L.Faehigkeiten IsNot Nothing AndAlso G2L.Faehigkeiten.Count > 0
                                           End Function).ToList.ForEach(Sub(G2L)
                                                                            G2L.Faehigkeiten.ToList.ForEach(Sub(G2F)
                                                                                                                G2F = EindeutigeFaehigkeiten.Where(Function(G4F) G2F.Benennung = G4F.Benennung).Single
                                                                                                            End Sub)
                                                                            Leistungsstufen.Add(G2L)
                                                                        End Sub)

        Skiclub.AlleTeilnehmer.Where(Function(T) T.Leistungsstufe IsNot Nothing).ToList.ForEach(Sub(T) Leistungsstufen.Add(T.Leistungsstufe))
        Skiclub.Gruppenliste.Where(Function(G) G.Leistungsstufe IsNot Nothing).ToList.ForEach(Sub(G) Leistungsstufen.Add(G.Leistungsstufe))
        Skiclub.Gruppenliste.Where(Function(g) g.Mitgliederliste IsNot Nothing).ToList.Where(Function(M) M.Leistungsstufe IsNot Nothing).Select(Function(M) M.Leistungsstufe).ToList.ForEach(Sub(L) Leistungsstufen.Add(L))

        ' Entferne doppelte Fähigkeiten
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of String)(Function(L) L.Benennung).Select(Function(L) L.First))

        Return Leistungsstufen

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
        Skiclub.AlleTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        Skiclub.GruppenloseTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        Skiclub.EingeteilteTeilnehmer.ToList.ForEach(Sub(T) Teilnehmer.Add(T))
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))

        ' Entferne doppelte Teilnehmer
        Teilnehmer = New Generation4.TeilnehmerCollection(Teilnehmer.GroupBy(Of String)(Function(T) T.VorUndNachname).Select(Function(Gruppe) Gruppe.First).SelectMany(Of Generation2.Teilnehmer)(Function(T)
                                                                                                                                                                                                      Return New Generation4.Teilnehmer With {
                                                                                                                                                                              .Ident = T.TeilnehmerID,
                                                                                                                                                                              .Vorname = T.Vorname,
                                                                                                                                                                              .Nachname = T.Nachname}
                                                                                                                                                                                                  End Function).ToList)

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
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g))

        ' Entferne doppelte Gruppen
        Gruppen = Gruppen.GroupBy(Of String)(Function(L) L.Benennung).Select(Function(L) L.First)

        Return Gruppen

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
            t.LeistungsstufeID = If(t.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstufe.Benennung).Ident, Guid.Empty)
            t.Leistungsstufe = If(t.Leistungsstufe IsNot Nothing, Leistungsstufen.First(Function(Ls) Ls.Benennung = t.Leistungsstufe.Benennung), Nothing)
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
