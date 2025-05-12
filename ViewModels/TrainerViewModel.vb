Imports System.ComponentModel
Imports Microsoft.Office.Interop.Excel
Imports Groupies.Entities

Public Class TrainerViewModel
    Inherits ViewModelBase
    Implements IViewModelSpecial

#Region "Konstruktor"
    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen
        OkCommand = New RelayCommand(AddressOf OnOk, Function() IstEingabeGueltig)
    End Sub

#End Region

#Region "Methoden"
    Public Overrides Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Trainer.speichern()

        MyBase.OnOk(Me)

    End Sub

#End Region

#Region "Properties"

    Private _Trainer As Trainer

    Public Property Trainer As IModel Implements IViewModelSpecial.Model
        Get
            Return _Trainer
        End Get
        Set(value As IModel)
            _Trainer = DirectCast(value, Trainer)
        End Set
    End Property

    Public Property TrainerID As Guid
        Get
            Return _Trainer.TrainerID
        End Get
        Set(value As Guid)
            _Trainer.TrainerID = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Vorname As String
        Get
            Return _Trainer.Vorname
        End Get
        Set(value As String)
            _Trainer.Vorname = value
            OnPropertyChanged()
            ValidateVorname()
        End Set
    End Property

    Public Property Nachname As String
        Get
            Return _Trainer.Nachname
        End Get
        Set(value As String)
            _Trainer.Nachname = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property Spitzname As String
        Get
            Return _Trainer.Spitzname
        End Get
        Set(value As String)
            _Trainer.Spitzname = value
            OnPropertyChanged()
            ValidateSpitzname()
        End Set
    End Property

    Public Property Foto As Byte()
        Get
            Return _Trainer.Foto
        End Get
        Set(value As Byte())
            _Trainer.Foto = value
            OnPropertyChanged()
        End Set
    End Property

    Public Property EMail As String
        Get
            Return _Trainer.EMail
        End Get
        Set(value As String)
            _Trainer.EMail = value
            OnPropertyChanged()
            ValidateEMail()
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return _Trainer.Telefonnummer
        End Get
        Set(value As String)
            _Trainer.Telefonnummer = value
            OnPropertyChanged()
        End Set
    End Property


#End Region

#Region "Gültigkeitsprüfung"
    Private Sub ValidateVorname()
        ClearErrors(NameOf(_Trainer.Vorname))
        If String.IsNullOrWhiteSpace(_Trainer.Vorname) Then
            AddError(NameOf(_Trainer.Vorname), "Name darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateSpitzname()
        ClearErrors(NameOf(_Trainer.Spitzname))
        If String.IsNullOrWhiteSpace(_Trainer.Spitzname) Then
            AddError(NameOf(_Trainer.Spitzname), "Spitzname darf nicht leer sein.")
        End If
        'todo: Hier muss die Function geprüft werden! -> vorhandenen Trainer bearbeiten
        'Funktionsprüfung erledigt: Neuen Trainer anlegen 
        'todo: Es muss eine Liste mit allen Trainern aus dieser Datei geben! Diese wird auf alle Einteilungen verteilt!
        If Groupies.Controller.AppController.AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso Groupies.Controller.AppController.AktuellerClub.SelectedEinteilung.AlleTrainer.Where(Function(tr) Not tr.TrainerID = _Trainer.TrainerID).Where(Function(tr) tr.Spitzname = _Trainer.Spitzname).Count = 1 Then
            AddError(NameOf(_Trainer.Spitzname), $"Der Spitzname {_Trainer.Spitzname} ist bereits vergeben und darf nicht doppelt vergeben werden.")
        End If
    End Sub

    Private Sub ValidateEMail()
        ClearErrors(NameOf(_Trainer.EMail))

        Dim TextVorhanden = _Trainer.EMail.Count() > 0
        Dim KeinAtVorhanden = _Trainer.EMail.IndexOf("@") < 0
        Dim KeinDotVorhanden = _Trainer.EMail.LastIndexOf(".") < 0
        Dim DotNichtNachAt = _Trainer.EMail.IndexOf("@") > _Trainer.EMail.LastIndexOf(".")
        Dim AnzahlExtensionKleiner2 = _Trainer.EMail.Count() - _Trainer.EMail.LastIndexOf(".") - 1 < 2

        If TextVorhanden AndAlso
            (KeinAtVorhanden Or
            KeinDotVorhanden Or
            DotNichtNachAt Or
            AnzahlExtensionKleiner2) Then

            AddError(NameOf(_Trainer.EMail), "Eine gültige eMail Adresse eingeben oder leer lassen")
        End If


    End Sub

#End Region

End Class

