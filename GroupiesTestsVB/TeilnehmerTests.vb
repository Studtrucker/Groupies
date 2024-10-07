Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies.Entities

<TestClass>
Public Class TeilnehmerTests

    <TestMethod>
    Public Sub TestTeilnehmerErstellen()
        Dim Elke As New Teilnehmer("Elke", "Steiner")
        Assert.AreEqual("Elke Steiner", Elke.ToString)
        Assert.AreEqual("Elke Steiner", Elke.VorUndNachname)
        Assert.AreEqual("Elke Steiner, Leistungsstand unbekannt", Elke.AusgabeTrainerinfo)
        Dim Manu As New Teilnehmer("Manuela", "Steiner", New Leistungsstufe("Könner") With {.Sortierung = 1})
        Assert.AreEqual("Manuela Steiner, Könner", Manu.AusgabeTrainerinfo)
        Assert.AreEqual("Manuela Steiner", Manu.AusgabeTeilnehmerinfo)

        Dim Willi As New Teilnehmer("Willi", "Steiner", New Leistungsstufe("Experte") With {.Sortierung = 2})
        Dim Lothar As New Teilnehmer("Lothar", "Hötger", New Leistungsstufe("Experte") With {.Sortierung = 2})
        Dim Liane As New Teilnehmer("Liane", "Hötger")
        Liane.Leistungsstand = New Leistungsstufe("Anfänger", 4)

        Dim tnL = New TeilnehmerCollection From {Manu, Willi, Liane}
        tnL.Add(Elke)
        tnL.Add(Lothar)

        Dim x = tnL
        Dim y = tnL.GeordnetLeistungsstufeNachnameVorname
        Dim z = tnL.GeordnetNachnameVorname


        Debug.Print(Environment.NewLine)
        Debug.Print("ungeord")
        For Each item As Teilnehmer In x
            Debug.Print(item.AusgabeTrainerinfo)
        Next
        Debug.Print(Environment.NewLine)
        Debug.Print("Leist, Nachname, Vorn")
        For Each item As Teilnehmer In y
            Debug.Print(item.AusgabeTrainerinfo & vbTab & item.Leistungsstand.Sortierung)
        Next
        Debug.Print(Environment.NewLine)
        Debug.Print($"Nachname, Vorname")
        For Each item In z
            Debug.Print(item)
            'Debug.Print(item.AusgabeTrainerinfo & vbTab & item.Leistungsstand.Sortierung)
        Next

        Debug.Print(Environment.NewLine)
        Debug.Print("Gruppen")

        Dim Anfaenger = New Leistungsstufe("Anfänger", 1)
        Dim Experte = New Leistungsstufe("Experte", 3)

        Dim Andreas = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Studtrucker", .Leistungsstand = Experte}
        Dim Marwin = New Teilnehmer With {.Vorname = "AMarwin", .Nachname = "Studtrucker", .Leistungsstand = Anfaenger}
        Dim Stefan = New Teilnehmer With {.Vorname = "Stefan", .Nachname = "Studtrucker", .Leistungsstand = Anfaenger}
        Dim Andreas1 = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Hötger", .Leistungsstand = Experte}
        Dim Andreas2 = New Teilnehmer With {.Vorname = "Andreas", .Nachname = "Zeisig", .Leistungsstand = Experte}
        Dim Frank = New Teilnehmer With {.Vorname = "Frank", .Nachname = "Hötger", .Leistungsstand = Anfaenger}
        Dim tnl1 = New TeilnehmerCollection From {Frank, Marwin, Andreas1, Stefan, Andreas2, Andreas}

        For Each item In tnl1.GruppeLeistungNachnameVorname
            For Each it As Teilnehmer In item
                Debug.Print(it.AusgabeTrainerinfo)
            Next
        Next

        ' Andreas Studtrucker, Experte
        ' Marwin Studtrucker, Anfänger
        ' Andreas Hötger, Experte

        ' 1. Nachname
        ' 2. Vorname

        'tnl1.GruppeLeistungNachnameVorname.tolist.foreach(Function())


    End Sub
End Class
