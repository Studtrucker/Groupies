Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class TeilnehmerTests

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
