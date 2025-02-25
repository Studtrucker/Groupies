Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Entities
Imports Microsoft.Win32

Namespace Controller

    Public Class SkiDatenLaden

#Region "Felder"
#End Region

#Region "Komplette Dateien lesen"

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und als Club zurückgegeben
        ''' </summary>
        Public Shared Function SkiDateiLesen() As Club
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

            If dlg.ShowDialog = True Then
                Dim Club = SkiDateiLesen(dlg.FileName)
                Club.Einteilungsliste = EinteilungenLesen(Club)
                Return Club
            End If
            Return Nothing
        End Function


        ''' <summary>
        ''' Eine Dateipfad wird übergeben, 
        ''' die angebene Datei wird eingelesen,
        ''' deserialisiert und als Club zurückgegeben
        ''' </summary>
        ''' <param name="Datei"></param>
        ''' <returns></returns>
        Public Shared Function SkiDateiLesen(Datei As String) As Club
            If File.Exists(Datei) Then
                Try
                    Dim DateiGelesen
                    Dim aktuellerClub As New Club
                    Using fs = New FileStream(Datei, FileMode.Open)
                        ' Versuche Ski (XML) mit Struktur Groupies 2 zu lesen
                        DateiGelesen = LeseSkiDateiVersion2(fs, aktuellerClub)
                        If DateiGelesen Then
                            Return aktuellerClub
                        End If
                    End Using
                    ' Versuche Ski (XML) mit Struktur Groupies 1 zu lesen
                    Using fs = New FileStream(Datei, FileMode.Open)
                        If Not DateiGelesen Then
                            LeseSkiDateiVersion1(fs, aktuellerClub)
                            Return aktuellerClub
                        End If
                    End Using
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Return Nothing
                    Exit Function
                End Try
            End If
            Return Nothing
        End Function

        ''' <summary>
        ''' Liest eine Ski-Datei mit der Struktur von Groupies 1
        ''' (englische Namen für die Klassen)
        ''' </summary>
        ''' <param name="Filestream"></param>
        ''' <param name="Club"></param>
        ''' <returns></returns>
        Public Shared Function LeseSkiDateiVersion1(Filestream As FileStream, ByRef Club As Club) As Boolean
            Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
            Dim loadedSkiclub As Veraltert.Skiclub
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
                Club = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)
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
        Public Shared Function LeseSkiDateiVersion2(Filestream As FileStream, ByRef Club As Club) As Boolean
            Dim serializer = New XmlSerializer(GetType(Club))
            Try
                Club = TryCast(serializer.Deserialize(Filestream), Club)
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
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen() As TeilnehmerCollection
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.AlleTeilnehmer
        End Function



        Public Shared Function GruppenLesen() As GruppeCollection
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.Gruppenliste
        End Function
        Public Shared Function TrainerLesen(Datei As String) As TrainerCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return AktuellerClub.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen(Datei As String) As TeilnehmerCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return AktuellerClub.AlleTeilnehmer
        End Function

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <param name="Datei"></param>
        ''' <param name="Name"></param>
        ''' <returns></returns>
        Public Shared Function TeilnehmerLesen(Datei As String, Name As String) As TeilnehmerCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Dim Liste = aktuellerClub.AlleTeilnehmer.Where(Function(tn) tn.Vorname = Name).ToList
            Liste.AddRange(aktuellerClub.GruppenloseTeilnehmer.Where(Function(tn) tn.Nachname = Name))
            Return New TeilnehmerCollection(Liste)
        End Function

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <returns>EinteilungCollection</returns>
        Public Shared Function EinteilungenLesen() As EinteilungCollection
            Dim aktuellerClub = SkiDateiLesen()
            Return EinteilungenLesen(aktuellerClub)
        End Function

        ''' <summary>
        ''' Eine Dateipfad wird übergeben, 
        ''' Sie wird deserialisiert und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <param name="Datei"></param>
        ''' <returns>EinteilungCollection</returns>
        Public Shared Function EinteilungenLesen(Datei As String) As EinteilungCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return EinteilungenLesen(aktuellerClub)
        End Function

        ''' <summary>
        ''' Ein Club wird übergeben
        ''' und die darin gespeicherten Einteilungen zurückgegeben
        ''' </summary>
        ''' <param name="Club"></param>
        ''' <returns>EinteilungCollection</returns>
        Public Shared Function EinteilungenLesen(Club As Club) As EinteilungCollection
            Dim Einteilungen = Club.Einteilungsliste
            ' Wenn keine Einteilungen vorhanden sind, wird eine Standard-Einteilung erstellt
            ' diese wird aus den Gruppen und Gruppenlosen des Clubs erstellt
            If Einteilungen.Count = 0 Then
                Einteilungen.Add(New Einteilung With {
                                 .Benennung = "Tag1",
                                 .Gruppenliste = Club.Gruppenliste,
                                 .GruppenloseTeilnehmer = Club.GruppenloseTeilnehmer,
                                 .GruppenloseTrainer = Club.GruppenloseTrainer})
            End If
            Return Einteilungen
        End Function

        Public Shared Function GruppenLesen(Datei As String) As GruppeCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return AktuellerClub.Gruppenliste
        End Function

#End Region


    End Class

End Namespace
