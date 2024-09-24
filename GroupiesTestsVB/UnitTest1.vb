Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies

Namespace GroupiesTestsVB
    <TestClass>
    Public Class UnitTest1
        <TestMethod>
        Sub TestSub()
            Assert.AreEqual("Es wurde keine Datei geladen", AppController.LoadFromJson("Test"))
        End Sub
    End Class
End Namespace

