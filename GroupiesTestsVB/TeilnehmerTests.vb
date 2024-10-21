Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities
Imports Groupies
Imports Groupies.Controller

<TestClass>
Public Class TeilnehmerTests

    <TestMethod>
    Public Sub TestImportDaten()

        Dim Pfad As String
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        End If

        Dim Teilnehmerliste = ExcelDataReaderService.LeseTeilnehmerAusExcel(Pfad)
        Dim Trainerliste = ExcelDataReaderService.LeseTrainerAusExcel(Pfad)

        Assert.AreEqual(91, Teilnehmerliste.Count)
        Assert.AreEqual(12, Trainerliste.Count)

        'Todo: AppTn erstellen
        AppController.NeuenClubErstellen("Testclub")

        Dim Stephan As New Teilnehmer("Stephan", "Rath")
        Dim Manuela As New Teilnehmer("Manuela", "Ramm")
        Dim Manuel As New Teilnehmer("Manuel", "Adler")
        Dim Julia As New Teilnehmer("Julia", "Crone")
        Dim Jutta As New Teilnehmer("Jutta", "Meier")
        Dim Andrea As New Teilnehmer("Andrea", "Heintz")

        AppController.CurrentClub.GruppenloseTeilnehmer = New TeilnehmerCollection From {Stephan, Manuela, Manuel, Julia, Jutta, Andrea}
        For Each appTn In AppController.CurrentClub.AlleTeilnehmer
            Teilnehmerliste.Where(Function(importTn) appTn.Nachname = importTn.Nachname AndAlso appTn.Vorname = importTn.Vorname).ToList.ForEach(Sub(importTn) importTn.IstBekannt = True)
        Next
        Debug.Print(Teilnehmerliste.Where(Function(Tn) Tn.IstBekannt).Count)

    End Sub



    <TestMethod>
    Public Sub TestTeilnehmerErstellen()

        ' Vor- und Nachname
        Dim Elke As New Teilnehmer("Elke", "Steiner")
        Assert.AreEqual("Elke Steiner", Elke.ToString)
        Assert.AreEqual("Elke Steiner", Elke.VorUndNachname)
        Assert.AreEqual("Elke Steiner, Leistungsstand unbekannt", Elke.AusgabeTrainerinfo)

        ' Vor-, Nachname und Leistungsstufe
        Dim Manu As New Teilnehmer("Manuela", "Ramm", New Leistungsstufe("Könner") With {.Sortierung = 1})
        Assert.AreEqual("Manuela Ramm, Könner", Manu.AusgabeTrainerinfo)
        Assert.AreEqual("Manuela Ramm", Manu.AusgabeTeilnehmerinfo)


        Dim Willi As New Teilnehmer("Willi", "Steiner", New Leistungsstufe("Experte") With {.Sortierung = 2})
        Dim Lothar As New Teilnehmer("Lothar", "Hötger", New Leistungsstufe("Experte") With {.Sortierung = 2})
        Dim Liane As New Teilnehmer("Liane", "Hötger") With {.Leistungsstand = New Leistungsstufe("Anfänger", 0)}

        Dim tnL = New TeilnehmerCollection From {Manu, Willi, Liane}
        tnL.Add(Elke)
        tnL.Add(Lothar)





        CollectionAssert.AreEqual(New List(Of String) From {
                                  "Liane Hötger",
                                  "Lothar Hötger",
                                  "Manuela Ramm",
                                  "Elke Steiner",
                                  "Willi Steiner"
                                  },
                                  tnL.TeilnehmerinfoGeordnet.ToList)


        CollectionAssert.AreEqual(New List(Of String) From {
                                  "Lothar Hötger, Experte",
                                  "Willi Steiner, Experte",
                                  "Manuela Ramm, Könner",
                                  "Liane Hötger, Anfänger",
                                  "Elke Steiner, Leistungsstand unbekannt"
                                  },
                                  tnL.TrainerinfoGeordnet.ToList)

        Dim Anfaenger = New Leistungsstufe("Anfänger", 1)
        Dim Experte = New Leistungsstufe("Experte", 3)

        Dim AndreasStEx = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Studtrucker", .Leistungsstand = Experte}
        Dim MarwinAnf = New Teilnehmer With {.Vorname = "AMarwin", .Nachname = "Studtrucker", .Leistungsstand = Anfaenger}
        Dim StefanAnf = New Teilnehmer With {.Vorname = "Stefan", .Nachname = "Studtrucker", .Leistungsstand = Anfaenger}
        Dim AndreasHEx = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Hötger", .Leistungsstand = Experte}
        Dim AndreasZEx = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Zeisig", .Leistungsstand = Experte}
        Dim FrankAnf = New Teilnehmer With {.Vorname = "Frank", .Nachname = "Hötger", .Leistungsstand = Anfaenger}
        Dim tnl1 = New TeilnehmerCollection From {FrankAnf, MarwinAnf, AndreasHEx, StefanAnf, AndreasZEx, AndreasStEx}

        CollectionAssert.AreEqual(New List(Of Teilnehmer) From {
                                  AndreasHEx,
                                  AndreasStEx,
                                  AndreasZEx,
                                  FrankAnf,
                                  MarwinAnf,
                                  StefanAnf},
                                  tnl1.TeilnehmerMitLeistungsstufeGeordnet.ToList)


    End Sub

    <TestMethod>
    Public Sub TestTeilnehmerlisten()

        Dim Anfaenger = New Leistungsstufe("Anfänger", 1)
        Dim Koenner = New Leistungsstufe("Könner") With {.Sortierung = 2}
        Dim Experte = New Leistungsstufe("Experte") With {.Sortierung = 3}

        Dim Elke As New Teilnehmer("Elke", "Steiner")
        Dim Manu As New Teilnehmer("Manuela", "Ramm", Koenner)
        Dim Willi As New Teilnehmer("Willi", "Sensmeier", Experte)
        Dim Lothar As New Teilnehmer("Lothar", "Hötger", Experte)
        Dim Liane As New Teilnehmer("Liane", "Hötger") With {.Leistungsstand = Anfaenger}

        Dim tnL = New TeilnehmerCollection From {Manu, Willi, Liane}
        tnL.Add(Elke)
        tnL.Add(Lothar)


        CollectionAssert.AreEqual(New List(Of String) From {
                                  "Liane Hötger",
                                  "Lothar Hötger",
                                  "Manuela Ramm",
                                  "Willi Sensmeier",
                                  "Elke Steiner"
                                  },
                                  tnL.TeilnehmerinfoGeordnet.ToList)


        CollectionAssert.AreEqual(New List(Of String) From {
                                  "Lothar Hötger, Experte",
                                  "Willi Sensmeier, Experte",
                                  "Manuela Ramm, Könner",
                                  "Liane Hötger, Anfänger",
                                  "Elke Steiner, Leistungsstand unbekannt"
                                  },
                                  tnL.TrainerinfoGeordnet.ToList)




        Dim AndreasStEx = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Studtrucker", .Leistungsstand = Experte}
        Dim MarwinAnf = New Teilnehmer With {.Vorname = "AMarwin", .Nachname = "Studtrucker", .Leistungsstand = Anfaenger}
        Dim StefanAnf = New Teilnehmer With {.Vorname = "Stefan", .Nachname = "Studtrucker", .Leistungsstand = Anfaenger}
        Dim AndreasHEx = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Hötger", .Leistungsstand = Experte}
        Dim AndreasZEx = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Zeisig", .Leistungsstand = Experte}
        Dim FrankAnf = New Teilnehmer With {.Vorname = "Frank", .Nachname = "Hötger", .Leistungsstand = Anfaenger}
        Dim tnl1 = New TeilnehmerCollection From {FrankAnf, MarwinAnf, AndreasHEx, StefanAnf, AndreasZEx, AndreasStEx}

        CollectionAssert.AreEqual(New List(Of Teilnehmer) From {
                                  AndreasHEx,
                                  AndreasStEx,
                                  AndreasZEx,
                                  FrankAnf,
                                  MarwinAnf,
                                  StefanAnf},
                                  tnl1.TeilnehmerMitLeistungsstufeGeordnet.ToList)
    End Sub

End Class
