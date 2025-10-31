Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation4

    Public Class GruppeVariabelCollection
        Inherits ObservableCollection(Of GruppeVariable)
        Implements IEnumerable(Of GruppeVariable)
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Gruppenliste As IEnumerable(Of GruppeVariable))
            MyBase.New
            Gruppenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub


#Region "Funktionen und Methoden"

#End Region

    End Class



End Namespace
