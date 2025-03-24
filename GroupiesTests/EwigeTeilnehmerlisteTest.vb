Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Controller
Imports Groupies.Controller.AppController
Imports Groupies.Entities
Imports Groupies.Entities.Generation3

<TestClass>
Public Class EwigeTeilnehmerlisteTest
    <TestMethod>
    Public Sub TestTeilnehmerHinzufuegen()

        Dim Stubai24 As New Club

        Dim Studti = New EwigerTeilnehmer(New Teilnehmer("Andreas", "Studtrucker"), "10.11.2023")
        Dim Rene = New EwigerTeilnehmer(New Teilnehmer("Rene", "Werthschütz"), "10.11.2013")
        Dim Ralf = New EwigerTeilnehmer(New Teilnehmer("Ralf", "Granderath"), "05.12.2020")
        Dim Sandra = New EwigerTeilnehmer(New Teilnehmer("Sandra", "Oelschläger"), "10.11.2013")
        Dim Lothar = New EwigerTeilnehmer(New Teilnehmer("Lothar", "Hötger"), "10.11.2023")


        Dim EwigeStubailiste As New EwigeTeilnehmerCollection From {Studti, Rene, Ralf, Sandra, Lothar}

        Assert.AreEqual(5, EwigeStubailiste.Count)

        Assert.AreEqual(2, EwigeStubailiste.OrderByDescending(Function(Tn) Tn.Archivierungsdatum).GroupBy(Function(tn) tn.Archivierungsdatum).OrderByDescending(Function(x) x.Key)(0).Count)
        Assert.AreEqual(1, EwigeStubailiste.OrderByDescending(Function(Tn) Tn.Archivierungsdatum).GroupBy(Function(tn) tn.Archivierungsdatum).OrderByDescending(Function(x) x.Key)(1).Count)
        Assert.AreEqual(2, EwigeStubailiste.OrderByDescending(Function(Tn) Tn.Archivierungsdatum).GroupBy(Function(tn) tn.Archivierungsdatum).OrderByDescending(Function(x) x.Key)(2).Count)

        Rene.Archivierungsdatum = "31.12.2023"

        EwigeStubailiste.Add(Rene)
        Assert.AreEqual(5, EwigeStubailiste.Count)

    End Sub
End Class
