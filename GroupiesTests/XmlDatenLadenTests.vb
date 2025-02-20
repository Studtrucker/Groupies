Imports Groupies
Imports Groupies.Entities
Imports Groupies.Controller
Imports System.Windows
Imports System.IO

<TestClass>
Public Class XmlDatenLadenTests

    <TestMethod>
    Public Sub TestInvalidFilenames()
        Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", XMLDatenLaden.LoadFromJson("invalid\\filename"))
        Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", XMLDatenLaden.LoadFromJson("invalid/filename"))
        Assert.AreEqual("Bitte geben Sie einen Dateinamen ohne Schrägstriche oder Leerzeichen ein.", XMLDatenLaden.LoadFromJson("invalid filename"))
    End Sub

    <TestMethod>
    Public Sub TestNotExistingFile()
        Dim filename As String
        filename = "tester"
        Assert.AreEqual(String.Format("Die Datei {0} im Ordner {1} existiert nicht.", filename & ".json", Environment.CurrentDirectory), XMLDatenLaden.LoadFromJson(filename))
        Assert.AreEqual(String.Format("Die Datei {0} im Ordner {1} existiert nicht.", filename & ".xml", Environment.CurrentDirectory), XMLDatenLaden.LoadFromXML(filename))
    End Sub

    <TestMethod>
    Sub TestLoadJson()
        Dim filename As String
        filename = "Test"
        ' File erst erstellen
        File.Create(String.Format("{0}.json", filename)).Close()
        Assert.AreEqual(String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.json", filename)), XMLDatenLaden.LoadFromJson(filename))
        ' File wieder löschen
        File.Delete(String.Format("{0}.json", filename))
    End Sub

    <TestMethod>
    Sub TestLoadXML()
        Dim filename As String
        filename = "test"
        ' File erst erstellen
        File.Create(String.Format("{0}.xml", filename)).Close()
        Assert.AreEqual(String.Format("Die Datei {0} wurde geladen.", String.Format("{0}.xml", filename)), XMLDatenLaden.LoadFromXML(filename))
        ' File wieder löschen
        File.Delete(String.Format("{0}.xml", filename))
    End Sub


End Class
