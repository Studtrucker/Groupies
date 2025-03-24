Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities
Imports Groupies.Services

<TestClass>
Public Class SkiDateienServiceTests

    <TestMethod>
    Public Sub IdentifiziereDateiGenerationTest()
        Dim filelist As List(Of String) = New List(Of String) From {
            "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration3.ski",
            "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration1.ski",
            "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration2.ski"}

        Dim typen = New List(Of String)
        filelist.ForEach(Sub(f) typen.Add(SkiDateienService.IdentifiziereDateiGeneration(f).GetType.FullName))

        Assert.AreEqual(3, typen.Count)
        Assert.AreEqual("Groupies.Entities.Generation3.Club", typen(0))
        Assert.AreEqual("Groupies.Entities.Generation1.Skiclub", typen(1))
        Assert.AreEqual("Groupies.Entities.Generation2.Club", typen(2))
    End Sub


    <TestMethod>
    Public Sub LadenDateiGeneration1_In_AktuelleStruktur()
        Dim Pfad

        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration1.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration1.ski"
        End If

        Dim Club = SkiDateienService.SkiDateiLesen(Pfad)

        Assert.AreEqual(1, Club.Einteilungsliste.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).Sortierung)
        Assert.AreEqual("Club", Club.ClubName)
        Assert.AreEqual("Tag1", Club.Einteilungsliste(0).Benennung)
        Assert.AreEqual(43, Club.Einteilungsliste(0).AlleTeilnehmer.Count)
        Assert.AreEqual(9, Club.Einteilungsliste(0).AlleTrainer.Count)
        Assert.AreEqual(2, Club.Einteilungsliste(0).GruppenloseTrainer.Count)
    End Sub

    <TestMethod>
    Public Sub LadenDateiGeneration2_In_AktuelleStruktur()
        Dim Pfad

        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration2.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiVersion2.ski"
        End If

        Dim Club = SkiDateienService.SkiDateiLesen(Pfad)

        Assert.AreEqual(1, Club.Einteilungsliste.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).Sortierung)
        Assert.AreEqual("Club", Club.ClubName)
        Assert.AreEqual("Tag1", Club.Einteilungsliste(0).Benennung)
        Assert.AreEqual(9, Club.Einteilungsliste(0).Gruppenliste.Count)
        Assert.AreEqual(55, Club.Einteilungsliste(0).EingeteilteTeilnehmer.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(56, Club.Einteilungsliste(0).AlleTeilnehmer.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(9, Club.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(10, Club.Einteilungsliste(0).AlleTrainer.Count)
        Assert.AreEqual(5, Club.Leistungsstufenliste.Count)
    End Sub

    <TestMethod>
    Public Sub LadenDateiGeneration3_In_AktuelleStruktur()
        Dim Pfad

        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration3.ski"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Testdateien\TestdateiGeneration3.ski"
        End If

        Dim Club = SkiDateienService.SkiDateiLesen(Pfad)

        Assert.AreEqual(1, Club.Einteilungsliste.Count)
        Assert.AreEqual(1, Club.Einteilungsliste(0).Sortierung)
        Assert.AreEqual("Club", Club.ClubName)
        Assert.AreEqual("Tag1", Club.Einteilungsliste(0).Benennung)
        Assert.AreEqual(9, Club.Einteilungsliste(0).Gruppenliste.Count)
        Assert.AreEqual(55, Club.Einteilungsliste(0).EingeteilteTeilnehmer.Count)
        Assert.AreEqual(0, Club.Einteilungsliste(0).GruppenloseTeilnehmer.Count)
        Assert.AreEqual(55, Club.Einteilungsliste(0).AlleTeilnehmer.Count)
        Assert.AreEqual(2, Club.Einteilungsliste(0).GruppenloseTrainer.Count)
        Assert.AreEqual(8, Club.Einteilungsliste(0).EingeteilteTrainer.Count)
        Assert.AreEqual(10, Club.Einteilungsliste(0).AlleTrainer.Count)
        Assert.AreEqual(5, Club.Leistungsstufenliste.Count)
    End Sub

End Class
