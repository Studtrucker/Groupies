Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Text
Imports ExcelDataReader
Imports System.Text.CodePagesEncodingProvider

''' <summary>
''' https//github.com/ExcelDataReader/ExcelDataReader
''' https://stackoverflow.com/questions/32999550/read-data-from-an-excel-file
''' </summary>

Public Class DatenTabelle

    Public Shared Function LoadDataTable(Pfad As String) As DataTable
        Dim fileExtension As String = Path.GetExtension(Pfad)
        Select Case fileExtension.ToLower
            Case ".xlsx"
                Return ConvertExcelToDataTable(Pfad, True)
            Case ".xls"
                Return ConvertExcelToDataTable(Pfad)
            Case ".csv"
                Return ConvertCsvToDataTable(Pfad)
            Case Else
                Return New DataTable()
        End Select

    End Function

    Public Shared Function ConvertExcelToDataTable(Pfad As String, Optional isXlsx As Boolean = False) As DataTable
        Dim Stream As FileStream = Nothing
        Dim excelReader As IExcelDataReader = Nothing
        Dim DataTable As DataTable = Nothing

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

        'Dim result As DataSet
        Using varStream = File.Open(Pfad, FileMode.Open, FileAccess.Read)
            Using varReader = If(isXlsx, ExcelReaderFactory.CreateOpenXmlReader(varStream), ExcelReaderFactory.CreateBinaryReader(varStream))

                Dim result = varReader.AsDataSet(New ExcelDataSetConfiguration)
                'Todo: Header Zeile feststellen
                '{
                'ConfigureDataTable = (_) => New ExcelDataTableConfiguration()
                '{
                'UseHeaderRow = True
                '}
                '});
                If result IsNot Nothing AndAlso result.Tables.Count > 0 Then
                    DataTable = result.Tables(0)
                    Return DataTable
                End If


            End Using
        End Using



        Return Nothing

    End Function

    Public Shared Function ConvertCsvToDataTable(Pfad As String) As DataTable

        Dim dt = New DataTable()
        Using sr = New StreamReader(Pfad)

            Dim headers() As String = sr.ReadLine().Split(";")
            For Each header In headers
                dt.Columns.Add(header)
                While Not sr.EndOfStream
                    Dim zeilen() = sr.ReadLine().Split(";")
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
