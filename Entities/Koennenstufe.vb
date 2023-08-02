Imports System.ComponentModel

Namespace Entities

    Public Class Koennenstufe
        Implements INotifyPropertyChanged

        Private koennenstufeIDFeld As Guid
        Private _benennung As String
        Private _beschreibung As String
        Private _angezeigteBenennung As String
        Public Sub New()
            koennenstufeIDFeld = Guid.NewGuid()
        End Sub

        Public Property KoennenstufeID As Guid
            Get
                Return koennenstufeIDFeld
            End Get
            Set(value As Guid)
                koennenstufeIDFeld = value
            End Set
        End Property

        Public Property Benennung As String
            Get
                Return _benennung
            End Get
            Set(value As String)
                _benennung = value
                Changed("Benennung")
            End Set
        End Property

        Public Property Beschreibung As String
            Get
                Return _beschreibung
            End Get
            Set(value As String)
                _beschreibung = value
                Changed("Beschreibung")
            End Set
        End Property

        Public Property AngezeigteBenennung As String
            Get
                Return _angezeigteBenennung
            End Get
            Set(value As String)
                _angezeigteBenennung = value
                Changed("AngezeigteBenennung")
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
