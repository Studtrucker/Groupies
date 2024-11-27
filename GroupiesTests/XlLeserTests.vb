Imports Groupies.Services
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports System.Data
Imports System.IO
Imports System.Net.WebRequestMethods


<TestClass>
Public Class XlLeserTests

    <TestMethod>
    Public Sub TestXLSXReaderMitFehler()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Reise_2024_Teilnehmer.xlsx"
        End If

        Assert.ThrowsException(Of InvalidDataException)(Sub() XlLeser.LoadDataSet(Pfad, "Teilnehmer"))

    End Sub

    <TestMethod>
    Public Sub TestXLSXReaderTeilnehmer()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\2024_TeilnehmerBearbeitet.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\2024_TeilnehmerBearbeitet.xlsx"
        End If

        Dim Typ = "Teilnehmer"
        Dim data = XlLeser.LoadDataSet(Pfad, Typ)

        Assert.AreEqual(1, data.Tables.Count)
        Assert.AreEqual(3, data.Tables(data.Tables.IndexOf(Typ)).Columns.Count)
        Assert.AreEqual(91, data.Tables(data.Tables.IndexOf(Typ)).Rows.Count)
        Assert.AreEqual("Beckmann", data.Tables(data.Tables.IndexOf(Typ)).Rows(0).ItemArray(data.Tables(Typ).Columns.IndexOf("Nachname")))

    End Sub

    <TestMethod>
    Public Sub TestXLSXReaderTrainer()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\2024_TrainerBearbeitet.xlsx"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\2024_TrainerBearbeitet.xlsx"
        End If

        Dim Typ = "Trainer"

        Dim data = XlLeser.LoadDataSet(Pfad, Typ)

        Assert.AreEqual(1, data.Tables.Count)
        Assert.AreEqual(3, data.Tables(data.Tables.IndexOf(Typ)).Columns.Count)
        Assert.AreEqual(13, data.Tables(data.Tables.IndexOf(Typ)).Rows.Count)
        'Assert.AreEqual("Reinhard", data.Tables(data.Tables.IndexOf(Typ)).Rows(0).ItemArray(data.Tables(Typ).Columns.IndexOf("Vorname")))
        'Assert.AreEqual("Krön", data.Tables(data.Tables.IndexOf(Typ)).Rows(0).ItemArray(data.Tables(Typ).Columns.IndexOf("Nachname")))

    End Sub

    <TestMethod>
    Public Sub TestXLSReaderTeilnehmer()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\2024_TeilnehmerBearbeitet.xls"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\2024_TeilnehmerBearbeitet.xls"
        End If

        Dim Typ = "Teilnehmer"
        Dim data = XlLeser.LoadDataSet(Pfad, Typ)

        Assert.AreEqual(1, data.Tables.Count)
        Assert.AreEqual(3, data.Tables(data.Tables.IndexOf(Typ)).Columns.Count)
        Assert.AreEqual(91, data.Tables(data.Tables.IndexOf(Typ)).Rows.Count)
        Assert.AreEqual("Beckmann", data.Tables(data.Tables.IndexOf(Typ)).Rows(0).ItemArray(data.Tables(Typ).Columns.IndexOf("Nachname")))

    End Sub

    <TestMethod>
    Public Sub TestCSVReaderTeilnehmer()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Teilnehmer.csv"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Teilnehmer.csv"
        End If

        Try
            Dim Dataset = XlLeser.LoadDataSet(Pfad, "Teilnehmer")
        Catch ex As Exception
            Assert.IsNotNull(ex)
        End Try


    End Sub

    <TestMethod>
    Public Sub TestCSVReaderTrainer()
        Dim Pfad
        If Environment.MachineName = "DESKTOP-JGIR9SQ" Then
            Pfad = "C:\Users\studt_era90oc\OneDrive\Dokumente\Reisen\Stubaital\Trainer.csv"
        Else
            Pfad = "C:\Users\studtan\OneDrive\Dokumente\Reisen\Stubaital\Trainer.csv"
        End If

        Dim Dataset = XlLeser.LoadDataSet(Pfad, "Trainer")

        Dim Tabelle = Dataset.Tables(0)
        Dim SpalteNachname = Dataset.Tables(0).Rows(0).ItemArray(Tabelle.Columns.IndexOf("Nachname"))
        Dim SpalteVorname = Tabelle.Rows(0).ItemArray(Tabelle.Columns.IndexOf("Vorname"))
        If Tabelle Is Nothing Then Exit Sub

        Assert.AreEqual(12, Dataset.Tables(0).Rows.Count)
        Assert.AreEqual("Krön", SpalteNachname)
        Assert.AreEqual("Reinhard", SpalteVorname)

    End Sub

End Class
