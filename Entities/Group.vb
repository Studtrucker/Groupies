Imports System.ComponentModel

Namespace Entities
    Public Class Group
        Implements INotifyPropertyChanged

        Private _groupID As Guid
        Private _groupName As String
        Private _groupPrintName As String
        Private _grouplevelID As Level
        Private _groupleaderID As Instructor
        Private _groupmembers As Entities.ParticipantCollection

        Public Sub New()
            _groupID = Guid.NewGuid()
            _groupmembers = New ParticipantCollection
        End Sub

        Public Property GroupID As Guid
            Get
                Return _groupID
            End Get
            Set(value As Guid)
                _groupID = value
            End Set
        End Property

        Public Property GroupName As String
            Get
                Return _groupName
            End Get
            Set(value As String)
                _groupName = value
                Changed("GroupName")
            End Set
        End Property

        Public Property GroupPrintName As String
            Get
                Return _groupPrintName
            End Get
            Set(value As String)
                _groupPrintName = value
                Changed("GroupPrintName")
            End Set
        End Property

        Public Property Grouplevel As Level
            Get
                Return _grouplevelID
            End Get
            Set(value As Level)
                _grouplevelID = value
                Changed("Grouplevel")
            End Set
        End Property

        Public Property Groupleader As Instructor
            Get
                Return _groupleaderID
            End Get
            Set(value As Instructor)
                _groupleaderID = value
                Changed("Groupleader")
            End Set
        End Property

        Public Property Groupmembers As ParticipantCollection
            Get
                Return _groupmembers
            End Get
            Set(value As Entities.ParticipantCollection)
                _groupmembers = value
                Changed("Groupmembers")
            End Set
        End Property

        Public Sub AddMember(Teilnehmer As Participant)
            _groupmembers.Add(Teilnehmer)
            Changed("Groupmembers")
        End Sub

        Public Sub AddMembers(Teilnehmerliste As ParticipantCollection)
            Teilnehmerliste.ToList.ForEach(Sub(x) _groupmembers.Add(x))
            Changed("Groupmembers")
        End Sub

        Public Sub RemoveMember(Teilnehmer As Participant)
            _groupmembers.Remove(Teilnehmer)
            Changed("Groupmembers")
        End Sub

        Public Sub RemoveMembers(Teilnehmerliste As ParticipantCollection)
            _groupmembers.ToList.ForEach(Sub(x) _groupmembers.Remove(x))
            Changed("Groupmembers")
        End Sub

        Public ReadOnly Property CountOfMembers As Integer
            Get
                Return Groupmembers.Count
            End Get
        End Property

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            If handler IsNot Nothing Then
                handler(Me, New PropertyChangedEventArgs(propertyName))
            End If
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

End Namespace
