Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports Groupies.Controller

Namespace Entities.Generation2

    Public Class Gruppe

#Region "Properties"

        Public Property GruppenID As Guid
        Public Property Benennung As String
        Public Property AusgabeTeilnehmerInfo As String
        Public Property Sortierung As Integer
        Public Property Leistungsstufe As Leistungsstufe
        Public Property Trainer As Trainer
        Public Property Mitgliederliste As List(Of Teilnehmer)

#End Region


    End Class

End Namespace
