Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Namespace ViewModels


    Public Class GruppendetailViewModel
        Inherits BaseModel
        Implements IViewModelSpecial

#Region "Felder"
        Private _leistungsstufenListe As LeistungsstufeCollection
        Private _Gruppe As Gruppe
        Private _mitgliederCollectionNotifier As INotifyCollectionChanged
#End Region

#Region "Properties"

#End Region

#Region "Ereignisse"
        Public Event ModelChangedEvent As EventHandler(Of Boolean) Implements IViewModelSpecial.ModelChangedEvent
#End Region

#Region "Kontruktor"
        Public Sub New()

            UserControlLoadedCommand = New RelayCommand(Of Object)(AddressOf OnUserControlLoaded)

            AddHandler ServiceProvider.DateiService.DateiGeschlossen, AddressOf OnDateiGeschlossen
            AddHandler ServiceProvider.DateiService.DateiGeoeffnet, AddressOf OnDateiGeOeffnet
            AddHandler TrainerService.TrainerGeaendert, AddressOf OnTrainerGeaendert
            AddHandler TeilnehmerService.TeilnehmerGeaendert, AddressOf OnTeilnehmerGeaendert
            AddHandler GruppenstammService.GruppenstammBearbeitet, AddressOf OnGruppeGeaendert

            SelectedAlleMitglieder = New TeilnehmerCollection()

        End Sub



#End Region

#Region "Commands"
        Public Property TeilnehmerEntfernen As RelayCommand(Of Object)
        Public Property TrainerEntfernen As RelayCommand(Of Object)
        Public Property UserControlLoadedCommand As RelayCommand(Of Object)

#End Region

#Region "Command-Methoden"

        Private Sub OnUserControlLoaded(obj As Object)
            If ServiceProvider.DateiService.AktuellerClub IsNot Nothing Then
                LeistungsstufenListe = ServiceProvider.DateiService.AktuellerClub.Leistungsstufenliste.Sortieren
            End If
            OnPropertyChanged(NameOf(LeistungsstufenListe))
        End Sub

        Private Sub OnGruppeGeaendert(sender As Object, e As GruppenstammEventArgs)
            'DirectCast(Gruppe, Gruppe).Gruppenstamm = e.ChangedGruppenstamm
            'DirectCast(Gruppe, Gruppe).GruppenstammID = e.ChangedGruppenstamm.Ident
            OnPropertyChanged(NameOf(Leistungsstufe))
            OnPropertyChanged(NameOf(LeistungsstufeID))
            OnPropertyChanged(NameOf(Benennung))
            OnPropertyChanged(NameOf(Sortierung))
        End Sub

        Private Sub OnDateiGeOeffnet()
            Me.LeistungsstufenListe = ServiceProvider.DateiService.AktuellerClub.Leistungsstufenliste.Sortieren
        End Sub

        Private Sub OnDateiGeschlossen()
            Me.LeistungsstufenListe = Nothing
        End Sub

        Private Sub OnTeilnehmerGeaendert(sender As Object, e As EventArgs)
            OnPropertyChanged(NameOf(Mitgliederliste))
        End Sub

        Private Sub OnTrainerGeaendert(sender As Object, e As TrainerEventArgs)
            OnPropertyChanged(NameOf(Trainer))
        End Sub

        Public Sub OnOk(obj As Object) Implements IViewModelSpecial.OnOk
            Throw New NotImplementedException()
        End Sub

        Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
            Throw New NotImplementedException()
        End Sub

#End Region

