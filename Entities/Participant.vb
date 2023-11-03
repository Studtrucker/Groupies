Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel


Namespace Entities

    <DefaultBindingProperty("ParticipantFirstname")>
    <DefaultProperty("ParticipantFullName")>
    Public Class Participant
        Inherits BaseModel

        Public Sub New()
            _ParticipantID = Guid.NewGuid()
        End Sub

        Public Property ParticipantID As Guid

        Public Property ParticipantLastName As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property ParticipantFirstName As String

        Public ReadOnly Property ParticipantFullName As String
            Get
                If _ParticipantFirstName Is Nothing Then
                    Return _ParticipantLastName
                ElseIf _ParticipantLastName Is Nothing Then
                    Return _ParticipantFirstName
                Else
                    Return String.Format("{0} {1}", _ParticipantFirstName, _ParticipantLastName)
                End If
            End Get
        End Property


        Public Property ParticipantLevel As Level

        Public Property MemberOfGroup As Group

    End Class
End Namespace
