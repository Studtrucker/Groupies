Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports PropertyChanged

Namespace Entities
    Public Class Group
        Inherits BaseModel

        Public Sub New()
            _groupID = Guid.NewGuid()
            _groupmembers = New ParticipantCollection
        End Sub

        Public Property GroupID As Guid

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Gruppenname ist ein Pflichtfeld")>
        Public Property GroupName As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Printname ist ein Pflichtfeld")>
        Public Property GroupPrintName As String


        Public Property Grouplevel As Level


        Public Property Groupleader As Instructor


        Public Property Groupmembers As ParticipantCollection

        Public Sub AddMember(Teilnehmer As Participant)
            _groupmembers.Add(Teilnehmer)
        End Sub

        Public Sub AddMembers(Teilnehmerliste As ParticipantCollection)
            Teilnehmerliste.ToList.ForEach(Sub(x) _groupmembers.Add(x))
        End Sub

        Public Sub RemoveMember(Teilnehmer As Participant)
            _groupmembers.Remove(Teilnehmer)
        End Sub

        Public Sub RemoveMembers(Teilnehmerliste As ParticipantCollection)
            _groupmembers.ToList.ForEach(Sub(x) _groupmembers.Remove(x))
        End Sub

        Public ReadOnly Property CountOfMembers As Integer
            Get
                Return Groupmembers.Count
            End Get
        End Property

    End Class

End Namespace
