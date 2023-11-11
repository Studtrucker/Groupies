Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class LevelCollection
        Inherits ObservableCollection(Of Level)

        'Public Sub New()
        '    Me.Add(New Level(False) With {.LevelNaming = String.Empty, .SortNumber = "000"})
        'End Sub

        Public ReadOnly Property SortedList As ObservableCollection(Of Level)
            Get
                Return New ObservableCollection(Of Level)(Me.OrderBy(Of String)(Function(x) x.SortNumber))
            End Get
        End Property

    End Class
End Namespace
