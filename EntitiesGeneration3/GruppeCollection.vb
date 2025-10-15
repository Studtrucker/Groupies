Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation3

    Public Class GruppeCollection
        Inherits ObservableCollection(Of Gruppe)
        Implements IEnumerable(Of Gruppe)
    End Class

End Namespace
