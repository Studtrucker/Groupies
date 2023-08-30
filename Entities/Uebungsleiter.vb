Imports System.ComponentModel

Namespace Entities

    Public Class Uebungsleiter
        Implements INotifyPropertyChanged

        Private skilehrerIDFeld As Guid
        Private _vorname As String
        Private _name As String
        Private _angezeigterName As String
        Private _foto As Byte()

        Public Sub New()
            skilehrerIDFeld = Guid.NewGuid()
        End Sub

        Public Property SkilehrerID As Guid
            Get
                Return skilehrerIDFeld
            End Get
            Set(value As Guid)
                skilehrerIDFeld = value
            End Set
        End Property

        Public Property Vorname As String
            Get
                Return _vorname
            End Get
            Set(value As String)
                _vorname = value
                Changed("Vorname")
            End Set
        End Property

        Public Property Name As String
            Get
                Return _name
            End Get
            Set(value As String)
                _name = value
                Changed("Name")
            End Set
        End Property

        Public Property AngezeigterName As String
            Get
                Return _angezeigterName
            End Get
            Set(value As String)
                _angezeigterName = value
                Changed("AngezeigterName")
            End Set
        End Property

        Public Property Foto As Byte()
            Get
                Return _foto
            End Get
            Set(value As Byte())
                _foto = value
                Changed("Foto")
                Changed("HatFoto")
            End Set
        End Property

        Public ReadOnly Property HatFoto As Boolean
            Get
                Return _foto IsNot Nothing AndAlso Foto.Length > 0
            End Get
        End Property

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            handler?(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    End Class

End Namespace
