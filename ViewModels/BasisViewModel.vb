Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports System.Windows.Input
Imports Groupies.Interfaces
Imports Groupies.UserControls


Public MustInherit Class BasisViewModel
    Implements INotifyDataErrorInfo, INotifyPropertyChanged, IViewModelBase

#Region "Konstruktor"

    Public Sub New()
        CancelCommand = New RelayCommand(AddressOf OnCancel)
        CloseCommand = New RelayCommand(AddressOf OnClose)
    End Sub

#End Region

#Region "Events"

    Public Event RequestClose As EventHandler(Of Boolean) Implements IViewModelBase.RequestClose

    Public Event Close As EventHandler Implements IViewModelBase.Close

    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region "Commands"
    Public Property OkCommand As ICommand Implements IViewModelBase.OkCommand
    Public Property CancelCommand As ICommand Implements IViewModelBase.CancelCommand
    Public Property CloseCommand As ICommand Implements IViewModelBase.CloseCommand

#End Region

#Region "Properties"

    ''' <summary>
    ''' Der Modus (Erstellen, Bearbeiten, Löschen)
    ''' stellt die Erscheinung und die Funktionalität des Dialogs dar.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Modus As IModus

    ''' <summary>
    ''' Der Datentyp (Trainer, Teilnehmer, Gruppe)
    ''' stellt die Datenstruktur des Dialogs dar.
    ''' </summary>
    ''' <remarks></remarks>
    Public Property Datentyp As IDatentyp

    ''' <summary>
    ''' Der aktuelle UserControl, der im Dialog angezeigt wird.
    ''' </summary>
    Public Property CurrentUserControl As UserControl


    Public ReadOnly Property WindowTitleText As String Implements IViewModelBase.WindowTitleText
        Get
            Return $"{Datentyp.DatentypText} {Modus.Titel}"
        End Get
    End Property

    Public ReadOnly Property WindowTitleIcon As String Implements IViewModelBase.WindowTitleIcon
        Get
            Return Modus.WindowIcon
        End Get
    End Property

    Public ReadOnly Property WindowHeaderImage As String Implements IViewModelBase.WindowHeaderImage
        Get
            Return Datentyp.DatentypIcon
        End Get
    End Property

    Public ReadOnly Property CloseButtonVisibility As Visibility Implements IViewModelBase.CloseButtonVisibility
        Get
            Return Modus.CloseButtonVisibility
        End Get
    End Property

    Public ReadOnly Property OkButtonVisibility As Visibility Implements IViewModelBase.OkButtonVisibility
        Get
            Return Modus.OkButtonVisibility
        End Get
    End Property

    Public ReadOnly Property CancelButtonVisibility As Visibility Implements IViewModelBase.CancelButtonVisibility
        Get
            Return Modus.CancelButtonVisibility
        End Get
    End Property

    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return _fehler.Any(Function(kv) kv.Value.Count > 0)
        End Get
    End Property

    Public ReadOnly Property IstEingabeGueltig As Boolean
        Get
            Return Not HasErrors
        End Get
    End Property

#End Region

#Region "Methoden"

    Public Overridable Sub OnOk(obj As Object)
        ' Businesslogik erfolgreich → Dialog schließen mit OK
        RaiseEvent RequestClose(Me, True)
    End Sub

    Public Sub OnCancel(obj As Object)
        ' Businesslogik nicht erfolgreich → Dialog schließen mit Cancel
        RaiseEvent RequestClose(Me, False)
    End Sub

    Private Sub OnClose(obj As Object)
        ' Businesslogik nicht erfolgreich → Dialog schließen mit Cancel
        RaiseEvent Close(Me, EventArgs.Empty)
    End Sub

    Protected Sub OnPropertyChanged(<Runtime.CompilerServices.CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub

#End Region

#Region "Fehlerverwaltung"

    Private _fehler As New Dictionary(Of String, List(Of String))

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
