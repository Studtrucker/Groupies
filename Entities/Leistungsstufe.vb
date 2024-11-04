Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Namespace Entities


    ''' <summary>
    ''' Beschreibt die Leistungsstufe mit einem Satz von Fähigkeiten
    ''' </summary>
    <DefaultProperty("Benennung")>
    Public Class Leistungsstufe
        Inherits BaseModel
        Implements INotifyDataErrorInfo

#Region "Felder"
        Private _Sortierung As Integer?
        Private _Benennung As String
#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt eine Leistungsstufe
        ''' </summary>
        Public Sub New()
            _LeistungsstufeID = Guid.NewGuid()
            _Faehigkeiten = New FaehigkeitCollection
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
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Sortierung ist eine Pflichtangabe")>
        Public Property Sortierung As Integer?
            Get
                Return _Sortierung
            End Get
            Set(value As Integer?)

            End Set
        End Property

        ''' <summary>
        ''' Die Benennung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property Benennung As String
            Get
                Return _Benennung
            End Get
            Set(value As String)
                _Benennung = value
                ' wpf Seite 715
                If Controller.AppController.CurrentClub.Leistungsstufenliste.Contains(New Leistungsstufe With {.Benennung = _Benennung}) Then
                    _errors.Add(NameOf(Benennung), New List(Of String) From {$"Die Leistungsstufe {value} wurde bereits definiert"})

                End If
            End Set
        End Property

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

        Public Overloads ReadOnly Property [Error] As String
            Get
                Return Nothing
            End Get
        End Property

        Private ReadOnly Property INotifyDataErrorInfo_HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
            Get
                Return _errors.Count > 0
            End Get
        End Property

        Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

        Private _errors As New Dictionary(Of String, List(Of String))
        Public Function INotifyDataErrorInfo_GetErrors(PropertyName As String)
            If PropertyName = NameOf(Benennung) OrElse PropertyName = NameOf(Sortierung) Then
                If _errors.ContainsKey(NameOf(Benennung)) Then
                    Return _errors(NameOf(Benennung))
                End If
                If _errors.ContainsKey(NameOf(Sortierung)) Then
                    Return _errors(NameOf(Sortierung))
                End If
            End If
            Return Nothing
        End Function
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

        Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
            Throw New NotImplementedException()
        End Function

#End Region

    End Class
End Namespace
