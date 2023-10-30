Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel


Namespace Entities

    Public Class Participant
        Inherits BaseModel


        'Private _name As String
        'Private _vorname As String
        'Private _persoenlichesLevelID As Guid
        'Private _skikurs As String
        'Private teilnehmerIDFeld As Guid

        Public Sub New()
            _ParticipantID = Guid.NewGuid()
        End Sub

        Public Property ParticipantID As Guid
        '    Get
        '        Return teilnehmerIDFeld
        '    End Get
        '    Set(ByVal value As Guid)
        '        teilnehmerIDFeld = value
        '    End Set
        'End Property

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property ParticipantName As String
        '    Get
        '        Return _name
        '    End Get
        '    Set(ByVal value As String)
        '        _name = value
        '        Changed("ParticipantName")
        '    End Set
        'End Property

        Public Property ParticipantFirstname As String
        '    Get
        '        Return _vorname
        '    End Get
        '    Set(ByVal value As String)
        '        _vorname = value
        '        Changed("ParticipantFirstname")
        '    End Set
        'End Property

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

        Public Property ParticipantLevel As Guid
        '    Get
        '        Return _persoenlichesLevelID
        '    End Get
        '    Set(value As Guid)
        '        _persoenlichesLevelID = value
        '        Changed("ParticipantLevel")
        '    End Set
        'End Property

        Public Property MemberOfGroup As String
        '    Get
        '        Return _skikurs
        '    End Get
        '    Set(ByVal value As String)
        '        _skikurs = value
        '        Changed("MemberOfGroup")
        '    End Set
        'End Property

    End Class
End Namespace
