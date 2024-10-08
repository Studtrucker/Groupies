Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities

    Public Class Club

#Region "Fields"

        Private _Gruppenliste = New GruppeCollection
        Private _Teilnehmerliste = New TeilnehmerCollection
        Private _EingeteilteTeilnehmer = New TeilnehmerCollection
        Private _Trainerliste = New TrainerCollection
        Private _EingeteilteTrainer = New TrainerCollection

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

        Private _TeilnehmerInGruppen = New TeilnehmerCollection
        ''' <summary>
        ''' Gibt eine Liste den Teilnehmern zurück, die bereits in Gruppen eingeteilt wurden 
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property EingeteilteTeilnehmer() As TeilnehmerCollection
            Get
                Return _EingeteilteTeilnehmer
            End Get
        End Property


        Public ReadOnly Property TeilnehmerInGruppen As IEnumerable(Of Teilnehmer)
            Get
                _TeilnehmerInGruppen.Clear()
                Gruppenliste.ToList.ForEach(Sub(G) G.Mitgliederliste.ToList.ForEach(AddressOf EingeteilteTeilnehmerLesen))
                Return _TeilnehmerInGruppen
            End Get
        End Property

        Private Sub EingeteilteTeilnehmerLesen(Teilnehmer As Teilnehmer)
            _TeilnehmerInGruppen.add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Gibt eine Liste mit den Teilnehmern zurück, die noch keiner Gruppe angehören
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FreieTeilnehmer() As TeilnehmerCollection
            Get
                Return New TeilnehmerCollection(Teilnehmerliste.Except(_TeilnehmerInGruppen))
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
                Return _EingeteilteTrainer
            End Get
        End Property

        ''' <summary>
        ''' Gibt eine Liste mit den Trainer zurück, die noch keine Gruppe haben
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property FreieTrainer() As TrainerCollection
            Get
                Return New TrainerCollection(Trainerliste.Except(_EingeteilteTrainer))
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
            EingeteilteTeilnehmer.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Der Teilnehmer wird aus der angegebenen Gruppe als Mitglied entfernt
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Remove(Teilnehmer)
            EingeteilteTeilnehmer.Remove(Teilnehmer)
        End Sub

        Public Sub TrainerEinerGruppeZuweisen(Trainer As Trainer, Gruppe As Gruppe)
            Gruppe.Trainer = Trainer
            EingeteilteTrainer.Add(Trainer)
        End Sub
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe)
            EingeteilteTrainer.Remove(Gruppe.Trainer)
            Gruppe.Trainer = Nothing
        End Sub


        Public Overrides Function ToString() As String
            Return ClubName
        End Function

#End Region

    End Class

End Namespace
