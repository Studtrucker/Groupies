Imports Groupies.Services
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports System

Namespace GroupiesTestsVB

    <TestClass>
    Public Class ExcelServiceTests

        <TestMethod>
        Public Sub TestOpenExcelFile()
            Assert.AreEqual(True, ExcelService.OpenExcelFile($"{Environment.CurrentDirectory}\Services\GroupLevelDistribution.xlsx"))
            ExcelService.SchliesseExcel
        End Sub

    End Class
End Namespace
