Imports System.IO
Imports Groupies.Entities
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Services
Imports System.Xml.Serialization

Public Class AppController

#Region "Eigenschaften"
    ''' <summary>
    ''' Der aktuell verwaltete Club
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property CurrentClub As Club

    ''' <summary>
    ''' Die aktuellen Leistungsstufen
    ''' Standard sind 5 Stufen und die Beschreibung 
    ''' der notwendigen Fähigkeiten, 
    ''' um der Stufe gerecht zu werden
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property AktuelleLeistungsstufen = PresetService.StandardLeistungsstufenErstellen

    ''' <summary>
    ''' Aktuelle Gruppen
    ''' Es können bis zu 15 verschiedene Gruppen
    ''' angelegt werden, die mit verschiedenen Namen
    ''' versehen werden
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property AktuelleGruppen = PresetService.StandardGruppenErstellen(10)

#End Region

#Region "Funktionen und Methoden"
    ''' <summary>
    ''' Eine gespeicherte Datei wird eingelesen 
    ''' </summary>
    ''' <param name="Datei"></param>
    Public Shared Function DateiEinlesen(Datei As String)
        If File.Exists(Datei) Then

            ' Datei deserialiseren
            ' Datei aus Groupies Version 1
            ' Todo: Datei aus Groupies Version 2
            Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
            Using fs = New FileStream(Datei, FileMode.Open)
                Try
                    LeseVersuchDateiVersion1(fs)
                    LeseVersuchDateiVersion2(fs)
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Return Nothing
                    Exit Function
                End Try
            End Using
        End If
    End Function

    Private Shared Sub LeseVersuchDateiVersion1(Filestream As FileStream)
        Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
        Dim loadedSkiclub As Veraltert.Skiclub
        Try
            loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
            AktuelleGruppen = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub).Gruppenliste
            AktuelleLeistungsstufen = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub).Leistungsstufeliste
        Catch ex As InvalidDataException
            Debug.Print("Datei ungültig: " & ex.Message)
            Exit Sub
        End Try
    End Sub

    Private Shared Sub LeseVersuchDateiVersion2(Filestream As FileStream)
        Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
        Dim loadedSkiclub As Veraltert.Skiclub
        Try
            loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
            AktuelleGruppen = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub).Gruppenliste
            AktuelleLeistungsstufen = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub).Leistungsstufeliste
        Catch ex As InvalidDataException
            Debug.Print("Datei ungültig: " & ex.Message)
            Exit Sub
        End Try
    End Sub
#End Region



    Public Shared Function CreateNewClub(Clubname As String, NumberOfGroups As Integer) As String

        CurrentClub = New Club(Clubname, NumberOfGroups)
        Return $"{Clubname} wurde mit {CurrentClub.Gruppenliste.Count} Gruppen erfolgreich erstellt."

    End Function

    Public Function Status() As String
        Return $"Der aktuelle Club heißt {CurrentClub}." & Environment.NewLine &
            $"Er hat {CurrentClub.Teilnehmerliste.Count} Mitglieder" & Environment.NewLine &
            "{String.Join(Environment.NewLine & " - ", CurrentClub.Participantlist)}" & $"{Environment.NewLine}"
    End Function

    ''' <summary>
    ''' Lädt Daten aus einer XML Datei
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function LoadFromXML(Filename As String) As String
        If Filename.Contains("/") OrElse Filename.Contains("\") OrElse Filename.Contains(" ") Then
            Return "Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein."
        ElseIf Not File.Exists(String.Format("{0}.xml", Filename)) Then
            Return String.Format("Die Datei {1} im Ordner {0} existiert nicht.", Environment.CurrentDirectory, String.Format("{0}.xml", Filename))
        End If
        Return String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.xml", Filename))
    End Function

    ''' <summary>
    ''' Lädt Daten aus einer JSON Datei
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function LoadFromJson(Filename As String) As String
        If Filename.Contains("/") OrElse Filename.Contains(" ") OrElse Filename.Contains("\") Then
            Return "Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein."
        ElseIf Not File.Exists(String.Format("{0}.json", Filename)) Then
            Return String.Format("Die Datei {1} im Ordner {0} existiert nicht.", Environment.CurrentDirectory, String.Format("{0}.json", Filename))
        End If
        Return String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.json", Filename))
    End Function

End Class
