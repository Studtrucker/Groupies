Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities
    Public Class ParticipantRelationship
        Implements INotifyPropertyChanged

        Private _teilnemer As Entities.Participant
        Public Property Teilnehmer() As Entities.Participant
            Get
                Return _teilnemer
            End Get
            Set(ByVal value As Entities.Participant)
                _teilnemer = value
            End Set
        End Property

        Private _partner As Entities.Participant
        Public Property Partner() As Entities.Participant
            Get
                Return _partner
            End Get
            Set(ByVal value As Entities.Participant)
                _partner = value
            End Set
        End Property

        Private _beziehung As String
        Public Property Beziehung() As String
            Get
                Return _beziehung
            End Get
            Set(ByVal value As String)
                _beziehung = value
                Changed("Beziehung")
            End Set
        End Property

        Private _bindung As String
        Public Property Bindung() As String
            Get
                Return _bindung
            End Get
            Set(ByVal value As String)
                _bindung = value
                Changed("Bindung")
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
