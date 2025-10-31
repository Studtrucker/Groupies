Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Xml.Serialization

Namespace Entities.Generation4


    ''' <summary>
    ''' Nicht veränderbare Gruppendaten
    ''' </summary>
    Public Class Gruppenstamm
        Inherits BaseModel
        Implements IModel

#Region "Felder"
        Private _Ident As Guid
        Private _Sortierung As Integer
        Private _Benennung As String
        Private _LeistungsstufeID As Guid
        Private _Leistungsstufe As Leistungsstufe
#End Region

#Region "Konstruktor"

        Public Sub New()
            Ident = Guid.NewGuid()
            Sortierung = -1
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und einer Sortierungszahl
        ''' </summary>
        ''' <param name="Ausgabename"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Ausgabename As String, Sortierung As Integer)
            Me.New
            _Benennung = Ausgabename
            _Sortierung = Sortierung
        End Sub

        ''' <summary>
        ''' Erstellt eine Gruppe unter Angabe des Namens für die Information und der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            Me.New
            _Benennung = Benennung
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
        ''' Der Fk für die Leistungsstufe der Gruppe, einmal fetsgelegt, nicht mehr änderbar
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
        ''' Die Leistungsstufe der Gruppe, einmal fetsgelegt, nicht mehr änderbar
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

