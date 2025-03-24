Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Entities.Generation3
Imports Groupies.Entities.Generation1
Imports Groupies.Entities.Generation2
Imports Groupies.Entities
Imports Microsoft.Win32

Namespace Controller

    Public Class SkiDatenLaden

#Region "Felder"
#End Region

#Region "Komplette Dateien lesen"

        ''' <summary>
        ''' Liest eine Ski-Datei mit der Struktur von Groupies 1
        ''' (englische Namen für die Klassen)
        ''' </summary>
        ''' <param name="Filestream"></param>
        ''' <param name="Club"></param>
        ''' <returns></returns>
        Public Shared Function LeseSkiDateiVersion1(Filestream As FileStream, ByRef Club As Generation3.Club) As Boolean
            Dim serializer = New XmlSerializer(GetType(Generation1.Skiclub))
            Dim loadedSkiclub As Generation1.Skiclub
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Generation1.Skiclub)
                Club = MappingGeneration1.MapSkiClub2Club(loadedSkiclub)
                Return True
            Catch ex As InvalidDataException
                Throw ex
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Liest eine Ski-Datei mit der Struktur von Groupies 2
        ''' (deutsche Namen für die Klassen)
        ''' </summary>
        ''' <param name="Filestream"></param>
        ''' <param name="Club"></param>
        ''' <returns></returns>
        Public Shared Function LeseSkiDateiVersion2(Filestream As FileStream, ByRef Club As Generation3.Club) As Boolean
            Dim serializer = New XmlSerializer(GetType(Generation2.Club))
            Dim loadedSkiclub As Generation2.Club
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Generation2.Club)
                Club = MappingGeneration2.MapSkiClub2Club(loadedSkiclub)
                Return True
            Catch ex As InvalidOperationException
                Return False
            Catch ex As InvalidDataException
                Throw ex
                Return False
            End Try
        End Function

#End Region

#Region "Teile aus Datei laden"

        Public Shared Function TrainerLesen() As TrainerCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return aktuellerClub.SelectedEinteilung.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen() As TeilnehmerCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return aktuellerClub.SelectedEinteilung.AlleTeilnehmer
        End Function



        Public Shared Function GruppenLesen() As GruppeCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return aktuellerClub.SelectedEinteilung.Gruppenliste
        End Function
        Public Shared Function TrainerLesen(Datei As String) As TrainerCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return aktuellerClub.SelectedEinteilung.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen(Datei As String) As TeilnehmerCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return aktuellerClub.SelectedEinteilung.AlleTeilnehmer
        End Function

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <param name="Datei"></param>
        ''' <param name="Name"></param>
        ''' <returns></returns>
        Public Shared Function TeilnehmerLesen(Datei As String, Name As String) As TeilnehmerCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Dim Liste = aktuellerClub.SelectedEinteilung.AlleTeilnehmer.Where(Function(tn) tn.Vorname = Name).ToList
            Liste.AddRange(aktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer.Where(Function(tn) tn.Nachname = Name))
            Return New TeilnehmerCollection(Liste)
        End Function

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <returns>EinteilungCollection</returns>
        Public Shared Function EinteilungenLesen() As EinteilungCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return EinteilungenLesen(aktuellerClub)
        End Function

        ''' <summary>
        ''' Eine Dateipfad wird übergeben, 
        ''' Sie wird deserialisiert und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <param name="Datei"></param>
        ''' <returns>EinteilungCollection</returns>
        Public Shared Function EinteilungenLesen(Datei As String) As EinteilungCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return EinteilungenLesen(aktuellerClub)
        End Function

        ''' <summary>
        ''' Ein Club wird übergeben
        ''' und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <param name="Club"></param>
        ''' <returns>EinteilungCollection</returns>
        Public Shared Function EinteilungenLesen(Club As Generation3.Club) As EinteilungCollection
            Dim Einteilungen = Club.Einteilungsliste
            ' Wenn keine Einteilungen vorhanden sind, wird eine Standard-Einteilung erstellt
            ' diese wird aus den Gruppen und Gruppenlosen des Clubs erstellt
            If Einteilungen.Count = 0 Then
                Einteilungen.Add(New Einteilung With {
                                 .Benennung = "Tag1",
                                 .Sortierung = 1,
                                 .Gruppenliste = Club.SelectedEinteilung.Gruppenliste,
                                 .GruppenloseTeilnehmer = Club.SelectedEinteilung.GruppenloseTeilnehmer,
                                 .GruppenloseTrainer = Club.SelectedEinteilung.GruppenloseTrainer})
            End If
            Return Einteilungen
        End Function

        Public Shared Function GruppenLesen(Datei As String) As GruppeCollection
            Dim aktuellerClub = Services.SkiDateienService.SkiDateiLesen()
            Return aktuellerClub.SelectedEinteilung.Gruppenliste
        End Function

#End Region


    End Class

End Namespace
