Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities.Generation2

    Public Class LeistungsstufeCollection
        Inherits ObservableCollection(Of Leistungsstufe)
        Implements IEnumerable(Of Leistungsstufe)

    End Class
End Namespace
