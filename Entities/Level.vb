Imports System.ComponentModel

Namespace Entities

    Public Class Level
        Implements INotifyPropertyChanged

        Private levelIDFeld As Guid
        Private _benennung As String
        Private _beschreibung As String
        Private _skills As SkillCollection

        Public Sub New()
            levelIDFeld = Guid.NewGuid()
        End Sub

        Public Property LevelID As Guid
            Get
                Return levelIDFeld
            End Get
            Set(value As Guid)
                levelIDFeld = value
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

        Public Property Skills As SkillCollection
            Get
                Return _skills
            End Get
            Set(value As SkillCollection)
                _skills = value
            End Set
        End Property

        Public Sub AddSkill(skill As Skill)
            _skills.Add(skill)
        End Sub

        Public Sub RemoveSkill(skill As Skill)
            _skills.Remove(skill)
        End Sub

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            handler?(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    End Class
End Namespace
