Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class TrainerTests

    <TestMethod>
    Public Sub TestTrainerErstellen()
        Dim t1 = New Trainer("Ralf")
        Assert.AreEqual("Ralf", t1.Ausgabename)
        Dim t2 = New Trainer("Sandra", "Oelschläger")
        Assert.AreEqual("Sandra Oe.", t2.Ausgabename)
        Dim t3 = New Trainer("Andreas", "Studtrucker", "Studti")
        Assert.AreEqual("Studti", t3.Ausgabename)
    End Sub
End Class
