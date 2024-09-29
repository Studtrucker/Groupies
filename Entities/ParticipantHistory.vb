Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    Public Class ParticipantHistory
        Inherits BaseModel

        Public Property Teilnehmer As Entities.Teilnehmer

        Public Property Eintrag As String

        Public Property Hashtag As String

        Public Property EintragVom As Date


    End Class

End Namespace
