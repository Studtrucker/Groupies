Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities.Veraltert

    Public Class Skill
        Inherits BaseModel

        Public Sub New()
            _SkillID = Guid.NewGuid()
        End Sub

        Public Property SkillID As Guid

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property SkillNaming As String

        Public Property SortNumber As String

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Beschreibung ist ein Pflichtfeld")>
        Public Property Description As String



    End Class
End Namespace
