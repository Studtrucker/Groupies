Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports Groupies.Controller
'Imports Groupies.DataImport
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class TrainerViewModel
    Inherits MasterDetailViewModel(Of Trainer)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Trainer As Trainer
    Private ReadOnly _zulaessigeEndungen As String() = {".jpg", ".jpeg", ".gif", ".png"}
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

        NeuCommand = New RelayCommand(Of Trainer)(AddressOf OnNeu, Function() CanNeu())
        BearbeitenCommand = New RelayCommand(Of Trainer)(AddressOf OnBearbeiten, Function() CanBearbeiten())
        LoeschenCommand = New RelayCommand(Of Trainer)(AddressOf OnLoeschen, Function() CanLoeschen())
        TrainerCopyToCommand = New RelayCommand(Of Object)(AddressOf TrainerCopyTo, AddressOf CanTrainerCopyTo)

        AddHandler TrainerService.TrainerGeaendert, AddressOf OnTrainerGeaendert

    End Sub

    ' Innerhalb des ViewModels / Command-Handlers
    Public Sub TrainerCopyTo(param As Object)
        ' param ist ein Object-Array: { SelectedItemsEnumerable, TargetEinteilung }
        Dim arr = TryCast(param, Object())
        If arr Is Nothing OrElse arr.Length < 2 Then
            Return
        End If

        Dim selectedItemsEnumerable = TryCast(arr(0), System.Collections.IEnumerable)
        Dim targetEinteilung = arr(1) ' typisiere hier auf dein Modell, z.B. EinteilungViewModel oder Einteilung

        If selectedItemsEnumerable Is Nothing OrElse targetEinteilung Is Nothing Then
            Return
        End If


        Dim TrainerlisteToCopy As New List(Of Trainer)
        For Each t As Trainer In selectedItemsEnumerable
            TrainerlisteToCopy.Add(t)
        Next

        Dim TS As New TrainerService
        TrainerlisteToCopy.ForEach(Sub(t) TS.TrainerCopyToEinteilung(t, targetEinteilung))

    End Sub


    Private Function CanTrainerCopyTo(param As Object) As Boolean
        Return True
        'Try
        '    Dim arr = TryCast(param, Object())
        '    If arr Is Nothing OrElse arr.Length < 2 Then
        '        Debug.WriteLine("CanTrainerCopyTo: param null oder zu kurz")
        '        Return False
        '    End If

        '    Dim selectedItemsEnumerable = TryCast(arr(0), System.Collections.IEnumerable)
        '    Dim target = TryCast(arr(1), Einteilung)

        '    Dim selCount As Integer = 0
        '    If selectedItemsEnumerable IsNot Nothing Then
        '        For Each item In selectedItemsEnumerable
        '            selCount += 1
        '        Next
        '    End If

        '    Debug.WriteLine($"CanTrainerCopyTo: targetType={(If(arr(1) IsNot Nothing, arr(1).GetType().FullName, "NULL"))}, targetName={(If(target IsNot Nothing, target.Benennung, "(not Einteilung)"))}, selectedCount={selCount}")

        '    If target Is Nothing OrElse selectedItemsEnumerable Is Nothing Then
        '        Debug.WriteLine("CanTrainerCopyTo: target oder selectedItemsEnumerable ist Nothing -> False")
        '        Return False
        '    End If

        '    If target.Benennung = "Montag" Then
        '        Debug.WriteLine("CanTrainerCopyTo: Ziel 'Montag' -> False")
        '        Return False
        '    End If

        '    Debug.WriteLine("CanTrainerCopyTo: returning True")
        '    Return True
        'Catch ex As Exception
        '    Debug.WriteLine("CanTrainerCopyTo Exception: " & ex.ToString())
        '    Return False
        'End Try
    End Function

    'Private Function CanTrainerCopyTo(param As Object) As Boolean


    '    Dim arr = TryCast(param, Object())
    '    If arr Is Nothing OrElse arr.Length < 2 Then Return False

    '    Dim selectedItemsEnumerable = TryCast(arr(0), System.Collections.IEnumerable)
    '    Dim target = TryCast(arr(1), Einteilung)
    '    If target Is Nothing OrElse selectedItemsEnumerable Is Nothing Then Return False

    '    If target.Benennung = "Montag" Then
    '        Return False
    '    Else
    '        Return True
    '    End If

    '    'Dim club = DateiService.AktuellerClub
    '    'If club Is Nothing OrElse club.Einteilungsliste Is Nothing OrElse Not club.Einteilungsliste.Contains(target) Then Return False

    '    'For Each o In selectedItemsEnumerable
    '    '    Dim selTrainer = TryCast(o, Trainer)
    '    '    If selTrainer Is Nothing AndAlso o IsNot Nothing Then
    '    '        Dim prop = o.GetType().GetProperty("Model")
    '    '        If prop IsNot Nothing Then
    '    '            selTrainer = TryCast(prop.GetValue(o, Nothing), Trainer)
    '    '        End If
    '    '    End If
    '    '    If selTrainer Is Nothing Then Continue For

    '    '    Dim tnId = selTrainer.TrainerID
    '    '    If target.VerfuegbareTrainerIDListe IsNot Nothing AndAlso target.VerfuegbareTrainerIDListe.Contains(tnId) Then
    '    '        Return True
    '    '    End If

    '    '    Dim alreadyInGroup As Boolean = False
    '    '    If target.Gruppenliste IsNot Nothing Then
    '    '        For Each g In target.Gruppenliste
    '    '            If g IsNot Nothing AndAlso g.TrainerID = tnId Then
    '    '                alreadyInGroup = True
    '    '                Exit For
    '    '            End If
    '    '        Next
    '    '    End If
    '    '    If alreadyInGroup Then Continue For

    '    '    Return True ' mindestens ein passender Trainer
    '    'Next

    '    'Return False
    'End Function

    Private Sub RaiseCopyCommandsCanExecute()
        If TrainerCopyToCommand IsNot Nothing Then TrainerCopyToCommand.RaiseCanExecuteChanged()
    End Sub

