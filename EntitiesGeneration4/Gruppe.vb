Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Xml.Serialization

Namespace Entities.Generation4


    ''' <summary>
    ''' Gruppe mit Angabe seiner Leistungsstufe
    ''' </summary>
    <DefaultProperty("GruppenName")>
    Public Class Gruppe
        Inherits BaseModel
        Implements IModel

#Region "Felder"
        Private _Ident As Guid = Guid.NewGuid()
        Private _Sortierung As Integer
        Private _Benennung As String
        Private _LeistungsstufeID As Guid
        Private _Leistungsstufe As Leistungsstufe
        Private _MitgliederIDListe As ObservableCollection(Of Guid)
        Private _Mitgliederliste = New TeilnehmerCollection
        Private _TrainerID As Guid
        Private _Trainer As Trainer
#End Region

#Region "Konstruktor"

        Public Sub New()
            Sortierung = -1
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und einer Sortierungszahl
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Ausgabename As String, Sortierung As Integer)
            _Benennung = Ausgabename
            _Sortierung = Sortierung
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _Benennung = Benennung
        End Sub

        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="OriginGruppe"></param>
        Public Sub New(OriginGruppe As Gruppe)
            Ident = OriginGruppe.Ident
            Leistungsstufe = OriginGruppe.Leistungsstufe
            Benennung = OriginGruppe.Benennung
            Sortierung = OriginGruppe.Sortierung
            Trainer = OriginGruppe.Trainer
            Mitgliederliste = OriginGruppe.Mitgliederliste
        End Sub

#End Region

#Region "Properties"
        ''' <summary>
        ''' Eindeutige Gruppenkennung
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
        ''' Sortierungszahl für die Ausgabeinformationen 
        ''' </summary>
        ''' <returns></returns>
        Public Property Sortierung As Integer
            Get
                Return _Sortierung
            End Get
            Set(value As Integer)
                _Sortierung = value
                OnPropertyChanged(NameOf(Sortierung))
            End Set
        End Property

        ''' <summary>
        ''' Die globale Benennung der Gruppe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist ein Pflichtfeld")>
        Public Property Benennung As String
            Get
                Return _Benennung
            End Get
            Set(value As String)
                _Benennung = value
                OnPropertyChanged(NameOf(Benennung))
            End Set
        End Property

        ''' <summary>
        ''' Der Fk für die Leistungsstufe der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property LeistungsstufeID As Guid
            Get
                Return _LeistungsstufeID
            End Get
            Set(value As Guid)
                _LeistungsstufeID = value
                OnPropertyChanged(NameOf(LeistungsstufeID))
            End Set
        End Property


        ''' <summary>
        ''' Die Leistungsstufe der Gruppe
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Leistungsstufe As Leistungsstufe
            Get
                Return _Leistungsstufe
            End Get
            Set(value As Leistungsstufe)
                _Leistungsstufe = value
                OnPropertyChanged(NameOf(Leistungsstufe))
            End Set
        End Property

        ''' <summary>
        ''' Der FK für den Trainer der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property TrainerID As Guid
            Get
                Return _TrainerID
            End Get
            Set(value As Guid)
                _TrainerID = value
                OnPropertyChanged(NameOf(TrainerID))
            End Set
        End Property

        ''' <summary>
        ''' Der Trainer der Gruppe
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Trainer As Trainer
            Get
                Return _Trainer
            End Get
            Set(value As Trainer)
                _Trainer = value
                OnPropertyChanged(NameOf(Trainer))
            End Set
        End Property

        ''' <summary>
        ''' Liste der GruppenmitgliederIDs
        ''' </summary>
        ''' <returns></returns>
        Public Property MitgliederIDListe As ObservableCollection(Of Guid)
            Get
                Return _MitgliederIDListe
            End Get
            Set(value As ObservableCollection(Of Guid))
                _MitgliederIDListe = value
            End Set
        End Property

        ''' <summary>
        ''' Liste der Gruppenmitglieder
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Mitgliederliste As TeilnehmerCollection
            Get
                Return _Mitgliederliste
            End Get
            Set(value As TeilnehmerCollection)
                _Mitgliederliste = value
            End Set
        End Property


#End Region

#Region "Funktionen und Methoden"

        Public Overrides Function ToString() As String
            Return Benennung
        End Function

        Public Sub speichern() Implements IModel.speichern
            MessageBox.Show("Gruppe speichern")
        End Sub
#End Region

    End Class

End Namespace
