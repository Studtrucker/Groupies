Imports System.IO
Imports Groupies.Entities
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Services

Public Class AppController
    ''' <summary>
    ''' Der aktuell verwaltete Club
    ''' </summary>
    ''' <returns></returns>
    Public Shared Property CurrentClub As Club
    Public Shared Property PerformanceLevels = PresetService.CreateLevels

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
