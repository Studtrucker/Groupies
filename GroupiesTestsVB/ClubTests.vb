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

        Stubai2024.Teilnehmerliste.Add(Studti)
        Stubai2024.Teilnehmerliste.Add(Manuela)
        Stubai2024.Teilnehmerliste.Add(Lina)

        Assert.AreEqual(3, Stubai2024.Teilnehmerliste.Count)

        Stubai2024.Gruppenliste = StandardGruppen

        Stubai2024.TeilnehmerInGruppeEinteilen(Studti, CurrentClub.Gruppenliste.ElementAt(0))
        Assert.AreEqual(2, Stubai2024.FreieTeilnehmer.Count)
        Assert.AreEqual(1, Stubai2024.EingeteilteTeilnehmer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.EingeteilteTeilnehmer.Take(1).Single.VorUndNachname)
        Assert.AreEqual(3, Stubai2024.Teilnehmerliste.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.EingeteilteTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Manuela Ramm", Stubai2024.FreieTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Lina Hötger", Stubai2024.FreieTeilnehmer.ElementAt(1).VorUndNachname)

        Assert.AreEqual(3, Stubai2024.Teilnehmerliste.Count)


        CurrentClub.TeilnehmerAusGruppeEntfernen(Studti, Stubai2024.Gruppenliste.Take(1).Single)
        Assert.AreEqual(3, Stubai2024.FreieTeilnehmer.Count)
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

        Stubai2024.Trainerliste.Add(Studti)
        Stubai2024.Trainerliste.Add(Manuela)
        Stubai2024.Trainerliste.Add(Lina)

        Assert.AreEqual(3, Stubai2024.Trainerliste.Count)

        Stubai2024.TrainerEinerGruppeZuweisen(Studti, Stubai2024.Gruppenliste.ElementAt(0))
        Assert.AreEqual(2, Stubai2024.FreieTrainer.Count)
        Assert.AreEqual(1, Stubai2024.EingeteilteTrainer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.Gruppenliste.ElementAt(0).Trainer.VorUndNachname)
        Assert.AreEqual(3, Stubai2024.Trainerliste.Count)

        Assert.AreEqual("Manuela Ramm", Stubai2024.Trainerliste.ElementAt(1).VorUndNachname)
        Assert.AreEqual("Lina Hötger", Stubai2024.Trainerliste.ElementAt(2).VorUndNachname)

        Assert.AreEqual(3, Stubai2024.Trainerliste.Count)

        Stubai2024.TrainerAusGruppeEntfernen(Stubai2024.Gruppenliste.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.FreieTrainer.Count)
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
        Testverein.Teilnehmerliste = New TeilnehmerCollection(New List(Of Teilnehmer) From {Studti, Manu, Ralf, Sandra})
        ' Teilnehmer fünf wird der Teilnehmerliste hinzugefügt 
        Testverein.Teilnehmerliste.Add(Rene)
        ' 2 Teilnehmer werden der Gruppe als Mitglieder hinzugefügt
        Testverein.TeilnehmerInGruppeEinteilen(Manu, Experte)
        Testverein.TeilnehmerInGruppeEinteilen(Studti, Experte)

        ' Testn
        Assert.AreEqual(5, Testverein.Teilnehmerliste.Count)
        Assert.AreEqual(2, Testverein.EingeteilteTeilnehmer.Count)
        Assert.AreEqual(3, Testverein.FreieTeilnehmer.Count)


    End Sub

End Class
