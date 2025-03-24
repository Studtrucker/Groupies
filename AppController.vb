Imports System.IO
Imports Groupies.Entities
Imports Groupies.Entities.Generation3
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Services
Imports System.Xml.Serialization
Imports System.Security.Cryptography

Namespace Controller

    Public Class AppController

#Region "Eigenschaften"

        ''' <summary>
        ''' Die aktuelle Datei, die geöffnet ist
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property AktuelleDatei As FileInfo


        ''' <summary>
        ''' Der aktuell verwaltete Club
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property AktuellerClub As Club

        ''' <summary>
        ''' Die aktuellen Leistungsstufen
        ''' Standard sind 5 Stufen und die Beschreibung 
        ''' der notwendigen Fähigkeiten, 
        ''' um der Stufe gerecht zu werden
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property StandardLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen

        ''' <summary>
        ''' Aktuelle Gruppen
        ''' Es können bis zu 15 verschiedene Gruppen
        ''' angelegt werden, die mit verschiedenen Namen
        ''' versehen werden
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property StandardGruppen = TemplateService.StandardGruppenErstellen(15)

#End Region

#Region "Funktionen und Methoden"

#End Region

        Public Shared Function NeuenClubErstellen(Clubname As String) As String
            AktuellerClub = Nothing
            AktuellerClub = New Club(Clubname)

            AktuellerClub.Leistungsstufenliste = TemplateService.StandardLeistungsstufenErstellen
            'AktuellerClub.Gruppenliste = TemplateService.StandardGruppenErstellen(15)
            AktuellerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag 1"})
            AktuellerClub.Einteilungsliste.Item(0).Gruppenliste = TemplateService.StandardGruppenErstellen(15)

            Return $"{Clubname} wurde erfolgreich erstellt."
        End Function


        Public Shared Sub TeilnehmerSuchen()
            'Dim dlg As New TeilnehmerSuchErgebnis(CurrentClub.Gruppenliste)
            Dim dlg As New TeilnehmerSuchErgebnis
            dlg.ShowDialog()
        End Sub

    End Class
End Namespace
