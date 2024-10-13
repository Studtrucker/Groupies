Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Controller
Imports Groupies.Controller.AppController
Imports Groupies.Entities
<TestClass>
Public Class EwigeTeilnehmerlisteTest
    <TestMethod>
    Public Sub TestTeilnehmerHinzufuegen()

        Dim Stubai24 As New Club

        Dim Studti = New EwigerTeilnehmer("Andreas", "Studtrucker")
        Dim Rene = New EwigerTeilnehmer("Rene", "Werthschütz")
        Dim Ralf = New EwigerTeilnehmer("Ralf", "Granderath")
        Dim Sandra = New EwigerTeilnehmer("Sandra", "Oelschläger")
        Dim Lothar = New EwigerTeilnehmer("Lothar", "Hötger")

        Studti.ZuletztTeilgenommen = "10.11.2023"
        Rene.ZuletztTeilgenommen = "10.11.2013"
        Ralf.ZuletztTeilgenommen = "05.12.2020"
        Sandra.ZuletztTeilgenommen = "10.11.2013"
        Lothar.ZuletztTeilgenommen = "10.11.2023"

        Dim EwigeStubailiste As New EwigeTeilnehmerCollection From {Studti, Rene, Ralf, Sandra, Lothar}

        Assert.AreEqual(5, EwigeStubailiste.Count)

        Assert.AreEqual(2, EwigeStubailiste.OrderByDescending(Function(Tn) Tn.ZuletztTeilgenommen).GroupBy(Function(tn) tn.ZuletztTeilgenommen).OrderByDescending(Function(x) x.Key)(0).Count)
        Assert.AreEqual(1, EwigeStubailiste.OrderByDescending(Function(Tn) Tn.ZuletztTeilgenommen).GroupBy(Function(tn) tn.ZuletztTeilgenommen).OrderByDescending(Function(x) x.Key)(1).Count)
        Assert.AreEqual(2, EwigeStubailiste.OrderByDescending(Function(Tn) Tn.ZuletztTeilgenommen).GroupBy(Function(tn) tn.ZuletztTeilgenommen).OrderByDescending(Function(x) x.Key)(2).Count)

        Rene.ZuletztTeilgenommen = "31.12.2023"

        EwigeStubailiste.Add(Rene)
        Assert.AreEqual(5, EwigeStubailiste.Count)

    End Sub
End Class
