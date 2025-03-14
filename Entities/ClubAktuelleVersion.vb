Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities.AktuelleVersion

    Public Class Club
        Inherits BaseModel


#Region "Fields"

        Private _Einteilungsliste = New EinteilungCollection
        Private _Gruppenliste = New GruppeCollection
        Private _Gruppenhistorie = New List(Of GruppeCollection)



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
        Public Property ClubName As String


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

        ''' <summary>
        ''' Die aktuell ausgewählte Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property SelectedGruppe As Gruppe



        ''' <summary>
        ''' Ewige Teilnehmerliste
        ''' </summary>
        ''' <returns></returns>
        Public Property EwigeTeilnehmerliste() As EwigeTeilnehmerCollection

        ''' <summary>
        ''' Ewige Trainerliste
        ''' </summary>
        ''' <returns></returns>
        Public Property EwigeTrainerliste() As EwigeTrainerCollection





        ''' <summary>
        ''' Eine Liste der verwendeten Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufenliste() As LeistungsstufeCollection

        Public ReadOnly Property LeistungsstufenTextliste As IEnumerable(Of String)
            Get
                Return Leistungsstufenliste.OrderBy(Function(LS) LS.Sortierung).ToList.Select(Function(LS) LS.Benennung)
            End Get
        End Property

        ''' <summary>
        ''' Eine Liste der aller Faehigkeiten
        ''' </summary>
        ''' <returns></returns>
        Public Property AlleFaehigkeiten() As FaehigkeitCollection

#End Region

#Region "Funktionen und Methoden"




        ''' <summary>
        ''' Teilnehmer wird aus dem Club entfernt und 
        ''' in der Ewigen Teilnehmerliste archiviert
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerArchivieren(Teilnehmer As Teilnehmer)
            SelectedEinteilung.GruppenloseTeilnehmer.Remove(Teilnehmer)
            EwigeTeilnehmerliste.Add(Teilnehmer, Now)
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
            EwigeTrainerliste.Add(Trainer, Now)
        End Sub

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

#End Region

    End Class

End Namespace
