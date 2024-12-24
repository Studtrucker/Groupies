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

#Region "Felder"
        Private _Sortierung As Integer
        Private _Benennung As String

#End Region

#Region "Events"


#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt eine Leistungsstufe
        ''' </summary>
        Public Sub New()
            _LeistungsstufeID = Guid.NewGuid()
            _Faehigkeiten = New FaehigkeitCollection
            Benennung = String.Empty
            Sortierung = 0
        End Sub

        ''' <summary>
        ''' Erstellt eine Leistungsstufe mit Angabe der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _LeistungsstufeID = Guid.NewGuid()
            _Benennung = Benennung
            _Faehigkeiten = New FaehigkeitCollection
        End Sub

        ''' <summary>
        ''' Erstellt eine Leistungsstufe mit Angabe Benennung und Sortierung
        ''' </summary>
        ''' <param name="Benennung"></param>
        ''' <param name="Sortierung"></param>
        Public Sub New(Benennung As String, Sortierung As Integer)
            _LeistungsstufeID = Guid.NewGuid()
            _Benennung = Benennung
            _Sortierung = Sortierung
            _Faehigkeiten = New FaehigkeitCollection
        End Sub

#End Region

#Region "Eigenschaften"

        ''' <summary>
        ''' Eindeutige Kennzeichnung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property LeistungsstufeID As Guid

        ''' <summary>
        ''' Sortierungszahl für die Ausgabeinformationen
        ''' </summary>
        ''' <returns></returns>
        <Required()>'AllowEmptyStrings:=False, ErrorMessage:="Die Sortierung ist eine Pflichtangabe"
        Public Property Sortierung As Integer
            Get
                Return _Sortierung
            End Get
            Set(value As Integer)
                Dim errorMessage As String = String.Empty
                If SortierungValidation(value, errorMessage) Then
                    _Errors.Clear()
                Else
                    _Errors(NameOf(Sortierung)) = New List(Of String) From {errorMessage}
                End If
                _Sortierung = value
            End Set
        End Property

        ''' <summary>
        ''' Die Benennung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist eine Pflichtangabe")>
        Public Property Benennung As String
            Get
                Return _Benennung
            End Get
            Set(value As String)

                Dim errorMessage As String = ""
                If BenennungValidation(value, errorMessage) Then
                    _Errors.Clear()
                Else
                    _Errors(NameOf(Benennung)) = New List(Of String) From {errorMessage}
                End If
                _Benennung = value
            End Set
        End Property

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

        Public Overrides Function ToString() As String
            Return Benennung
        End Function

#End Region

#Region "Validation"
        Private Function SortierungValidation(Value As Integer, <Out> ByRef errorMessage As String) As Boolean
            Dim isValid = True
            If Not IsNumeric(Value) Then
                Throw New Exception("Die Sortierung muss numerisch sein")
                isValid = False
            End If

            'If Value <= 0 Then
            '    Throw New ArgumentOutOfRangeException("Die Sortierung muss eine positive Zahl größer als Null sein")
            '    isValid = False
            'End If
            If CurrentClub IsNot Nothing AndAlso CurrentClub.Leistungsstufenliste IsNot Nothing Then
                If CurrentClub.Leistungsstufenliste.ToList.Select(Function(Ls) $"{Ls.Sortierung}").Contains(Value) Then
                    errorMessage = $"Die Sortierung [{Value}] wird bereits verwendet und darf aber nur für eine Leistungsstufe vergeben werden"
                    isValid = False
                End If
            End If
            Return isValid
        End Function

        Private Function BenennungValidation(Value As String, <Out> ByRef errorMessage As String) As Boolean
            Dim isValid = True
            If CurrentClub IsNot Nothing AndAlso CurrentClub.Leistungsstufenliste IsNot Nothing Then
                If CurrentClub.Leistungsstufenliste.ToList.Select(Function(Ls) $"{Ls.Benennung.ToLower}").Contains(Value.ToLower) Then
                    errorMessage = $"Die Benennung [{Value}] wird bereits verwendet und darf aber nur für eine Leistungsstufe vergeben werden"
                    isValid = False
                End If
            End If
            Return isValid
        End Function

#End Region

#Region "Funktionen und Methoden"

#End Region


    End Class
End Namespace
