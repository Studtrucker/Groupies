Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Entities
Imports Microsoft.Win32

Namespace Controller

    Public Class SkiDatenLaden

#Region "Felder"
        'Private Shared AktuellerClub As Club
#End Region

#Region "Komplette Dateien lesen"

        ''' <summary>
        ''' Eine gespeicherte Datei wird eingelesen 
        ''' </summary>
        Public Shared Function SkiDateiLesen() As Club
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

            If dlg.ShowDialog = True Then
                Dim Datei = dlg.FileName
                Return SkiDateiLesen(Datei)
            End If
            Return Nothing
        End Function

        Public Shared Function SkiDateiLesen(Datei As String) As Club
            If File.Exists(Datei) Then
                Try
                    Dim DateiGelesen
                    Dim aktuellerClub As New Club
                    Using fs = New FileStream(Datei, FileMode.Open)
                        ' Versuche Ski (XML) mit Struktur Groupies 2 zu lesen
                        DateiGelesen = LeseSkiDateiVersion2(fs, aktuellerClub)
                        If DateiGelesen Then
                            aktuellerClub = PruefeEinteilungen(aktuellerClub)
                            Return aktuellerClub
                        End If
                    End Using
                    ' Versuche Ski (XML) mit Struktur Groupies 1 zu lesen
                    Using fs = New FileStream(Datei, FileMode.Open)
                        If Not DateiGelesen Then
                            aktuellerClub = PruefeEinteilungen(aktuellerClub)
                            LeseSkiDateiVersion1(fs, aktuellerClub)
                            Return aktuellerClub
                        End If
                    End Using
                Catch ex As InvalidDataException
                    Debug.Print("Datei ungültig: " & ex.Message)
                    Return Nothing
                    Exit Function
                End Try
            End If
            Return Nothing
        End Function

        'Public Shared Function LeseSkiDateiVersion1(Filestream As FileStream) As Boolean
        '    Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
        '    Dim loadedSkiclub As Veraltert.Skiclub
        '    Try
        '        loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
        '        AktuellerClub = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)
        '        Return True
        '    Catch ex As InvalidDataException
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        Public Shared Function LeseSkiDateiVersion1(Filestream As FileStream, ByRef Club As Club) As Boolean
            Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
            Dim loadedSkiclub As Veraltert.Skiclub
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(Filestream), Veraltert.Skiclub)
                Club = VeralterteKlassenMapping.MapSkiClub2Club(loadedSkiclub)
                Return True
            Catch ex As InvalidDataException
                Throw ex
                Return False
            End Try
        End Function

        'Public Shared Function LeseSkiDateiVersion2(Filestream As FileStream) As Boolean
        '    Dim serializer = New XmlSerializer(GetType(Club))
        '    Try
        '        Dim AktuellerClub = TryCast(serializer.Deserialize(Filestream), Club)
        '        Return True
        '    Catch ex As InvalidOperationException
        '        Return False
        '    Catch ex As InvalidDataException
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        Public Shared Function LeseSkiDateiVersion2(Filestream As FileStream, ByRef Club As Club) As Boolean
            Dim serializer = New XmlSerializer(GetType(Club))
            Try
                Club = TryCast(serializer.Deserialize(Filestream), Club)
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
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen() As TeilnehmerCollection
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.AlleTeilnehmer
        End Function

        Public Shared Function EinteilungenLesen() As EinteilungCollection
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.Einteilungsliste
        End Function

        Public Shared Function GruppenLesen() As GruppeCollection
            Dim aktuellerClub = SkiDateiLesen()
            Return AktuellerClub.Gruppenliste
        End Function
        Public Shared Function TrainerLesen(Datei As String) As TrainerCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return AktuellerClub.AlleTrainer
        End Function

        Public Shared Function TeilnehmerLesen(Datei As String) As TeilnehmerCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return AktuellerClub.AlleTeilnehmer
        End Function

        Public Shared Function EinteilungenLesen(Datei As String) As EinteilungCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
            Return AktuellerClub.Einteilungsliste
        End Function

        Public Shared Function GruppenLesen(Datei As String) As GruppeCollection
            Dim aktuellerClub = SkiDateiLesen(Datei)
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

        Private Shared Function PruefeEinteilungen(Club As Club) As Club
            If Club.Einteilungsliste Is Nothing OrElse Club.Einteilungsliste.Count = 0 Then
                Dim Tag = New Einteilung With {.Benennung = BestimmeEinteilungsbenennung(Club.Einteilungsliste)}
                Tag.Gruppenliste = Club.Gruppenliste
                Tag.GruppenloseTrainer = Club.GruppenloseTrainer
                Tag.GruppenloseTeilnehmer = Club.GruppenloseTeilnehmer
                Club.Einteilungsliste = New EinteilungCollection From {Tag}
            End If
            Return Club
        End Function

    End Class
#End Region

End Namespace
