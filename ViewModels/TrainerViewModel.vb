Imports System.ComponentModel
Imports System.IO
Imports Groupies.Controller
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

        DropCommand = New RelayCommand(Of DragEventArgs)(AddressOf OnDrop)
        DragOverCommand = New RelayCommand(Of DragEventArgs)(AddressOf OnDragOver)
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)

        NeuCommand = New RelayCommand(Of Entities.Trainer)(AddressOf OnNeu, Function() CanNeu())
        BearbeitenCommand = New RelayCommand(Of Entities.Trainer)(AddressOf OnBearbeiten, Function() CanBearbeiten())
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
            OnPropertyChanged(NameOf(HatFoto))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public ReadOnly Property HatFoto As Boolean
        Get
            Return _Trainer.HatFoto
        End Get
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
            OnPropertyChanged(NameOf(Daten))
            OnPropertyChanged(NameOf(Items))
        End Set
    End Property

    Public Overloads ReadOnly Property IstEingabeGueltig As Boolean Implements IViewModelSpecial.IstEingabeGueltig
        Get
            Return MyBase.IstEingabeGueltig
        End Get
    End Property

#End Region

#Region "Command-Properties"
    Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand

    Public ReadOnly Property DropCommand As ICommand

    Public ReadOnly Property DragOverCommand As ICommand

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
                Foto = File.ReadAllBytes(validFiles(0))
            Else
                'todo: Hier muss eine Fehlermeldung kommen, dass nur jpg, gif und png Dateien erlaubt sind.Also die Variable zulaessigeEndungen verwenden.
                Dim Fm As String = String.Empty
                _zulaessigeEndungen.ToList.ForEach(Sub(zE) Fm &= $"{zE}, ")
                Fm = Left(Fm, Fm.Length - 2)
                MessageBox.Show($"Nur {Fm} Dateien sind erlaubt.")
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
        If _Trainer IsNot Nothing Then
            ValidateVorname()
            ValidateSpitzname()
            ValidateEMail()
            ValidateTrainerID()
        End If
    End Sub

    Public Overloads Sub OnNeu(obj As Object) 'Implements IViewModelSpecial.OnNeu
        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Trainer),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)}

        mvw.AktuellesViewModel.Model = New Entities.Trainer
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.Trainerliste.Add(mvw.AktuellesViewModel.Model)
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Entities.Trainer).Spitzname} wurde gespeichert")
        End If
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu


        ' Hier können Sie die Logik für den Neu-Button implementieren
        Dim dialog = New BasisDetailWindow() With {
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Trainer),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)}

        mvw.AktuellesViewModel.Model = New Entities.Trainer(SelectedItem)
        dialog.DataContext = mvw

        Dim result As Boolean = dialog.ShowDialog()

        If result = True Then
            Dim index = Services.DateiService.AktuellerClub.Trainerliste.IndexOf(SelectedItem)
            ' Todo: Das Speichern muss im ViewModel erledigt werden
            Services.DateiService.AktuellerClub.Trainerliste(index) = mvw.AktuellesViewModel.Model
            MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Entities.Trainer).Spitzname} wurde gespeichert")
        End If

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

