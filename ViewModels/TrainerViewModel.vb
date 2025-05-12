Imports System.ComponentModel
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Entities

Public Class TrainerViewModel
    Inherits ViewModelBase
    Implements INotifyDataErrorInfo, INotifyPropertyChanged, IViewModel


#Region "Events"
    Public Shadows Event RequestClose As EventHandler(Of Boolean)
#End Region

#Region "Konstruktor"
    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

        OkCommand = New RelayCommand(AddressOf OnOK, Function() IstEingabeGueltig)
        'AddHandler RequestClose, AddressOf HandleCloseRequest

    End Sub

#End Region

#Region "Methoden"
    Public Overloads Sub OnOK()
        ' Hier können Sie die Logik für den OK-Button implementieren
        Trainer.speichern()

        MyBase.OnOK(Me)

    End Sub

    Public Overloads Sub OnCancel()
        ' Hier können Sie die Logik für den OK-Button implementieren
        MyBase.OnCancel(Me)
    End Sub

#End Region

#Region "Properties"

    'Public Property OkCommand As ICommand
    'Public Property CancelCommand As ICommand 'Implements IViewModel.CancelCommand

    Public Property Trainer As Trainer 'Implements IViewModel.DatenObjekt

    Public Property TrainerID As Guid
        Get
            Return Trainer.TrainerID
        End Get
        Set(value As Guid)
            Trainer.TrainerID = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Vorname As String
        Get
            Return Trainer.Vorname
        End Get
        Set(value As String)
            Trainer.Vorname = value
            OnPropertyChanged()
            ValidateVorname()
        End Set
    End Property

    Public Property Nachname As String
        Get
            Return Trainer.Nachname
        End Get
        Set(value As String)
            Trainer.Nachname = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Spitzname As String
        Get
            Return Trainer.Spitzname
        End Get
        Set(value As String)
            Trainer.Spitzname = value
            OnPropertyChanged()
            ValidateSpitzname()
        End Set
    End Property

    Public Property Foto As Byte()
        Get
            Return Trainer.Foto
        End Get
        Set(value As Byte())
            Trainer.Foto = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property EMail As String
        Get
            Return Trainer.EMail
        End Get
        Set(value As String)
            Trainer.EMail = value
            OnPropertyChanged()
            ValidateEMail()
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return Trainer.Telefonnummer
        End Get
        Set(value As String)
            Trainer.Telefonnummer = value
            OnPropertyChanged()
        End Set
    End Property

    Public ReadOnly Property IstEingabeGueltig As Boolean
        Get
            Return Not HasErrors
        End Get
    End Property

#End Region

