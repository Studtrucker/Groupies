Imports System.Windows.Forms
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.ObjectModel
Imports Microsoft.Win32
Imports Skireisen.Entities

Public Module DatenImport
    Private ReadOnly _ofdDokument As New Forms.OpenFileDialog
    Private _Dokument As Excel.Workbook
    Private _xlSheet As Excel.Worksheet
    Private ReadOnly _xlCell As Excel.Range

    Sub ImportMitarbeiterListe()
        Dim xlApp As New Excel.Application

        _Dokument = Nothing

        _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
        _ofdDokument.FilterIndex = 1
        _ofdDokument.RestoreDirectory = True

        If _ofdDokument.ShowDialog = DialogResult.OK Then
            _Dokument = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
            If CheckExcelFileFormat(_Dokument) Then
                'todo: Upload auf den SQL hier nicht mehr notwendig
                'UploadMitarbeiterListe(ReadMitarbeiterExcelliste(_Dokument.ActiveSheet))
            End If
            _Dokument.Close()
        End If

    End Sub

    Private Function ReadMitarbeiterExcelliste(Excelsheet As Excel.Worksheet) As Collection(Of Teilnehmer)
        Dim CurrentRow = 2
        Dim Teilnehmerliste As New Collection(Of Teilnehmer)
        Dim RowCount = Excelsheet.UsedRange.Rows.Count
        Do Until CurrentRow > RowCount
            Dim Teilnehmer As New Teilnehmer With {
            .Vorname = Excelsheet.UsedRange(CurrentRow, 1).Value,
            .Name = Excelsheet.UsedRange(CurrentRow, 2).Value}
            Teilnehmerliste.Add(Teilnehmer)
            CurrentRow += 1
        Loop
        Return Teilnehmerliste
    End Function


    Private Function CheckExcelFileFormat(Excelfile As Excel.Workbook) As Boolean

        Dim XlValid As Boolean

        ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
        _xlSheet = Excelfile.ActiveSheet

        If _xlSheet IsNot Nothing Then
            XlValid = _xlSheet.UsedRange.Columns.Count = 2

            ' Check column caption
            XlValid = XlValid And _xlSheet.Range("A1").Value = "Vorname"
            XlValid = XlValid And _xlSheet.Range("B1").Value = "Nachname"

            ' Check first data row
            XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("A2").Value)
            XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("B2").Value)

        End If

        Return XlValid

    End Function

End Module
