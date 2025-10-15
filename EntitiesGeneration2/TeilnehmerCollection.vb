Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities.Generation2

    Public Class TeilnehmerCollection
        Inherits ObservableCollection(Of Teilnehmer)
        Implements IEnumerable(Of Teilnehmer)



    End Class
End Namespace
