Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class GruppeCollection
        Inherits ObservableCollection(Of Gruppe)

        Public ReadOnly Property GruppenListeSortierungNachName As ObservableCollection(Of Gruppe)
            Get
                Return New ObservableCollection(Of Gruppe)(Me.OrderBy(Of String)(Function(x) x.Benennung))
            End Get
        End Property

    End Class
End Namespace
