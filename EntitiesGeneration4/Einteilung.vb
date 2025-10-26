Imports System.Collections.ObjectModel
Imports System.Xml.Serialization
Imports Groupies.DataImport

Namespace Entities.Generation4


    Public Class Einteilung
        Inherits BaseModel
        Implements IModel

#Region "Felder"
        Private _Ident As Guid
        Private _Sortierung As Integer
        Private _Benennung As String

        Private _GruppenIDListe As ObservableCollection(Of Guid)
        Private _Gruppenliste As New GruppeCollection

        Private _VerfuegbareTrainerIDListe As ObservableCollection(Of Guid)
        Private _VerfuegbareTrainerListe As New TrainerCollection

        Private _NichtZugewieseneTeilnehmerIDListe As ObservableCollection(Of Guid)
        Private _NichtZugewieseneTeilnehmerListe As New TeilnehmerCollection

#End Region

#Region "Konstruktor"
        ''' <summary>
        ''' Erstellt eine Einteilung mit einer eindeutigen ID
        ''' </summary>
        Public Sub New()
            Ident = Guid.NewGuid()
            Sortierung = -1
        End Sub

        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="Origin"></param>
        Public Sub New(Origin As Einteilung)
            Ident = Origin.Ident
            Benennung = Origin.Benennung
            Sortierung = Origin.Sortierung
            Gruppenliste = Origin.Gruppenliste
            NichtZugewieseneTeilnehmerListe = Origin.NichtZugewieseneTeilnehmerListe
            VerfuegbareTrainerListe = Origin.VerfuegbareTrainerListe
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Eindeutige ID der Einteilung
        ''' </summary>
        ''' <returns></returns>
        Public Property Ident As Guid Implements IModel.Ident
            Get
                Return _Ident
            End Get
            Set(value As Guid)
                _Ident = value
            End Set
        End Property

        ''' <summary>
        ''' Einfache Sortierung der Einteilungen
        ''' </summary>
        ''' <returns></returns>
        Public Property Sortierung() As Integer
            Get
                Return _Sortierung
            End Get
            Set(value As Integer)
                _Sortierung = value
            End Set
        End Property

        ''' <summary>
        ''' Die Benennung. Es können beispielsweise
        ''' die Wochentage verwendet werden
        ''' </summary>
        ''' <returns></returns>
        Public Property Benennung() As String
            Get
                Return _Benennung
            End Get
            Set(value As String)
                _Benennung = value
            End Set
        End Property

        ''' <summary>
        ''' Liste mit den IDs der verfügbaren Gruppen
        ''' </summary>
        ''' <returns></returns>
        Public Property GruppenIDListe() As ObservableCollection(Of Guid)
            Get
                Return _GruppenIDListe
            End Get
            Set(value As ObservableCollection(Of Guid))
                _GruppenIDListe = value
            End Set
        End Property

        ''' <summary>
        ''' Eine Liste aller Gruppen der Einteilung
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Gruppenliste() As GruppeCollection
            Get
                Return _Gruppenliste
            End Get
            Set(value As GruppeCollection)
                _Gruppenliste = value
            End Set
        End Property


        ''' <summary>
        ''' TeilnehmerIDs, die keiner Gruppe zugewiesen sind
        ''' </summary>
        ''' <returns></returns>
        Public Property NichtZugewieseneTeilnehmerIDListe() As ObservableCollection(Of Guid)
            Get
                Return _NichtZugewieseneTeilnehmerIDListe
            End Get
            Set(value As ObservableCollection(Of Guid))
                _NichtZugewieseneTeilnehmerIDListe = value
            End Set
        End Property

        ''' <summary>
        ''' Teilnehmer, die keiner Gruppe zugewiesen sind
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property NichtZugewieseneTeilnehmerListe() As TeilnehmerCollection
            Get
                Return _NichtZugewieseneTeilnehmerListe
            End Get
            Set(value As TeilnehmerCollection)
                _NichtZugewieseneTeilnehmerListe = value
            End Set
        End Property

        ''' <summary>
        ''' TrainerIDs, die keiner Gruppe zugewiesen sind
        ''' </summary>
        ''' <returns></returns>
        Public Property VerfuegbareTrainerIDListe() As ObservableCollection(Of Guid)
            Get
                Return _VerfuegbareTrainerIDListe
            End Get
            Set(value As ObservableCollection(Of Guid))
                _VerfuegbareTrainerIDListe = value
            End Set
        End Property

        ''' <summary>
        ''' Trainer, die keiner Gruppe zugewiesen sind
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property VerfuegbareTrainerListe() As TrainerCollection
            Get
                Return _VerfuegbareTrainerListe
            End Get
            Set(value As TrainerCollection)
                _VerfuegbareTrainerListe = value
            End Set
        End Property

#End Region

#Region "Methoden und Funktionen"

        ''' <summary>
        ''' Trainer dieser Einteilung hinzufügen
        ''' </summary>
        ''' <param name="Trainer"></param>
        Public Sub TrainerHinzufuegen(Trainer As Trainer)
            VerfuegbareTrainerListe.Add(Trainer)
        End Sub

        ''' <summary>
        ''' Trainer aus dieser Einteilung entfernen
        ''' </summary>
        ''' <param name="Trainer"></param>
        Public Sub TrainerLoeschen(Trainer As Trainer)
            VerfuegbareTrainerListe.Remove(Trainer)
        End Sub


        ''' <summary>
        ''' Teilnehmer dieser Einteilung hinzufügen
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerHinzufuegen(Teilnehmer As Teilnehmer)
            NichtZugewieseneTeilnehmerListe.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Der Trainer wird der angegebenen Gruppe zugewiesen
        ''' </summary>
        ''' <param name="Trainer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerEinerGruppeZuweisen(Trainer As Trainer, Gruppe As Gruppe)
            Gruppe.Trainer = Trainer
            VerfuegbareTrainerListe.Remove(Trainer)
        End Sub

        ''' <summary>
        ''' Der Trainer wird aus der angegebenen Gruppe entfernt
        ''' </summary>
        ''' <param name="Gruppe"></param>
        Public Sub TrainerAusGruppeEntfernen(Gruppe As Gruppe)
            VerfuegbareTrainerListe.Add(Gruppe.Trainer)
            Gruppe.Trainer = Nothing
        End Sub

        ''' <summary>
        ''' Teilnehmer aus dieser Einteilung entfernen
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        Public Sub TeilnehmerLoeschen(Teilnehmer As Teilnehmer)
            NichtZugewieseneTeilnehmerListe.Remove(Teilnehmer)
        End Sub




        ''' <summary>
        ''' Der Teilnehmer wird aus der angegebenen Gruppe als Mitglied entfernt
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerAusGruppeEntfernen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Remove(Teilnehmer)
            NichtZugewieseneTeilnehmerListe.Add(Teilnehmer)
        End Sub

        ''' <summary>
        ''' Die angegebene Gruppe bekommt den Teilnehmer als Mitglied
        ''' </summary>
        ''' <param name="Teilnehmer"></param>
        ''' <param name="Gruppe"></param>
        Public Sub TeilnehmerInGruppeEinteilen(Teilnehmer As Teilnehmer, Gruppe As Gruppe)
            Gruppe.Mitgliederliste.Add(Teilnehmer)
            NichtZugewieseneTeilnehmerListe.Remove(Teilnehmer)
        End Sub

        Public Overrides Function ToString() As String
            Return Benennung
        End Function


        Public Sub speichern() Implements IModel.speichern
            Throw New NotImplementedException()
        End Sub



#End Region

    End Class

End Namespace
