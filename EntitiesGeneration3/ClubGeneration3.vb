Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Imports Groupies.Interfaces
Imports System.IO
Imports System.Xml.Serialization

Namespace Entities.Generation3


    Public Class Club
        Inherits BaseModel
        Implements IClub


#Region "Properties"


        Public Property ClubName As String Implements IClub.Name
        Public Property DateiGeneration As String Implements IClub.DateiGeneration
        Public Property Einteilungsliste() As EinteilungCollection
        Public Property Leistungsstufenliste() As LeistungsstufeCollection = New LeistungsstufeCollection


        Public Function LadeGroupies(Datei As String) As Generation4.Club Implements IClub.LadeGroupies
            Dim Dateiname = Path.GetFileNameWithoutExtension(Datei)
            Using fs = New FileStream(Datei, FileMode.Open)
                Dim serializer = New XmlSerializer(GetType(Club))
                Dim loadedSkiclub As Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Club)
                    Return Map2AktuelleGeneration(loadedSkiclub, Dateiname)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub, Dateiname As String) As Generation4.Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration3.MapSkiClub2Club(Skiclub, Dateiname)
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Generation4.Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration3.MapSkiClub2Club(Skiclub)
        End Function

#End Region

    End Class

End Namespace
