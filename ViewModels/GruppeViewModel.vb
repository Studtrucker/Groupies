Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Controller.AppController

Public Class GruppeViewModel
    Inherits ViewModelBase
    Implements IViewModelSpecial

#Region "Konstruktor"

    ''' <summary>
    ''' Parameterloser Konstruktor für den GruppeViewModel.
    ''' Die Instanz benötigt den Modus und ein Gruppen-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppe)
        LeistungsstufenListCollectionView = New CollectionView(AktuellerClub.LeistungsstufenTextliste)
        CurrentUserControl = Datentyp.DatentypUserControl
        OkCommand = New RelayCommand(AddressOf OnOk, Function() IstEingabeGueltig)
        UserControlLoaded = New RelayCommand(AddressOf OnLoaded)
    End Sub

#End Region

#Region "Methoden"
    Public Overrides Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Gruppe.speichern()

        MyBase.OnOk(Me)

    End Sub

    Public Sub OnLoaded() Implements IViewModelSpecial.OnLoaded
        ValidateBenennung()
        ValidateTeilnehmerinfo()
        ValidateSortierung()
        ValidateLeistungsstufe()
    End Sub

#End Region

#Region "Properties"
    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

    Private _Gruppe As Gruppe

    Public Property Gruppe As IModel Implements IViewModelSpecial.Model
        Get
            Return _Gruppe
        End Get
        Set(value As IModel)
            _Gruppe = DirectCast(value, Gruppe)
        End Set
    End Property

    Public Property GruppenID As Guid
        Get
            Return _Gruppe.GruppenID
        End Get
        Set(value As Guid)
            _Gruppe.GruppenID = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Gruppe.Benennung
        End Get
        Set(value As String)
            _Gruppe.Benennung = value
            OnPropertyChanged()
            ValidateBenennung()
        End Set
    End Property

    Public Property AusgabeTeilnehmerinfo As String
        Get
            Return _Gruppe.AusgabeTeilnehmerinfo
        End Get
        Set(value As String)
            _Gruppe.AusgabeTeilnehmerinfo = value
            OnPropertyChanged()
            ValidateTeilnehmerinfo()
        End Set
    End Property

    Public Property Sortierung As String
        Get
            Return _Gruppe.Sortierung
        End Get
        Set(value As String)
            _Gruppe.Sortierung = value
            OnPropertyChanged()
            ValidateSortierung()
        End Set
    End Property

    Public Property Leistungsstufe As Leistungsstufe
        Get
            Return _Gruppe.Leistungsstufe
        End Get
        Set(value As Leistungsstufe)
            _Gruppe.Leistungsstufe = value
            OnPropertyChanged()
            ValidateLeistungsstufe()
        End Set
    End Property

    Public Property Mitgliederliste As TeilnehmerCollection
        Get
            Return _Gruppe.Mitgliederliste
        End Get
        Set(value As TeilnehmerCollection)
            _Gruppe.Mitgliederliste = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property LeistungsstufenListCollectionView As ICollectionView

#End Region

#Region "Gültigkeitsprüfung"

    Private Sub ValidateTeilnehmerinfo()
        ClearErrors(NameOf(_Gruppe.AusgabeTeilnehmerinfo))
        If String.IsNullOrWhiteSpace(_Gruppe.AusgabeTeilnehmerinfo) Then
            AddError(NameOf(_Gruppe.AusgabeTeilnehmerinfo), "Ausgabe für die Teilnehmerinfo darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateLeistungsstufe()
        ClearErrors(NameOf(_Gruppe.Leistungsstufe))
        If _Gruppe.Leistungsstufe Is Nothing Then
            AddError(NameOf(_Gruppe.Leistungsstufe), "Leistungsstufe muss ausgewählt werden.")
        End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Gruppe.Sortierung))
        'If _Gruppe.Sortierung = 0 Then
        '    AddError(NameOf(_Gruppe.Sortierung), "Sortierung darf 0 sein.")
        'End If
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Gruppe.Benennung))
        If String.IsNullOrWhiteSpace(_Gruppe.Benennung) Then
            AddError(NameOf(_Gruppe.Benennung), "Benennung darf nicht leer sein.")
        End If
    End Sub

#End Region

End Class

