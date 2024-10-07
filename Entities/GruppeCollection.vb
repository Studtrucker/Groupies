Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class GruppeCollection
        Inherits ObservableCollection(Of Gruppe)
        Implements IEnumerable(Of Gruppe)
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Gruppenliste As IEnumerable(Of Gruppe))
            MyBase.New
            Gruppenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public ReadOnly Property GruppenListeSortierungNachName As ObservableCollection(Of Gruppe)
            Get
                Return New ObservableCollection(Of Gruppe)(Me.OrderBy(Of String)(Function(x) x.Benennung))
            End Get
        End Property

        Public ReadOnly Property BenennungGeordnet As IEnumerable(Of String) =
            OrderBy(Function(G) G.Sortierung) _
            .ThenBy(Function(G) G.Benennung) _
            .Select(Function(G) G.Benennung)

        Public ReadOnly Property GruppeGeordnet As IEnumerable(Of Gruppe) =
            OrderBy(Function(G) G.Sortierung) _
            .ThenBy(Function(G) G.Benennung)

    End Class
End Namespace
