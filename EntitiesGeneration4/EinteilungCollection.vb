Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Linq

Namespace Entities.Generation4

    Public Class EinteilungCollection
        Inherits ObservableCollection(Of Einteilung)
        Implements IEnumerable(Of Einteilung)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Einteilungsliste As IEnumerable(Of Einteilung))
            MyBase.New
            Einteilungsliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Function Sortieren() As EinteilungCollection
            Dim SortedList As New EinteilungCollection(Me.OrderBy(Function(x) x.Sortierung))
            Return SortedList
        End Function

    End Class

End Namespace
