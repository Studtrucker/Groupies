Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class ParticipantCollection
        Inherits ObservableCollection(Of Participant)
        Implements IEnumerable

        Public ReadOnly Property ParticipantCollectionOrdered As ParticipantCollection
            Get
                Dim Ordered = New ParticipantCollection
                Me.OrderBy(Of String)(Function(x) x.ParticipantFullName).ToList.ForEach(Sub(x) Ordered.Add(x))
                Return Ordered
            End Get
        End Property

    End Class
End Namespace
