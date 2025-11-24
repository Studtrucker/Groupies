Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation4

    Public Class GruppeCollection
        Inherits ObservableCollection(Of Gruppe)
        Implements IEnumerable(Of Gruppe)
        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Gruppenliste As IEnumerable(Of Gruppe))
            MyBase.New
            Gruppenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public ReadOnly Property GruppenListeOrderByName As GruppeCollection
            Get
                Return New GruppeCollection(Me.OrderBy(Of String)(Function(x) x.Benennung))
            End Get
        End Property

        Public ReadOnly Property GruppenListeOrderByNumber As GruppeCollection
            Get
                Return New GruppeCollection(Me.OrderBy(Of Integer)(Function(x) x.Sortierung))
            End Get
        End Property

#Region "Funktionen und Methoden"

#End Region

    End Class



End Namespace
