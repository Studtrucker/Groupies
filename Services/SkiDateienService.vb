Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Interfaces
Imports Microsoft.Win32
Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Namespace Services

    Public Class SkiDateienService

#Region "Öffentliche FunKtionen"

        ''' <summary>
        ''' Zeigt einen OpenFileDialog.
        ''' Prüft, ob der Dialog auf eine existierende Datei führt und
        ''' das diese Datei noch nicht geladen ist.
        ''' Wenn die Prüfung erfolgreich war,
        ''' dann wird das geladene Objekt im 
        ''' AppController.AktuellerClub gespeichert 
        ''' </summary>
        ''' <returns>True or False</returns>
        Public Shared Function OpenSkiDatei() As Boolean
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
            If dlg.ShowDialog = True Then
                Return OpenSkiDatei(dlg.FileName)
            End If
            Return False
        End Function

        ''' <summary>
        ''' Prüft den angegebenen String,
        ''' ob er auf eine existierende Datei führt und
        ''' das diese Datei noch nicht geladen ist.
        ''' Wenn die Prüfung erfolgreich war,
        ''' dann wird das geladene Objekt im 
        ''' AppController.AktuellerClub gespeichert 
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns>True or False</returns>
        Public Shared Function OpenSkiDatei(fileName As String) As Boolean

            If Not File.Exists(fileName) Then
                MessageBox.Show("Die Datei existiert nicht")
                Return False
                Exit Function
            End If

            If AppController.GroupiesFile IsNot Nothing AndAlso fileName.Equals(AppController.GroupiesFile.FullName) Then
                MessageBox.Show("Groupies " & AppController.GroupiesFile.Name & " ist bereits geöffnet")
                Return False
                Exit Function
            End If

            AppController.GroupiesFile = New FileInfo(fileName)
            AppController.AktuellerClub = SkiDateiLesen(fileName)

            Return True

        End Function

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und als Club zurückgegeben
        ''' </summary>
        Public Shared Function SkiDateiLesen() As Generation4.Club
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

            If dlg.ShowDialog = True Then
                Dim Club = SkiDateiLesen(dlg.FileName)
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
        Public Shared Function SkiDateiLesen(Datei As String) As Generation4.Club
            If File.Exists(Datei) Then
                Dim x = SkiDateienService.IdentifiziereDateiGeneration(Datei).LadeGroupies(Datei)
                Return x

            End If
            Return Nothing
        End Function


        ''' <summary>
        ''' Identifiziert die Generation der Datei
        ''' und gibt die passende Generation des Clubs zurück
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <returns></returns>
        Public Shared Function IdentifiziereDateiGeneration(filePath As String) As IClub
            Dim ElementListe As List(Of String) = LeseXmlDatei(filePath)
            Dim revision = LeseXmlRevision(filePath)
            Dim Generation = Erkennen(revision)

            If Generation Is Nothing Then
                Return Erkennen(ElementListe)
            Else
                Return Generation
            End If

        End Function

#End Region

#Region "Private Funktionen"

        ''' <summary>
        ''' Werte den RevisionNode aus und 
        ''' gib die passende Generation
        ''' des Clubs zurück
        ''' </summary>
        ''' <param name="RevisionNode"></param>
        ''' <returns></returns>
        Private Shared Function Erkennen(RevisionNode As XmlNode) As IClub
            If RevisionNode Is Nothing Then
                Return Nothing
            End If
            Dim Revision = RevisionNode.InnerText
            Select Case Revision
                Case 1
                    Return New Generation1.Skiclub
                Case 2
                    Return New Generation2.Club
                Case 3
                    Return New Generation3.Club
                Case 4
                    Return New Generation4.Club
                Case Else
                    Throw New InvalidDataException("Die Datei ist nicht lesbar oder nicht kompatibel.")
            End Select
        End Function

        Private Shared Function Erkennen(ElementListe As List(Of String)) As IClub
            If ElementListe.Contains("Skiclub") Then
                Return New Generation1.Skiclub
            ElseIf ElementListe.Contains("Club") AndAlso ElementListe.Contains("AlleEinteilungen") Then
                Return New Generation4.Club
            ElseIf ElementListe.Contains("Club") AndAlso ElementListe.Contains("Einteilungsliste") Then
                Return New Generation3.Club
            ElseIf ElementListe.Contains("Club") Then
                Return New Generation2.Club
            Else
                Throw New InvalidDataException("Die Datei ist nicht lesbar oder nicht kompatibel.")
            End If
        End Function

        ''' <summary>
        ''' Liest eine XML Datei und 
        ''' sammelt die XML-Elemente der 
        ''' in filepath angegebenen Datei
        ''' </summary>
        ''' <param name="filePath"></param>
        ''' <returns></returns>
        Private Shared Function LeseXmlDatei(filePath As String) As List(Of String)
            Dim ElementListe As New List(Of String)
            ' Erstelle einen FileStream 
            Using FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
                ' und einen StreamReader, um die Datei zu lesen
                Using reader = New StreamReader(FileStream)
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

        Private Shared Function LeseXmlRevision(filePath As String) As XmlNode

            Dim doc As New XmlDocument()
            doc.Load(filePath)
            Dim revisionNode As XmlNode = doc.SelectSingleNode("//DateiGeneration")

            If (revisionNode IsNot Nothing) Then
                Return revisionNode
            Else
                Return Nothing
            End If

        End Function
    End Class

#End Region

End Namespace
