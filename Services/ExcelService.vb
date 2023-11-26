Imports System.Windows.Forms
Imports Excel = Microsoft.Office.Interop.Excel
Imports Groupies.Entities

Namespace ExcelService

    Public Module ExcelService
        Private ReadOnly _ofdDokument As New Forms.OpenFileDialog
        Public Workbook As Excel.Workbook
        Private _xlSheet As Excel.Worksheet
        Private ReadOnly _xlCell As Excel.Range
        Private ReadOnly _skischule = New Entities.Skiclub
        Private dic As Dictionary(Of Level, Integer)

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
                XlValid = XlValid And _xlSheet.Range("F1").Value = "Experte"

                ' Check first data row
                XlValid = XlValid And _xlSheet.Range("A16").Value = 15
                XlValid = XlValid And _xlSheet.UsedRange.Rows.Count = 16

            End If

            If Not XlValid Then MessageBox.Show("Die Datei beinhaltet keine Gruppenverteilung")

            Return XlValid

        End Function

        Public Function ReadLevelDistribution(CountOfGroups As Integer, Levels As LevelCollection) As Dictionary(Of Level, Integer)

            InitDictionary(Levels)

            Dim wb As Excel.Workbook = OpenAndCheckExcelFile("Services/GroupLevelDistribution.xlsx")

            If wb Is Nothing Then
                MessageBox.Show("Gruppenverteilung steht nicht zur Verfügung, es wird eine Standardverteilung genutzt")
                Return StandardDistribution()
            End If

            Dim CurrentRow = 1 + CountOfGroups

            dic.Item(Levels.Where(Function(x) x.LevelNaming = "Anfänger").Single) = CInt(Trim(wb.ActiveSheet.Range("B" & CurrentRow).Value))
            dic.Item(Levels.Where(Function(x) x.LevelNaming = "Fortgeschritten").Single) = CInt(Trim(wb.ActiveSheet.Range("C" & CurrentRow).Value))
            dic.Item(Levels.Where(Function(x) x.LevelNaming = "Genießer").Single) = CInt(Trim(wb.ActiveSheet.Range("D" & CurrentRow).Value))
            dic.Item(Levels.Where(Function(x) x.LevelNaming = "Könner").Single) = CInt(Trim(wb.ActiveSheet.Range("E" & CurrentRow).Value))
            dic.Item(Levels.Where(Function(x) x.LevelNaming = "Experte").Single) = CInt(Trim(wb.ActiveSheet.Range("F" & CurrentRow).Value))

            Return dic

        End Function

        Private Sub InitDictionary(Levels As LevelCollection)
            dic = New Dictionary(Of Level, Integer)
            For Each item In Levels
                dic.Add(item, 0)
            Next
        End Sub


        Private Function StandardDistribution() As Dictionary(Of Level, Integer)

            dic.Item(Services.Skiclub.Levellist.Where(Function(x) x.LevelNaming = "Anfänger").Single) = 2
            dic.Item(Services.Skiclub.Levellist.Where(Function(x) x.LevelNaming = "Fortgeschrittene").Single) = 4
            dic.Item(Services.Skiclub.Levellist.Where(Function(x) x.LevelNaming = "Genießer").Single) = 5
            dic.Item(Services.Skiclub.Levellist.Where(Function(x) x.LevelNaming = "Könner").Single) = 3
            dic.Item(Services.Skiclub.Levellist.Where(Function(x) x.LevelNaming = "Experten").Single) = 1

            Return dic

        End Function

    End Module

End Namespace
