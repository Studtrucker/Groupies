Imports System.IO
Imports Groupies.Entities.Generation4
Imports Groupies.Services

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
        ''' Standard Leistungsstufen, um die Teilnehmer und Gruppen zu beschreiben
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property StandardLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen

        ''' <summary>
        ''' Standard Fähigkeiten, um die Leistungsstufen zu beschreiben
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property StandardFaehigkeiten = TemplateService.StandardFaehigkeitenErstellen

        ''' <summary>
        ''' Standard Gruppen, um die Teilnehmer leistungssmäßig zu gruppieren
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property StandardGruppen = TemplateService.StandardGruppenErstellen(15)

        ''' <summary>
        ''' Standard Einteilungen, um die Teilnehmer in Gruppen zu bringen
        ''' </summary>
        Public Shared Property StandardEinteilungen = TemplateService.StandardEinteilungenErstellen

        ''' <summary>
        ''' Command zum Sortieren der Gruppen im aktuellen Club
        ''' </summary>
        ''' <returns></returns>
        Public Shared Property SortiereGruppenCommand = New RelayCommand(Of Object)(AddressOf OnSortiereGruppen)

        Private Shared Sub OnSortiereGruppen(obj As Object)
            MessageBox.Show("Sortieren")
        End Sub

        ''' <summary>
        ''' Command zum Suchen von Teilnehmern
        ''' </summary>
        ''' <returns></returns>


#End Region

#Region "Funktionen und Methoden"
        Public Shared Function KopiereListeMitNeuenObjekten(Of T)(originalList As List(Of T), copyConstructor As Func(Of T, T)) As List(Of T)
            Dim copiedList As New List(Of T)
            For Each item In originalList
                copiedList.Add(copyConstructor(item))
            Next
            Return copiedList
        End Function


        Public Shared Sub NeuenClubErstellen()
            Dim dlg = InputBox("Bitte geben Sie den Namen des neuen Clubs ein", "Neuen Club erstellen", "Groupies Club")
            If String.IsNullOrEmpty(dlg) OrElse String.IsNullOrWhiteSpace(dlg) Then
                MessageBox.Show("Der Clubname darf nicht leer sein.")
                Exit Sub
            End If
            NeuenClubErstellen(dlg)
        End Sub

        Public Shared Function NeuenClubErstellen(Clubname As String) As String
            'AktuellerClub = Nothing
            AktuellerClub = New Club(Clubname) With {
                .ClubName = Clubname,
                .AlleGruppen = TemplateService.StandardGruppenErstellen(15),
                .AlleLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen,
                .AlleFaehigkeiten = TemplateService.StandardFaehigkeitenErstellen,
                .AlleEinteilungen = TemplateService.StandardEinteilungenErstellen}

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
#End Region

    End Class
End Namespace
