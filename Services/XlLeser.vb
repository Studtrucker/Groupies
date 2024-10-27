Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.IO
Imports System.Text
Imports ExcelDataReader
Imports System.Text.CodePagesEncodingProvider

''' <summary>
''' Liest Daten aus den Dateiformaten xls, xlsx und csv
''' Für die App-Daten Teilnehmer und Trainer
''' </summary>
Public Class XlLeser

    ' Anleitungen:
    ' https//github.com/ExcelDataReader/ExcelDataReader
    ' https://stackoverflow.com/questions/32999550/read-data-from-an-excel-file

    Private _Tabelle As String
    Private _Spalten As List(Of String)

    Public Shared Function LoadDataSet(Pfad As String, Datentyp As String) As DataSet

        Dim fileExtension As String = Path.GetExtension(Pfad)
        Dim Dataset As DataSet
        Dim xlLeser As New XlLeser

        xlLeser.ErhalteBenennungenTabelleUndSpalte(Datentyp, IO.Path.GetExtension(Pfad))

        Select Case fileExtension.ToLower
            Case ".xlsx"
                Dataset = ConvertExcelToDataSet(Pfad, True)
            Case ".xls"
                Dataset = ConvertExcelToDataSet(Pfad)
            Case ".csv"
                Dataset = ConvertCsvToDataSet(Pfad)
            Case Else
                Dataset = New DataSet()
        End Select

        Try

            xlLeser.PruefeDataset(Dataset)

        Catch ex As Exception
            Throw New Exception("Formatprüfung nicht bestanden", ex)
        End Try

        Return Dataset

    End Function

    Private Shared Function ConvertExcelToDataSet(Pfad As String, Optional isXlsx As Boolean = False) As DataSet
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

    Private Shared Function ConvertCsvToDataSet(Pfad As String) As DataSet

        Dim stream = File.Open(Pfad, FileMode.Open, FileAccess.Read)
        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance)
        Using csvReader = ExcelReaderFactory.CreateCsvReader(stream)
            Dim conf = New ExcelDataSetConfiguration With {.ConfigureDataTable = Function(x) New ExcelDataTableConfiguration With {.UseHeaderRow = True}}

            Dim result As DataSet = csvReader.AsDataSet(conf)
            If result IsNot Nothing AndAlso result.Tables.Count > 0 Then
                Return result
            End If

        End Using

        Return Nothing

    End Function

    Private Sub ErhalteBenennungenTabelleUndSpalte(Datentyp As String, Dateityp As String)

        Dim fabrikImport As New FabrikDataImport
        Try
            Dim importFormat = fabrikImport.ErzeugeImportformat(Datentyp, Dateityp)
            _Tabelle = importFormat.Tabelle
            _Spalten = importFormat.Spalten
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Private Function PruefeDataset(DataSet As DataSet) As Boolean

        ' Prüfung, ob erforderliches Excelsheet vorhanden ist
        Dim BlattIndex = DataSet.Tables.IndexOf(_Tabelle)
        If BlattIndex < 0 Then
            Throw New Exception($"Tabellenblatt mit der Benennung [{_Tabelle}] fehlt")
            Return False
        End If

        ' Prüfung, ob Excelsheet erforderlichen Spalten enthält
        Dim Fehlerliste = New List(Of String)
        For Each Spalte In _Spalten
            Dim SpaltenIndex = DataSet.Tables(BlattIndex).Columns.IndexOf(Spalte)
            If SpaltenIndex < 0 Then
                Fehlerliste.Add(Spalte)
            End If
        Next
        If Fehlerliste.Count > 0 Then
            Dim sb As New StringBuilder
            For Each Fehler In Fehlerliste
                sb.Append("[")
                sb.Append(Fehler)
                sb.Append("], ")
            Next
            sb.Remove(sb.Length - 2, 2)
            If Fehlerliste.Count > 1 Then
                Throw New Exception($"Die Spalten mit der Benennung {sb} fehlen")
            Else
                Throw New Exception($"Die Spalte mit der Benennung {sb} fehlt")
            End If
            Return False
        End If
        Return True


    End Function

End Class
