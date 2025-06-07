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

        Public Property BenennungGruppeneinteilung As String

        Public ReadOnly Property GruppenListeSortierungNachName As ObservableCollection(Of Gruppe)
            Get
                Return New ObservableCollection(Of Gruppe)(Me.OrderBy(Of String)(Function(x) x.Alias))
            End Get
        End Property

        Public ReadOnly Property BenennungGeordnet As IEnumerable(Of String) =
            OrderBy(Function(G) G.Sortierung) _
            .ThenBy(Function(G) G.Alias) _
            .Select(Function(G) G.Alias)

        Public ReadOnly Property GruppeGeordnet As IEnumerable(Of Gruppe) =
            OrderBy(Function(G) G.Sortierung) _
            .ThenBy(Function(G) G.Alias)

#Region "Funktionen und Methoden"
        Public Overloads Sub AddRange(Gruppenliste As IEnumerable(Of Gruppe))
            For Each Gruppe As Gruppe In Gruppenliste
                Add(Gruppe)
            Next
        End Sub

#End Region

    End Class



End Namespace
