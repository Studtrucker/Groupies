Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities.Generation3

    Public Class Faehigkeit

#Region "Properties"
        Public Property FaehigkeitID As Guid
        Public Property Benennung As String
        Public Property Beschreibung As String
        Public Property Sortierung As Integer
        Public ReadOnly Property AusgabeAnTrainerInfo As String

#End Region
    End Class
End Namespace
