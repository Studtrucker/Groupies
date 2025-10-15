Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation4

    Public Class FaehigkeitCollection
        Inherits ObservableCollection(Of Faehigkeit)
        Implements IEnumerable(Of Faehigkeit)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Faehigkeitenliste As IEnumerable(Of Faehigkeit))
            MyBase.New
            Faehigkeitenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Function Sortieren() As FaehigkeitCollection
            Dim SortedList As New FaehigkeitCollection(Me.OrderBy(Function(x) x.Sortierung))
            Return SortedList
        End Function

    End Class

End Namespace
