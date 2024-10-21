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

Public Class XlLeser

    Public Shared Function LoadDataSet(Pfad As String) As DataSet
        Dim fileExtension As String = Path.GetExtension(Pfad)
        Select Case fileExtension.ToLower
            Case ".xlsx"
                Return ConvertExcelToDataSet(Pfad, True)
            Case ".xls"
                Return ConvertExcelToDataSet(Pfad)
            Case Else
                Return New DataSet()
        End Select
    End Function

    Public Shared Function ConvertExcelToDataSet(Pfad As String, Optional isXlsx As Boolean = False) As DataSet
        Dim DataTable As DataTable = Nothing

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

        Using stream = File.Open(Pfad, FileMode.Open, FileAccess.Read)

            Using excelReader = If(isXlsx, ExcelReaderFactory.CreateOpenXmlReader(stream), ExcelReaderFactory.CreateBinaryReader(stream))


                Dim conf = New ExcelDataSetConfiguration With {.ConfigureDataTable = Function(x) New ExcelDataTableConfiguration With {.UseHeaderRow = True}}
                Dim result As DataSet = excelReader.AsDataSet(conf)

                If result IsNot Nothing AndAlso result.Tables.Count > 0 Then
                    Return result
                End If

            End Using

        End Using

        Return Nothing

    End Function


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

        Dim stream As FileStream = Nothing
        Dim excelReader As IExcelDataReader = Nothing
        Dim DataTable As DataTable = Nothing

        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

        stream = File.Open(Pfad, FileMode.Open, FileAccess.Read)
        excelReader = If(isXlsx, ExcelReaderFactory.CreateOpenXmlReader(stream), ExcelReaderFactory.CreateBinaryReader(stream))

        Dim conf = New ExcelDataSetConfiguration With {.ConfigureDataTable = Function(x) New ExcelDataTableConfiguration With {.UseHeaderRow = True}}
        Dim result As DataSet = excelReader.AsDataSet(conf)

        If result IsNot Nothing AndAlso result.Tables.Count > 0 Then
            DataTable = result.Tables(0)
            Return DataTable
        End If

        Return Nothing

    End Function

    Public Shared Function ConvertCsvToDataTable(Pfad As String) As DataTable

        Dim dt = New DataTable()
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)

        Using sr = New StreamReader(Pfad)

            Dim headers() As String = sr.ReadLine().Split(";")
            For Each header In headers
                dt.Columns.Add(header)
            Next

            While Not sr.EndOfStream
                Dim zeilen() = sr.ReadLine().Split(";")
                Dim dr As DataRow = dt.NewRow
                For i As Integer = 0 To headers.Length - 1
                    dr(i) = zeilen(i)
                Next
                dt.Rows.Add(dr)
            End While

        End Using

        Return dt

    End Function

End Class
