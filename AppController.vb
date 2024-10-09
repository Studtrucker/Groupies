Imports System.IO
Imports Groupies.Entities
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Services
Imports System.Xml.Serialization
Imports System.Security.Cryptography

Namespace Controller

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
        Public Shared Property StandardLeistungsstufen = PresetService.StandardLeistungsstufenErstellen

        ''' <summary>
        ''' Aktuelle Gruppen
        ''' Es können bis zu 15 verschiedene Gruppen
        ''' angelegt werden, die mit verschiedenen Namen
        ''' versehen werden
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property StandardGruppen = PresetService.StandardGruppenErstellen(10)

#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Eine gespeicherte Datei wird eingelesen 
        ''' </summary>
        ''' <param name="Datei"></param>
        Public Shared Sub XMLDateiEinlesen(Datei As String)
            If File.Exists(Datei) Then
                Try
                    Dim DateiGelesen
                    Using fs = New FileStream(Datei, FileMode.Open)
                        ' Versuche XML mit Struktur Groupies 2 zu lesen
                        DateiGelesen = LeseXMLDateiVersion2(fs)
                    End Using
                    ' Versuche XML mit Struktur Groupies 1 zu lesen
                    Using fs = New FileStream(Datei, FileMode.Open)
                        If Not DateiGelesen Then
                            LeseXMLDateiVersion1(fs)
                        End If
                    End Using
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Exit Sub
                End Try
            End If
        End Sub

        Private Shared Function LeseXMLDateiVersion1(Filestream As FileStream) As Boolean
            Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
            Dim loadedSkiclub As Veraltert.Skiclub
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
                CurrentClub = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)
                Return True
            Catch ex As InvalidDataException
                Throw ex
                Return False
            End Try
        End Function

        Private Shared Function LeseXMLDateiVersion2(Filestream As FileStream) As Boolean
            Dim serializer = New XmlSerializer(GetType(Club))
            Try
                CurrentClub = TryCast(serializer.Deserialize(Filestream), Club)
                Return True
            Catch ex As InvalidOperationException
                Return False
            Catch ex As InvalidDataException
                Throw ex
                Return False
            End Try
        End Function
#End Region



        Public Shared Function NeuenClubErstellen(Clubname As String) As String
            CurrentClub = New Club(Clubname)
            Return $"{Clubname} wurde erfolgreich erstellt."
        End Function

        Public Shared Function NeuenClubErstellen(Clubname As String, NumberOfGroups As Integer) As String
            CurrentClub = New Club(Clubname) With {.Gruppenliste = PresetService.StandardGruppenErstellen(NumberOfGroups)}
            Return $"{Clubname} wurde mit {CurrentClub.Gruppenliste.Count} Gruppen erfolgreich erstellt."
        End Function

        Public Function Status() As String
            Return $"Der aktuelle Club heißt {CurrentClub}." & Environment.NewLine &
                $"Er hat {CurrentClub.GruppenloseTeilnehmer.Count} Mitglieder" & Environment.NewLine &
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
End Namespace
