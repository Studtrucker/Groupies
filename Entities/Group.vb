Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports PropertyChanged

Namespace Entities

    <DefaultProperty("GroupName")>
    Public Class Group
        Inherits BaseModel

        Public Sub New()
            _GroupID = Guid.NewGuid()
            _GroupMembers = New ParticipantCollection
        End Sub

        Public Property GroupID As Guid

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Gruppenbennung ist ein Pflichtfeld")>
        Public Property GroupNaming As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Printbenennung ist ein Pflichtfeld")>
        Public Property GroupPrintNaming As String


        Public Property GroupLevel As Level


        Public Property GroupLeader As Instructor


        Public Property GroupMembers As ParticipantCollection

        Public Sub AddMember(Teilnehmer As Participant)
            Teilnehmer.MemberOfGroup = GroupID
            _GroupMembers.Add(Teilnehmer)
        End Sub

        Public Sub AddMembers(Teilnehmerliste As ParticipantCollection)
            Teilnehmerliste.ToList.ForEach(Sub(x) _GroupMembers.Add(x))
        End Sub

        Public Sub RemoveMember(Teilnehmer As Participant)
            _GroupMembers.Remove(Teilnehmer)
        End Sub

        Public Sub RemoveMembers(Teilnehmerliste As ParticipantCollection)
            Teilnehmerliste.ToList.ForEach(Sub(x) _GroupMembers.Remove(x))
        End Sub

    End Class

End Namespace
