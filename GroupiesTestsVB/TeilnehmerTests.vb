Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class TeilnehmerTests

    <TestMethod>
    Public Sub TestTeilnehmerErstellen()
        Dim Tn As New Teilnehmer("Manuela", "Ramm")
        Assert.AreEqual("Manuela Ramm", Tn.ToString)
        Assert.AreEqual("Manuela Ramm", Tn.VorUndNachname)
        Dim Tn1 As New Teilnehmer("Manuela", "Ramm", New Leistungsstufe("Könner"))
        Assert.AreEqual("Manuela Ramm, Könner", Tn1.AusgabeAnTrainerinfo)
        Assert.AreEqual("Manuela Ramm", Tn1.AusgabeAnTeilnehmerinfo)
    End Sub
End Class
