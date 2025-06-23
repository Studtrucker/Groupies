Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Controller.AppController



Namespace Entities


    '''' <summary>
    '''' Beschreibt die Leistungsstufe mit einem Satz von Fähigkeiten
    '''' </summary>
    Public Class Leistungsstufe
        Inherits BaseModel
        Implements IModel

#Region "Felder"

        Private _LeistungsstufeID = Guid.NewGuid()

#End Region

#Region "Events"


#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt eine Leistungsstufe
        ''' </summary>
        Public Sub New()
            _Faehigkeiten = New FaehigkeitCollection
            Benennung = String.Empty
            Sortierung = -1
        End Sub

        ''' <summary>
        ''' Erstellt eine Leistungsstufe mit Angabe der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _Benennung = Benennung
            _Faehigkeiten = New FaehigkeitCollection
        End Sub


#End Region

#Region "Properties"

        ''' <summary>
        ''' Eindeutige Kennzeichnung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property LeistungsstufeID As Guid Implements IModel.Ident
            Get
                Return _LeistungsstufeID
            End Get
            Set(value As Guid)
                _LeistungsstufeID = value
            End Set
        End Property

        ''' <summary>
        ''' Sortierungszahl für die Ausgabeinformationen
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Sortierung ist eine Pflichtangabe (Required)")>
        Public Property Sortierung As Integer


        ''' <summary>
        ''' Die Benennung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist eine Pflichtangabe (Required)")>
        Public Property Benennung As String


        Public ReadOnly Property Beschreibungstext As String
            Get
                Return GeneriereBeschreibungstext()
            End Get
        End Property

        Private Function GeneriereBeschreibungstext() As String
            Dim txt As New StringBuilder
            txt.Append($"Beschreibung: {Beschreibung}")
            Me.Faehigkeiten.OrderBy(Function(f) f.Sortierung).ToList.ForEach(Sub(f) txt.Append($"{f.Benennung}: {f.Beschreibung}{vbNewLine}"))

            Return txt.ToString

        End Function

        ''' <summary>
        ''' Beschreibung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property Beschreibung As String

        ''' <summary>
        ''' Ein Liste von erforderlichen Fähigkeiten für diese Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property Faehigkeiten As FaehigkeitCollection


#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Fügt der Leistungsstufe eine erforderliche Fähigkeit hinzu 
        ''' </summary>
        ''' <param name="Faehigkeit"></param>
        Public Sub FaehigkeitHinzufuegen(Faehigkeit As Faehigkeit)
            _Faehigkeiten.Add(Faehigkeit)
        End Sub

        ''' <summary>
        ''' Entfernt die Leistungsstufe aus den Fähigkeiten 
        ''' </summary>
        ''' <param name="Faehigkeit"></param>
        Public Sub FaehigkeitEntfernen(Faehigkeit As Faehigkeit)
            _Faehigkeiten.Remove(Faehigkeit)
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
