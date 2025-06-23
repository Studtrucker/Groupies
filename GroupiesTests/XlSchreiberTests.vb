Imports Groupies
Imports Groupies.Entities
Imports Groupies.Controller
Imports Groupies.Entities.Generation4

<TestClass>
Public Class XlSchreiberTests
    '<TestMethod>
    Public Sub TestXlExport()
        Dim xl = New XLSchreiber
        Dim Stephan As New Teilnehmer("Stephan", "Rath")
        Dim Manuela As New Teilnehmer("Manuela", "Ramm")
        Dim Manuel As New Teilnehmer("Manuel", "Adler")
        Dim Julia As New Teilnehmer("Julia", "Crone")
        Dim Jutta As New Teilnehmer("Jutta", "Meier")
        Dim Andrea As New Teilnehmer("Andrea", "Heintz")

        AppController.AktuellerClub = New Club("Stuabi2024")
        AppController.AktuellerClub.AlleEinteilungen(0).GruppenloseTeilnehmer = New TeilnehmerCollection From {Stephan, Manuela, Manuel, Julia, Jutta, Andrea}

        xl.ExportDatenAlsXl(".xlsx", "Teilnehmer")
    End Sub
End Class
