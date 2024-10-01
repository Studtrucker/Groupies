Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class TeilnehmerCollection
        Inherits ObservableCollection(Of Teilnehmer)
        Implements IEnumerable

        Public Sub New()
            MyBase.New
        End Sub

        <Obsolete>
        Public Sub New(Teilnehmerliste As List(Of Teilnehmer))
            MyBase.New
            Teilnehmerliste.ForEach(Sub(x) Add(x))
        End Sub

        Public ReadOnly Property ParticipantCollectionOrdered As TeilnehmerCollection
            Get
                Dim Ordered = New TeilnehmerCollection
                Me.OrderBy(Of String)(Function(x) x.VorUndNachname).ToList.ForEach(Sub(x) Ordered.Add(x))
                Return Ordered
            End Get
        End Property

        Public ReadOnly Property NotInAGroup As TeilnehmerCollection
            Get
                Dim List = New TeilnehmerCollection
                Me.Where(Function(x) x.IsNotInGroup).ToList.ForEach(Sub(item) List.Add(item))
                Return List
            End Get
        End Property

        Public Property Teilnehmerliste = Me.Select(Function(t) t.VorUndNachname)

        Public Property GeordnetNachNachnameVorname = Me.OrderBy(Function(t) t.Nachname).OrderBy(Function(t) t.Vorname).Select(Function(t) t.VorUndNachname)
        Public Property GeordnetNachLeistungsstufeNachnameVorname = OrderBy(Function(t) t.Leistungsstand.Sortierung).OrderBy(Function(t) t.Nachname).OrderBy(Function(t) t.Vorname).Select(Function(t) t.VorUndNachname)



        '    Exits
        '    .OrderBy(KeyValuePair >= (Int())KeyValuePair.Key)
        '.OrderBy(KeyValuePair >= Math.Abs((Int())KeyValuePair.Key))
        '.Select(KeyValuePair >= $"the {KeyValuePair.Value} is {DescribeDirection(KeyValuePair.Key)}");

        Public Overrides Function ToString() As String

            '            Dim Namen As String = Me.ToList.OrderBy(Function(T) T.ParticipantLastName).OrderBy(Function(T) T.ParticipantFirstName).Select(Of String)(Function(TN) String.Format("{0}{1}", TN.ParticipantFullName, vbCrLf))
            Dim Namen = Me.ToList.Select(Of String)(Function(TN) TN.VorUndNachname)
            Return Namen.ToString
        End Function

    End Class
End Namespace
