Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports System.IO

Namespace GroupiesTestsVB
    <TestClass>
    Public Class AppControllerTests
        <TestMethod>
        Sub TestLoadJson()
            Dim filename As String
            filename = "Test"
            ' File erst erstellen
            File.Create(String.Format("{0}.json", filename)).Close()
            Assert.AreEqual(String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.json", filename)), AppController.LoadFromJson(filename))
            ' File wieder löschen
            File.Delete(String.Format("{0}.json", filename))
        End Sub
        <TestMethod>
        Sub TestLoadXML()
            Dim filename As String
            filename = "test"
            ' File erst erstellen
            File.Create(String.Format("{0}.xml", filename)).Close()
            Assert.AreEqual(String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.xml", filename)), AppController.LoadFromXML(filename))
            ' File wieder löschen
            File.Delete(String.Format("{0}.xml", filename))
        End Sub

        <TestMethod>
        Public Sub TestInvalidFilenames()
            Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", AppController.LoadFromJson("invalid\\filename"))
            Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", AppController.LoadFromJson("invalid/filename"))
            Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", AppController.LoadFromJson("invalid filename"))
        End Sub

        <TestMethod>
        Public Sub TestNotExistingFile()
            Dim filename As String
            filename = "tester"
            Assert.AreEqual(String.Format("Die Datei {0} im Ordner {1} existiert nicht.", filename & ".json", Environment.CurrentDirectory), AppController.LoadFromJson(filename))
            Assert.AreEqual(String.Format("Die Datei {0} im Ordner {1} existiert nicht.", filename & ".xml", Environment.CurrentDirectory), AppController.LoadFromXML(filename))
        End Sub

    End Class

End Namespace

