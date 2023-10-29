Imports System.ComponentModel

Namespace Entities

    Public Class Instructor
        Implements INotifyPropertyChanged

        Private UebungsleiterIDFeld As Guid
        Private _vorname As String
        Private _name As String
        Private _printName As String
        Private _foto As Byte()

        Public Sub New()
            UebungsleiterIDFeld = Guid.NewGuid()
        End Sub

        Public Property UebungsleiterID As Guid
            Get
                Return UebungsleiterIDFeld
            End Get
            Set(value As Guid)
                UebungsleiterIDFeld = value
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

        Public Property PrintName As String
            Get
                Return _printName
            End Get
            Set(value As String)
                _printName = value
                Changed("PrintName")
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

        Public ReadOnly Property VollerName As String
            Get
                If _vorname Is Nothing Then
                    Return _name
                ElseIf _name Is Nothing Then
                    Return _vorname
                Else
                    Return String.Format("{0} {1}", _vorname, _name)
                End If
            End Get
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
