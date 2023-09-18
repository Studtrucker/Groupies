Imports System.Windows.Forms
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.Collections.ObjectModel
Imports Microsoft.Win32
Imports Skischule.Entities

Public Module DatenImport
    Private ReadOnly _ofdDokument As New Forms.OpenFileDialog
    Public Workbook As Excel.Workbook
    Private _xlSheet As Excel.Worksheet
    Private ReadOnly _xlCell As Excel.Range
    Private _skischule As Entities.Skischule = New Entities.Skischule

    Public Function ImportSkischule() As Entities.Skischule

        Workbook = Nothing

        _ofdDokument.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
        _ofdDokument.Filter = "Excel Dateien (*.xlsx)| *.xlsx"
        _ofdDokument.FilterIndex = 1
        _ofdDokument.RestoreDirectory = True

        If _ofdDokument.ShowDialog = DialogResult.OK Then
            Dim xlApp = New Excel.Application
            Workbook = xlApp.Workbooks.Open(_ofdDokument.FileName,, True)
            If CheckExcelFileFormat(Workbook) Then
                Return ReadImportedExcelliste(Workbook.ActiveSheet)
            End If
            Workbook.Close()
        End If
        Return Nothing

    End Function

    Private Function ReadImportedExcelliste(Excelsheet As Excel.Worksheet) As Entities.Skischule
        Dim CurrentRow = 4
        Dim RowCount = Excelsheet.UsedRange.Rows.Count
        Dim Skikursgruppe As Skikursgruppe
        Do Until CurrentRow > RowCount

            Dim Teilnehmer As New Teilnehmer With {
            .Vorname = Trim(Excelsheet.UsedRange(CurrentRow, 1).Value),
            .Name = Trim(Excelsheet.UsedRange(CurrentRow, 2).Value),
            .PersoenlichesLevel = FindLevel(Trim(Excelsheet.UsedRange(CurrentRow, 3).Value)),
            .Skikursgruppe = Trim(Excelsheet.UsedRange(CurrentRow, 4).Value)}
            _skischule.Teilnehmerliste.Add(Teilnehmer)

            'Gibt es die Skikursgruppe aus der Excelliste schon?
            If Teilnehmer.Skikursgruppe IsNot Nothing Then
                Skikursgruppe = FindSkikursgruppe(Teilnehmer.Skikursgruppe)
                ' Skikursgruppe gefunden, aktuellen Teilnehmer hinzufügen
                If Skikursgruppe IsNot Nothing Then
                    Skikursgruppe.AddMitglied(Teilnehmer)
                    Skikursgruppe.Gruppenlevel = Teilnehmer.PersoenlichesLevel
                End If
            End If
            CurrentRow += 1
        Loop
        Return _skischule
    End Function

    Private Function FindLevel(Benennung As String) As Level

        Dim Level = _skischule.Levelliste.FirstOrDefault(Function(k) k.Benennung = Benennung)
        If Level Is Nothing Then
            Level = New Level With {.Benennung = Benennung}
            _skischule.Levelliste.Add(Level)
        End If

        Return Level
    End Function

    Private Function FindSkikursgruppe(Gruppenname As String) As Skikursgruppe
        Dim Skikursgruppe = _skischule.Skikursgruppenliste.FirstOrDefault(Function(s) s.Gruppenname = Gruppenname)
        If Skikursgruppe Is Nothing Then
            Skikursgruppe = New Skikursgruppe With {.Gruppenname = Gruppenname}
            _skischule.Skikursgruppenliste.Add(Skikursgruppe)
        End If
        Return Skikursgruppe
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
