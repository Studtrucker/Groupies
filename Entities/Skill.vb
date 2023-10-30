Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    Public Class Skill
        Implements INotifyPropertyChanged

        Private skillIDFeld As Guid
        Private _SkillName As String

        Public Sub New()
            skillIDFeld = Guid.NewGuid()
        End Sub

        Public Property SkillID As Guid
            Get
                Return skillIDFeld
            End Get
            Set(value As Guid)
                skillIDFeld = value
            End Set
        End Property

        Public Property SkillName As String
            Get
                Return _SkillName
            End Get
            Set(value As String)
                _SkillName = value
                Changed("SkillName")
            End Set
        End Property

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            handler?(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    End Class
End Namespace
