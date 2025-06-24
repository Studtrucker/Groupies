Imports System.ComponentModel


''' <summary>
''' Basisklasse für ViewModel-Klassen.
''' Diese Klasse implementiert die Interfaces INotifyPropertyChanged und INotifyDataErrorInfo.
''' Sie stellt die Basisfunktionalität für die Änderungsbenachrichtigung und Fehlerverwaltung bereit.
''' </summary>
''' <remarks>Die Klasse ist nicht instanziierbar.</remarks>
Public MustInherit Class ViewModelBase
    Implements INotifyPropertyChanged, INotifyDataErrorInfo

#Region "Events"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

#End Region

#Region "Properties"
    Public ReadOnly Property IstEingabeGueltig As Boolean
        Get
            Return Not HasErrors
        End Get
    End Property

#End Region

#Region "Methoden"
    Protected Sub OnPropertyChanged(propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region

#Region "Fehlerverwaltung"

    Private ReadOnly _fehler As New Dictionary(Of String, List(Of String))

    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return _fehler.Any(Function(kv) kv.Value.Count > 0)
        End Get
    End Property


    Public Sub AddError(prop As String, fehlertext As String)
        If Not _fehler.ContainsKey(prop) Then _fehler(prop) = New List(Of String)
        If Not _fehler(prop).Contains(fehlertext) Then
            _fehler(prop).Add(fehlertext)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(prop))
        End If
        OnPropertyChanged(NameOf(IstEingabeGueltig))
    End Sub

    Public Sub ClearErrors(prop As String)
        If _fehler.ContainsKey(prop) Then
            _fehler.Remove(prop)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(prop))
        End If
        OnPropertyChanged(NameOf(IstEingabeGueltig))
    End Sub

    Public Function GetErrors(propertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
        If String.IsNullOrEmpty(propertyName) Then Return Nothing
        If _fehler.ContainsKey(propertyName) Then Return _fehler(propertyName)
        Return Nothing
    End Function

#End Region

End Class
