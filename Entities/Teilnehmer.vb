Imports System.ComponentModel

Namespace Entities

    Public Class Teilnehmer
        Implements INotifyPropertyChanged

        Private _name As String
        Private _vorname As String
        Private _persoenlichesLevel As Entities.Level
        Private _skikursgruppe As String
        Private teilnehmerIDFeld As Guid

        Public Sub New()
            teilnehmerIDFeld = Guid.NewGuid()
            _persoenlichesLevel = New Entities.Level
            '_skikursgruppe = New Entities.Skikursgruppe
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

        Public Property PersoenlichesLevel As Entities.Level
            Get
                Return _persoenlichesLevel
            End Get
            Set(value As Entities.Level)
                _persoenlichesLevel = value
                Changed("PersoenlichesLevel")
            End Set
        End Property

        Public Property Skikursgruppe As String
            Get
                Return _skikursgruppe
            End Get
            Set(ByVal value As String)
                _skikursgruppe = value
                Changed("Skikursgruppe")
            End Set
        End Property

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            handler?(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    End Class
End Namespace
