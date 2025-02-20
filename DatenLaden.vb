Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Entities
Imports Microsoft.Win32

Namespace Controller

    Public Class DatenLaden

#Region "Felder"
        Private Shared AktuellerClub As Club
#End Region

#Region "Komplette Dateien lesen"

        ''' <summary>
        ''' Eine gespeicherte Datei wird eingelesen 
        ''' </summary>
        Public Shared Function XMLDateiLesen() As Club
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

            If dlg.ShowDialog = True Then
                Dim Datei = dlg.FileName
                Return XMLDateiLesen(Datei)
            End If
            Return Nothing
        End Function

        Public Shared Function XMLDateiLesen(Datei As String) As Club
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
                    Return AktuellerClub
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Return Nothing
                    Exit Function
                End Try
            End If
            Return Nothing
        End Function

        Public Shared Function LeseXMLDateiVersion1(Filestream As FileStream) As Boolean
            Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
            Dim loadedSkiclub As Veraltert.Skiclub
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
                AktuellerClub = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)
                Return True
            Catch ex As InvalidDataException
                Throw ex
                Return False
            End Try
        End Function

        Public Shared Function LeseXMLDateiVersion2(Filestream As FileStream) As Boolean
            Dim serializer = New XmlSerializer(GetType(Club))
            Try
                AktuellerClub = TryCast(serializer.Deserialize(Filestream), Club)
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
            XMLDateiLesen()
            Return AktuellerClub.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen() As TeilnehmerCollection
            XMLDateiLesen()
            Return AktuellerClub.AlleTeilnehmer
        End Function

        Public Shared Function EinteilungenLesen() As EinteilungCollection
            XMLDateiLesen()
            Return AktuellerClub.Einteilungsliste
        End Function

        Public Shared Function GruppenLesen() As GruppeCollection
            XMLDateiLesen()
            Return AktuellerClub.Gruppenliste
        End Function

#End Region

#Region "Hilfsfunktionen"

        ''' <summary>
        ''' Benennt eine neue Einteilung
        ''' </summary>
        ''' <param name="Einteilungsliste"></param>
        ''' <returns></returns>
        Public Shared Function BestimmeEinteilungsbenennung(Einteilungsliste As EinteilungCollection) As String

            If Einteilungsliste Is Nothing Then Return "Tag1"

            Dim Tage = Einteilungsliste.ToList.Where(Function(e) e.Benennung.StartsWith("Tag")).OrderByDescending(Function(e) e.Benennung)
            If Tage.Count > 0 Then
                Dim z = Val(Tage(0).Benennung.Last)
                Return $"Tag{z + 1}"
            Else
                Return $"Tag{Einteilungsliste.Count + 1}"
            End If

        End Function

    End Class
#End Region

End Namespace
