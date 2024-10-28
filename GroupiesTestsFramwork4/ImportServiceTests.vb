Imports Groupies.Services
<TestClass>
Public Class ImportServiceTests
    <TestMethod>
    Public Sub TestStarteOpenFileDialog()
        Dim Pfad = StarteOpenFileDialog("C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\", "2024_TeilnehmerBearbeitet.xlsx")
        Assert.AreEqual("C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\2024_TeilnehmerBearbeitet.xlsx", Pfad)
    End Sub
End Class
