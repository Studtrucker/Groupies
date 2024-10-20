Imports Groupies.Services
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.DatenTabelle
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

    End Sub

    <TestMethod>
    Public Sub testXLSReader()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\GroupLevelDistribution.xls"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xls"
        End If

        Dim x = LoadDataTable(Pfad)

    End Sub

    <TestMethod>
    Public Sub testCSVReader()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\GroupLevelDistribution.csv"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\GroupLevelDistribution.csv"
        End If

        Dim x As DataTable = LoadDataTable(Pfad)

        For Each zeile As DataRow In x.Rows
            Debug.Print(zeile.ToString)
        Next

    End Sub


    <TestMethod>
    Public Sub TestOpenExcelFile()

        Dim xy = ExcelInteropService.OpenExcelAusInternet($"{Environment.CurrentDirectory}\Services\StandardGruppenverteilung.xlsx")

    End Sub

End Class
