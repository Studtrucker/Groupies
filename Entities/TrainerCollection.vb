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

        ''' <summary>
        ''' Liste verfügbare Trainer
        ''' Geordnet:
        ''' Nachname
        ''' Vorname
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property VorUndNachnameVerfuegbar As IEnumerable(Of String) =
            Where(Function(Tr) Tr.IstEinerGruppeZugewiesen = False) _
            .OrderBy(Function(Tr) Tr.Nachname) _
            .ThenBy(Function(Tr) Tr.Vorname) _
            .Select(Function(Tr) Tr.VorUndNachname)

        ''' <summary>
        ''' Liste aller Trainer
        ''' Geordnet:
        ''' Nachname
        ''' Vorname
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property VorUndNachname As IEnumerable(Of String) =
            OrderBy(Function(Tr) Tr.Nachname) _
            .ThenBy(Function(Tr) Tr.Vorname) _
            .Select(Function(Tr) Tr.VorUndNachname)

    End Class
End Namespace
