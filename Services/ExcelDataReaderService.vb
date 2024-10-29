Imports System.Data
Imports Groupies.DataImport
Imports Groupies.ExcelDataReaderService
Imports Groupies.XlLeser


Public Module ExcelDataReaderService
    Private Teilnehmerliste As New List(Of Teilnehmer)
    Private Trainerliste As New List(Of Trainer)
    Private xl As DataSet


    Public Function LeseTeilnehmerAusDataset(Pfad As String) As List(Of Teilnehmer)

        Teilnehmerliste.Clear()
        xl = LoadDataSet(Pfad, "Teilnehmer")

        If xl Is Nothing Then Return Nothing

        For Each zeile As DataRow In xl.Tables("Teilnehmer").Rows
            Dim x = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("TeilnehmerID"))
            Dim Tn = New Teilnehmer With {
                .Vorname = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Vorname")),
                .Nachname = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Nachname")),
                .TeilnehmerIDText = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("TeilnehmerID"))}
            Teilnehmerliste.Add(Tn)
        Next

        Return Teilnehmerliste

    End Function

    Public Function LeseTrainerAusDataset(Pfad As String) As List(Of Trainer)

        Trainerliste.Clear()

        xl = LoadDataSet(Pfad, "Trainer")

        If xl Is Nothing Then Return Nothing

        For Each zeile As DataRow In xl.Tables("Trainer").Rows
            Dim x As Guid
            Guid.TryParse(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("TrainerID")), x)

            Dim Tr = New Trainer With {
                .Vorname = zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Vorname")),
                .Nachname = zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Nachname"))}

            Guid.TryParse(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("TrainerID")), Tr.TrainerID)

            Trainerliste.Add(Tr)
        Next

        Return Trainerliste

    End Function


End Module
