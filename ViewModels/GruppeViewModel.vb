Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Controller.AppController

Public Class GruppeViewModel
    Inherits MasterDetailViewModel(Of Gruppe)
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
        CurrentUserControl = Datentyp.DatentypDetailUserControl
        OkCommand = New RelayCommand(Of Gruppe)(AddressOf OnOk, Function() IstEingabeGueltig)
        UserControlLoaded = New RelayCommand(Of Gruppe)(AddressOf OnLoaded)
        TeilnehmerAusGruppeEntfernen = New RelayCommand(Of Teilnehmer)(AddressOf OnTeilnehmerAusGruppeEntfernen)
    End Sub

#End Region

#Region "Methoden"
    Public Overrides Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Gruppe.speichern()

        MyBase.OnOk(Me)

    End Sub

    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        ValidateBenennung()
        ValidateTeilnehmerinfo()
        ValidateSortierung()
        ValidateLeistungsstufe()
    End Sub

    Private Sub OnTeilnehmerAusGruppeEntfernen()
        MessageBox.Show("Teilnehmer raus")
    End Sub

#End Region

#Region "Properties"
    Public ReadOnly Property OkCommand As ICommand

    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded
    Public Property TeilnehmerAusGruppeEntfernen As ICommand


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
            OnPropertyChanged(NameOf(GruppenID))
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Gruppe.Benennung
        End Get
        Set(value As String)
            _Gruppe.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
        End Set
    End Property

    Public Property AusgabeTeilnehmerinfo As String
        Get
            Return _Gruppe.AusgabeTeilnehmerinfo
        End Get
        Set(value As String)
            _Gruppe.AusgabeTeilnehmerinfo = value
            OnPropertyChanged(NameOf(AusgabeTeilnehmerinfo))
            ValidateTeilnehmerinfo()
        End Set
    End Property

    Public Property Sortierung As String
        Get
            Return _Gruppe.Sortierung
        End Get
        Set(value As String)
            _Gruppe.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
        End Set
    End Property

    Public Property Leistungsstufe As Leistungsstufe
        Get
            Return _Gruppe.Leistungsstufe
        End Get
        Set(value As Leistungsstufe)
            _Gruppe.Leistungsstufe = value
            OnPropertyChanged(NameOf(Leistungsstufe))
            ValidateLeistungsstufe()
        End Set
    End Property

    Public Property Mitgliederliste As TeilnehmerCollection
        Get
            Return _Gruppe.Mitgliederliste
        End Get
        Set(value As TeilnehmerCollection)
            _Gruppe.Mitgliederliste = value
            OnPropertyChanged(NameOf(Mitgliederliste))
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

