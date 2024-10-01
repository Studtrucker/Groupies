Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class TeilnehmerTests

    <TestMethod>
    Public Sub TestTeilnehmerErstellen()
        Dim Elke As New Teilnehmer("Elke", "Steiner")
        Assert.AreEqual("Elke Steiner", Elke.ToString)
        Assert.AreEqual("Elke Steiner", Elke.VorUndNachname)
        Dim Manu As New Teilnehmer("Manuela", "Steiner", New Leistungsstufe("Könner"))
        Assert.AreEqual("Manuela Steiner, Könner", Manu.AusgabeInTrainerinfo)
        Assert.AreEqual("Manuela Steiner", Manu.AusgabeInTeilnehmerinfo)
        Dim Willi As New Teilnehmer("Willi", "Steiner", New Leistungsstufe("Experte"))
        Dim Lothar As New Teilnehmer("Lothar", "Hötger", New Leistungsstufe("Experte"))
        Dim Liane As New Teilnehmer("Liane", "Hötger")

        Dim tnL = New TeilnehmerCollection From {Elke, Manu, Liane}
        tnL.Add(Willi)
        tnL.Add(Lothar)

        Dim x = tnL.GeordnetFunctionT
        Dim y = tnL.GeordnetMe

    End Sub
End Class
