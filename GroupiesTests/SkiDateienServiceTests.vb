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
        filelist.ForEach(Sub(f) typen.Add(IdentifiziereDateiGeneration(f).GetType.FullName))

        Assert.AreEqual(3, typen.Count)
        Assert.AreEqual("Groupies.Entities.AktuelleVersion.Club", typen(0))
        Assert.AreEqual("Groupies.Entities.Generation1.Skiclub", typen(1))
        Assert.AreEqual("Groupies.Entities.Generation2.Club", typen(2))
    End Sub

    '<TestMethod>
    'Public Sub TestTrainerlisten()
    '    Dim t1 = New Trainer("Ralf")
    '    Dim t2 = New Trainer("Sandra", "Witzel")
    '    Dim t3 = New Trainer("Andreas", "Studtrucker", "Studti")
    '    Dim l = New List(Of Trainer) From {t2, t1, t3}
    '    Dim trl = New TrainerCollection(l)

    '    CollectionAssert.AreEqual(New List(Of String) From {"Ralf", "Andreas Studtrucker", "Sandra Witzel"}, trl.VorUndNachname.ToList)

    'End Sub


End Class
