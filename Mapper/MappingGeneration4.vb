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
        Skiclub.Gruppenstammliste = GetGruppenstammdaten(Skiclub)
        Skiclub.Gruppenliste = GetAlleGruppen(Skiclub)
        Skiclub.Einteilungsliste = GetAlleEinteilungen(Skiclub)

        Return Skiclub

    End Function


    ''' <summary>
    ''' Fähigkeitenskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Fähigkeiten</returns>
    Private Function GetAlleFaehigkeiten(Skiclub As Club) As FaehigkeitCollection
        Return Skiclub.Faehigkeitenliste
    End Function


    ''' <summary>
    ''' Trainerskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Trainer</returns>
    Private Function GetAlleTrainer(Skiclub As Club) As TrainerCollection
        Return Skiclub.Trainerliste
    End Function

    ''' <summary>
    ''' Leistungsstufenskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Leistungsstufen und Fähigkeitenliste mit Fähigkeitobjekten</returns>
    Private Function GetAlleLeistungsstufen(Skiclub As Club) As LeistungsstufeCollection

        Skiclub.Leistungsstufenliste.ToList.ForEach(Sub(L)
                                                        L.FaehigkeitenIDListe.ToList.ForEach(Sub(FID) L.Faehigkeiten.Add(Skiclub.Faehigkeitenliste.FirstOrDefault(Function(F) F.FaehigkeitID = FID)))
                                                    End Sub)

        Return Skiclub.Leistungsstufenliste
    End Function


    ''' <summary>
    ''' Teilnehmerskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Teilnehmer mit Leistungsstufenobjekt, wenn Leistungsstufe gespeichert ist</returns>
    Private Function GetAlleTeilnehmer(Skiclub As Club) As TeilnehmerCollection
        Skiclub.Teilnehmerliste.Where(Function(T)
                                          ' Hat der Teilnehmer eine Leistungsstufe zugewiesen bekommen?
                                          Return Not T.LeistungsstufeID = Guid.Empty
                                      End Function).ToList.ForEach(Sub(T)
                                                                       T.Leistungsstufe = Skiclub.Leistungsstufenliste.FirstOrDefault(Function(L) L.Ident = T.LeistungsstufeID)
                                                                   End Sub)
        Return Skiclub.Teilnehmerliste
    End Function

    ''' <summary>
    ''' Gruppenstammskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Gruppenstammdaten mit Leistungsstufen, wenn Leistungsstufe gespeichert wurde</returns>
    Private Function GetGruppenstammdaten(Skiclub As Club) As GruppenstammCollection
        ' Es werden die Leistungsstufenobjekte zugeordnet
        Skiclub.Gruppenstammliste.Where(Function(G)
                                            Return G.LeistungsstufeID <> Guid.Empty
                                        End Function).ToList.ForEach(Sub(G)
                                                                         G.Leistungsstufe = Skiclub.Leistungsstufenliste.FirstOrDefault(Function(L) L.Ident = G.LeistungsstufeID)
                                                                     End Sub)
        Return Skiclub.Gruppenstammliste
    End Function

    ''' <summary>
    ''' Gruppenskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Gruppendaten mit den Trainer-, Leistungsstufe-, Gruppenstammdatenobjekten und die Mitgliederliste mit Teilnehmerobjekten </returns>
    Private Function GetAlleGruppen(Skiclub As Club) As GruppeCollection
        Skiclub.Gruppenliste.ToList.ForEach(Sub(G)
                                                G.Leistungsstufe = Skiclub.Leistungsstufenliste.DefaultIfEmpty(New Leistungsstufe() With {.Ident = Guid.Empty, .Benennung = "unbekannt"}).FirstOrDefault(Function(Ls) Ls.Ident = G.LeistungsstufeID)
                                                G.Trainer = Skiclub.Trainerliste.FirstOrDefault(Function(T) T.TrainerID = G.TrainerID)
                                                G.Gruppenstamm = Skiclub.Gruppenstammliste.FirstOrDefault(Function(GS) GS.Ident = G.GruppenstammID)
                                                G.MitgliederIDListe.ToList.ForEach(Sub(TID) G.Mitgliederliste.Add(Skiclub.Teilnehmerliste.FirstOrDefault(Function(T) T.Ident = TID)))
                                            End Sub)
        Return Skiclub.Gruppenliste
    End Function

    ''' <summary>
    ''' Einteilungenskelettdaten
    ''' </summary>
    ''' <param name="Skiclub"></param>
    ''' <returns>Die Einteilungen mit Gruppenobjekten, nicht zugewiesene Teilnehmerliste mit Teilnehmerobjekten und verfügbare Trainerliste mit Trainerobjekten</returns>
    Private Function GetAlleEinteilungen(Skiclub As Club) As EinteilungCollection
        Skiclub.Einteilungsliste.ToList.ForEach(Sub(E)
                                                    E.GruppenIDListe.ToList.ForEach(Sub(GID) E.Gruppenliste.Add(Skiclub.Gruppenliste.FirstOrDefault(Function(G) G.Ident = GID)))
                                                    E.Gruppenliste = E.Gruppenliste
                                                    E.NichtZugewieseneTeilnehmerIDListe.ToList.ForEach(Sub(TID) E.NichtZugewieseneTeilnehmerListe.Add(Skiclub.Teilnehmerliste.FirstOrDefault(Function(T) T.Ident = TID)))
                                                    E.VerfuegbareTrainerIDListe.ToList.ForEach(Sub(TID) E.VerfuegbareTrainerListe.Add(Skiclub.Trainerliste.FirstOrDefault(Function(T) T.TrainerID = TID)))
                                                End Sub)
        Return Skiclub.Einteilungsliste
    End Function

End Module
