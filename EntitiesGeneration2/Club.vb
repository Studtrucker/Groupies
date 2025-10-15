Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services
Imports Groupies.Interfaces
Imports System.IO
Imports System.Xml.Serialization

Namespace Entities.Generation2

    Public Class Club
        Implements IClub

        Public Function LadeGroupies(Datei As String) As Generation4.Club Implements IClub.LadeGroupies
            Dim Dateiname = Path.GetFileNameWithoutExtension(Datei)

            Using fs = New FileStream(Datei, FileMode.Open)

                Dim serializer = New XmlSerializer(GetType(Generation2.Club))
                Dim loadedSkiclub As Generation2.Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Generation2.Club)
                    'Return MappingGeneration2.MapSkiClub2Club(loadedSkiclub)
                    Return Map2AktuelleGeneration(loadedSkiclub, Dateiname)

                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using

        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Generation4.Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration2.MapSkiClub2Club(Skiclub)
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub, Dateiname As String) As Generation4.Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration2.MapSkiClub2Club(Skiclub, Dateiname)
        End Function


#Region "Eigenschaften"

        Public Property ClubName As String Implements IClub.Name
        Public Property DateiGeneration As String Implements IClub.DateiGeneration
        Public Property Gruppenliste() As List(Of Gruppe)
        Public Property GruppenloseTeilnehmer() As List(Of Teilnehmer)
        Public Property EwigeTeilnehmerliste As List(Of Teilnehmer)
        Public Property EwigeTrainerliste() As List(Of Trainer)
        Public Property EingeteilteTeilnehmer As List(Of Teilnehmer)
        Public Property AlleTeilnehmer As List(Of Teilnehmer)
        Public Property GruppenloseTrainer() As List(Of Trainer)
        Public Property EingeteilteTrainer() As List(Of Trainer)
        Public Property AlleTrainer() As List(Of Trainer)
        Public Property Leistungsstufenliste() As List(Of Leistungsstufe)
        Public Property Gruppenhistorie() As List(Of List(Of Gruppe))
        Public Property AlleFaehigkeiten() As List(Of Faehigkeit)

#End Region

#Region "Funktionen und Methoden"


#End Region

    End Class

End Namespace
