Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.InteropServices


Namespace Entities


    ''' <summary>
    ''' Beschreibt die Leistungsstufe mit einem Satz von Fähigkeiten
    ''' </summary>
    <DefaultProperty("Benennung")>
    Public Class Leistungsstufe
        Inherits BaseModel
        'Implements INotifyDataErrorInfo

#Region "Felder"
        Private _Sortierung As Integer?
        Private _Benennung As String
#End Region

#Region "Events"

        Public Shadows Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs)

#End Region

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt eine Leistungsstufe
        ''' </summary>
        Public Sub New()
            _LeistungsstufeID = Guid.NewGuid()
            _Faehigkeiten = New FaehigkeitCollection
            Benennung = String.Empty
            Sortierung = Nothing
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
        Public Property Sortierung As Nullable(Of Integer)
            Get
                Return _Sortierung
            End Get
            Set(value As Integer?)
                _Sortierung = value
                If Controller.AppController.CurrentClub IsNot Nothing AndAlso Controller.AppController.CurrentClub.Leistungsstufenliste IsNot Nothing Then
                    Dim errorMessage As String = ""
                    If SortierungCheck(_Sortierung, errorMessage) Then
                        Errors.Clear()
                    Else
                        Errors(NameOf(Sortierung)) = New List(Of String) From {errorMessage}
                    End If
                    OnPropertyChanged(NameOf(Sortierung))
                End If
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
                Dim errorMessage As String = ""
                If BenennungCheck(_Benennung, errorMessage) Then
                    Errors.Clear()
                Else
                    Errors(NameOf(Benennung)) = New List(Of String) From {errorMessage}
                End If
                If True Then
                    OnPropertyChanged(NameOf(Benennung))
                End If
            End Set
        End Property

        Private Function BenennungCheck(Value As String, ByRef errorMessage As String)
            errorMessage = ""
            Dim isValid = True
            If Controller.AppController.CurrentClub IsNot Nothing AndAlso Controller.AppController.CurrentClub.Leistungsstufenliste IsNot Nothing Then
                If Controller.AppController.CurrentClub.Leistungsstufenliste.ToList.Select(Function(Ls) $"{Ls.Benennung}").Contains(Value) Then
                    errorMessage = "Die Benennung der Leistungsstufe darf nicht doppelt vergeben werden"
                    isValid = False
                End If
            End If
            If String.IsNullOrEmpty(Trim(Value)) Then
                errorMessage = "Die Benennung ist eine Pflichtangabe"
                isValid = False
            End If
            Return isValid
        End Function

        Private Function SortierungCheck(Value As Integer, ByRef errorMessage As String)
            errorMessage = ""
            Dim isValid = True
            If Controller.AppController.CurrentClub IsNot Nothing AndAlso Controller.AppController.CurrentClub.Leistungsstufenliste IsNot Nothing Then
                If Controller.AppController.CurrentClub.Leistungsstufenliste.ToList.Select(Function(Ls) Ls.Sortierung).Contains(Value) Then
                    errorMessage = "Die Sortierung der Leistungsstufe darf nicht doppelt vergeben werden"
                    isValid = False
                End If
            End If
            If Value <= 0 Then
                errorMessage = "Die Sortierung der Leistungsstufe muss größer als Null sein"
                isValid = False
            End If
            Return isValid
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

        'Public Overloads ReadOnly Property [Error] As String
        '    Get
        '        Return Nothing
        '    End Get
        'End Property

        'Private ReadOnly Property INotifyDataErrorInfo_HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        '    Get
        '        Return _errors.Count > 0
        '    End Get
        'End Property

        'Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

        'Private _errors As New Dictionary(Of String, List(Of String))
        'Public Function INotifyDataErrorInfo_GetErrors(PropertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        '    If PropertyName = NameOf(Benennung) OrElse PropertyName = NameOf(Sortierung) Then
        '        If _errors.ContainsKey(NameOf(Benennung)) Then
        '            Return _errors(NameOf(Benennung))
        '        End If
        '        If _errors.ContainsKey(NameOf(Sortierung)) Then
        '            Return _errors(NameOf(Sortierung))
        '        End If
        '    End If
        '    Return Nothing
        'End Function
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

        'Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        '    If _errors(NameOf(propertyName)).Count > 0 Then
        '        Return _errors(propertyName)
        '    End If
        '    Return Nothing
        'End Function

#End Region

    End Class
End Namespace
