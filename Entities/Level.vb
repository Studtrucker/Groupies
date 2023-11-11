Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Namespace Entities

    <DefaultProperty("LevelName")>
    Public Class Level
        Inherits BaseModel

        Public Sub New()
        End Sub

        Public Sub New(SaveMe As Boolean)
            _LevelID = Guid.NewGuid()
            SaveOrDisplay = SaveMe
        End Sub

        Public Property LevelID As Guid

        <StringLength(3)>
        Public Property SortNumber As String

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property LevelNaming As String

        Public Property LevelDescription As String

        Public Property LevelSkills As SkillCollection

        Public Sub AddSkill(skill As Skill)
            _LevelSkills.Add(skill)
        End Sub

        Public Sub RemoveSkill(skill As Skill)
            _LevelSkills.Remove(skill)
        End Sub

        Property SaveOrDisplay As Boolean

    End Class
End Namespace
