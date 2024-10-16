Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Services


<TestClass>
Public Class ExcelImportTests
    <TestMethod>
    Public Sub ImportTeilnehmer()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        End If

        Assert.AreEqual(True, OpenExcelFile(Pfad))
    End Sub
End Class
