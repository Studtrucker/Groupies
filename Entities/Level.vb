Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Namespace Entities

    <DefaultProperty("ValueName")>
    Public Class Level
        Inherits BaseModel

        Public Sub New()
            _levelID = Guid.NewGuid()
        End Sub

        Public Property LevelID As Guid

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property LevelName As String


        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Beschreibung ist eine Pflichtangabe")>
        Public Property LevelDescription As String


        Public Property LevelSkills As SkillCollection


        Public Sub AddSkill(skill As Skill)
            _LevelSkills.Add(skill)
        End Sub

        Public Sub RemoveSkill(skill As Skill)
            _LevelSkills.Remove(skill)
        End Sub

    End Class
End Namespace
