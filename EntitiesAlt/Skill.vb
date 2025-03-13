Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities.Generation1

    Public Class Skill
        Inherits BaseModel

        Public Sub New()
            _SkillID = Guid.NewGuid()
        End Sub

        Public Property SkillID As Guid

        Public Property SkillNaming As String

        Public Property SortNumber As String

        Public Property Description As String



    End Class
End Namespace
