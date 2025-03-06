Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports System.IO
Imports Groupies.Entities
Imports Groupies.Controller

<TestClass>
Public Class AppControllerTests

    <TestMethod>
    Sub TestNeuenSkiclubErstellen()

        Dim numberOfGroups = 9
        Assert.AreEqual($"[Stubaital2024] wurde erfolgreich erstellt.", AppController.NeuenClubErstellen("Stubaital2024"))
        Assert.AreEqual("Stubaital2024", AppController.AktuellerClub.ClubName)
        Assert.AreEqual(0, AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.Count)
        Assert.AreEqual(0, AppController.AktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer.Count)
        Assert.AreEqual(6, AppController.StandardLeistungsstufen.Count)
        Assert.AreEqual(0, AppController.AktuellerClub.SelectedEinteilung.GruppenloseTrainer.Count)

        Dim Studti As New Teilnehmer("Andreas", "Studtrucker")
        Dim Manuela As New Teilnehmer("Manuela", "Ramm")

        AppController.AktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer = New TeilnehmerCollection From {Studti, Manuela}
        Assert.AreEqual(2, AppController.AktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer.Count)
        Assert.AreEqual(2, AppController.AktuellerClub.SelectedEinteilung.AlleTeilnehmer.Count)
        Assert.AreEqual(0, AppController.AktuellerClub.SelectedEinteilung.EingeteilteTeilnehmer.Count)

        'Assert.AreEqual(String.Format("Manuela Ramm{0}Andreas Studtrucker{0}", vbCrLf), AppController.CurrentClub.Teilnehmerliste)

    End Sub


End Class


