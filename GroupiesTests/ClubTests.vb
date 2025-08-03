Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Entities.Generation3

'<TestClass>
Public Class ClubTests
    '<TestMethod>
    Public Sub TestTeilnehmerEingruppierenUndEntfernen()
        Dim Stubai2024 = New Club("Stubaital2024")

        Dim Studti As New Teilnehmer("Andreas", "Studtrucker")
        Dim Manuela As New Teilnehmer("Manuela", "Ramm")
        Dim Lina As New Teilnehmer("Lina", "Hötger")
        Dim Tag1 As New Einteilung
        Stubai2024.Einteilungsliste.Add(Tag1)
        Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Add(Studti)
        Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Add(Manuela)
        Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Add(Lina)

        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(0, Stubai2024.Einteilungsliste(0).EingeteilteTeilnehmer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Count)

        Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen = AppController.AktuellerClub.AlleGruppen

        Stubai2024.Einteilungsliste(0).TeilnehmerInGruppeEinteilen(Studti, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Count)
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(1, Stubai2024.Einteilungsliste(0).EingeteilteTeilnehmer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.Einteilungsliste(0).EingeteilteTeilnehmer.ElementAt(0).VorUndNachname)
        Assert.AreEqual("Manuela Ramm", Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Geordnet.ElementAt(1).VorUndNachname)
        Assert.AreEqual("Lina Hötger", Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Geordnet.ElementAt(0).VorUndNachname)

        Stubai2024.Einteilungsliste(0).TeilnehmerInGruppeEinteilen(Manuela, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Count)
        Assert.AreEqual(1, Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).EingeteilteTeilnehmer.Count)

        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0).Mitgliederliste.Count)
        Assert.AreEqual(0, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(1).Mitgliederliste.Count)

        Stubai2024.Einteilungsliste(0).TeilnehmerAusGruppeEntfernen(Studti, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Count)
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(1, Stubai2024.Einteilungsliste(0).EingeteilteTeilnehmer.Count)

        Stubai2024.Einteilungsliste(0).TeilnehmerAusGruppeEntfernen(Manuela, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0))
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTeilnehmer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(0, Stubai2024.Einteilungsliste(0).EingeteilteTeilnehmer.Count)

    End Sub

    '<TestMethod>
    Public Sub TestTrainerEingruppierenUndEntfernen()
        Dim Stubai2024 = New Club("Stubaital2024")
        Dim Tag1 As New Einteilung
        Stubai2024.Einteilungsliste.Add(Tag1)

        Assert.AreEqual(0, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.Count)


        ' Eine Gruppe wird neu instanziiert
        Dim Experte = New Gruppe("Experte")
        Dim Racer = New Gruppe("Racer")

        Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen = New GruppeCollection() From {Experte, Racer}
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.Count)

        Dim Studti As New Trainer("Andreas", "Studtrucker")
        Dim Manuela As New Trainer("Manuela", "Ramm")
        Dim Lina As New Trainer("Lina", "Hötger")

        Stubai2024.Einteilungsliste(0).TrainerHinzufuegen(Studti)
        Stubai2024.Einteilungsliste(0).TrainerHinzufuegen(Manuela)
        Stubai2024.Einteilungsliste(0).TrainerHinzufuegen(Lina)

        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(0, Stubai2024.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTrainer.Count)

        Stubai2024.Einteilungsliste(0).TrainerEinerGruppeZuweisen(Studti, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0))
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(1, Stubai2024.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTrainer.Count)

        Stubai2024.Einteilungsliste(0).TrainerEinerGruppeZuweisen(Lina, Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(1))
        Assert.AreEqual(1, Stubai2024.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTrainer.Count)

        Assert.AreEqual("Andreas Studtrucker", Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0).Trainer.VorUndNachname)
        Assert.AreEqual("Lina Hötger", Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(1).Trainer.VorUndNachname)
        Assert.AreEqual("Manuela Ramm", Stubai2024.Einteilungsliste(0).GruppenloseTrainer.ElementAt(0).VorUndNachname)

        Stubai2024.Einteilungsliste(0).TrainerAusGruppeEntfernen(Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(0))
        Assert.AreEqual(2, Stubai2024.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(1, Stubai2024.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTrainer.Count)

        Stubai2024.Einteilungsliste(0).TrainerAusGruppeEntfernen(Stubai2024.Einteilungsliste(0).EinteilungAlleGruppen.ElementAt(1))
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(0, Stubai2024.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(3, Stubai2024.Einteilungsliste(0).EinteilungAlleTrainer.Count)

    End Sub

    '<TestMethod>
    Public Sub TestEingeteilteTeilnehmer()
        ' Ein Verein wird neu instanziiert
        Dim Testverein = New Club("Testverein09")
        ' Ein Einteilungstag wird neu instanziiert
        Dim Tag = New Einteilung()
        Testverein.Einteilungsliste.Add(Tag)
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
        Testverein.Einteilungsliste(0).EinteilungAlleGruppen.Add(Experte)
        Testverein.Einteilungsliste(0).EinteilungAlleGruppen.Add(Racer)
        ' fünf Teilnehmer werden der Teilnehmerliste hinzugefügt 
        Testverein.Einteilungsliste(0).GruppenloseTeilnehmer = New TeilnehmerCollection(New List(Of Teilnehmer) From {Studti, Manu, Ralf, Sandra, Rene})
        ' 2 Teilnehmer werden der Gruppe als Mitglieder hinzugefügt
        Testverein.Einteilungsliste(0).TeilnehmerInGruppeEinteilen(Manu, Experte)
        Testverein.Einteilungsliste(0).TeilnehmerInGruppeEinteilen(Studti, Experte)

        ' Test
        Assert.AreEqual(3, Testverein.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(2, Testverein.Einteilungsliste(0).EingeteilteTeilnehmer.Count)
        Assert.AreEqual(5, Testverein.Einteilungsliste(0).EinteilungAlleTeilnehmer.Count)


    End Sub

End Class
