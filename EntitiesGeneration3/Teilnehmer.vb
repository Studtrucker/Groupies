Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports Groupies.Controller


Namespace Entities.Generation3


    Public Class Teilnehmer


#Region "Properties"

        Public Property TeilnehmerID As Guid
        Public Property Nachname As String
        Public Property Geburtsdatum As Date
        Public Property Vorname As String
        Public Property Leistungsstand As Leistungsstufe
        Public Property Archivieren As Boolean

#End Region

    End Class
End Namespace
