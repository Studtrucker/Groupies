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
        Public Shared Property GroupiesFile As FileInfo

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
        Public Shared Function KopiereListeMitNeuenObjekten(Of T)(originalList As List(Of T), copyConstructor As Func(Of T, T)) As List(Of T)
            Dim copiedList As New List(Of T)
            For Each item In originalList
                copiedList.Add(copyConstructor(item))
            Next
            Return copiedList
        End Function

#End Region

        Public Shared Sub NeuenClubErstellen()
            Dim dlg = InputBox("Bitte geben Sie den Namen des neuen Clubs ein", "Neuen Club erstellen", "Groupies Club")
            If dlg = "" Then
                MessageBox.Show("Der Clubname darf nicht leer sein.")
                Exit Sub
            End If
            NeuenClubErstellen(dlg)
        End Sub

        'Todo: Bei neuen Clubs  den Standard- Speicherort und Dateiname festlegen

        Public Shared Function NeuenClubErstellen(Clubname As String) As String
            'AktuellerClub = Nothing
            AktuellerClub = New Club(Clubname)

            AktuellerClub.AlleLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen
            AktuellerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag 1"})
            AktuellerClub.Einteilungsliste.Item(0).Gruppenliste = TemplateService.StandardGruppenErstellen(15)

            AppController.GroupiesFile = New FileInfo(Environment.CurrentDirectory & "\" & Clubname & ".ski")

            Dim Meldung = $"{Clubname} wurde erfolgreich erstellt."

            MessageBox.Show(Meldung)
            Return Meldung

        End Function


        Public Shared Sub TeilnehmerSuchen()
            'Dim dlg As New TeilnehmerSuchErgebnis(CurrentClub.Gruppenliste)
            Dim dlg As New TeilnehmerSuchErgebnis
            dlg.ShowDialog()
        End Sub

    End Class
End Namespace
