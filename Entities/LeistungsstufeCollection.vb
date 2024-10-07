Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class LeistungsstufeCollection
        Inherits ObservableCollection(Of Leistungsstufe)
        Implements IEnumerable(Of Leistungsstufe)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Leistungsstufenliste As IEnumerable(Of Leistungsstufe))
            MyBase.New
            Leistungsstufenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Property BenennungGeordnet As IEnumerable(Of String) =
            OrderBy(Function(L) L.Sortierung) _
            .ThenBy(Function(L) L.Benennung) _
            .Select(Function(L) L.Benennung)

        Public Property LeistungsstufeGeordnet As IEnumerable(Of Leistungsstufe) =
            OrderBy(Function(L) L.Sortierung) _
            .ThenBy(Function(L) L.Benennung)

    End Class
End Namespace
