Imports System.ComponentModel

Namespace Entities

    Public Class Teilnehmer
        Implements INotifyPropertyChanged

        Private _name As String
        Private _vorname As String
        Private _koennenstufe As Entities.Koennenstufe
        Private _skigruppe As Entities.Skigruppe
        Private teilnehmerIDFeld As Guid

        Public Sub New()
            teilnehmerIDFeld = Guid.NewGuid()
            _koennenstufe = New Entities.Koennenstufe
            _skigruppe = New Entities.Skigruppe
        End Sub

        Public Property TeilnehmerID As Guid
            Get
                Return teilnehmerIDFeld
            End Get
            Set(ByVal value As Guid)
                teilnehmerIDFeld = value
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(ByVal value As String)
                _name = value
                Changed("Name")
            End Set
        End Property

        Public Property Vorname As String
            Get
                Return _vorname
            End Get
            Set(ByVal value As String)
                _vorname = value
                Changed("Vorname")
            End Set
        End Property

        Public Property Koennenstufe As Entities.Koennenstufe
            Get
                Return _koennenstufe
            End Get
            Set(value As Entities.Koennenstufe)
                _koennenstufe = value
                Changed("Koennenstufe")
            End Set
        End Property

        Public Property Skigruppe As Entities.Skigruppe
            Get
                Return _skigruppe
            End Get
            Set(ByVal value As Entities.Skigruppe)
                _skigruppe = value
                Changed("Skigruppe")
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
