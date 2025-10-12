Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services
Imports Groupies.Interfaces
Imports System.IO
Imports System.Xml.Serialization

Namespace Entities.Generation2
    Public Class Club
        Inherits BaseModel
        Implements IClub

#Region "Fields"

        Private _Gruppenliste = New GruppeCollection
        Private _Gruppenhistorie = New List(Of GruppeCollection)

        Private _GruppenloseTeilnehmer As New TeilnehmerCollection
        Private ReadOnly _EingeteilteTeilnehmer As New TeilnehmerCollection
        Private ReadOnly _AlleTeilnehmer As New TeilnehmerCollection

        Private _GruppenloseTrainer As New TrainerCollection
        Private ReadOnly _EingeteilteTrainer As New TrainerCollection
        Private ReadOnly _AlleTrainer As New TrainerCollection

#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Parameterloser Konstruktor für das
        ''' Einlesen von XML Dateien notwendig
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Erstellung eines neuen Clubs
        ''' Gruppenliste, Teilnehmerliste und Trainerliste werden instanziiert
        ''' </summary>
        Public Sub New(Clubname As String)
            _ClubName = Clubname
        End Sub

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

#End Region

#Region "Eigenschaften"
        ''' <summary>
        ''' Der Clubname
        ''' </summary>
        ''' <returns></returns>
        Public Property ClubName As String Implements IClub.Name

        Public Property DateiGeneration As String Implements IClub.DateiGeneration

        ''' <summary>
        ''' Eine Liste aller Gruppen
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

        Public Property Gruppenhistorie() As List(Of GruppeCollection)
            Get
                Return _Gruppenhistorie
            End Get
            Set(value As List(Of GruppeCollection))
                _Gruppenhistorie = value
            End Set
        End Property

        ''' <summary>
        ''' Teilnehmer ohne Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenloseTeilnehmer() As TeilnehmerCollection
            Get
                Return _GruppenloseTeilnehmer
            End Get
            Set(value As TeilnehmerCollection)
                _GruppenloseTeilnehmer = value
            End Set
        End Property


        ''' <summary>
        '''  Teilnehmer, die bereits in Gruppen eingeteilt wurden 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTeilnehmer As TeilnehmerCollection
            Get
                _EingeteilteTeilnehmer.Clear()
                Gruppenliste.ToList.ForEach(Sub(G) G.Mitgliederliste.ToList.ForEach(Sub(M) _EingeteilteTeilnehmer.Add(M)))
                Return _EingeteilteTeilnehmer
            End Get
        End Property


        ''' <summary>
        ''' Liste mit allen Teilnehmern, Eingeteilte und die ohne Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AlleTeilnehmer As TeilnehmerCollection
            Get
                _AlleTeilnehmer.Clear()
                EingeteilteTeilnehmer.ToList.ForEach(Sub(M) _AlleTeilnehmer.Add(M))
                GruppenloseTeilnehmer.ToList.ForEach(Sub(M) _AlleTeilnehmer.Add(M))
                Return _AlleTeilnehmer
            End Get
        End Property


        ''' <summary>
        ''' Trainer ohne Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenloseTrainer() As TrainerCollection
            Get
                Return _GruppenloseTrainer
            End Get
            Set(value As TrainerCollection)
                _GruppenloseTrainer = value
            End Set
        End Property

        ''' <summary>
        ''' Trainer, die bereits in Gruppen eingeteilt wurden
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTrainer() As TrainerCollection
            ' = Gruppenliste.ToList.Select(Function(Gr) Gr.Trainer))
            Get
                _EingeteilteTrainer.Clear()
                Gruppenliste.ToList.Where(Function(Gr) Gr.Trainer IsNot Nothing).ToList.ForEach(Sub(Gr) _EingeteilteTrainer.Add(Gr.Trainer))
                Return _EingeteilteTrainer
            End Get
        End Property

        ''' <summary>
        ''' Eine Liste der verwendeten Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufenliste() As LeistungsstufeCollection


        ''' <summary>
        ''' Eine Liste der aller Faehigkeiten
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleFaehigkeiten() As FaehigkeitCollection

#End Region

#Region "Funktionen und Methoden"


#End Region

    End Class

End Namespace
