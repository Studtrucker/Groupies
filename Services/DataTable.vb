Imports excel
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Text
Imports ExcelDataReader

''' <summary>
''' aus https://stackoverflow.com/questions/32999550/read-data-from-an-excel-file
''' </summary>

Public Class DatenTabelle
    Public Shared Function LoadDataTable(Pfad As String) As DataTable
        Dim fileExtension As String = Path.GetExtension(Pfad)
        Select Case fileExtension.ToLower
            Case ".xlsx"
                Return ConvertExcelToDataTable(Pfad, True)
            Case ".xls"
                Return ConvertExcelToDataTable(Pfad, False)
            Case ".csv"
                Return ConvertCsvToDataTable(Pfad)
            Case Else
                Return New DataTable()
        End Select

    End Function

    Public Shared Function ConvertExcelToDataTable(Pfad As String, isXLSX As Boolean) As DataTable
        Dim Stream As FileStream = Nothing
        Dim excelReader As IExcelDataReader = Nothing
        Dim DataTable As DataTable = Nothing
        Stream = File.Open(Pfad, FileMode.Open, FileAccess.Read)

        excelReader = If(isXLSX, ExcelReaderFactory.CreateOpenXmlReader(Stream), ExcelReaderFactory.CreateBinaryReader(Stream))
        excelReader.IsFirstRowAsColumnNames = True
        Dim result As DataSet = excelReader.AsDataSet()

        If result IsNot Nothing AndAlso result.Tables.Count > 0 Then
            DataTable = result.Tables(0)
            Return DataTable
        End If

    End Function


    Public Shared Function ConvertCsvToDataTable(Pfad As String) As DataTable

        Dim dt = New DataTable()
        Using sr = New StreamReader(Pfad)

            Dim headers() As String = sr.ReadLine().Split(",")
            For Each header In headers
                dt.Columns.Add(header)
                While Not sr.EndOfStream
                    Dim zeilen() = sr.ReadLine().Split(",")
                    Dim dr As DataRow = dt.NewRow
                    For i As Integer = 0 To i < header.Length
                        dr(i) = zeilen(i)
                    Next
                    dt.Rows.Add(dr)
                End While
            Next

        End Using

        Return dt

    End Function

End Class
