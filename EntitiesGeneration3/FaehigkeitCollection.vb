Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation3

    Public Class FaehigkeitCollection
        Inherits ObservableCollection(Of Faehigkeit)
        Implements IEnumerable(Of Faehigkeit)

    End Class

End Namespace
