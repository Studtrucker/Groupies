Imports System.Collections.ObjectModel
Imports System.Security.Cryptography
Imports Groupies.Entities.Generation4

Public Module MappingGeneration4

    Public Function MapSkiClub2Club(Skiclub As Club, Dateiname As String) As Club
        Skiclub.ClubName = Dateiname
        Return MapSkiClub2Club(Skiclub)
    End Function

    Public Function MapSkiClub2Club(Skiclub As Club) As Club

        ' Ladereihenfolge nicht ändern, da Abhängigkeiten bestehen
        Skiclub.Faehigkeitenliste = GetAlleFaehigkeiten(Skiclub)
        Skiclub.Trainerliste = GetAlleTrainer(Skiclub)
        Skiclub.Leistungsstufenliste = GetAlleLeistungsstufen(Skiclub)
        Skiclub.Teilnehmerliste = GetAlleTeilnehmer(Skiclub)
        Skiclub.Gruppenliste = GetAlleGruppen(Skiclub)
        Skiclub.Einteilungsliste = GetAlleEinteilungen(Skiclub)

        Return Skiclub

    End Function


    ''' <summary>
    ''' Faehigkeiten hat alle Daten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Club) As FaehigkeitCollection
        Return Skiclub.Faehigkeitenliste
    End Function


    ''' <summary>
    ''' Trainer hat alle Daten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    Private Function GetAlleTrainer(Skiclub As Club) As TrainerCollection
        Return Skiclub.Trainerliste
    End Function

    ''' <summary>
    ''' Leistungsstufen und deren Fähigkeitenlisten füllen
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleLeistungsstufen(Skiclub As Club) As LeistungsstufeCollection
        'Skiclub.Leistungsstufenliste.ToList.ForEach(Sub(L) L.FaehigkeitenIDListe.ToList.ForEach(Sub(FID) L.Faehigkeiten.Add(Skiclub.Faehigkeitenliste.FirstOrDefault(Function(F) F.FaehigkeitID = FID))))

        For Each L In Skiclub.Leistungsstufenliste
            For Each FID In L.FaehigkeitenIDListe
                L.Faehigkeiten.Add(Skiclub.Faehigkeitenliste.Where(Function(F) F.FaehigkeitID = FID).SingleOrDefault)
            Next
        Next

        Return Skiclub.Leistungsstufenliste
    End Function


    ''' <summary>
    ''' Teilnehmer und deren Leistungsstufen füllen
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleTeilnehmer(Skiclub As Club) As TeilnehmerCollection
        Skiclub.Teilnehmerliste.Where(Function(T) Not T.LeistungsstufeID = Guid.Empty).ToList.ForEach(Sub(T) T.Leistungsstufe = Skiclub.Leistungsstufenliste.FirstOrDefault(Function(L) L.Ident = T.LeistungsstufeID))
        Return Skiclub.Teilnehmerliste
    End Function

    ''' <summary>
    ''' Gruppen und deren Leistungsstufen, Trainer und Mitgliederliste füllen
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleGruppen(Skiclub As Club) As GruppeCollection
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G) G.Leistungsstufe = Skiclub.Leistungsstufenliste.FirstOrDefault(Function(L) L.Ident = G.LeistungsstufeID))
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G) G.Trainer = Skiclub.Trainerliste.FirstOrDefault(Function(T) T.TrainerID = G.TrainerID))
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G) G.MitgliederIDListe.ToList.ForEach(Sub(TID) G.Mitgliederliste.Add(Skiclub.Teilnehmerliste.FirstOrDefault(Function(T) T.Ident = TID))))
        Return Skiclub.Gruppenliste
    End Function

    ''' <summary>
    ''' Einteilungen und deren Gruppenliste, NichtZugewieseneTeilnehmerListe und VerfuegbareTrainerListe füllen
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns></returns>
    Private Function GetAlleEinteilungen(Skiclub As Club) As EinteilungCollection
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.GruppenIDListe.ToList.ForEach(Sub(GID) E.Gruppenliste.Add(Skiclub.Gruppenliste.FirstOrDefault(Function(G) G.Ident = GID))))
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.NichtZugewieseneTeilnehmerIDListe.ToList.ForEach(Sub(TID) E.NichtZugewieseneTeilnehmerListe.Add(Skiclub.Teilnehmerliste.FirstOrDefault(Function(T) T.Ident = TID))))
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E) E.VerfuegbareTrainerIDListe.ToList.ForEach(Sub(TID) E.VerfuegbareTrainerListe.Add(Skiclub.Trainerliste.FirstOrDefault(Function(T) T.TrainerID = TID))))
        Return Skiclub.Einteilungsliste
    End Function

End Module
