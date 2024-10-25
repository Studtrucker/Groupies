Imports Groupies.Entities
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Controller.AppController
Imports System.IO
Imports System.Runtime.InteropServices

Namespace Services

    Public Module ExcelInteropService


        Public Function OpenExcelAusInternet(Pfad As String) As List(Of String())
            Dim xl As New Microsoft.Office.Interop.Excel.Application()
            Dim Workbook As Workbook = xl.Workbooks.Open(Pfad)
            Dim sheet As Worksheet = Workbook.Sheets(1)
            sheet.Visible = True

            Dim numRows = sheet.UsedRange.Rows.Count
            Dim numColumns = 2     ' according To your sample

            Dim contents = New List(Of String())

            Dim record As String() = New String(2) {}

            For rowIndex = 1 To numRows
                ' assuming the data starts at 1,1
                For colIndex = 1 To numColumns
                    Dim cell As Range = DirectCast(sheet.Cells(rowIndex, colIndex), Range)
                    If cell.Value IsNot Nothing Then
                        record(colIndex - 1) = Convert.ToString(cell.Value)
                    End If
                Next
                contents.Add(record)
            Next

            xl.Quit()
            Marshal.ReleaseComObject(xl)

            Return contents

        End Function











































































        'Private ReadOnly _ofdDokument As New Forms.OpenFileDialog
        'Private ReadOnly _xlCell As Excel.Range
        'Private ReadOnly _skischule = New Entities.Club
        Private xlApp As Microsoft.Office.Interop.Excel.Application
        Private dic As Dictionary(Of Leistungsstufe, Integer)

        ''' <summary>
        ''' Liest eine Liste von Teilnehmern aus einer Excel Datei
        ''' Format:
        ''' Spalte A: Vorname
        ''' Spalte B: Nachname
        ''' </summary>
        ''' <returns></returns>
        Public Function LeseExcelTeilnehmer(Pfad As String) As IEnumerable(Of Teilnehmer)

            Dim StreamReader As New StreamReader(Pfad)


            While Not StreamReader.EndOfStream
                Dim ZeileAusExcel = StreamReader.ReadLine

                Debug.Print(ZeileAusExcel)
            End While
            StreamReader.Close()

            Return New List(Of Teilnehmer)
        End Function










        Private Function KontrolliereUndOeffneExcelFile(FilePath As String) As Boolean

            If IO.File.Exists(FilePath) Then
                OpenWorkbook(FilePath)
                If CheckFileFormat() Then
                    Return True
                End If
            End If
            Return False

        End Function

        Private Function CheckFileFormat() As Boolean

            Dim XlValid As Boolean = True

            ' Check first data row
            XlValid = XlValid And xlApp.ActiveSheet.Range("A16").Value = 15
            XlValid = XlValid And xlApp.ActiveSheet.UsedRange.Rows.Count = 16

            XlValid = XlValid And xlApp.ActiveSheet.Range("B1").Value = "Anfänger"
            XlValid = XlValid And xlApp.ActiveSheet.Range("C1").Value = "Fortgeschritten"
            XlValid = XlValid And xlApp.ActiveSheet.Range("D1").Value = "Genießer"
            XlValid = XlValid And xlApp.ActiveSheet.Range("E1").Value = "Könner"
            XlValid = XlValid And xlApp.ActiveSheet.Range("F1").Value = "Experte"


            If Not XlValid Then
                MessageBox.Show("Die Datei beinhaltet keine Gruppenverteilung")
                Return Nothing
            End If

            Return XlValid

        End Function

        Private Function CheckFileFormat(wb As Microsoft.Office.Interop.Excel.Workbook) As Boolean
            Dim XlValid As Boolean

            ' Excel Sheet (Überschrift Zeile 1, Daten Zeile 2 bis zum Dateiende)
            Dim CurrentSheet = xlApp.CurrentWorkbook.ActiveSheet

            If CurrentSheet IsNot Nothing Then

                XlValid = True

                ' Check column caption
                XlValid = XlValid And CurrentSheet.Range("B1").Value = "Anfänger"
                XlValid = XlValid And CurrentSheet.Range("C1").Value = "Fortgeschritten"
                XlValid = XlValid And CurrentSheet.Range("D1").Value = "Genießer"
                XlValid = XlValid And CurrentSheet.Range("E1").Value = "Könner"
                XlValid = XlValid And CurrentSheet.Range("F1").Value = "Experte"

                ' Check first data row
                XlValid = XlValid And CurrentSheet.Range("A16").Value = 15
                XlValid = XlValid And CurrentSheet.UsedRange.Rows.Count = 16

            End If

            If Not XlValid Then MessageBox.Show("Die Datei beinhaltet keine Gruppenverteilung")

            Return XlValid

        End Function

        Public Function ReadLevelVerteilung(NumberOfGroups As Integer) As Dictionary(Of Leistungsstufe, Integer)

            If KontrolliereUndOeffneExcelFile($"{Environment.CurrentDirectory}\Services\GroupLevelDistribution.xlsx") Then

                Dim CurrentRow = 1 + NumberOfGroups

                Dim dic = New Dictionary(Of Leistungsstufe, Integer)
                Dim Levels As LeistungsstufeCollection = StandardLeistungsstufen
                dic.Item(Levels.Where(Function(x) x.Benennung = "Anfänger").Single) = CInt(Trim(xlApp.ActiveSheet.Range("B" & CurrentRow).Value))
                dic.Item(Levels.Where(Function(x) x.Benennung = "Fortgeschritten").Single) = CInt(Trim(xlApp.ActiveSheet.Range("C" & CurrentRow).Value))
                dic.Item(Levels.Where(Function(x) x.Benennung = "Genießer").Single) = CInt(Trim(xlApp.ActiveSheet.Range("D" & CurrentRow).Value))
                dic.Item(Levels.Where(Function(x) x.Benennung = "Könner").Single) = CInt(Trim(xlApp.ActiveSheet.Range("E" & CurrentRow).Value))
                dic.Item(Levels.Where(Function(x) x.Benennung = "Experte").Single) = CInt(Trim(xlApp.ActiveSheet.Range("F" & CurrentRow).Value))

                Return dic
            Else
                MessageBox.Show("Gruppenverteilung steht nicht zur Verfügung, es wird eine Standardverteilung genutzt")
                Return StandardDistribution()
            End If

        End Function

        Private Sub InitDictionary(Levels As LeistungsstufeCollection)
            dic = New Dictionary(Of Leistungsstufe, Integer)
            For Each item In Levels
                dic.Add(item, 0)
            Next
        End Sub


        Private Function StandardDistribution() As Dictionary(Of Leistungsstufe, Integer)

            dic.Item(Services.Club.Leistungsstufenliste.Where(Function(x) x.Benennung = "Anfänger").Single) = 2
            dic.Item(Services.Club.Leistungsstufenliste.Where(Function(x) x.Benennung = "Fortgeschrittene").Single) = 4
            dic.Item(Services.Club.Leistungsstufenliste.Where(Function(x) x.Benennung = "Genießer").Single) = 5
            dic.Item(Services.Club.Leistungsstufenliste.Where(Function(x) x.Benennung = "Könner").Single) = 3
            dic.Item(Services.Club.Leistungsstufenliste.Where(Function(x) x.Benennung = "Experten").Single) = 1

            Return dic

        End Function

        Public Function OpenExcelFile(Path As String) As Boolean

            xlApp = New Microsoft.Office.Interop.Excel.Application
            Try
                xlApp.Workbooks.Open(Path)
            Catch ex As Exception
                Return False
            End Try
            xlApp.Visible = True
            xlApp.Workbooks.Close()
            Return True

        End Function

        Public Sub OpenWorkbook(Path As String)

            xlApp = New Microsoft.Office.Interop.Excel.Application
            xlApp.Workbooks.Open(Path)
            xlApp.Visible = True

        End Sub

        Public Sub SchliesseExcel()
            xlApp.ActiveWorkbook.Close()
        End Sub

    End Module

End Namespace
