
Imports System.Data
Imports Groupies.DataImport
Imports Groupies.ExcelDataReaderService
Imports Groupies.XlLeser


Public Module ExcelDataReaderService
    Private Teilnehmerliste As New List(Of Teilnehmer)
    Private Trainerliste As New List(Of Trainer)
    Private xl As DataSet


    Public Function LeseTeilnehmerAusExcel(Pfad As String) As List(Of Teilnehmer)

        Teilnehmerliste.Clear()

        If xl Is Nothing Then
            xl = LoadDataSet(Pfad)
        End If

        '' Prüfung, Excelsheet "Teilnehmer" vorhanden
        'If xl.Tables.IndexOf("Teilnehmer2") < 0 Then
        '    MessageBox.Show("Die Datei enthält kein Tabellenblatt 'Teilnehmer' und kann nicht ausgewertet werden")
        '    Return Teilnehmerliste
        'End If

        '' Prüfung, Excelsheet "Teilnehmer", erforderlichen Spalten vorhanden
        'If xl.Tables("Teilnehmer").Columns.Count < 3 Then
        '    MessageBox.Show("In dem Tabellenblatt 'Teilnehmer' fehlt eine der Spalten 'Vorname', 'Nachname' oder 'TeilnehmerID' und kann nicht ausgewertet werden")
        '    Return Teilnehmerliste
        'End If

        For Each zeile As DataRow In xl.Tables("Teilnehmer").Rows
            Dim Tn = New Teilnehmer With {
                .Vorname = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Vorname")),
                .Nachname = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Nachname")),
                .TeilnehmerIDText = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("TeilnehmerID"))}
            Teilnehmerliste.Add(Tn)
        Next

        Return Teilnehmerliste

    End Function

    Public Function LeseTrainerAusExcel(Pfad As String) As List(Of Trainer)

        Trainerliste.Clear()

        If xl Is Nothing Then
            xl = LoadDataSet(Pfad)
        End If

        For Each zeile As DataRow In xl.Tables("Trainer").Rows
            Dim Tr = New Trainer With {.Vorname = zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Vorname")), .Nachname = zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Name"))}
            Trainerliste.Add(Tr)
        Next

        Return Trainerliste

    End Function


End Module
