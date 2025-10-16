Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities.Generation4

'<TestClass>
Public Class TrainerTests

    '<TestMethod>
    Public Sub TestTrainerErstellen()
        Dim t1 = New Trainer("Ralf")
        Assert.AreEqual("Ralf", t1.VorNachnameAlias)
        Dim t2 = New Trainer("Sandra", "Oelschläger")
        Assert.AreEqual("Sandra Oelschläger", t2.VorNachnameAlias)
        Dim t3 = New Trainer("Andreas", "Studtrucker", "Studti")
        Assert.AreEqual("Studti", t3.VorNachnameAlias)
    End Sub

    '<TestMethod>
    Public Sub TestTrainerlisten()
        Dim t1 = New Trainer("Ralf")
        Dim t2 = New Trainer("Sandra", "Witzel")
        Dim t3 = New Trainer("Andreas", "Studtrucker", "Studti")
        Dim l = New List(Of Trainer) From {t2, t1, t3}
        Dim trl = New TrainerCollection(l)

        CollectionAssert.AreEqual(New List(Of String) From {"Ralf", "Andreas Studtrucker", "Sandra Witzel"}, trl.Select(Of String)(Function(T) T.VorNachnameAlias).ToList)

    End Sub


End Class
