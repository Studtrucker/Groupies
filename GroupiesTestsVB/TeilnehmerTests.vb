Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class TeilnehmerTests

    <TestMethod>
    Public Sub TestTeilnehmerErstellen()
        Dim Tn As New Teilnehmer("Elke", "Steiner")
        Assert.AreEqual("Elke Steiner", Tn.ToString)
        Assert.AreEqual("Elke Steiner", Tn.VorUndNachname)
        Dim Tn1 As New Teilnehmer("Manuela", "Ramm", New Leistungsstufe("Könner"))
        Assert.AreEqual("Manuela Ramm, Könner", Tn1.AusgabeInTrainerinfo)
        Assert.AreEqual("Manuela Ramm", Tn1.AusgabeInTeilnehmerinfo)
        Dim Tn2 As New Teilnehmer("Willi", "Sensmeier", New Leistungsstufe("Experte"))
        Dim Tn3 As New Teilnehmer("Lothar", "Hötger", New Leistungsstufe("Experte"))
        Dim Tn4 As New Teilnehmer("Liane", "Hötger")

        Dim tnL = New TeilnehmerCollection From {Tn, Tn1, Tn4}
        tnL.Add(Tn2)
        tnL.Add(Tn3)

        Dim x = tnL.GeordnetNachLeistungsstufeNachnameVorname

    End Sub
End Class
