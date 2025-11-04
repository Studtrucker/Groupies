Imports System.ComponentModel.DataAnnotations
Imports System.Collections.ObjectModel
Imports System.Xml.Serialization


Namespace Entities.Generation4


    '''' <summary>
    '''' Beschreibt die Leistungsstufe mit einem Satz von Fähigkeiten
    '''' </summary>
    Public Class Leistungsstufe
        Inherits BaseModel
        Implements IModel

#Region "Felder"

        Private _Ident As Guid
        Private _Sortierung As Integer
        Private _Benennung As String
        Private _Beschreibung As String
        Private _FaehigkeitenListe As FaehigkeitCollection
        Private _FaehigkeitenIDListe As ObservableCollection(Of Guid)
#End Region

#Region "Events"


#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt eine Leistungsstufe
        ''' </summary>
        Public Sub New()
            _FaehigkeitenListe = New FaehigkeitCollection
            _Ident = Guid.NewGuid()
            Benennung = String.Empty
            Sortierung = -1
        End Sub

        ''' <summary>
        ''' Erstellt eine Leistungsstufe mit Angabe der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _Benennung = Benennung
            _FaehigkeitenListe = New FaehigkeitCollection
        End Sub


        ''' <summary>
        ''' Kopierkonstruktor für tiefes Kopieren
        ''' </summary>
        ''' <param name="Origin"></param>
        Public Sub New(origin As Leistungsstufe)
            _Ident = origin.Ident
            _Benennung = origin.Benennung
            _Sortierung = origin.Sortierung
            _Beschreibung = origin.Beschreibung
            _FaehigkeitenListe = origin.Faehigkeiten
        End Sub

#End Region

#Region "Properties"

        ''' <summary>
        ''' Eindeutige Kennzeichnung der Leistungsstufe
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
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Sortierung ist eine Pflichtangabe (Required)")>
        Public Property Sortierung As Integer
            Get
                Return _Sortierung
            End Get
            Set(value As Integer)
                _Sortierung = value
            End Set
        End Property


        ''' <summary>
        ''' Die Benennung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist eine Pflichtangabe (Required)")>
        Public Property Benennung As String
            Get
                Return _Benennung
            End Get
            Set(value As String)
                _Benennung = value
            End Set
        End Property

        ''' <summary>
        ''' Beschreibung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property Beschreibung As String
            Get
                Return _Beschreibung
            End Get
            Set(value As String)
                _Beschreibung = value
            End Set
        End Property

        ''' <summary>
        ''' Ein Liste von erforderlichen Fähigkeiten für diese Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        <XmlIgnore>
        Public Property Faehigkeiten As FaehigkeitCollection
            Get
                Return _FaehigkeitenListe
            End Get
            Set(value As FaehigkeitCollection)
                _FaehigkeitenListe = value
            End Set
        End Property

        Public Property FaehigkeitenIDListe As ObservableCollection(Of Guid)
            Get
                Return _FaehigkeitenIDListe
            End Get
            Set(value As ObservableCollection(Of Guid))
                _FaehigkeitenIDListe = value
            End Set
        End Property


#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Fügt der Leistungsstufe eine erforderliche Fähigkeit hinzu 
        ''' </summary>
        ''' <param name="Faehigkeit"></param>
        Public Sub FaehigkeitHinzufuegen(Faehigkeit As Faehigkeit)
            _FaehigkeitenListe.Add(Faehigkeit)
        End Sub

        ''' <summary>
        ''' Entfernt die Leistungsstufe aus den Fähigkeiten 
        ''' </summary>
        ''' <param name="Faehigkeit"></param>
        Public Sub FaehigkeitEntfernen(Faehigkeit As Faehigkeit)
            _FaehigkeitenListe.Remove(Faehigkeit)
        End Sub


        Public Sub speichern() Implements IModel.speichern
            Throw New NotImplementedException()
        End Sub

        Public Overrides Function ToString() As String
            Return Benennung
        End Function

#End Region

    End Class
End Namespace
