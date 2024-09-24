Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports CDS = Groupies.Services.CurrentDataService


Namespace Entities

    <DefaultBindingProperty("ParticipantFirstname")>
    <DefaultProperty("ParticipantFullName")>
    Public Class Participant
        Inherits BaseModel

        Public Event ChangeGroup(Participant As Participant)

        Public Sub New()
            _ParticipantID = Guid.NewGuid()
        End Sub


        ''' <summary>
        ''' Das ist eine eindeutige Kennzeichnung für den Teilnehmer 
        ''' </summary>
        ''' <returns></returns>
        Public Property ParticipantID As Guid

        ''' <summary>
        ''' Der Nachname des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property ParticipantLastName As String

        ''' <summary>
        ''' Der Vorname des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Vorname ist eine Pflichtangabe")>
        Public Property ParticipantFirstName As String

        ''' <summary>
        ''' Der Vor- und Nachname
        ''' </summary>
        ''' <returns></returns>
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

        ''' <summary>
        ''' Der Leistungsstand des Teilnehmers
        ''' </summary>
        ''' <returns></returns>
        Public Property ParticipantLevel As Level

        ''' <summary>
        ''' Gruppenmitglied in der Gruppe 
        ''' mit dem Gruppenkennzeichen
        ''' </summary>
        ''' <returns></returns>
        Public Property MemberOfGroup As Guid


        ''' <summary>
        ''' Gruppenmitglied in der Gruppe 
        ''' mit dem Gruppennamen 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property MemberOfGroup_Naming() As String
            Get
                Return CDS.Skiclub.Grouplist.Where(Function(x) x.GroupID.Equals(MemberOfGroup)).DefaultIfEmpty(New Group With {.GroupNaming = String.Empty}).Single.GroupNaming
            End Get
        End Property


        ''' <summary>
        ''' Wird Gruppenmitglied von der Gruppe
        ''' mit dem Gruppenkennzeichen
        ''' </summary>
        ''' <param name="GroupID"></param>
        Public Sub SetAsGroupMember(GroupID As Guid)
            MemberOfGroup = GroupID
            _IsGroupMember = True
        End Sub

        ''' <summary>
        ''' Wird als Gruppenmitgleid gekennzeichnet
        ''' </summary>
        Public Sub SetAsGroupMember()
            _IsGroupMember = True
        End Sub

        ''' <summary>
        ''' Wird als Gruppenmitglied entfernt
        ''' </summary>
        Public Sub RemoveFromGroup()
            MemberOfGroup = Nothing
            _IsGroupMember = False
        End Sub

        ''' <summary>
        ''' Setzt und liest die Gruppenmitgliedschaft
        ''' </summary>
        Private _IsGroupMember As Boolean
        Public Property IsGroupMember As Boolean
            Get
                Return _IsGroupMember
            End Get
            Set(value As Boolean)
                _IsGroupMember = value
            End Set
        End Property


        ''' <summary>
        ''' Liest die umgekehrte Gruppenmitgliedschaft
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property IsNotInGroup As Boolean
            Get
                Return Not _IsGroupMember
            End Get
        End Property


    End Class
End Namespace
