Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Controller
Imports Groupies.Controller.AppController
Imports Groupies.Entities
<TestClass>
Public Class ClubTests
    <TestMethod>
    Public Sub TestTeilnehmerEingruppierenUndEntfernen()
        NeuenClubErstellen("Stubaital2024", 1)

        Dim Studti As New Teilnehmer("Andreas", "Studtrucker")
        Dim Manuela As New Teilnehmer("Manuela", "Ramm")
        Dim Lina As New Teilnehmer("Lina", "Hötger")

        CurrentClub.Teilnehmerliste.Add(Studti)
        CurrentClub.Teilnehmerliste.Add(Manuela)
        CurrentClub.Teilnehmerliste.Add(Lina)

        Assert.AreEqual(3, AppController.CurrentClub.Teilnehmerliste.Count)

        CurrentClub.Gruppenliste = StandardGruppen

        CurrentClub.TeilnehmerInGruppeEinteilen(Studti, CurrentClub.Gruppenliste.Take(1).Single)
        Assert.AreEqual(2, AppController.CurrentClub.FreieTeilnehmer.Count)
        Assert.AreEqual(1, AppController.CurrentClub.EingeteilteTeilnehmer.Count)

        Assert.AreEqual("Andreas Studtrucker", CurrentClub.EingeteilteTeilnehmer.Take(1).Single.VorUndNachname)
        Assert.AreEqual(3, CurrentClub.Teilnehmerliste.Count)

        Assert.AreEqual("Andreas Studtrucker", CurrentClub.EingeteilteTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Manuela Ramm", CurrentClub.FreieTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Lina Hötger", CurrentClub.FreieTeilnehmer.ElementAt(1).VorUndNachname)

        Assert.AreEqual(3, AppController.CurrentClub.Teilnehmerliste.Count)



        CurrentClub.TeilnehmerAusGruppeEntfernen(Studti, CurrentClub.Gruppenliste.Take(1).Single)
        Assert.AreEqual(3, AppController.CurrentClub.FreieTeilnehmer.Count)
        Assert.AreEqual(0, AppController.CurrentClub.EingeteilteTeilnehmer.Count)

    End Sub

    <TestMethod>
    Public Sub TestTrainerEingruppierenUndEntfernen()
        NeuenClubErstellen("Stubaital2024", 1)

        Dim Studti As New Trainer("Andreas", "Studtrucker")
        Dim Manuela As New Trainer("Manuela", "Ramm")
        Dim Lina As New Trainer("Lina", "Hötger")

        CurrentClub.Trainerliste.Add(Studti)
        CurrentClub.Trainerliste.Add(Manuela)
        CurrentClub.Trainerliste.Add(Lina)

        Assert.AreEqual(3, AppController.CurrentClub.Trainerliste.Count)

        CurrentClub.Gruppenliste = StandardGruppen

        CurrentClub.TrainerGruppeZuweisen(Studti, CurrentClub.Gruppenliste.ElementAt(0))
        Assert.AreEqual(2, AppController.CurrentClub.FreieTrainer.Count)
        Assert.AreEqual(1, AppController.CurrentClub.EingeteilteTrainer.Count)

        Assert.AreEqual("Andreas Studtrucker", CurrentClub.Gruppenliste.ElementAt(0).Trainer.VorUndNachname)
        Assert.AreEqual(3, CurrentClub.Trainerliste.Count)

        Assert.AreEqual("Manuela Ramm", CurrentClub.FreieTrainer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Lina Hötger", CurrentClub.FreieTrainer.ElementAt(1).VorUndNachname)

        Assert.AreEqual(3, AppController.CurrentClub.Trainerliste.Count)

        CurrentClub.TrainerAusGruppeEntfernen(CurrentClub.Gruppenliste.ElementAt(0))
        Assert.AreEqual(3, AppController.CurrentClub.FreieTrainer.Count)
        Assert.AreEqual(0, AppController.CurrentClub.EingeteilteTrainer.Count)

    End Sub

End Class
