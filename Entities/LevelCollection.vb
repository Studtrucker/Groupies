Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class LevelCollection
        Inherits ObservableCollection(Of Level)

        'Public ReadOnly Property SortedList As LevelCollection
        '    Get
        '        Dim Sorted = New LevelCollection
        '        OrderBy(Of String)(Function(x) x.SortNumber).ToList.ForEach(Sub(i) Sorted.Add(i))
        '        Return Sorted
        '    End Get
        'End Property
        'Public ReadOnly Property SortedListDisplayables As LevelCollection
        '    Get
        '        Dim Sorted = New LevelCollection
        '        Me.Where(Function(d) d.SaveAndShow = True).OrderBy(Of String)(Function(x) x.SortNumber).ToList.ForEach(Sub(i) Sorted.Add(i))
        '        Return Sorted
        '    End Get
        'End Property

    End Class
End Namespace
