Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    Public Class Skill
        Inherits BaseModel

        Public Sub New()
            _skillID = Guid.NewGuid()
        End Sub

        Public Property SkillID As Guid

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property SkillNaming As String

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Sortierung ist eine Pflichtangabe")>
        Public Property SortingNumber As String
        Public Property Description As String



    End Class
End Namespace
