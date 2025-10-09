Imports System.IO
Imports System.Xml.Serialization
Imports Groupies.Interfaces


' Damit eine Property nicht im xml gespeichert wird, muss es mit dem Attribut <XmlIgnore> gekennzeichnet werden

Namespace Entities.Generation4

    Public Class Club


        Inherits BaseModel
        Implements IClub

#Region "Fields"

        Private _Einteilungsliste As EinteilungCollection
        Private _Gruppenliste As GruppeCollection
        Private _Leistungsstufenliste As LeistungsstufeCollection
        Private _Faehigkeitenliste As FaehigkeitCollection
        Private _Trainerliste As TrainerCollection
        Private _Teilnehmerliste As TeilnehmerCollection


#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Parameterloser Konstruktor für das
        ''' Einlesen von XML Dateien notwendig
        ''' </summary>
        Public Sub New()
            DateiGeneration = "4"
        End Sub

#End Region

#Region "Properties"

        '<XmlElement("DateiGeneration")>
        Public Property DateiGeneration As String Implements IClub.DateiGeneration

        ''' <summary>
        ''' Der Clubname
        ''' </summary>
        ''' <returns></returns>
        Public Property ClubName As String Implements IClub.Name

        ''' <summary>
        ''' Die Einteilungen im aktuellen Club
        ''' Es kann hiermit eine Historie verwaltet werden
        ''' </summary>
        ''' <returns></returns>
        Public Property Einteilungsliste() As EinteilungCollection
            Get
                Return _Einteilungsliste
            End Get
            Set(value As EinteilungCollection)
                _Einteilungsliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller Leistungsstufen, ohne die leere Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufenliste() As LeistungsstufeCollection
            Get
                Return _Leistungsstufenliste
            End Get
            Set(value As LeistungsstufeCollection)
                _Leistungsstufenliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller Faehigkeiten
        ''' als Vorlage für die Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Faehigkeitenliste() As FaehigkeitCollection
            Get
                Return _Faehigkeitenliste
            End Get
            Set(value As FaehigkeitCollection)
                _Faehigkeitenliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste der aller Trainer
        ''' </summary>
        ''' <returns></returns>
        Public Property Trainerliste() As TrainerCollection
            Get
                Return _Trainerliste
            End Get
            Set(value As TrainerCollection)
                _Trainerliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste der aller Teilnehmer
        ''' </summary>
        ''' <returns></returns>
        Public Property Teilnehmerliste() As TeilnehmerCollection
            Get
                Return _Teilnehmerliste
            End Get
            Set(value As TeilnehmerCollection)
                _Teilnehmerliste = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste der aller Gruppen
        ''' </summary>
        ''' <returns></returns>
        Public Property Gruppenliste() As GruppeCollection
            Get
                Return _Gruppenliste
            End Get
            Set(value As GruppeCollection)
                _Gruppenliste = value
            End Set
        End Property

#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

        Public Function LadeGroupies(Datei As String) As Generation4.Club Implements IClub.LadeGroupies
            Using fs = New FileStream(Datei, FileMode.Open)
                Dim serializer = New XmlSerializer(GetType(Generation4.Club))
                Dim loadedSkiclub As Generation4.Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Generation4.Club)
                    Return Map2AktuelleGeneration(loadedSkiclub)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration4.MapSkiClub2Club(Skiclub)
        End Function

#End Region

    End Class


End Namespace
