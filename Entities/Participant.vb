Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel


Namespace Entities

    Public Class Participant
        Inherits BaseModel

        Public Sub New()
            _ParticipantID = Guid.NewGuid()
        End Sub

        Public Property ParticipantID As Guid

        Public Property ParticipantName As String

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property ParticipantFirstname As String

        Public ReadOnly Property ParticipantFullName As String
            Get
                If _ParticipantFirstname Is Nothing Then
                    Return _ParticipantName
                ElseIf _ParticipantName Is Nothing Then
                    Return _ParticipantFirstname
                Else
                    Return String.Format("{0} {1}", _ParticipantFirstname, _ParticipantName)
                End If
            End Get
        End Property

        Public Property ParticipantLevel As Level

        Public Property ParticipantMemberOfGroup As String

    End Class
End Namespace