#Region "Gültigkeitsprüfung"
    Private Sub ValidateVorname()
        ClearErrors(NameOf(Trainer.Vorname))
        If String.IsNullOrWhiteSpace(Trainer.Vorname) Then
            AddError(NameOf(Trainer.Vorname), "Name darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateSpitzname()
        ClearErrors(NameOf(Trainer.Spitzname))
        If String.IsNullOrWhiteSpace(Trainer.Spitzname) Then
            AddError(NameOf(Trainer.Spitzname), "Spitzname darf nicht leer sein.")
        End If
        'todo: Hier muss die Function geprüft werden! -> vorhandenen Trainer bearbeiten
        'Funktionsprüfung erledigt: Neuen Trainer anlegen 
        'todo: Es muss eine Liste mit allen Trainern aus dieser Datei geben! Diese wird auf alle Einteilungen verteilt!
        If Groupies.Controller.AppController.AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso Groupies.Controller.AppController.AktuellerClub.SelectedEinteilung.AlleTrainer.Where(Function(tr) Not tr.TrainerID = Trainer.TrainerID).Where(Function(tr) tr.Spitzname = Trainer.Spitzname).Count = 1 Then
            AddError(NameOf(Trainer.Spitzname), $"Der Spitzname {Trainer.Spitzname} ist bereits vergeben und darf nicht doppelt vergeben werden.")
        End If
    End Sub

    Private Sub ValidateEMail()
        ClearErrors(NameOf(Trainer.EMail))
        Dim countString = Trainer.EMail.Count()
        Dim indexAt = Trainer.EMail.IndexOf("@")
        Dim indexDot = Trainer.EMail.LastIndexOf(".")
        Dim CountExtension = countString - indexDot + 1

        Dim TextVorhanden = countString > 0
        Dim KeinAtVorhanden = indexAt < 0
        Dim KeinDotVorhanden = indexDot < 0
        Dim DotNichtNachAt = indexAt > indexDot
        Dim ExtensionAnzahlNichtGroesser1 = countString - indexDot < 2

        If TextVorhanden Then
            If KeinAtVorhanden Or KeinDotVorhanden Or DotNichtNachAt Or ExtensionAnzahlNichtGroesser1 Then
                AddError(NameOf(Trainer.EMail), "Eine gültige eMail Adresse eingeben oder leer lassen")
            End If
        End If


    End Sub

#End Region

#Region "INotifyPropertyChanged"

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Sub OnPropertyChanged(<Runtime.CompilerServices.CallerMemberName> Optional name As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
    End Sub


#End Region

#Region "Fehlerverwaltung"

    Private _fehler As New Dictionary(Of String, List(Of String))

    Private Sub AddError(prop As String, fehlertext As String)
        If Not _fehler.ContainsKey(prop) Then _fehler(prop) = New List(Of String)
        If Not _fehler(prop).Contains(fehlertext) Then
            _fehler(prop).Add(fehlertext)
            RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(prop))
        End If
        OnPropertyChanged(NameOf(IstEingabeGueltig))
    End Sub

    Private Sub ClearErrors(prop As String)
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

#Region "INotifyDataErrorInfo"


    Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged
    Private Event IViewModel_RequestClose As EventHandler(Of Boolean) Implements IViewModel.RequestClose
    Private Event IViewModel_Close As EventHandler Implements IViewModel.Close

    Public ReadOnly Property HasErrors As Boolean Implements INotifyDataErrorInfo.HasErrors
        Get
            Return _fehler.Any(Function(kv) kv.Value.Count > 0)
        End Get
    End Property

    Private ReadOnly Property IViewModel_WindowTitleText As String Implements IViewModel.WindowTitleText
        Get
            Return WindowTitleText
        End Get
    End Property

    Private ReadOnly Property IViewModel_WindowTitleIcon As String Implements IViewModel.WindowTitleIcon
        Get
            Return WindowTitleIcon
        End Get
    End Property

    Private ReadOnly Property IViewModel_WindowHeaderImage As String Implements IViewModel.WindowHeaderImage
        Get
            Return WindowHeaderImage
        End Get
    End Property

    Private ReadOnly Property IViewModel_CancelButtonVisibility As Visibility Implements IViewModel.CancelButtonVisibility
        Get
            Return CancelButtonVisibility
        End Get
    End Property

    Private ReadOnly Property IViewModel_OkButtonVisibility As Visibility Implements IViewModel.OkButtonVisibility
        Get
            Return OkButtonVisibility
        End Get
    End Property

    Private ReadOnly Property IViewModel_CloseButtonVisibility As Visibility Implements IViewModel.CloseButtonVisibility
        Get
            Return CloseButtonVisibility
        End Get
    End Property

    Private Property IViewModel_OkCommand As ICommand Implements IViewModel.OkCommand
        Get
            Return OkCommand
        End Get
        Set(value As ICommand)
            OkCommand = value
        End Set
    End Property

    Private Property IViewModel_CancelCommand As ICommand Implements IViewModel.CancelCommand
        Get
            Return CancelCommand
        End Get
        Set(value As ICommand)
            CancelCommand = value
        End Set
    End Property

    Private Property IViewModel_CloseCommand As ICommand Implements IViewModel.CloseCommand
        Get
            Return CloseCommand
        End Get
        Set(value As ICommand)
            CloseCommand = value
        End Set
    End Property

#End Region

End Class

