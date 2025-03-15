Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Namespace Entities.Generation1

    Public Class Level
        Inherits BaseModel

        Public Sub New()
        End Sub

        Public Sub New(SaveMe As Boolean)
            _LevelID = Guid.NewGuid()
            SaveOrDisplay = SaveMe
        End Sub

        Public Property LevelID As Guid

        Public Property SortNumber As String

        Public Property LevelNaming As String

        Public Property LevelDescription As String

        Public Property LevelSkills As SkillCollection


        Property SaveOrDisplay As Boolean

    End Class
End Namespace
