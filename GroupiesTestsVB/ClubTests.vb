Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Controller
Imports Groupies.Controller.AppController
Imports Groupies.Entities
<TestClass>
Public Class ClubTests
    <TestMethod>
    Public Sub TestTeilnehmerEingruppierenUndEntfernen()
        Dim Stubai2024 = New Club("Stubaital2024")

        Dim Studti As New Teilnehmer("Andreas", "Studtrucker")
        Dim Manuela As New Teilnehmer("Manuela", "Ramm")
        Dim Lina As New Teilnehmer("Lina", "Hötger")

        Stubai2024.GruppenloseTeilnehmer.Add(Studti)
        Stubai2024.GruppenloseTeilnehmer.Add(Manuela)
        Stubai2024.GruppenloseTeilnehmer.Add(Lina)

        Assert.AreEqual(3, Stubai2024.GruppenloseTeilnehmer.Count)

        Stubai2024.Gruppenliste = StandardGruppen

        Stubai2024.TeilnehmerInGruppeEinteilen(Studti, CurrentClub.Gruppenliste.ElementAt(0))
        Assert.AreEqual(2, Stubai2024.AlleTeilnehmer.Count)
        Assert.AreEqual(1, Stubai2024.EingeteilteTeilnehmer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.EingeteilteTeilnehmer.Take(1).Single.VorUndNachname)
        Assert.AreEqual(3, Stubai2024.GruppenloseTeilnehmer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.EingeteilteTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Manuela Ramm", Stubai2024.AlleTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Lina Hötger", Stubai2024.AlleTeilnehmer.ElementAt(1).VorUndNachname)

        Assert.AreEqual(3, Stubai2024.GruppenloseTeilnehmer.Count)


        CurrentClub.TeilnehmerAusGruppeEntfernen(Studti, Stubai2024.Gruppenliste.Take(1).Single)
        Assert.AreEqual(3, Stubai2024.AlleTeilnehmer.Count)
        Assert.AreEqual(0, Stubai2024.EingeteilteTeilnehmer.Count)

    End Sub

    <TestMethod>
    Public Sub TestTrainerEingruppierenUndEntfernen()
        Dim Stubai2024 = New Club("Stubaital2024")

        Assert.AreEqual(0, Stubai2024.Gruppenliste.Count)

        ' Eine Gruppe wird neu instanziiert
        Dim Experte = New Gruppe("Experte")
        Dim Racer = New Gruppe("Racer")

        Stubai2024.Gruppenliste = New GruppeCollection() From {Experte, Racer}
        Assert.AreEqual(2, Stubai2024.Gruppenliste.Count)

        Dim Studti As New Trainer("Andreas", "Studtrucker")
        Dim Manuela As New Trainer("Manuela", "Ramm")
        Dim Lina As New Trainer("Lina", "Hötger")

        Stubai2024.GruppenloseTrainer.Add(Studti)
        Stubai2024.GruppenloseTrainer.Add(Manuela)
        Stubai2024.GruppenloseTrainer.Add(Lina)

        Assert.AreEqual(3, Stubai2024.GruppenloseTrainer.Count)
        Assert.AreEqual(0, Stubai2024.EingeteilteTrainer.Count)
        Assert.AreEqual(3, Stubai2024.AlleTrainer.Count)

        Stubai2024.TrainerEinerGruppeZuweisen(Studti, Stubai2024.Gruppenliste.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.AlleTrainer.Count)
        Assert.AreEqual(1, Stubai2024.EingeteilteTrainer.Count)
        Assert.AreEqual(2, Stubai2024.GruppenloseTrainer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.Gruppenliste.ElementAt(0).Trainer.VorUndNachname)
        Assert.AreEqual(3, Stubai2024.GruppenloseTrainer.Count)

        Assert.AreEqual("Manuela Ramm", Stubai2024.GruppenloseTrainer.ElementAt(1).VorUndNachname)
        Assert.AreEqual("Lina Hötger", Stubai2024.GruppenloseTrainer.ElementAt(2).VorUndNachname)

        Assert.AreEqual(3, Stubai2024.GruppenloseTrainer.Count)

        Stubai2024.TrainerAusGruppeEntfernen(Stubai2024.Gruppenliste.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.AlleTrainer.Count)
        Assert.AreEqual(0, Stubai2024.EingeteilteTrainer.Count)

    End Sub

    <TestMethod>
    Public Sub TestEingeteilteTeilnehmer()
        ' Ein Verein wird neu instanziiert
        Dim Testverein = New Club("Testverein09")
        ' Eine Gruppe wird neu instanziiert
        Dim Experte = New Gruppe("Experte")
        Dim Racer = New Gruppe("Racer")

        ' Es werden fünf Teilnehmer erstellt
        Dim Studti = New Teilnehmer("Andreas", "Studtrucker")
        Dim Manu = New Teilnehmer("Manuela", "Ramm")
        Dim Ralf = New Teilnehmer("Ralf", "Granderath")
        Dim Sandra = New Teilnehmer("Sandra", "Oelschläger")
        Dim Rene = New Teilnehmer("Rene", "van Gansewinkel")

        ' Die Gruppe wird der Gruppenliste hinzugefügt 
        Testverein.Gruppenliste.Add(Experte)
        Testverein.Gruppenliste.Add(Racer)
        ' Vier Teilnehmer werden der Teilnehmerliste hinzugefügt 
        Testverein.GruppenloseTeilnehmer = New TeilnehmerCollection(New List(Of Teilnehmer) From {Studti, Manu, Ralf, Sandra})
        ' Teilnehmer fünf wird der Teilnehmerliste hinzugefügt 
        Testverein.GruppenloseTeilnehmer.Add(Rene)
        ' 2 Teilnehmer werden der Gruppe als Mitglieder hinzugefügt
        Testverein.TeilnehmerInGruppeEinteilen(Manu, Experte)
        Testverein.TeilnehmerInGruppeEinteilen(Studti, Experte)

        ' Test
        Assert.AreEqual(3, Testverein.GruppenloseTeilnehmer.Count)
        Assert.AreEqual(2, Testverein.EingeteilteTeilnehmer.Count)
        Assert.AreEqual(5, Testverein.AlleTeilnehmer.Count)


    End Sub

End Class
