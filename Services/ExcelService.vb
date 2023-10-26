Imports System.Windows.Forms
Imports Excel = Microsoft.Office.Interop.Excel
Imports Skischule.Entities

Namespace ExcelService

    Public Module ExcelService
        Private ReadOnly _ofdDokument As New Forms.OpenFileDialog
        Public Workbook As Excel.Workbook
        Private _xlSheet As Excel.Worksheet
        Private ReadOnly _xlCell As Excel.Range
        Private _skischule As Entities.Skischule = New Entities.Skischule

        Private Function OpenAndCheckExcelFile(FilePath As String) As Excel.Workbook

            Workbook = Nothing
            If Not IO.File.Exists(FilePath) Then
                Return Nothing
            End If

            Dim xlApp = New Excel.Application
            Workbook = xlApp.Workbooks.Open(Environment.CurrentDirectory & "\Services\" & "GroupLevelDistribution.xlsx",, True)

            If Not CheckFileFormat(Workbook) Then
                Return Nothing
            End If
            Return Workbook

        End Function

        Private Function CheckFileFormat(wb As Excel.Workbook) As Boolean
            Dim XlValid As Boolean

            ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
            _xlSheet = wb.ActiveSheet

            If _xlSheet IsNot Nothing Then

                XlValid = True

                ' Check column caption
                XlValid = XlValid And _xlSheet.Range("B1").Value = "Anfänger"
                XlValid = XlValid And _xlSheet.Range("C1").Value = "Fortgeschritten"
                XlValid = XlValid And _xlSheet.Range("D1").Value = "Genießer"
                XlValid = XlValid And _xlSheet.Range("E1").Value = "Könner"
                XlValid = XlValid And _xlSheet.Range("F1").Value = "Experten"

                ' Check first data row
                XlValid = XlValid And _xlSheet.Range("A16").Value = 15
                XlValid = XlValid And _xlSheet.UsedRange.Rows.Count = 16

            End If

            If Not XlValid Then MessageBox.Show("Die Datei beinhaltet keine Gruppenverteilung")

            Return XlValid

        End Function

        Public Function ReadLevelDistribution(CountOfGroups As Integer) As Dictionary(Of Level, Integer)

            Dim wb As Excel.Workbook= OpenAndCheckExcelFile("Services/GroupLevelDistribution.xlsx")

            If wb Is Nothing Then
                MessageBox.Show("Gruppenverteilung steht nicht zur Verfügung, es wird eine Standardverteilung genutzt")
                Return StandardDistribution()
            End If

            Dim CurrentRow = 1 + CountOfGroups
            Dim dic = InitDictionary()

            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Anfänger").Single) = CInt(Trim(wb.ActiveSheet.Range("B" & CurrentRow).Value))
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Fortgeschrittene").Single) = CInt(Trim(wb.ActiveSheet.Range("C" & CurrentRow).Value))
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Genießer").Single) = CInt(Trim(wb.ActiveSheet.Range("D" & CurrentRow).Value))
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Könner").Single) = CInt(Trim(wb.ActiveSheet.Range("E" & CurrentRow).Value))
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Experten").Single) = CInt(Trim(wb.ActiveSheet.Range("F" & CurrentRow).Value))

            Return dic

        End Function

        Private Function InitDictionary() As Dictionary(Of Level, Integer)
            Dim dic = New Dictionary(Of Level, Integer)

            For Each item In DataService.Skiclub.Levelliste
                dic.Add(item, 0)
            Next

            Return dic
        End Function


        Private Function StandardDistribution() As Dictionary(Of Level, Integer)
            Dim dic = InitDictionary()

            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Anfänger").Single) = 2
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Fortgeschrittene").Single) = 4
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Genießer").Single) = 5
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Könner").Single) = 3
            dic.Item(DataService.Skiclub.Levelliste.Where(Function(x) x.LevelName = "Experten").Single) = 1

            Return dic

        End Function

    End Module

End Namespace
