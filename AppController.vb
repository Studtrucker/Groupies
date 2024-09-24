Imports System.IO
Imports Groupies.Entities

Public Class AppController
    ''' <summary>
    ''' Der aktuell verwaltete Club
    ''' </summary>
    ''' <returns></returns>
    Public Property CurrentGroupies As Skiclub


    ''' <summary>
    ''' Lädt Daten aus einer XML Datei
    ''' </summary>
    ''' <returns></returns>
    Public Function LoadFromXML(Filename As String) As String
        If Filename.Contains("/") OrElse Filename.Contains("\") OrElse Filename.Contains(" ") Then
            Return "Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein."
        ElseIf Not File.Exists(Format("{0}.xml", Filename)) Then
            Return "Die Datei existiert nicht"
        End If
        Return "Es wurde keine Datei geladen"
    End Function

    ''' <summary>
    ''' Lädt Daten aus einer JSON Datei
    ''' </summary>
    ''' <returns></returns>
    Public Function LoadFromJson(Filename As String) As String
        Return "Es wurde keine Datei geladen"
    End Function

End Class
