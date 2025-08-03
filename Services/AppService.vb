Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Interfaces


Namespace Services

    ''' <summary>
    ''' Stellt grundlegende Anwendungsdienste bereit, wie z.B. das Erstellen eines neuen Clubs.
    ''' </summary>
    Public Class AppService

        ''' <summary>
        ''' Erstellt einen neuen Club, indem der Benutzer nach dem Namen gefragt wird.
        ''' Wenn der Name leer ist, wird eine Fehlermeldung angezeigt und es wird kein Club erstellt.
        ''' </summary>
        ''' <returns>Ein neues Club-Objekt.</returns>
        Public Shared Function NeuenClubErstellen() As Generation4.Club
            Dim dlg = InputBox("Bitte geben Sie den Namen des neuen Clubs ein", "Neuen Club erstellen", "Groupies")
            If String.IsNullOrEmpty(dlg) OrElse String.IsNullOrWhiteSpace(dlg) Then
                MessageBox.Show("Der Clubname darf nicht leer sein.")
                Return Nothing
            End If
            Return NeuenClubErstellen(dlg)
        End Function

        ''' <summary>
        ''' Erstellt einen neuen Club mit dem angegebenen Namen.
        ''' Der Clubname wird in der aktuellen Arbeitsverzeichnis gespeichert.
        ''' <param name="Clubname">Der Name des neuen Clubs.</param>
        ''' <returns>Ein neues Club-Objekt.</returns>
        ''' </summary>
        Public Shared Function NeuenClubErstellen(Clubname As String) As Generation4.Club

            Dim AktuellerClub = New Generation4.Club(Clubname) With {
                .ClubName = Clubname,
                .AlleGruppen = TemplateService.StandardGruppenErstellen(15),
                .AlleFaehigkeiten = TemplateService.StandardFaehigkeitenErstellen,
                .AlleLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen,
                .AlleEinteilungen = TemplateService.StandardEinteilungenErstellen}
            '.GroupiesFile = New FileInfo(Environment.CurrentDirectory & "\" & Clubname & ".ski"),

            AppController.GroupiesFile = New FileInfo(Environment.CurrentDirectory & "\" & Clubname & ".ski")

            MessageBox.Show($"{Clubname} wurde erfolgreich erstellt.")

            Return AktuellerClub

        End Function


        Public Shared Function ClubLaden(Filename As String) As Generation4.Club
            Using fs = New FileStream(Filename, FileMode.Open)
                Dim serializer = New XmlSerializer(GetType(Generation4.Club))
                Dim loadedSkiclub As Generation4.Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Generation4.Club)
                    Return Map2AktuelleGeneration(loadedSkiclub)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Shared Function Map2AktuelleGeneration(Skiclub As IClub) As Generation4.Club 'Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration4.MapSkiClub2Club(Skiclub)
        End Function
    End Class

End Namespace
