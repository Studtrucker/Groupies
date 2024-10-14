Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class EwigeTeilnehmerCollection
        Inherits ObservableCollection(Of EwigerTeilnehmer)
        Implements IEnumerable(Of EwigerTeilnehmer)

#Region "Konstruktor"
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Teilnehmerliste As IEnumerable(Of EwigerTeilnehmer))
            MyBase.New
            Teilnehmerliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Sub New(Teilnehmer As Teilnehmer, Datum As Date)
            Add(New EwigerTeilnehmer(Teilnehmer, Datum))
        End Sub

        Public Sub New(Teilnehmer As Teilnehmer, Datum As Date, LetzteGruppenmitgliedschaft As Guid)
            Add(New EwigerTeilnehmer(Teilnehmer, Datum))
        End Sub

#End Region

#Region "Porperties IEnumerable(String) Rückgabe"


#End Region

#Region "Porperties IEnumerable(Teilnehmer) Rückgabe"

#End Region

#Region "Funktionen und Methoden"

        Public Overloads Sub Add(Teilnehmer As EwigerTeilnehmer)
            If Me.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID).Count = 1 Then
                Me.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID).Single.ZuletztTeilgenommen = Teilnehmer.ZuletztTeilgenommen
            Else
                MyBase.Add(Teilnehmer)
            End If

        End Sub

        Public Overloads Sub Add(Teilnehmer As Teilnehmer, Datum As Date)
            If Me.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID).Count = 1 Then
                Me.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID).Single.ZuletztTeilgenommen = Datum
            Else
                MyBase.Add(New EwigerTeilnehmer(Teilnehmer, Datum))
            End If

        End Sub

        Public Overloads Sub Add(Teilnehmer As Teilnehmer, Datum As Date, GruppenID As Guid)
            If Me.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID).Count = 1 Then
                Me.Where(Function(Tn) Tn.TeilnehmerID = Teilnehmer.TeilnehmerID).Single.ZuletztTeilgenommen = Datum
            Else
                MyBase.Add(New EwigerTeilnehmer(Teilnehmer, Datum))
            End If

        End Sub

#End Region

    End Class
End Namespace
