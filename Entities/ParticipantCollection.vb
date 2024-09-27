Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class ParticipantCollection
        Inherits ObservableCollection(Of Participant)
        Implements IEnumerable

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Teilnehmerliste As List(Of Participant))
            MyBase.New
            Teilnehmerliste.ForEach(Sub(x) Add(x))
        End Sub

        Public ReadOnly Property ParticipantCollectionOrdered As ParticipantCollection
            Get
                Dim Ordered = New ParticipantCollection
                Me.OrderBy(Of String)(Function(x) x.ParticipantFullName).ToList.ForEach(Sub(x) Ordered.Add(x))
                Return Ordered
            End Get
        End Property

        Public ReadOnly Property NotInAGroup As ParticipantCollection
            Get
                Dim List = New ParticipantCollection
                Me.Where(Function(x) x.IsNotInGroup).ToList.ForEach(Sub(item) List.Add(item))
                Return List
            End Get
        End Property

    End Class
End Namespace
