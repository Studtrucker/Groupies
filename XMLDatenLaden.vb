Imports System.IO

Namespace Controller


    Public Class XMLDatenLaden

#Region "XML-Datei laden"

        ''' <summary>
        ''' Lädt Daten aus einer XML Datei
        ''' </summary>
        ''' <param name="Filename"></param>
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
        ''' <param name="Filename"></param>
        ''' <returns></returns>
        Public Shared Function LoadFromJson(Filename As String) As String
            If Filename.Contains("/") OrElse Filename.Contains(" ") OrElse Filename.Contains("\") Then
                Return "Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein."
            ElseIf Not File.Exists(String.Format("{0}.json", Filename)) Then
                Return String.Format("Die Datei {1} im Ordner {0} existiert nicht.", Environment.CurrentDirectory, String.Format("{0}.json", Filename))
            End If
            Return String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.json", Filename))
        End Function

#End Region
    End Class

End Namespace
