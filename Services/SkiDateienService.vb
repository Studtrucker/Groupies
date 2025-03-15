Imports Groupies.Entities
Imports Microsoft.Win32
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Namespace Services

    Public Module SkiDateienService

#Region "Öffentliche FunKtionen"

        ''' <summary>
        ''' Identifiziert die Generation der Datei
        ''' und gibt die passende Generation des Clubs zurück
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <returns></returns>
        Public Function IdentifiziereDateiGeneration(filePath As String) As IClub
            Dim ElementListe As List(Of String) = LeseXmlDatei(filePath)
            Return Auswertung(ElementListe)
        End Function

#End Region

#Region "Private Funktionen"

        ''' <summary>
        ''' Werte die Elementliste aus und 
        ''' gib die passende Generation
        ''' des Clubs zurück
        ''' </summary>
        ''' <param name="ElementListe"></param>
        ''' <returns></returns>
        Private Function Auswertung(ElementListe As List(Of String)) As IClub
            If ElementListe.Contains("Skiclub") Then
                Return New Generation1.Skiclub
            ElseIf ElementListe.Contains("Club") AndAlso ElementListe.Contains("Einteilungsliste") Then
                Return New AktuelleVersion.Club
            Else
                Return New Generation2.Club
            End If
        End Function

        ''' <summary>
        ''' Liest eine XML Datei und 
        ''' sammelt die XML-Elemente der 
        ''' in filepath angegebenen Datei
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <returns></returns>
        Private Function LeseXmlDatei(filePath As String) As List(Of String)
            Dim ElementListe As New List(Of String)
            ' Erstelle einen FileStream 
            Using FileStream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
                ' und einen StreamReader, um die Datei zu lesen
                Using reader As StreamReader = New StreamReader(FileStream)
                    ' Erstelle den XmlReader aus dem StreamReader
                    Using xmlReader As XmlReader = XmlReader.Create(reader)
                        '  Solange es noch Elemente im XML gibt
                        While xmlReader.Read()
                            ' Überprüfe den NodeType und verarbeite je nach Bedarf
                            If xmlReader.NodeType = XmlNodeType.Element Then
                                ElementListe.Add(xmlReader.Name)
                            End If
                        End While
                    End Using
                End Using
            End Using
            Return ElementListe.Distinct().ToList()

        End Function

    End Module

#End Region

End Namespace
