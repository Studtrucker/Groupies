Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class TeilnehmerCollection
        Inherits ObservableCollection(Of Teilnehmer)
        Implements IEnumerable


        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Teilnehmerliste As IEnumerable(Of Teilnehmer))
            MyBase.New
            Teilnehmerliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public ReadOnly Property ParticipantCollectionOrdered As TeilnehmerCollection
            Get
                Dim Ordered = New TeilnehmerCollection
                Me.OrderBy(Of String)(Function(x) x.VorUndNachname).ToList.ForEach(Sub(x) Ordered.Add(x))
                Return Ordered
            End Get
        End Property


        Public ReadOnly Property SortierteListe
            Get
                Dim ersteSortierung = Me.OrderBy(Function(x) x.Nachname)
                Dim zweiteSortierung = ersteSortierung.OrderBy(Function(x) x.Vorname)
                Return zweiteSortierung
            End Get
        End Property


        ''' <summary>
        ''' Die Teilnehmerliste geordnet nach Nachname, Vorname
        ''' </summary>
        ''' <returns></returns>
        Public Property GeordneterTextNachnameVorname As IEnumerable(Of String) =
            OrderBy(Function(Tn) Tn.Nachname) _
            .ThenBy(Function(Tn) Tn.Vorname) _
            .Select(Function(Tn) $"{Tn.VorUndNachname}")

        Public Property GeordnetTeilnehmerNachnameVorname As IEnumerable(Of Teilnehmer) =
            OrderBy(Function(Tn) Tn.Nachname) _
            .ThenBy(Function(Tn) Tn.Vorname)

        ''' <summary>
        ''' Die Teilnehmerliste geordnet nach 
        ''' der absteigenden Sortierkennzahl 
        ''' aus der Teilnehmer Leistungsstufe, 
        ''' dann aufsteigend Nachname, Vorname 
        ''' </summary>
        ''' <returns></returns>
        Public Property GeordnetLeistungsstufeNachnameVorname As IEnumerable(Of Teilnehmer) =
            OrderByDescending(Function(o) o.Leistungsstand.Sortierung) _
            .ThenBy(Function(o) o.Nachname) _
            .ThenBy(Function(o) o.Vorname)

        Public Property GruppeLeistungNachnameVorname =
            OrderBy(Function(TN) TN.Nachname) _
            .ThenBy(Function(TN) TN.Vorname) _
            .GroupBy(Function(TN) TN.Leistungsstand.Sortierung) _
            .OrderByDescending(Function(TN) TN.Key) _
            .Select(Function(TNG) TNG.ToList.Select(Function(Tt) Tt))


    End Class
End Namespace
