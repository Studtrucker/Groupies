
Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation4

    Public Class GruppenstammCollection
        Inherits ObservableCollection(Of Gruppenstamm)
        Implements IEnumerable(Of Gruppenstamm)
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Gruppenliste As IEnumerable(Of Gruppenstamm))
            MyBase.New
            Gruppenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

#Region "Funktionen und Methoden"

#End Region

    End Class



End Namespace