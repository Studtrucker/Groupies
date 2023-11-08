Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports CDS = Skiclub.Services.CurrentDataService


Namespace Entities

    <DefaultBindingProperty("ParticipantFirstname")>
    <DefaultProperty("ParticipantFullName")>
    Public Class Participant
        Inherits BaseModel

        Public Event ChangeGroup(Participant As Participant)

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

        Public Property MemberOfGroup As Guid

        Public ReadOnly Property MemberOfGroup_Naming() As String
            Get
                Return CDS.Skiclub.Grouplist.Where(Function(x) x.GroupID.Equals(MemberOfGroup)).DefaultIfEmpty(New Group With {.GroupNaming = String.Empty}).Single.GroupNaming
            End Get
        End Property

    End Class
End Namespace
