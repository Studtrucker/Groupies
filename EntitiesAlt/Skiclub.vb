Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Veraltert


    Public Class Skiclub

#Region "Fields"

        Public Property Name As String

        Public Property Participantlist() As ParticipantCollection
        Public Property Grouplist() As GroupCollection
        Public Property Levellist() As LevelCollection
        Public Property Instructorlist() As InstructorCollection
        Public Property ParticipantsNotInGroup() As ParticipantCollection

#End Region

    End Class

End Namespace
