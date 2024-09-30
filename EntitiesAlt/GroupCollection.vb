Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Veraltert

    Public Class GroupCollection
        Inherits ObservableCollection(Of Group)

        Public ReadOnly Property SortedListGroupNaming As ObservableCollection(Of Group)
            Get
                Return New ObservableCollection(Of Group)(Me.OrderBy(Of String)(Function(x) x.GroupNaming))
            End Get
        End Property

    End Class
End Namespace
