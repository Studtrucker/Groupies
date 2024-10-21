Imports Groupies.Services
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.XlLeser
Imports System.Data


<TestClass>
Public Class ExcelImportTests

    <TestMethod>
    Public Sub testXLSXReader()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        End If

        Dim x = LoadDataTable(Pfad)
        For Each zeile As DataRow In x.Rows
            Debug.Print($"{zeile.ItemArray(0)} {zeile.ItemArray(1)}")
            Debug.Print($"{zeile.ItemArray(x.Columns.IndexOf("Vorname"))} {zeile.ItemArray(x.Columns.IndexOf("Name"))}")
        Next
    End Sub

    <TestMethod>
    Public Sub testXLSReader()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xls"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xls"
        End If

        Dim x = LoadDataTable(Pfad)
        For Each zeile As DataRow In x.Rows
            Debug.Print($"{zeile.ItemArray(0)} {zeile.ItemArray(1)}")
            Debug.Print($"{zeile.ItemArray(x.Columns.IndexOf("Vorname"))} {zeile.ItemArray(x.Columns.IndexOf("Name"))}")
        Next
    End Sub

    <TestMethod>
    Public Sub testCSVReader()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.csv"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.csv"
        End If

        Dim x = LoadDataTable(Pfad)

        For Each zeile As DataRow In x.Rows
            Debug.Print($"{zeile.ItemArray(0)} {zeile.ItemArray(1)}")
            Debug.Print($"{zeile.ItemArray(x.Columns.IndexOf("Vorname"))} {zeile.ItemArray(x.Columns.IndexOf("Name"))}")
        Next

    End Sub

    <TestMethod>
    Public Sub TestExcelDataSetReader()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        End If

        Dim x = LoadDataSet(Pfad)

        For Each table As DataTable In x.Tables
            Debug.Print($"{table}")
            For Each zeile As DataRow In table.Rows
                Debug.Print($"{zeile.ItemArray(table.Columns.IndexOf("Vorname"))} {zeile.ItemArray(table.Columns.IndexOf("Name"))}")
            Next
        Next

    End Sub


    <TestMethod>
    Public Sub TestOpenExcelFile()

        Dim xy = ExcelInteropService.OpenExcelAusInternet($"{Environment.CurrentDirectory}\Services\StandardGruppenverteilung.xlsx")

    End Sub

End Class
