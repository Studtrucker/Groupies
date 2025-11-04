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
            If IsDBNull(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Ident"))) Then
                guid = Nothing
            Else
                Guid.TryParse(zeile.ItemArray(xl.Tables("Teilnehmer").Columns.IndexOf("Ident")), guid)
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

    Public Function LeseTeilnehmerAusDatasetKI(Pfad As String) As List(Of Teilnehmer)

        ' Lade Dataset und sichere Tabelle
        xl = LoadDataSet(Pfad, "Teilnehmer")
        If xl Is Nothing OrElse Not xl.Tables.Contains("Teilnehmer") Then Return Nothing

        Dim sheet = xl.Tables("Teilnehmer")

        ' Schnelle Rückgabe für leere Tabelle
        If sheet.Rows.Count = 0 Then
            Teilnehmerliste.Clear()
            Return Teilnehmerliste
        End If

        ' Spaltenindizes nur einmal ermitteln
        Dim ciIdent = sheet.Columns.IndexOf("Ident")
        Dim ciGeburt = sheet.Columns.IndexOf("Geburtsdatum")
        Dim ciVorname = sheet.Columns.IndexOf("Vorname")
        Dim ciNachname = sheet.Columns.IndexOf("Nachname")
        Dim ciTelefon = sheet.Columns.IndexOf("Telefonnummer")
        Dim ciLeistungsstufe = sheet.Columns.IndexOf("Leistungsstufe")

        ' Lokale Helfer
        Dim GetStringValue = Function(r As DataRow, idx As Integer) As String
                                 If idx < 0 Then Return String.Empty
                                 Dim o = r(idx)
                                 If IsDBNull(o) Then Return String.Empty
                                 Return Convert.ToString(o)
                             End Function

        Dim GetGuidValue = Function(r As DataRow, idx As Integer) As Guid
                               Dim result As Guid = Guid.Empty
                               If idx < 0 Then Return result
                               Dim o = r(idx)
                               If IsDBNull(o) Then Return result
                               Guid.TryParse(Convert.ToString(o), result)
                               Return result
                           End Function

        Dim GetDateValue = Function(r As DataRow, idx As Integer) As Date
                               If idx < 0 Then Return Date.MinValue
                               Dim o = r(idx)
                               If IsDBNull(o) Then Return Date.MinValue
                               Dim dt As Date = Date.MinValue
                               If Date.TryParse(Convert.ToString(o), dt) Then
                                   Return dt
                               End If
                               Return Date.MinValue
                           End Function

        ' Temporäre Liste befüllen
        Dim temp As New List(Of Teilnehmer)(sheet.Rows.Count)
        For Each zeile As DataRow In sheet.Rows
            Dim guid = GetGuidValue(zeile, ciIdent)
            Dim tn As New Teilnehmer With {
                .Geburtsdatum = GetDateValue(zeile, ciGeburt),
                .Vorname = GetStringValue(zeile, ciVorname),
                .Nachname = GetStringValue(zeile, ciNachname),
                .Telefonnummer = GetStringValue(zeile, ciTelefon),
                .Leistungsstand = GetStringValue(zeile, ciLeistungsstufe),
                .TeilnehmerID = guid
            }
            temp.Add(tn)
        Next

        ' Atomare Aktualisierung der Modul-Liste
        Teilnehmerliste.Clear()
        Teilnehmerliste.AddRange(temp)
        Return Teilnehmerliste

    End Function

    Public Function LeseTrainerAusDataset(Pfad As String) As List(Of Trainer)

        Trainerliste.Clear()

        xl = LoadDataSet(Pfad, "Trainer")

        If xl Is Nothing Then Return Nothing

        Dim sheet = xl.Tables("Trainer")
        For Each zeile As DataRow In sheet.Rows
            Dim guid As Guid
            If IsDBNull(zeile.ItemArray(sheet.Columns.IndexOf("Ident"))) Then
                guid = Guid.Empty
            Else
                Guid.TryParse(zeile.ItemArray(sheet.Columns.IndexOf("Ident")), guid)
            End If
            Dim Tr = New Trainer With {
                .Spitzname = If(IsDBNull(zeile.ItemArray(sheet.Columns.IndexOf("Alias"))), String.Empty, zeile.ItemArray(sheet.Columns.IndexOf("Alias"))),
                .Telefonnummer = If(IsDBNull(zeile.ItemArray(sheet.Columns.IndexOf("Telefonnummer"))), String.Empty, zeile.ItemArray(sheet.Columns.IndexOf("Telefonnummer"))),
                .eMail = If(IsDBNull(zeile.ItemArray(sheet.Columns.IndexOf("e-Mail"))), String.Empty, zeile.ItemArray(sheet.Columns.IndexOf("e-Mail"))),
                .Vorname = zeile.ItemArray(sheet.Columns.IndexOf("Vorname")),
                .Nachname = zeile.ItemArray(sheet.Columns.IndexOf("Nachname")),
                .TrainerID = guid}

            Trainerliste.Add(Tr)
        Next

        Return Trainerliste

    End Function

    Public Function LeseTrainerAusDatasetKI(Pfad As String) As List(Of Trainer)

        ' Lade Dataset
        xl = LoadDataSet(Pfad, "Trainer")
        If xl Is Nothing OrElse Not xl.Tables.Contains("Trainer") Then Return Nothing

        Dim sheet = xl.Tables("Trainer")

        ' Schnelle Rückgabe für leere Tabelle
        If sheet.Rows.Count = 0 Then
            Trainerliste.Clear()
            Return Trainerliste
        End If

        ' Spaltenindizes einmal ermitteln
        Dim ciIdent = sheet.Columns.IndexOf("Ident")
        Dim ciAlias = sheet.Columns.IndexOf("Alias")
        Dim ciTelefon = sheet.Columns.IndexOf("Telefonnummer")
        Dim ciEmail = sheet.Columns.IndexOf("e-Mail")
        Dim ciVorname = sheet.Columns.IndexOf("Vorname")
        Dim ciNachname = sheet.Columns.IndexOf("Nachname")

        ' Hilfsfunktionen
        Dim GetStringValue = Function(r As DataRow, idx As Integer) As String
                                 If idx < 0 Then Return String.Empty
                                 Dim o = r(idx)
                                 If IsDBNull(o) Then Return String.Empty
                                 Return Convert.ToString(o)
                             End Function

        Dim GetGuidValue = Function(r As DataRow, idx As Integer) As Guid
                               Dim result As Guid = Guid.Empty
                               If idx < 0 Then Return result
                               Dim o = r(idx)
                               If IsDBNull(o) Then Return result
                               Guid.TryParse(Convert.ToString(o), result)
                               Return result
                           End Function

        ' Lokale Liste erstellen und füllen (sichere, performante Variante)
        Dim temp As New List(Of Trainer)(sheet.Rows.Count)
        For Each zeile As DataRow In sheet.Rows
            Dim guid = GetGuidValue(zeile, ciIdent)

            Dim tr As New Trainer With {
                .Spitzname = GetStringValue(zeile, ciAlias),
                .Telefonnummer = GetStringValue(zeile, ciTelefon),
                .eMail = GetStringValue(zeile, ciEmail),
                .Vorname = GetStringValue(zeile, ciVorname),
                .Nachname = GetStringValue(zeile, ciNachname),
                .TrainerID = guid
            }

            temp.Add(tr)
        Next

        ' Module-level Liste aktualisieren und zurückgeben
        Trainerliste.Clear()
        Trainerliste.AddRange(temp)
        Return Trainerliste

    End Function

End Module
