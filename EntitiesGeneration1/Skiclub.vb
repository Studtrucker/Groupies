Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Interfaces
Imports Groupies.Entities.Generation4
Imports System.IO
Imports System.Xml.Serialization

Namespace Entities.Generation1


    Public Class Skiclub
        Implements IClub

#Region "Fields"


        Public Property Participantlist() As ParticipantCollection
        Public Property Grouplist() As GroupCollection
        Public Property Levellist() As LevelCollection
        Public Property Instructorlist() As InstructorCollection
        Public Property ParticipantsNotInGroup() As ParticipantCollection

        Public Property Name As String Implements IClub.Name

        Public ReadOnly Property DateiGeneration As String Implements IClub.DateiGeneration


        Public Property LeistungsstufenTemplate As LeistungsstufeCollection

        Public Property FaehigkeitenTemplate As FaehigkeitCollection


        Public Function LadeGroupies(Datei As String) As Club Implements IClub.LadeGroupies
            Using fs = New FileStream(Datei, FileMode.Open)

                Dim serializer = New XmlSerializer(GetType(Skiclub))
                Dim loadedSkiclub As Skiclub
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Skiclub)
                    Return Map2AktuelleGeneration(loadedSkiclub)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration1.MapSkiClub2Club(Skiclub)
        End Function

#End Region

    End Class

End Namespace
