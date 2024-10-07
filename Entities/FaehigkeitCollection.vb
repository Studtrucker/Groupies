Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities

    Public Class FaehigkeitCollection
        Inherits ObservableCollection(Of Faehigkeit)
        Implements IEnumerable(Of Faehigkeit)

        Public Sub New()
            MyBase.New
        End Sub

        Public Sub New(Faehigkeitenliste As IEnumerable(Of Faehigkeit))
            MyBase.New
            Faehigkeitenliste.ToList.ForEach(Sub(x) Add(x))
        End Sub

        Public Property TrainerInfoGeordnet As IEnumerable(Of String) =
            OrderBy(Function(f) f.Sortierung) _
            .ThenBy(Function(f) f.Benennung) _
            .Select(Function(f) f.AusgabeAnTrainerInfo)

        Public Property FaehigkeitGeordnet As IEnumerable(Of Faehigkeit) =
            OrderBy(Function(f) f.Sortierung)

    End Class

End Namespace
