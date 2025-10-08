Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Imports Groupies.Interfaces
Imports System.IO
Imports System.Xml.Serialization
Imports System.ComponentModel
Imports Groupies.Controller

Namespace Entities.Generation4Alt


    Public Class Club
        Inherits BaseModel
        'Implements IClub

#Region "Fields"

        Private _AlleEinteilungen As EinteilungCollection
        Private _AlleGruppen As GruppeCollection
        Private _AlleLeistungsstufen As LeistungsstufeCollection
        Private _AlleFaehigkeiten As FaehigkeitCollection
        Private _AlleTrainer As TrainerCollection
        Private _AlleTeilnehmer As TeilnehmerCollection


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

#End Region

#Region "Properties"

        ''' <summary>
        ''' Der Clubname
        ''' </summary>
        ''' <returns></returns>
        Public Property ClubName As String 'Implements IClub.Name

        Private _SelectedEinteilung As Einteilung
        ''' <summary>
        ''' Die aktuell ausgewählte Einteilung
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedEinteilung As Einteilung
            Get
                Return _SelectedEinteilung
            End Get
            Set(value As Einteilung)
                _SelectedEinteilung = value
                SelectedGruppe = Nothing
            End Set
        End Property

        Private _SelectedGruppe As Gruppe
        ''' <summary>
        ''' Die aktuell ausgewählte Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedGruppe As Gruppe
            Get
                Return _SelectedGruppe
            End Get
            Set(value As Gruppe)
                _SelectedGruppe = value
            End Set
        End Property

        ''' <summary>
        ''' Die Einteilungen im aktuellen Club
        ''' Es kann hiermit eine Historie verwaltet werden
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleEinteilungen() As EinteilungCollection
            Get
                Return _AlleEinteilungen
            End Get
            Set(value As EinteilungCollection)
                _AlleEinteilungen = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller Leistungsstufen, ohne die leere Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleLeistungsstufen() As LeistungsstufeCollection
            Get
                Return _AlleLeistungsstufen
            End Get
            Set(value As LeistungsstufeCollection)
                _AlleLeistungsstufen = value
            End Set
        End Property


        ''' <summary>
        ''' Eine Liste aller gültigen Leistungsstufen
        ''' </summary>
        ''' <returns></returns>

        <XmlIgnore>
        Public ReadOnly Property LeistungsstufenComboBox() As LeistungsstufeCollection
            Get
                Dim ComboLeistungsstufen = New LeistungsstufeCollection(AlleLeistungsstufen) From {
                    New Leistungsstufe With {.Benennung = String.Empty, .Sortierung = -1, .Ident = Guid.Empty}}
                'Return ComboLeistungsstufen.Sortieren()
                Return AlleLeistungsstufen.Sortieren
            End Get
        End Property



        ''' <summary>
        ''' Eine Liste aller Faehigkeiten
        ''' als Vorlage für die Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleFaehigkeiten() As FaehigkeitCollection
            Get
                Return _AlleFaehigkeiten
            End Get
            Set(value As FaehigkeitCollection)
                _AlleFaehigkeiten = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller gültigen Faehigkeiten
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public ReadOnly Property FaehigkeitenComboBox() As FaehigkeitCollection
            Get
                Dim Faehigkeiten = AlleFaehigkeiten
                Faehigkeiten.Add(New Faehigkeit With {
                    .Benennung = String.Empty,
                    .Sortierung = -1,
                    .FaehigkeitID = Guid.Empty
                })
                Return Faehigkeiten
            End Get
        End Property

        ''' <summary>
        ''' Eine Liste der aller Trainer
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleTrainer() As TrainerCollection
            Get
                Return _AlleTrainer
            End Get
            Set(value As TrainerCollection)
                _AlleTrainer = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste der aller Teilnehmer
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleTeilnehmer() As TeilnehmerCollection
            Get
                Return _AlleTeilnehmer
            End Get
            Set(value As TeilnehmerCollection)
                _AlleTeilnehmer = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste der aller Gruppen
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleGruppen() As GruppeCollection
            Get
                Return _AlleGruppen
            End Get
            Set(value As GruppeCollection)
                _AlleGruppen = value
            End Set
        End Property

#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Teilnehmer wird aus dem Club entfernt und 
        ''' in der Ewigen Teilnehmerliste archiviert
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerArchivieren(Teilnehmer As Teilnehmer)
            SelectedEinteilung.GruppenloseTeilnehmer.Remove(Teilnehmer)
            'EwigeTeilnehmerliste.Add(Teilnehmer, Now)
            Throw New NotImplementedException
        End Sub

        ''' <summary>
        ''' Ein Trainer wird aus der angegebenen Gruppe entfernt
        ''' </summary>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe)
            SelectedEinteilung.GruppenloseTrainer.Add(Gruppe.Trainer)
            Gruppe.Trainer = Nothing
        End Sub

        ''' <summary>
        ''' Trainer wird aus dem Club entfernt und 
        ''' in der Ewigen Trainerliste archiviert
        ''' </summary>
        ''' <param name="Trainer"></param>
        Public Sub TrainerArchivieren(Trainer As Trainer)
            SelectedEinteilung.GruppenloseTrainer.Remove(Trainer)
            'EwigeTrainerliste.Add(Trainer, Now)
            Throw New NotImplementedException
        End Sub

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

        Public Function LadeGroupies(Datei As String) As Club 'Implements IClub.LadeGroupies
            Using fs = New FileStream(Datei, FileMode.Open)
                Dim serializer = New XmlSerializer(GetType(Club))
                Dim loadedSkiclub As Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Club)
                    Return Map2AktuelleGeneration(loadedSkiclub)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Club 'Implements IClub.Map2AktuelleGeneration
            'Return MappingGeneration4.MapSkiClub2Club(Skiclub)
            Return Nothing
        End Function



#End Region

    End Class

End Namespace
