Imports System.Data
Imports Groupies.DataImport
Imports Groupies.ExcelDataReaderService
Imports Groupies.XlLeser


Public Module ExcelDataReaderService
    Private ReadOnly Teilnehmerliste As New List(Of Teilnehmer)
    Private ReadOnly Trainerliste As New List(Of Trainer)
    Private xl As DataSet


    Public Function LeseTeilnehmerAusDataset(Pfad As String) As List(Of Teilnehmer)

        Teilnehmerliste.Clear()
        xl = LoadDataSet(Pfad, "Teilnehmer")

        If xl Is Nothing Then Return Nothing

        For Each zeile As DataRow In xl.Tables("Teilnehmer").Rows
            Dim guid As Guid
            If IsDBNull(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("TeilnehmerID"))) Then
                guid = Nothing
            Else
                Guid.TryParse(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("TeilnehmerID")), guid)
            End If

            Dim Tn = New Teilnehmer With {
            .Geburtsdatum = If(IsDBNull(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Geburtsdatum"))), CDate("01.01.0001"), zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Geburtsdatum"))),
            .Vorname = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Vorname")),
            .Nachname = zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Nachname")),
            .Telefonnummer = If(IsDBNull(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Telefonnummer"))), String.Empty, zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Telefonnummer"))),
            .Leistungsstand = If(IsDBNull(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Leistungsstufe"))), String.Empty, zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Leistungsstufe"))),
            .TeilnehmerID = guid}

            Teilnehmerliste.Add(Tn)
        Next

        Return Teilnehmerliste

    End Function

    Public Function LeseTrainerAusDataset(Pfad As String) As List(Of Trainer)

        Trainerliste.Clear()

        xl = LoadDataSet(Pfad, "Trainer")

        If xl Is Nothing Then Return Nothing

        For Each zeile As DataRow In xl.Tables("Trainer").Rows
            Dim guid As Guid
            If IsDBNull(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("TrainerID"))) Then
                guid = Nothing
            Else
                Guid.TryParse(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("TrainerID")), guid)
            End If

            Dim Tr = New Trainer With {
                .Spitzname = If(IsDBNull(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Spitzname"))), String.Empty, zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Spitzname"))),
                .Telefonnummer = If(IsDBNull(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Telefonnummer"))), String.Empty, zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Telefonnummer"))),
                .eMail = If(IsDBNull(zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("e-Mail"))), String.Empty, zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("e-Mail"))),
                .Vorname = zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Vorname")),
                .Nachname = zeile.ItemArray(xl.Tables("Trainer").Columns.IndexOf("Nachname")),
                .TrainerID = guid}

            Trainerliste.Add(Tr)
        Next

        Return Trainerliste

    End Function


End Module
