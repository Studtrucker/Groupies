Imports System.ComponentModel
Imports System.IO
Imports Groupies.DataImport
Imports Groupies.Entities

Public Class TrainerViewModel
    Inherits MasterDetailViewModel(Of Entities.Trainer)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Trainer As Entities.Trainer
    Private ReadOnly _zulaessigeEndungen As String() = {".jpg", ".gif", ".png"}
#End Region

#Region "Events"

    Public Event DragOver As EventHandler

    Public Event Drop As EventHandler

    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent

#End Region

#Region "Konstruktor"

    ''' <summary>
    ''' Parameterloser Konstruktor für den TrainerViewModel.
    ''' Die Instanz benötigt den Modus und ein Trainer-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Public Sub New()
        'MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

        UserControlLoaded = New RelayCommand(Of Entities.Trainer)(AddressOf OnLoaded)
        DropCommand = New RelayCommand(Of DragEventArgs)(AddressOf OnDrop)
        DragOverCommand = New RelayCommand(Of DragEventArgs)(AddressOf OnDragOver)
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)

        NeuCommand = New RelayCommand(Of Entities.Trainer)(Sub() OnNeu(), Function() CanNeu())
        'LoeschenCommand = New RelayCommand(Of Entities.Trainer)(Sub() OnLoeschen(), Function() CanLoeschen())
        BearbeitenCommand = New RelayCommand(Of Entities.Trainer)(Sub() OnBearbeiten(), Function() CanBearbeiten())
    End Sub

#End Region

#Region "Properties"

    Public Property Trainer As IModel Implements IViewModelSpecial.Model
        Get
            Return _Trainer
        End Get
        Set(value As IModel)
            _Trainer = DirectCast(value, Entities.Trainer)
        End Set
    End Property

    Public Property TrainerID As Guid
        Get
            Return _Trainer.TrainerID
        End Get
        Set(value As Guid)
            _Trainer.TrainerID = value
            OnPropertyChanged(NameOf(TrainerID))
            ValidateTrainerID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Vorname As String
        Get
            Return _Trainer.Vorname
        End Get
        Set(value As String)
            _Trainer.Vorname = value
            OnPropertyChanged(NameOf(Vorname))
            ValidateVorname()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Nachname As String
        Get
            Return _Trainer.Nachname
        End Get
        Set(value As String)
            _Trainer.Nachname = value
            OnPropertyChanged(NameOf(Nachname))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Spitzname As String
        Get
            Return _Trainer.Spitzname
        End Get
        Set(value As String)
            _Trainer.Spitzname = value
            OnPropertyChanged(NameOf(Spitzname))
            ValidateSpitzname()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Foto As Byte()
        Get
            Return _Trainer.Foto
        End Get
        Set(value As Byte())
            _Trainer.Foto = value
            OnPropertyChanged(NameOf(Foto))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property EMail As String
        Get
            Return _Trainer.EMail
        End Get
        Set(value As String)
            _Trainer.EMail = value
            OnPropertyChanged(NameOf(EMail))
            ValidateEMail()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return _Trainer.Telefonnummer
        End Get
        Set(value As String)
            _Trainer.Telefonnummer = value
            OnPropertyChanged(NameOf(Telefonnummer))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Private Overloads Property Daten As IEnumerable(Of IModel) Implements IViewModelSpecial.Daten
        Get
            Return Items
        End Get
        Set(value As IEnumerable(Of IModel))
            Items = value
        End Set
    End Property

    Public Overloads ReadOnly Property IstEingabeGueltig As Boolean Implements IViewModelSpecial.IstEingabeGueltig
        Get
            Return MyBase.IstEingabeGueltig
        End Get
    End Property

    Private Property CanBearbeiten() As Boolean
        Get
            Return True ' ItemsView.CurrentPosition + 1 < Items.Count
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanBearbeiten))
        End Set
    End Property


    Private Property CanNeu() As Boolean
        Get
            Return False ' ItemsView.CurrentPosition + 1 < Items.Count
        End Get
        Set(value As Boolean)
            OnPropertyChanged(NameOf(CanNeu))
        End Set
    End Property

#End Region

#Region "Command-Properties"
    Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand
    Public ReadOnly Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded

    Public ReadOnly Property DropCommand As ICommand

    Public ReadOnly Property DragOverCommand As ICommand

    'Public ReadOnly Property LoeschenCommand As ICommand Implements IViewModelSpecial.LoeschenCommand
    Public ReadOnly Property BearbeitenCommand As ICommand Implements IViewModelSpecial.BearbeitenCommand
    Public ReadOnly Property NeuCommand As ICommand Implements IViewModelSpecial.NeuCommand

#End Region

#Region "Methoden"

    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk

        ' Hier können Sie die Logik für den OK-Button implementieren
        _Trainer.speichern()

    End Sub

    Private Sub OnDrop(e As DragEventArgs)
        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files = CType(e.Data.GetData(DataFormats.FileDrop), String())
            Dim validFiles = files.Where(Function(f) _zulaessigeEndungen.Contains(Path.GetExtension(f).ToLower())).ToArray()

            If validFiles.Any() Then
                MessageBox.Show("Zulässige Dateien: " & Environment.NewLine & String.Join(Environment.NewLine, validFiles))
            Else
                'todo: Hier muss eine Fehlermeldung kommen, dass nur jpg, gif und png Dateien erlaubt sind.Also die Variable zulaessigeEndungen verwenden.
                MessageBox.Show("Nur .txt oder .pdf Dateien sind erlaubt.")
            End If
        End If
        e.Handled = True
    End Sub

    Private Sub OnDragOver(e As DragEventArgs)

        If e.Data.GetDataPresent(DataFormats.FileDrop) Then
            Dim files = CType(e.Data.GetData(DataFormats.FileDrop), String())
            Dim allValid = files.All(Function(f) _zulaessigeEndungen.Contains(Path.GetExtension(f).ToLower()))

            e.Effects = If(allValid, DragDropEffects.Copy, DragDropEffects.None)
        Else
            e.Effects = DragDropEffects.None
        End If
        e.Handled = True

    End Sub


    Public Overloads Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        ValidateVorname()
        ValidateSpitzname()
        ValidateEMail()
        ValidateTrainerID()
    End Sub

    Public Sub OnNeu() 'Implements IViewModelSpecial.NeuCommand
        ' Hier können Sie die Logik für den Neu-Button implementieren
        '_Trainer = New Entities.Trainer()
        'OnPropertyChanged(NameOf(Trainer))
        'RaiseEvent ModelChangedEvent(Me, HasErrors)
    End Sub
    'Public Sub OnLoeschen() 'Implements IViewModelSpecial.LoeschenCommand
    '    ' Hier können Sie die Logik für den Löschen-Button implementieren
    '    'If _Trainer IsNot Nothing Then
    '    '    _Trainer.loeschen()
    '    '    _Trainer = Nothing
    '    '    OnPropertyChanged(NameOf(Trainer))
    '    '    RaiseEvent ModelChangedEvent(Me, HasErrors)
    '    'End If
    '    Items.Remove(SelectedItem)
    '    OnPropertyChanged(NameOf(Trainer))
    '    RaiseEvent ModelChangedEvent(Me, HasErrors)
    'End Sub

    Public Sub OnBearbeiten() ' Implements IViewModelSpecial.BearbeitenCommand
        ' Hier können Sie die Logik für den Bearbeiten-Button implementieren
        'If _Trainer IsNot Nothing Then
        '    ' Beispiel: Öffnen eines Dialogs zur Bearbeitung des Trainers
        '    Dim dialog = New TrainerEditDialog(_Trainer)
        '    If dialog.ShowDialog() = True Then
        '        _Trainer.speichern()
        '        OnPropertyChanged(NameOf(Trainer))
        '        RaiseEvent ModelChangedEvent(Me, HasErrors)
        '    End If
        'End If
    End Sub
#End Region

#Region "Validation"
    Private Sub ValidateVorname()
        ClearErrors(NameOf(_Trainer.Vorname))
        If String.IsNullOrWhiteSpace(_Trainer.Vorname) Then
            AddError(NameOf(_Trainer.Vorname), "Name darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateSpitzname()
        ClearErrors(NameOf(_Trainer.Spitzname))
        If _Trainer.Spitzname Is Nothing OrElse String.IsNullOrWhiteSpace(_Trainer.Spitzname) Then
            AddError(NameOf(_Trainer.Spitzname), "Spitzname darf nicht leer sein.")
        End If

        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Trainer, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Trainer.Spitzname), result.ErrorContent.ToString())
        End If
    End Sub

    Private Sub ValidateEMail()
        ClearErrors(NameOf(_Trainer.EMail))

        If _Trainer.EMail IsNot Nothing Then

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

        End If

    End Sub

    Private Sub ValidateTrainerID()
        ClearErrors(NameOf(_Trainer.TrainerID))
        If _Trainer.TrainerID = Guid.Empty Then
            AddError(NameOf(_Trainer.TrainerID), "TrainerID muss eingetragen werden.")
        End If
    End Sub
#End Region

End Class

