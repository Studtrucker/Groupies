Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Namespace Entities

    <DefaultProperty("ValueName")>
    Public Class Level
        Implements INotifyPropertyChanged

        Private levelIDFeld As Guid
        Private _LevelName As String
        Private _LevelDescription As String
        Private _LevelSkills As SkillCollection

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

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property LevelName As String
            Get
                Return _LevelName
            End Get
            Set(value As String)
                _LevelName = value
                Changed("Levelname")
            End Set
        End Property

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Beschreibung ist eine Pflichtangabe")>
        Public Property LevelDescription As String
            Get
                Return _LevelDescription
            End Get
            Set(value As String)
                _LevelDescription = value
                Changed("LevelDescription")
            End Set
        End Property

        Public Property LevelSkills As SkillCollection
            Get
                Return _LevelSkills
            End Get
            Set(value As SkillCollection)
                _LevelSkills = value
            End Set
        End Property

        Public Sub AddSkill(skill As Skill)
            _LevelSkills.Add(skill)
        End Sub

        Public Sub RemoveSkill(skill As Skill)
            _LevelSkills.Remove(skill)
        End Sub

        Private Sub Changed(propertyName As String)
            Dim handler = PropertyChangedEvent
            handler?(Me, New PropertyChangedEventArgs(propertyName))
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    End Class
End Namespace
