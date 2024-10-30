Imports Groupies.Services
<TestClass>
Public Class ImportServiceTests
    <TestMethod>
    Public Sub TestStarteOpenFileDialog()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\2024_StubaiBearbeitet.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\2024_StubaiBearbeitet.xlsx"
        End If
        Dim Result = StarteOpenFileDialog("C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\", "2024_StubaiBearbeitet.xlsx")
        Assert.AreEqual(Pfad, Result)
    End Sub
End Class
