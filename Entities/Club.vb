Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports Groupies.Services

Namespace Entities

    Public Class Club
        Inherits BaseModel


#Region "Fields"

        Private _Einteilungsliste = New EinteilungCollection
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

#End Region

#Region "Eigenschaften"
        ''' <summary>
        ''' Der Clubname
        ''' </summary>
        ''' <returns></returns>
        Public Property ClubName As String

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
        ''' Liste mit allen Trainern, Eingeteilte und die ohne Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property AlleTrainer() As TrainerCollection
            Get
                _AlleTrainer.Clear()
                EingeteilteTrainer.ToList.ForEach(Sub(T) _AlleTrainer.Add(T))
                GruppenloseTrainer.ToList.ForEach(Sub(T) _AlleTrainer.Add(T))
                Return _AlleTrainer
            End Get
        End Property

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
        ''' Prüfung, ob die Leistungsstufe in Gebrauch ist
        ''' </summary>
        ''' <param name="Leistungsstufe"></param>
        ''' <returns></returns>
        Public Function LeistungsstufeWirdNichtGenutzt(Leistungsstufe As Leistungsstufe) As Boolean
            Dim TnL = AlleTeilnehmer.Where(Function(Tn) Tn.Leistungsstand.Benennung.Equals(Leistungsstufe.Benennung)).ToList
            Dim GrL = Gruppenliste.Where(Function(Gr) Gr.Leistungsstufe.Benennung.Equals(Leistungsstufe.Benennung)).ToList
            If TnL.Count = 0 AndAlso GrL.Count = 0 Then
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' Die angegebene Gruppe bekommt den Teilnehmer als Mitglied
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerInGruppeEinteilen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Add(Teilnehmer)
            GruppenloseTeilnehmer.Remove(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Der Teilnehmer wird aus der angegebenen Gruppe als Mitglied entfernt
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Remove(Teilnehmer)
            GruppenloseTeilnehmer.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Teilnehmer wird aus dem Club entfernt und 
        ''' in der Ewigen Teilnehmerliste archiviert
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerArchivieren(Teilnehmer As Teilnehmer)
            GruppenloseTeilnehmer.Remove(Teilnehmer)
            EwigeTeilnehmerliste.Add(Teilnehmer, Now)
        End Sub

        ''' <summary>
        ''' Der Trainer wird der angegebenen Gruppe zugewiesen
        ''' </summary>
        ''' <param name="Trainer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerEinerGruppeZuweisen(Trainer As Trainer, Gruppe As Gruppe)
            Gruppe.Trainer = Trainer
            GruppenloseTrainer.Remove(Trainer)
        End Sub

        ''' <summary>
        ''' Ein Trainer wird aus der angegebenen Gruppe entfernt
        ''' </summary>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe)
            GruppenloseTrainer.Add(Gruppe.Trainer)
            Gruppe.Trainer = Nothing
        End Sub

        '''' <summary>
        '''' FreieTrainer=AlleTrainer-EingeteilteTrainer 
        '''' </summary>
        '''' <param name="Trainer"></param>
        'Private Sub FreieTrainerLesen(Trainer As Trainer)
        '    _AlleTrainer.Remove(Trainer)
        'End Sub

        ''' <summary>
        ''' Trainer wird aus dem Club entfernt und 
        ''' in der Ewigen Trainerliste archiviert
        ''' </summary>
        ''' <param name="Trainer"></param>
        Public Sub TrainerArchivieren(Trainer As Trainer)
            GruppenloseTrainer.Remove(Trainer)
            EwigeTrainerliste.Add(Trainer, Now)
        End Sub


        Public Overrides Function ToString() As String
            Return ClubName
        End Function

#End Region

    End Class

End Namespace
