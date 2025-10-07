Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

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

        Public Property BenennungGruppeneinteilung As String

        Public ReadOnly Property GruppenListeSortierungNachName As ObservableCollection(Of Gruppe)
            Get
                Return New ObservableCollection(Of Gruppe)(Me.OrderBy(Of String)(Function(x) x.Benennung))
            End Get
        End Property

        Friend Sub KorrekturLeistungsstufen(AlleLeistungsstufen As LeistungsstufeCollection)
            Me.ToList.ForEach(Sub(Gr) Gr.Leistungsstufe = AlleLeistungsstufen.Where(Function(LS) LS.Benennung = Gr.Leistungsstufe.Benennung).Single)
        End Sub

#Region "Funktionen und Methoden"


        Public Function Sortieren() As GruppeCollection
            Dim SortedList As New GruppeCollection(Me.OrderBy(Function(x) x.Sortierung))
            Return SortedList
        End Function

#End Region

    End Class



End Namespace
