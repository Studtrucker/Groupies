Imports System.Security.Cryptography
Imports Groupies.Entities

Public Module MappingGeneration4

    Private NeuerClub As Generation4.Club

    Public Function MapSkiClub2Club(Skiclub As Generation4.Club) As Generation4.Club
        ' Schreibe die Club.Gruppenliste.Mitgliederliste anhand der gespeicherten TeilnehmerIDs aus der Liste TeilnehmerIDListe  
        Skiclub.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste = New TeilnehmerCollection(
                                            (From TID In g.MitgliederIDListe
                                             Let T = Skiclub.Teilnehmerliste.FirstOrDefault(Function(tn) tn.TeilnehmerID = TID)
                                             Where T IsNot Nothing
                                             Select T).ToList()))
        ' Schreibe die Club.Einteilungen.Gruppenliste anhand der gespeicherten GruppenIDs aus der Liste GruppenIDListe  
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) Skiclub.Gruppenliste.Where(Function(G) E.GruppenIDListe.Contains(G.Ident)).ToList.ForEach(Sub(g) E.Gruppenliste.Add(g)))
        ' Schreibe die Club.Einteilungen.NichtZugewieseneTeilnehmerListe anhand der gespeicherten Teilnehmer IDs aus der Liste NichtZugewieseneTeilnehmerIDListe  
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) Skiclub.Teilnehmerliste.Where(Function(T) E.NichtZugewieseneTeilnehmerIDListe.Contains(T.TeilnehmerID)).ToList.ForEach(Sub(T) E.NichtZugewieseneTeilnehmerListe.Add(T)))
        ' Schreibe die Club.Einteilungen.VerfuegbareTrainerListe anhand der gespeicherten Trainer IDs aus der Liste VerfuegbareTrainerIDListe  
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) Skiclub.Trainerliste.Where(Function(T) E.VerfuegbareTrainerIDListe.Contains(T.TrainerID)).ToList.ForEach(Sub(T) E.VerfuegbareTrainerListe.Add(T)))
        Return Skiclub

    End Function

    Public Function MapSkiClub2Club(Skiclub As Generation4.Club, Dateiname As String) As Generation4.Club
        Skiclub.ClubName = Dateiname
        Return MapSkiClub2Club(Skiclub)
    End Function

    ''' <summary>
    ''' Einteilungen werden aus dem Skiclub extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleEinteilungen(Skiclub As Generation4.Club) As EinteilungCollection
        Return Skiclub.Einteilungsliste
    End Function

    ''' <summary>
    ''' Gruppen werden aus den Einteilungen des Skiclubs extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Generation4.Club) As GruppeCollection
        Dim Gruppen = New GruppeCollection
        ' Gruppen aus dem Skiclub entnehmen und in die Collection einfügen
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) Gruppen.Add(g)))

        ' Entferne doppelte Gruppen
        Gruppen = New GruppeCollection(Gruppen.GroupBy(Of Guid)(Function(G) G.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

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
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(M) M.Leistungsstand.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f)))))
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(e) e.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(T) T.Leistungsstand.Faehigkeiten.ToList.ForEach(Sub(f) Faehigkeiten.Add(f))))


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

        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) g.Mitgliederliste.ToList.ForEach(Sub(T) Teilnehmer.Add(T))))
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(T) Teilnehmer.Add(T)))

        Return Teilnehmer

    End Function

    ''' <summary>
    ''' Trainer werden aus den Gruppen und der gruppenlose Trainer-Liste extrahiert
    ''' </summary>
    ''' <param name="Skiclub"></param>
    Private Function GetAlleTrainer(Skiclub As Generation4.Club) As TrainerCollection

        Dim Trainer = New TrainerCollection

        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) Trainer.Add(g.Trainer)))
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.VerfuegbareTrainerListe.ToList.ForEach(Sub(T) Trainer.Add(T)))

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
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(g) Leistungsstufen.Add(g.Leistungsstufe)))
        ' Entferne doppelte Leistungsstufen
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

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
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.Gruppenliste.ToList.ForEach(Sub(Gl) Gl.Mitgliederliste.ToList.ForEach(Sub(M) Leistungsstufen.Add(M.Leistungsstand))))
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(T) Leistungsstufen.Add(T.Leistungsstand)))
        ' Entferne doppelte Leistungsstufen
        Leistungsstufen = New LeistungsstufeCollection(Leistungsstufen.GroupBy(Of Guid)(Function(LS) LS.Ident).Select(Function(Gruppe) Gruppe.First).ToList)

        Return Leistungsstufen

    End Function


End Module
