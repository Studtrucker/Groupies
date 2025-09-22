Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel
Imports Groupies.Controller

Namespace Entities


    ''' <summary>
    ''' Gruppe mit Angabe seiner Leistungsstufe
    ''' </summary>
    <DefaultProperty("GruppenName")>
    Public Class Gruppe
        Inherits BaseModel
        Implements IModel

#Region "Felder"
        Private _GruppenID = Guid.NewGuid()
        Private _Mitgliederliste = New TeilnehmerCollection
        Private _Sortierung As Integer
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
            GruppenID = OriginGruppe.GruppenID
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
        Public Property GruppenID As Guid Implements IModel.Ident
            Get
                Return _GruppenID
            End Get
            Set(value As Guid)
                _GruppenID = value
            End Set
        End Property


        ''' <summary>
        ''' Die globale Benennung der Gruppe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist ein Pflichtfeld")>
        Public Property Benennung As String

        '''' <summary>
        '''' Der Ausgabename für die Trainerinformation
        '''' TeilnehmerinfoName und die Benennung
        '''' </summary>
        '''' <returns></returns>
        'Public ReadOnly Property AusgabeTrainerinfo As String = $"{Benennung} {[Alias]}"


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


        Private _Leistungsstufe As Leistungsstufe
        ''' <summary>
        ''' Die Leistungsstufe der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property Leistungsstufe As Leistungsstufe
            Get
                Return _Leistungsstufe
            End Get
            Set(value As Leistungsstufe)
                _Leistungsstufe = value
                LeistungsstufeID = value.LeistungsstufeID
            End Set
        End Property

        ''' <summary>
        ''' Die Leistungsstufe der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property LeistungsstufeID As Guid

        ''' <summary>
        ''' Der Trainer der Gruppe
        ''' </summary>
        ''' <returns></returns>
        Public Property Trainer As Trainer

        ''' <summary>
        ''' Liste der Gruppenmitglieder
        ''' </summary>
        ''' <returns></returns>
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
