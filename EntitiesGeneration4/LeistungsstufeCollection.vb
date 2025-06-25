Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Text

Namespace Entities

    Public Class LeistungsstufeCollection
        Inherits ObservableCollection(Of Leistungsstufe)
        Implements IEnumerable(Of Leistungsstufe)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Leistungsstufenliste As IEnumerable(Of Leistungsstufe))
            MyBase.New
            Leistungsstufenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

    End Class
End Namespace
