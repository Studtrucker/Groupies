Imports Groupies.Services
<TestClass>
Public Class ImportServiceTests

    <TestMethod>
    Public Sub TestStarteOpenFileDialog()
        Dim Pfad
        Dim Result
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\ExcelTestdatei.xlsx"
            Result = StarteOpenFileDialog("C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\", "ExcelTestdatei.xlsx")
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\ExcelTestdatei.xlsx"
            Result = StarteOpenFileDialog("C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\", "ExcelTestdatei.xlsx")
        End If

        Assert.AreEqual(Pfad, Result)

    End Sub

End Class
