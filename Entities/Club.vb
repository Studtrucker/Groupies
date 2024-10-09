Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities

    Public Class Club

#Region "Fields"

        Private _Gruppenliste = New GruppeCollection
        Private _Teilnehmerliste = New TeilnehmerCollection
        Private _Trainerliste = New TrainerCollection

        Private _EingeteilteTeilnehmer As TeilnehmerCollection
        Private _FreieTeilnehmer As TeilnehmerCollection
        Private _EingeteilteTrainer As TrainerCollection
        Private _FreieTrainer As TrainerCollection
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
        ''' Eine Liste aller Gruppen im Club
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
        ''' Eine Liste aller Teilnehmer im Club
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
        '''  Gibt eine Liste der Teilnehmern zurück, die bereits in Gruppen eingeteilt wurden 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTeilnehmer As TeilnehmerCollection
            Get
                _EingeteilteTeilnehmer = New TeilnehmerCollection
                Gruppenliste.ToList.ForEach(Sub(G) G.Mitgliederliste.ToList.ForEach(AddressOf EingeteilteTeilnehmerLesen))
                Return _EingeteilteTeilnehmer
            End Get
        End Property


        ''' <summary>
        ''' Gibt eine Liste mit den Teilnehmern zurück, die noch keiner Gruppe angehören
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FreieTeilnehmer As TeilnehmerCollection
            Get
                _FreieTeilnehmer = New TeilnehmerCollection(_Teilnehmerliste)
                Gruppenliste.ToList.ForEach(Sub(G) G.Mitgliederliste.ToList.ForEach(AddressOf EingeteilteAusFreieTeilnehmerEntfernen))
                Return _FreieTeilnehmer
            End Get
        End Property


        ''' <summary>
        ''' Eine Liste aller Trainer im Club
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
        ''' Gibt eine Liste der Trainer zurück, die bereits in eine Gruppe eingeteilt wurden 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTrainer() As TrainerCollection
            Get
                _EingeteilteTrainer = New TrainerCollection
                Gruppenliste.ToList.ForEach(Sub(Gr) EingeteilteTrainerLesen(Gr.Trainer))
                Return _EingeteilteTrainer
            End Get
        End Property

        ''' <summary>
        ''' Gibt eine Liste mit den Trainer zurück, die noch keine Gruppe haben
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FreieTrainer() As TrainerCollection
            Get
                _FreieTrainer = New TrainerCollection(Trainerliste)
                Gruppenliste.ToList.ForEach(Sub(Gr) FreieTrainerLesen(Gr.Trainer))
                Return _FreieTrainer
            End Get
        End Property

        ''' <summary>
        ''' Eine Liste der verwendeten Leistungsstufen
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufenliste() As LeistungsstufeCollection

#End Region

#Region "Funktionen und Methoden"
        ''' <summary>
        ''' Die angegebene Gruppe bekommt den Teilnehmer als Mitglied
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerInGruppeEinteilen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Der Teilnehmer wird aus der angegebenen Gruppe als Mitglied entfernt
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Remove(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Der Trainer wird der angegebenen Gruppe zugewiesen
        ''' </summary>
        ''' <param name="Trainer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerEinerGruppeZuweisen(Trainer As Trainer, Gruppe As Gruppe)
            Gruppe.Trainer = Trainer
        End Sub

        ''' <summary>
        ''' Ein Trainer wird aus der angegebenen Gruppe entfernt
        ''' </summary>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe)
            Gruppe.Trainer = Nothing
        End Sub

        ''' <summary>
        ''' FreieTrainer=AlleTrainer-EingeteilteTrainer 
        ''' </summary>
        ''' <param name="Trainer"></param>
        Private Sub FreieTrainerLesen(Trainer As Trainer)
            _FreieTrainer.Remove(Trainer)
        End Sub

        ''' <summary>
        ''' EingeteilteTrainer=EingeteilteTrainer+Gruppentrainer
        ''' </summary>
        ''' <param name="Trainer"></param>
        Private Sub EingeteilteTrainerLesen(Trainer As Trainer)
            If Trainer IsNot Nothing Then
                _EingeteilteTrainer.Add(Trainer)
            End If
        End Sub

        Private Sub EingeteilteAusFreieTeilnehmerEntfernen(Teilnehmer As Teilnehmer)
            _FreieTeilnehmer.Remove(Teilnehmer)
        End Sub

        Private Sub EingeteilteTeilnehmerLesen(Teilnehmer As Teilnehmer)
            _EingeteilteTeilnehmer.Add(Teilnehmer)
        End Sub

        Public Overrides Function ToString() As String
            Return ClubName
        End Function

#End Region

    End Class

End Namespace
