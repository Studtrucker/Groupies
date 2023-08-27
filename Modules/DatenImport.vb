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

    Public Function ImportTeilnehmerListe() As TeilnehmerCollection

        _Dokument = Nothing

        _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
        _ofdDokument.FilterIndex = 1
        _ofdDokument.RestoreDirectory = True

        If _ofdDokument.ShowDialog = DialogResult.OK Then
            Dim xlApp = New Excel.Application
            _Dokument = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
            If CheckExcelFileFormat(_Dokument) Then
                'Todo: Upload auf den SQL hier nicht mehr notwendig
                Return ReadImportExcelliste(_Dokument.ActiveSheet)
            End If
            _Dokument.Close()
        End If
        Return Nothing

    End Function

    Private Function ReadImportExcelliste(Excelsheet As Excel.Worksheet) As TeilnehmerCollection
        Dim CurrentRow = 2
        Dim Teilnehmerliste As New TeilnehmerCollection
        Dim RowCount = Excelsheet.UsedRange.Rows.Count
        Do Until CurrentRow > RowCount
            Dim Koennenstufe = FindKoennenstufe(Excelsheet.UsedRange(CurrentRow, 3).Value)
            Dim Skigruppe = FindSkigruppe(Excelsheet.UsedRange(CurrentRow, 4).Value)
            Dim Teilnehmer As New Teilnehmer With {
            .Vorname = Excelsheet.UsedRange(CurrentRow, 1).Value,
            .Name = Excelsheet.UsedRange(CurrentRow, 2).Value,
            .Koennenstufe = Koennenstufe,
            .Skigruppe = Skigruppe}
            Teilnehmerliste.Add(Teilnehmer)
            CurrentRow += 1
        Loop
        Return Teilnehmerliste
    End Function

    Private Function FindKoennenstufe(Benennung As String) As Koennenstufe
        Dim Koennenstufenliste As New KoennenstufenCollection
        Dim Koennenstufe = Koennenstufenliste.FirstOrDefault(Function(k) k.Benennung = Benennung)
        If Koennenstufe Is Nothing Then
            Koennenstufe = New Koennenstufe With {.Benennung = Benennung}
        End If

        Return Koennenstufe
    End Function

    Private Function FindSkigruppe(Gruppenname As String) As Skigruppe
        Dim Skigruppenliste As New SkigruppenCollection
        Dim Skigruppe = Skigruppenliste.FirstOrDefault(Function(s) s.Gruppenname = Gruppenname)
        If Skigruppe Is Nothing Then
            Skigruppe = New Skigruppe With {.Gruppenname = Gruppenname}
        End If
        Return Skigruppe
    End Function

    Private Function CheckExcelFileFormat(Excelfile As Excel.Workbook) As Boolean

        Dim XlValid As Boolean

        ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
        _xlSheet = Excelfile.ActiveSheet

        If _xlSheet IsNot Nothing Then
            XlValid = _xlSheet.UsedRange.Columns.Count = 4

            ' Check column caption
            XlValid = XlValid And _xlSheet.Range("A1").Value = "Vorname"
            XlValid = XlValid And _xlSheet.Range("B1").Value = "Nachname"
            XlValid = XlValid And _xlSheet.Range("C1").Value = "Level"
            XlValid = XlValid And _xlSheet.Range("D1").Value = "Skigruppe"

            ' Check first data row
            XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("A2").Value)
            'XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("B2").Value)
            'XlValid = XlValid And Not String.IsNullOrEmpty(_xlSheet.Range("C2").Value)

        End If

        If Not XlValid Then MessageBox.Show("Die Datei ist nicht zum Teilnehmerimport geeignet")

        Return XlValid

    End Function

End Module
