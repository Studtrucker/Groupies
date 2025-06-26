Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Linq

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

        Public Function Sortieren() As EinteilungCollection
            Dim SortedList As New EinteilungCollection(Me.OrderBy(Function(x) x.Sortierung))
            Return SortedList
        End Function

        ''' <summary>
        ''' Benennt eine neue Einteilung
        ''' </summary>
        ''' <returns></returns>
        Private Function BenenneEinteilung() As String

            If Count = 0 Then Return "Tag1"
            If Count = 1 AndAlso Me(0).Benennung Is Nothing Then Me(0).Benennung = "Tag1"

            Dim Tage = ToList.Where(Function(e) e.Benennung.StartsWith("Tag")).OrderByDescending(Function(e) e.Benennung)
            If Tage.Count > 0 Then
                Dim z = Val(Tage(0).Benennung.Last)
                Return $"Tag{z + 1}"
            Else
                Return $"Tag{Count + 1}"
            End If

        End Function

    End Class
End Namespace
