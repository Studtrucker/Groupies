Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports PropertyChanged

Namespace Entities.Veraltert

    <DefaultProperty("GroupName")>
    Public Class Group
        Inherits BaseModel

        Public Sub New()
            _GroupID = Guid.NewGuid()
            _GroupMembers = New ParticipantCollection
        End Sub

        Public Property GroupID As Guid

        Public Property GroupNaming As String

        Public Property GroupPrintNaming As String

        Public Property GroupSort As String

        Public Property GroupLevel As Level

        Public Property GroupLeader As Instructor

        Public Property GroupMembers As ParticipantCollection


    End Class

End Namespace
