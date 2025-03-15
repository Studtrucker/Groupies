Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel

Namespace Entities.Generation1


    Public Class Skiclub
        Implements IClub

#Region "Fields"


        Public Property Participantlist() As ParticipantCollection
        Public Property Grouplist() As GroupCollection
        Public Property Levellist() As LevelCollection
        Public Property Instructorlist() As InstructorCollection
        Public Property ParticipantsNotInGroup() As ParticipantCollection

        Public Property Name As String Implements IClub.ClubName


        Public Property LeistungsstufenTemplate As LeistungsstufeCollection Implements IClub.LeistungsstufenTemplate

        Public Property FaehigkeitenTemplate As FaehigkeitCollection Implements IClub.FaehigkeitenTemplate


#End Region

    End Class

End Namespace
