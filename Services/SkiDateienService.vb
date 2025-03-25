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
        ''' Zeigt einen OpenFileDialog
        ''' und lädt damit eine Ski-Datei
        ''' </summary>
        ''' <param name="Club"></param>
        ''' <returns></returns>
        Public Shared Function OpenSkiDatei(ByRef Club As Generation3.Club) As Boolean
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
            If dlg.ShowDialog = True Then
                Club = OpenSkiDatei(dlg.FileName)
                If Club IsNot Nothing Then
                    Return True
                End If
            End If
            Club = Nothing
            Return False
        End Function

        ''' <summary>
        ''' Prüft, ob die angegebene Datei die bereits geladene wurde 
        ''' oder ob die Datei existiert, 
        ''' und gibt die geladene Ski-Datei als 
        ''' Club-Objekt zurück
        ''' </summary>
        ''' <param name="fileName"></param>
        ''' <returns></returns>
        Public Shared Function OpenSkiDatei(fileName As String) As Generation3.Club

            If AppController.AktuelleDatei IsNot Nothing AndAlso fileName.Equals(AppController.AktuelleDatei.FullName) Then
                MessageBox.Show("Groupies " & AppController.AktuelleDatei.Name & " ist bereits geöffnet")
                Return Nothing
                Exit Function
            End If

            If Not File.Exists(fileName) Then
                MessageBox.Show("Die Datei existiert nicht")
                Return Nothing
                Exit Function
            End If

            Return SkiDateiLesen(fileName)

        End Function

        ''' <summary>
        ''' Der Benutzer muss eine Datei auswählen, die eingelesen wird.
        ''' Sie wird deserialisiert und als Club zurückgegeben
        ''' </summary>
        Public Shared Function SkiDateiLesen() As Generation3.Club
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
        Public Shared Function SkiDateiLesen(Datei As String) As Generation3.Club
            If File.Exists(Datei) Then
                Dim x = SkiDateienService.IdentifiziereDateiGeneration(Datei).LadeGroupies(Datei)
                Return x

                'Try
                '    Dim DateiGelesen
                '    Dim aktuellerClub As New AktuelleVersion.Club
                '    Using fs = New FileStream(Datei, FileMode.Open)
                '        ' Versuche Ski (XML) mit Struktur Groupies2 (englische Objektnamen) zu lesen
                '        DateiGelesen = LeseSkiDateiVersion2(fs, aktuellerClub)
                '        If DateiGelesen Then
                '            'aktuellerClub.Einteilungsliste = EinteilungenLesen(aktuellerClub)
                '            Return aktuellerClub
                '        End If
                '    End Using
                '    ' Versuche Ski (XML) mit Struktur Groupies 1  (deutsche Objektnamen) zu lesen
                '    Using fs = New FileStream(Datei, FileMode.Open)
                '        If Not DateiGelesen Then
                '            LeseSkiDateiVersion1(fs, aktuellerClub)
                '            'aktuellerClub.Einteilungsliste = EinteilungenLesen(aktuellerClub)
                '            Return aktuellerClub
                '        End If
                '    End Using
                'Catch ex As InvalidDataException
                '    Debug.Print("Datei ungültig: " & ex.Message)
                '    Return Nothing
                '    Exit Function
                'End Try
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
        Private Shared Function Auswertung(ElementListe As List(Of String)) As IClub
            If ElementListe.Contains("Skiclub") Then
                Return New Generation1.Skiclub
            ElseIf ElementListe.Contains("Club") AndAlso ElementListe.Contains("Einteilungsliste") Then
                Return New Generation3.Club
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
        Private Shared Function LeseXmlDatei(filePath As String) As List(Of String)
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

    End Class

#End Region

End Namespace