#End Region

#Region "Properties"

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

    Public Property [Alias] As String
        Get
            Return _Trainer.Alias
        End Get
        Set(value As String)
            _Trainer.Alias = value
            OnPropertyChanged(NameOf([Alias]))
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

    Public ReadOnly Property TrainerCopyToCommand As RelayCommand(Of Object)

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
        Dim ts As New TrainerService
        ts.TrainerErstellen()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu
        Dim trainerToEdit = DirectCast(SelectedItem, Trainer)
        Dim ts As New TrainerService
        ts.TrainerBearbeiten(trainerToEdit)
    End Sub

    Public Overloads Sub OnLoeschen(obj As Object)
        Dim trainerToDelete = DirectCast(SelectedItem, Trainer)
        Dim TS As New TrainerService()
        TS.TrainerLoeschen(trainerToDelete)
    End Sub

    Public Property IconPath
        Set(value)
            Throw New NotImplementedException()
        End Set
        Get
            Throw New NotImplementedException()
        End Get
    End Property

#End Region

#Region "Validation"
    Private Sub ValidateVorname()
        ClearErrors(NameOf(_Trainer.Vorname))
        If String.IsNullOrWhiteSpace(_Trainer.Vorname) Then
            AddError(NameOf(_Trainer.Vorname), "Name darf nicht leer sein.")
        End If
    End Sub

    Private Sub ValidateSpitzname()
        ClearErrors(NameOf(_Trainer.Alias))
        If _Trainer.Alias Is Nothing OrElse String.IsNullOrWhiteSpace(_Trainer.Alias) Then
            AddError(NameOf(_Trainer.Alias), "Alias darf nicht leer sein.")
        End If

        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Trainer, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Trainer.Alias), result.ErrorContent.ToString())
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

    Private Sub OnTrainerGeaendert(sender As Object, e As EventArgs)
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub

    Private Sub ValidateTrainerID()
        ClearErrors(NameOf(_Trainer.TrainerID))
        If _Trainer.TrainerID = Guid.Empty Then
            AddError(NameOf(_Trainer.TrainerID), "TrainerID muss eingetragen werden.")
        End If
    End Sub
#End Region

End Class

