Imports System.ComponentModel

Namespace Entities

    Public Class ParticipantHistory
        Implements INotifyPropertyChanged

        Private _teilnehmer As Entities.Participant
        Private _eintragVom As Date
        Private _eintrag As String
        Private _hashtag As String

        Public Property Teilnehmer As Entities.Participant
            Get
                Return _teilnehmer
            End Get
            Set(value As Entities.Participant)
                _teilnehmer = value
                Changed("Teilnehmer")
            End Set
        End Property

        Public Property Eintrag As String
            Get
                Return _eintrag
            End Get
            Set(value As String)
                _eintrag = value
                Changed("Eintrag")
            End Set
        End Property
        Public Property Hashtag As String
            Get
                Return _hashtag
            End Get
            Set(value As String)
                _hashtag = value
                Changed("Hashtag")
            End Set
        End Property
        Public Property EintragVom As Date
            Get
                Return _eintragVom
            End Get
            Set(value As Date)
                _eintragVom = value
                Changed("EintragVom")
            End Set
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
