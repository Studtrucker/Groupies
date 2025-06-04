Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Imports Groupies.Interfaces
Imports System.IO
Imports System.Xml.Serialization
Imports System.ComponentModel
Imports Groupies.Controller

Namespace Entities.Generation3


    Public Class Club
        Inherits BaseModel
        Implements IClub

#Region "Fields"

        Private _Einteilungsliste = New EinteilungCollection

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

#Region "Eigenschaften"


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
        ''' Eine Liste aller Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleLeistungsstufen() As LeistungsstufeCollection = New LeistungsstufeCollection

        Public ReadOnly Property LeistungsstufenTextliste As IEnumerable(Of String)
            Get
                If AlleLeistungsstufen Is Nothing OrElse AlleLeistungsstufen.Count = 0 Then
                    Return New List(Of String) From {"Keine Leistungsstufen definiert"}
                Else
                    Return AlleLeistungsstufen.OrderBy(Function(LS) LS.Sortierung).ToList.Select(Function(LS) LS.Benennung)
                End If
            End Get
        End Property


        ''' <summary>
        ''' Eine Liste aller Faehigkeiten
        ''' als Vorlage für die Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleFaehigkeiten() As FaehigkeitCollection = New FaehigkeitCollection

        ''' <summary>
        ''' Eine Liste aller Faehigkeiten
        ''' als Vorlage für die Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AlleValidenFaehigkeiten() As FaehigkeitCollection
            Get
                Return New FaehigkeitCollection(AlleFaehigkeiten.Where(Function(f) f.Sortierung > -1))
            End Get
        End Property

        ''' <summary>
        ''' Eine Liste der aller Trainer
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleTrainer() As TrainerCollection = New TrainerCollection

        ''' <summary>
        ''' Eine Liste der aller Teilnehmer
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleTeilnehmer() As TeilnehmerCollection = New TeilnehmerCollection


        ''' <summary>
        ''' Eine Liste der aller Gruppen
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleGruppen() As GruppeCollection = New GruppeCollection

        ''' <summary>
        ''' Eine Liste der aller Einteilungen
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleEinteilungen() As EinteilungCollection = New EinteilungCollection

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

        Public Function LadeGroupies(Datei As String) As Club Implements IClub.LadeGroupies
            Using fs = New FileStream(Datei, FileMode.Open)
                Dim serializer = New XmlSerializer(GetType(Generation3.Club))
                Dim loadedSkiclub As Generation3.Club
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Generation3.Club)
                    'Return MappingGeneration3.MapSkiClub2Club(loadedSkiclub)
                    Return Map2AktuelleGeneration(loadedSkiclub)
                Catch ex As InvalidDataException
                    Throw ex
                End Try
            End Using
        End Function

        Public Function Map2AktuelleGeneration(Skiclub As IClub) As Club Implements IClub.Map2AktuelleGeneration
            Return MappingGeneration3.MapSkiClub2Club(Skiclub)
        End Function

#End Region

    End Class

End Namespace
