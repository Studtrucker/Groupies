Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports CDS = Groupies.Controller.AppController

Namespace Entities.Generation1


    Public Class Participant
        Inherits BaseModel

        Public Event ChangeGroup(Participant As Participant)

        Public Sub New()
            _ParticipantID = Guid.NewGuid()
        End Sub

        Public Property ParticipantID As Guid

        Public Property ParticipantLastName As String

        Public Property ParticipantFirstName As String

        Public Property ParticipantLevel As Level

    End Class
End Namespace
