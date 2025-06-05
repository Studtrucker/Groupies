Imports System.Data.Common
Imports Groupies.Entities

Public Class EinteilungViewModel
    Inherits MasterDetailViewModel(Of Einteilung)
    Implements IViewModelSpecial


#Region "Konstruktor"
    ''' <summary>
    ''' Parameterloser Konstruktor für den EinteilungViewModel.
    ''' Die Instanz benötigt den Modus und ein Einteilungs-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        UserControlLoaded = New RelayCommand(Of Einteilung)(AddressOf OnLoaded)
        OkCommand = New RelayCommand(Of Einteilung)(AddressOf OnOk, Function() IstEingabeGueltig)
    End Sub

#End Region


#Region "Properties"
    Public ReadOnly Property OkCommand As ICommand
    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

    Private _Einteilung As Einteilung

    Public Property Einteilung As IModel Implements IViewModelSpecial.Model
        Get
            Return _Einteilung
        End Get
        Set(value As IModel)
            _Einteilung = value
        End Set
    End Property

    Public Property ID As Guid
        Get
            Return _Einteilung.Ident
        End Get
        Set(value As Guid)
            _Einteilung.Ident = value
        End Set
    End Property

    Public Property Benennung As String
        Get
            Return _Einteilung.Benennung
        End Get
        Set(value As String)
            _Einteilung.Benennung = value
        End Set
    End Property

    Public Property Sortierung As String
        Get
            Return _Einteilung.Sortierung
        End Get
        Set(value As String)
            _Einteilung.Sortierung = value
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


    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        'Throw New NotImplementedException()
    End Sub

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Einteilung.speichern()

    End Sub

End Class
