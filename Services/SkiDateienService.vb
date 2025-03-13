Imports Groupies.Entities
Imports Microsoft.Win32
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Module SkiDateienService

    Public Sub IdentifiziereDateiGeneration(filePath As String)
        Dim ElementListe As List(Of String) = LeseXmlDatei(filePath)
        Auswertung(ElementListe)
    End Sub

    Private Function Auswertung(ElementListe As List(Of String))
        If ElementListe.Contains("Skiclub") Then
            Return New Generation1.Skiclub
        ElseIf ElementListe.Contains("Club") AndAlso ElementListe.Contains("Einteilungsliste") Then
            Return New Generation3.Club
        Else
            Return New Generation3.Club
        End If
    End Function

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
                            '    ' Überprüfe, ob das Element Attribute hat
                            '    If xmlReader.HasAttributes Then
                            '        ' Gehe durch die Attribute
                            '        While xmlReader.MoveToNextAttribute()
                            '            Console.WriteLine("Attribute: " & xmlReader.Name & " = " & xmlReader.Value)
                            '        End While
                            '    End If
                            '    ' Wenn du auch den Textinhalt eines Elements lesen möchtest
                            'ElseIf xmlReader.NodeType = XmlNodeType.Text Then
                            '    Console.WriteLine("Text Inhalt: " & xmlReader.Value)
                        End If
                    End While
                End Using
            End Using
        End Using
        Return ElementListe.Distinct().ToList()

    End Function

    Public Sub SkiDateiEinlesen(filePath As String)
        Dim ElementListe As List(Of String) = LeseXmlDatei(filePath)
        If ElementListe.Contains("Skiclub") Then
            Dim club As Club = Deserialize(Of Club)(filePath)
            Console.WriteLine(club.ClubName)
        ElseIf ElementListe.Contains("Club") AndAlso ElementListe.Contains("Einteilungsliste") Then
            Dim club As Club = Deserialize(Of Club)(filePath)
            Console.WriteLine(club.ClubName)
        Else
            Console.WriteLine("Generation2")
        End If
    End Sub

End Module
