Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities.Generation4

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

        Public Function Sortieren() As LeistungsstufeCollection
            Dim SortedList As New LeistungsstufeCollection(Me.OrderBy(Function(x) x.Sortierung))
            Return SortedList
        End Function

    End Class
End Namespace
