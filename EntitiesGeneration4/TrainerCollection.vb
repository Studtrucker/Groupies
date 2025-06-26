Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class TrainerCollection
        Inherits ObservableCollection(Of Trainer)
        Implements IEnumerable(Of Trainer)


        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Trainerliste As IEnumerable(Of Trainer))
            MyBase.New
            Trainerliste.ToList.ForEach(Sub(T) Add(T))
        End Sub

        Public Sub RemoveByTrainerID(TrainerID As Guid)
            Dim Trainers = Where(Function(Tr) Tr.TrainerID = TrainerID)
            Trainers.ToList.ForEach(Sub(Tr) Remove(Tr))
        End Sub

        Public Function Sortieren() As TrainerCollection
            Dim SortedList As New TrainerCollection(Me.OrderBy(Function(x) x.Spitzname))
            Return SortedList
        End Function

    End Class
End Namespace
