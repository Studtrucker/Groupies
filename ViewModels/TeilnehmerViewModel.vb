Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports Groupies.Controller
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class TeilnehmerViewModel
    Inherits MasterDetailViewModel(Of Teilnehmer)
    Implements IViewModelSpecial

#Region "Felder"
    Private _Teilnehmer As Teilnehmer
#End Region

#Region "Events"
    Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Konstruktor"


    ''' <summary>
    ''' Parameterloser Konstruktor für den TeilnehmerViewModel.
    ''' Die Instanz benötigt den Modus und ein Teilnehmer-Objekt.
    ''' Der Datentyp und das UserControl werden automatisch gesetzt.
    ''' </summary>
    Sub New()
        MyBase.New()
        ' Hier können Sie den Konstruktor anpassen

        Dim DropDown = New ListCollectionView(Services.DateiService.AktuellerClub.Leistungsstufenliste.Sortieren)
        DropDown.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        LeistungsstufenListCollectionView = DropDown
        DataGridSortingCommand = New RelayCommand(Of DataGridSortingEventArgs)(AddressOf MyBase.OnDataGridSorting)
        LoeschenCommand = New RelayCommand(Of Teilnehmer)(AddressOf OnLoeschen, Function() MyBase.CanLoeschen)
        NeuCommand = New RelayCommand(Of Einteilung)(AddressOf OnNeu, Function() CanNeu)
        BearbeitenCommand = New RelayCommand(Of Einteilung)(AddressOf OnBearbeiten, Function() CanBearbeiten)
        TeilnehmerCopyToCommand = New RelayCommand(Of Einteilung)(AddressOf OnCopyTo, AddressOf CanCopyTo)

        AddHandler TeilnehmerService.TeilnehmerGeaendert, AddressOf TeilnehmerBearbeitet

    End Sub

    Private Function CanCopyTo(target As Einteilung) As Boolean
        ' Grundprüfungen
        If target Is Nothing Then Return False

        ' Verwende SelectedItem (MasterDetailBase) als aktuell ausgewählten Teilnehmer
        Dim selected = TryCast(SelectedItem, Teilnehmer)
        If selected Is Nothing Then Return False

        ' Stelle sicher, dass ein Club geladen ist und das Ziel in der Liste des Clubs liegt
        Dim club = DateiService.AktuellerClub
        If club Is Nothing OrElse club.Einteilungsliste Is Nothing Then Return False
        If Not club.Einteilungsliste.Contains(target) Then Return False

        Dim tnId = selected.Ident

        ' Prüfe, ob der Teilnehmer bereits in den gruppenlosen Teilnehmern der Zieleinteilung ist
        If target.NichtZugewieseneTeilnehmerIDListe IsNot Nothing AndAlso target.NichtZugewieseneTeilnehmerIDListe.Contains(tnId) Then
            Return False
        End If

        ' Prüfe, ob der Teilnehmer bereits in einer Gruppe der Zieleinteilung enthalten ist
        If target.Gruppenliste IsNot Nothing Then
            For Each g In target.Gruppenliste
                If g IsNot Nothing AndAlso g.MitgliederIDListe IsNot Nothing AndAlso g.MitgliederIDListe.Contains(tnId) Then
                    Return False
                End If
            Next
        End If

        ' Optional: weitere Regeln (z. B. Leistungsstufen/Trainer-Konflikte) hier ergänzen

        Return True
    End Function

    Private Sub OnCopyTo(einteilung As Einteilung)
        If einteilung Is Nothing Then Return
        Dim selected = TryCast(SelectedItem, Teilnehmer)
        If selected Is Nothing Then Return

        ' Sicherstellen, dass Ziel-Listen initialisiert sind
        If einteilung.NichtZugewieseneTeilnehmerListe Is Nothing Then
            einteilung.NichtZugewieseneTeilnehmerListe = New TeilnehmerCollection()
        End If
        If einteilung.NichtZugewieseneTeilnehmerIDListe Is Nothing Then
            einteilung.NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)()
        End If

        ' Teilnehmer nur hinzufügen, wenn noch nicht vorhanden
        If Not einteilung.NichtZugewieseneTeilnehmerIDListe.Contains(selected.Ident) Then
            einteilung.NichtZugewieseneTeilnehmerListe.Add(selected)
            einteilung.NichtZugewieseneTeilnehmerIDListe.Add(selected.Ident)
        End If

        ' Optional: entferne Teilnehmer aus vorheriger Einteilung / Gruppe hier, falls erwünscht
        ' Optional: Benachrichtige Services / UI
    End Sub



#End Region

#Region "Properties"
    Public Property LeistungsstufenListCollectionView As ICollectionView

    Public Property Model As IModel Implements IViewModelSpecial.Model
        Get
            Return _Teilnehmer
        End Get
        Set(value As IModel)
            _Teilnehmer = value
        End Set
    End Property

    Public Property Ident As Guid
        Get
            Return _Teilnehmer.Ident
        End Get
        Set(value As Guid)
            _Teilnehmer.Ident = value
            OnPropertyChanged(NameOf(Ident))
            ValidateTeilnehmerID()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Vorname As String
        Get
            Return _Teilnehmer.Vorname
        End Get
        Set(value As String)
            _Teilnehmer.Vorname = value
            OnPropertyChanged(NameOf(Vorname))
            ValidateVorname()
            RaiseEvent ModelChangedEvent(Me, Not HasErrors)
        End Set
    End Property

    Public Property Nachname As String
        Get
            Return _Teilnehmer.Nachname
        End Get
        Set(value As String)
            _Teilnehmer.Nachname = value
            OnPropertyChanged(NameOf(Nachname))
            ValidateNachname()
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Geburtsdatum As Date
        Get
            Return _Teilnehmer.Geburtsdatum
        End Get
        Set(value As Date)
            _Teilnehmer.Geburtsdatum = value
            OnPropertyChanged(NameOf(Geburtsdatum))
            OnPropertyChanged(NameOf(Alter))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public Property Telefonnummer As String
        Get
            Return _Teilnehmer.Telefonnummer
        End Get
        Set(value As String)
            _Teilnehmer.Telefonnummer = value
            OnPropertyChanged(NameOf(Telefonnummer))
            RaiseEvent ModelChangedEvent(Me, HasErrors)
        End Set
    End Property

    Public ReadOnly Property Alter As String
        Get
            Return $"Alter: {_Teilnehmer.Alter} Jahre"
        End Get
    End Property

    Public Property Leistungsstufe As Leistungsstufe
        Get
            Return _Teilnehmer.Leistungsstufe
        End Get
        Set(value As Leistungsstufe)
            _Teilnehmer.Leistungsstufe = value
            OnPropertyChanged(NameOf(Leistungsstufe))
            ValidateLeistungsstufe()
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

    Public ReadOnly Property BearbeitenCommand As ICommand Implements IViewModelSpecial.BearbeitenCommand

    Public ReadOnly Property NeuCommand As ICommand Implements IViewModelSpecial.NeuCommand

    Public ReadOnly Property TeilnehmerCopyToCommand As RelayCommand(Of Einteilung)

#End Region

#Region "Methoden"

    Private Sub TeilnehmerBearbeitet(sender As Object, e As EventArgs)
        MoveNextCommand.RaiseCanExecuteChanged()
        MovePreviousCommand.RaiseCanExecuteChanged()
    End Sub


    Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        ' Hier können Sie die Logik für den OK-Button implementieren
        _Teilnehmer.speichern()
    End Sub

    Private Overloads Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        MyBase.OnLoaded()
        If _Teilnehmer IsNot Nothing Then
            ValidateVorname()
            ValidateNachname()
            ValidateLeistungsstufe()
            ValidateTeilnehmerID()
        End If
    End Sub


    Public Overloads Sub OnNeu(obj As Object) 'Implements IViewModelSpecial.OnNeu
        Dim ts As New TeilnehmerService
        ts.TeilnehmerErstellen()
    End Sub

    Public Sub OnBearbeiten(obj As Object) 'Implements IViewModelSpecial.OnNeu
        Dim ts As New TeilnehmerService
        ts.TeilnehmerBearbeiten(SelectedItem)
    End Sub

    Private Overloads Sub OnLoeschen(obj As Object)
        Dim teilnehmerToDelete = DirectCast(SelectedItem, Teilnehmer)
        Dim TS As New TeilnehmerService()
        TS.TeilnehmerLoeschen(teilnehmerToDelete)
    End Sub

#End Region

#Region "Validation"

    Private Sub ValidateVorname()
        ClearErrors(NameOf(_Teilnehmer.Vorname))
        If String.IsNullOrWhiteSpace(_Teilnehmer.Vorname) Then
            AddError(NameOf(_Teilnehmer.Vorname), "Vorname darf nicht leer sein.")
        End If
        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Teilnehmer, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ClearErrors(NameOf(_Teilnehmer.Nachname))

            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Teilnehmer.Vorname), result.ErrorContent.ToString())
        End If
    End Sub

    Private Sub ValidateNachname()
        ClearErrors(NameOf(_Teilnehmer.Nachname))
        If _Teilnehmer.Nachname Is Nothing OrElse String.IsNullOrWhiteSpace(_Teilnehmer.Nachname) Then
            AddError(NameOf(_Teilnehmer.Nachname), "Nachname darf nicht leer sein.")
        End If

        Dim result = New ValidationRules.BenennungValidationRule().Validate(_Teilnehmer, Nothing)

        If Not result = ValidationResult.ValidResult Then
            ClearErrors(NameOf(_Teilnehmer.Vorname))

            ' Fehler hinzufügen, wenn die Validierung fehlschlägt
            AddError(NameOf(_Teilnehmer.Nachname), result.ErrorContent.ToString())
        End If
    End Sub

    Private Sub ValidateLeistungsstufe()
        ClearErrors(NameOf(_Teilnehmer.Leistungsstufe))
        If _Teilnehmer.Leistungsstufe Is Nothing OrElse _Teilnehmer.Leistungsstufe.Sortierung = -1 Then
            AddError(NameOf(_Teilnehmer.Leistungsstufe), "Leistungsstufe muss ausgewählt werden.")
        End If
    End Sub
    Private Sub ValidateTeilnehmerID()
        ClearErrors(NameOf(_Teilnehmer.Ident))
        If _Teilnehmer.Ident = Nothing Then
            AddError(NameOf(_Teilnehmer.Ident), "Eine Ident muss eingetragen werden.")
        End If
    End Sub
#End Region

End Class

