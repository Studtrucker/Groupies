Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Services


<TestClass>
Public Class ExcelImportTests
    <TestMethod>
    Public Sub ImportTeilnehmer()
        Assert.AreEqual(True, OpenExcelFile("C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"))
    End Sub
End Class