#Region "Properties"

        Public Property SelectedAlleMitglieder As IList(Of Teilnehmer)

        Public Property Gruppe As IModel Implements IViewModelSpecial.Model
            Get
                Return _Gruppe
            End Get
            Set(value As IModel)
                _Gruppe = value
                OnPropertyChanged(NameOf(Gruppe))
                OnPropertyChanged(NameOf(Sortierung))
                OnPropertyChanged(NameOf(Benennung))
                OnPropertyChanged(NameOf(Leistungsstufe))
                OnPropertyChanged(NameOf(LeistungsstufeID))
                OnPropertyChanged(NameOf(Trainer))
                OnPropertyChanged(NameOf(Mitgliederliste))
                OnPropertyChanged(NameOf(LeistungsstufenListe))
            End Set
        End Property

        Public Property Sortierung() As Integer
            Get
                If _Gruppe Is Nothing Then
                    Return Nothing
                End If
                Return _Gruppe.Sortierung
            End Get
            Set(ByVal value As Integer)
                _Gruppe.Sortierung = value
                OnPropertyChanged(NameOf(Sortierung))
                RaiseEvent ModelChangedEvent(Me, False)
            End Set
        End Property

        Property Benennung As String
            Get
                If _Gruppe Is Nothing Then
                    Return Nothing
                End If
                Return _Gruppe.Benennung
            End Get
            Set(value As String)
                _Gruppe.Benennung = value
                OnPropertyChanged(NameOf(Benennung))
                RaiseEvent ModelChangedEvent(Me, False)
            End Set
        End Property

        Property Leistungsstufe As Leistungsstufe
            Get
                If _Gruppe Is Nothing Then
                    Return Nothing
                End If
                Return _Gruppe.Leistungsstufe
            End Get
            Set(value As Leistungsstufe)
                _Gruppe.Leistungsstufe = value
                OnPropertyChanged(NameOf(Leistungsstufe))
                OnPropertyChanged(NameOf(LeistungsstufeID))
                RaiseEvent ModelChangedEvent(Me, False)
            End Set
        End Property

        Property LeistungsstufeID As Guid
            Get
                If _Gruppe Is Nothing Then
                    Return Nothing
                End If
                Return _Gruppe.LeistungsstufeID
            End Get
            Set(value As Guid)
                If _Gruppe IsNot Nothing Then
                    _Gruppe.LeistungsstufeID = value
                    _Gruppe.Leistungsstufe = ServiceProvider.DateiService.AktuellerClub.Leistungsstufenliste.Where(Function(Ls) Ls.Ident = value).First
                    OnPropertyChanged(NameOf(LeistungsstufeID))
                    OnPropertyChanged(NameOf(Leistungsstufe))
                    RaiseEvent ModelChangedEvent(Me, False)
                End If
            End Set
        End Property

        ''' <summary>
        ''' DropDownliste für die Combobobox
        ''' </summary>
        ''' <returns></returns>
        Public Property LeistungsstufenListe As LeistungsstufeCollection
            Get
                Return _leistungsstufenListe
            End Get
            Set(value As LeistungsstufeCollection)
                _leistungsstufenListe = value
                OnPropertyChanged(NameOf(LeistungsstufenListe))
            End Set
        End Property

        Property Trainer As Trainer
            Get
                If _Gruppe Is Nothing Then
                    Return Nothing
                End If
                Return _Gruppe.Trainer
            End Get
            Set(value As Trainer)
                _Gruppe.Trainer = value
                OnPropertyChanged(NameOf(Trainer))
                RaiseEvent ModelChangedEvent(Me, False)
            End Set
        End Property

        Property Mitgliederliste As TeilnehmerCollection
            Get
                If _Gruppe Is Nothing Then
                    Return Nothing
                End If

                Return _Gruppe.Mitgliederliste
            End Get
            Set(value As TeilnehmerCollection)
                _Gruppe.Mitgliederliste = value
                OnPropertyChanged(NameOf(Sortierung))
                RaiseEvent ModelChangedEvent(Me, False)
            End Set
        End Property

        Public Property Daten As IEnumerable(Of IModel) Implements IViewModelSpecial.Daten
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As IEnumerable(Of IModel))
                Throw New NotImplementedException()
            End Set
        End Property

        Public ReadOnly Property IstEingabeGueltig As Boolean Implements IViewModelSpecial.IstEingabeGueltig
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property DataGridSortingCommand As ICommand Implements IViewModelSpecial.DataGridSortingCommand
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property BearbeitenCommand As ICommand Implements IViewModelSpecial.BearbeitenCommand
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property NeuCommand As ICommand Implements IViewModelSpecial.NeuCommand
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        ' Read-only Property, die die sortierte Teilnehmerliste liefert
        Public ReadOnly Property MitgliederSortiert As TeilnehmerCollection
            Get
                If Gruppe Is Nothing OrElse _Gruppe.Mitgliederliste Is Nothing Then
                    Return New TeilnehmerCollection()
                End If
                Return _Gruppe.Mitgliederliste.TeilnehmerOrderByNachname
            End Get
        End Property

#End Region

#Region "Methoden"

        Private Sub OnMitgliederlisteChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
            ' Wenn sich die zugrunde liegende Collection ändert, UI darüber informieren
            OnPropertyChanged(NameOf(MitgliederSortiert))
        End Sub

        ' Rest der Klasse...
#End Region

    End Class

End Namespace
