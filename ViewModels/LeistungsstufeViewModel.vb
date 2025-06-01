Imports System.ComponentModel.DataAnnotations
Imports Groupies.Entities

Public Class LeistungsstufeViewModel
    Inherits MasterDetailViewModel(Of Leistungsstufe)
    Implements IViewModelSpecial

#Region "Konstruktor"

    ''' <summary>
    ''' Parameterloser Konstruktor für den LeistungsstufeViewModel.
    ''' Die Instanz benötigt den Modus und ein Leistungsstufe-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Leistungsstufe)
        'CurrentUserControl = Datentyp.DatentypDetailUserControl
        OkCommand = New RelayCommand(Of Leistungsstufe)(AddressOf OnOk, Function() IstEingabeGueltig)
        UserControlLoaded = New RelayCommand(Of Leistungsstufe)(AddressOf OnLoaded)
    End Sub

#End Region

#Region "Methoden"

    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        Validate()
    End Sub

    Public Overrides Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Leistungsstufe.speichern()

        MyBase.OnOk(Me)
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property OkCommand As ICommand


    Private _Leistungsstufe As Entities.Leistungsstufe

    Public Property Leistungsstufe As IModel Implements IViewModelSpecial.Model
        Get
            Return _Leistungsstufe
        End Get
        Set(value As IModel)
            _Leistungsstufe = DirectCast(value, Entities.Leistungsstufe)
        End Set
    End Property

    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

    Public Property Sortierung() As Integer
        Get
            Return _Leistungsstufe.Sortierung
        End Get
        Set(ByVal value As Integer)
            _Leistungsstufe.Sortierung = value
            OnPropertyChanged(NameOf(Sortierung))
            ValidateSortierung()
        End Set
    End Property

    Public Property Beschreibung() As String
        Get
            Return _Leistungsstufe.Beschreibung
        End Get
        Set(ByVal value As String)
            _Leistungsstufe.Beschreibung = value
            OnPropertyChanged(NameOf(Beschreibung))
            ValidateBeschreibung()
        End Set
    End Property

    Public Property Benennung() As String
        Get
            Return _Leistungsstufe.Benennung
        End Get
        Set(ByVal value As String)
            _Leistungsstufe.Benennung = value
            OnPropertyChanged(NameOf(Benennung))
            ValidateBenennung()
        End Set
    End Property

    Public Property Faehigkeiten() As FaehigkeitCollection
        Get
            Return _Leistungsstufe.Faehigkeiten
        End Get
        Set(ByVal value As FaehigkeitCollection)
            _Leistungsstufe.Faehigkeiten = value
            OnPropertyChanged(NameOf(Faehigkeiten))
            ValidateFaehigkeiten()
        End Set
    End Property

    Private Overloads Property Items As IEnumerable(Of IModel) Implements IViewModelSpecial.Items
        Get
            Return MyBase.Items
        End Get
        Set(value As IEnumerable(Of IModel))
            MyBase.Items = value
        End Set
    End Property

#End Region

#Region "Gültigkeitsprüfung"
    Private Sub Validate()
        ValidateSortierung
        ValidateBeschreibung
        ValidateFaehigkeiten
        ValidateBenennung
    End Sub

    Private Sub ValidateBenennung()
        ClearErrors(NameOf(_Leistungsstufe.Benennung))
        If String.IsNullOrWhiteSpace(_Leistungsstufe.Benennung) Then
            AddError(NameOf(_Leistungsstufe.Benennung), "Benennung darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateFaehigkeiten()
        ClearErrors(NameOf(_Leistungsstufe.Faehigkeiten))
        If _Leistungsstufe.Faehigkeiten Is Nothing OrElse _Leistungsstufe.Faehigkeiten.Count = 0 Then
            'AddError(NameOf(_Leistungsstufe.Faehigkeiten), "Es müssen mindestens 1 Fähigkeit zugeordnet werden.")
        End If
    End Sub

    Private Sub ValidateBeschreibung()
        ClearErrors(NameOf(_Leistungsstufe.Beschreibung))
        If String.IsNullOrWhiteSpace(_Leistungsstufe.Beschreibung) Then
            'AddError(NameOf(_Leistungsstufe.Beschreibung), "Beschreibung darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateSortierung()
        ClearErrors(NameOf(_Leistungsstufe.Sortierung))
        If _Leistungsstufe.Sortierung = 0 Then
            AddError(NameOf(_Leistungsstufe.Sortierung), "Sortierung darf nicht 0 sein.")
        End If
    End Sub


#End Region

End Class
