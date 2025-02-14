Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class EinteilungCollection
        Inherits ObservableCollection(Of Einteilung)
        Implements IEnumerable(Of Einteilung)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Einteilungsliste As IEnumerable(Of Einteilung))
            MyBase.New
            Einteilungsliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Property BenennungGruppeneinteilung As String

    End Class
End Namespace
