Imports System.Collections.ObjectModel
Imports System.Collections.Specialized
Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Markup
Imports System.Windows.Shell
Imports Groupies.Controller
Imports Groupies.Entities.Generation4
Imports Groupies.Interfaces
Imports Groupies.MainWindow
Imports Groupies.Services
Imports Groupies.UserControls
Imports Microsoft.Office.Interop.Excel
Imports Microsoft.Win32


Namespace ViewModels

    Public Enum Printversion
        TrainerInfo
        TeilnehmerInfo
    End Enum

    ''' <summary>
    ''' ViewModel für die Hauptansicht der Anwendung.
    ''' Verwaltet Einteilungen, Gruppen, Trainer und Teilnehmer.
    ''' </summary>
    Public Class MainViewModel
        Inherits BaseModel

#Region "Felder"
        Private ReadOnly _windowService As IWindowService
        Private ReadOnly DateiService As DateiService

        Private _WindowTitleText As String = String.Empty

        Private _AlleEinteilungenCV As CollectionView
        Private _SelectedEinteilung As Einteilung
        Private _SelectedGruppe As Gruppe
        Private _SelectedTeilnehmer As Teilnehmer
        Private _SelectedAlleMitglieder As New TeilnehmerCollection
        Private _SelectedGruppenloserTrainer As Trainer
        Private _selectedAlleGruppenloserTrainer As IList
        Private _GruppendetailViewModel As GruppendetailViewModel
        'Private _gruppeTransferToCommand As RelayCommand(Of Einteilung)
        Private _einteilungCopyToCommand As RelayCommand(Of Einteilung)

#End Region

#Region "Konstruktor"
        Public Sub New(windowService As IWindowService)
            MyBase.New()
            _windowService = windowService
            DateiService = New DateiService

            ' ViewModel-Sub-ViewModels initialisieren
            GruppendetailViewModel = New GruppendetailViewModel()

            ' Commands und Handler initialisieren
            WindowLoadedCommand = New RelayCommand(Of Object)(AddressOf OnWindowLoaded)
            InitializeCommands()
            InitializeHandlers()

        End Sub
#End Region

#Region "Public Properties"

        ' Window Commands (werden in InitializeCommands gesetzt)
        Public Property WindowLoadedCommand As ICommand
        Public Property WindowClosedCommand As ICommand
        Public Property WindowClosingCommand As ICommand

        ' Application Commands
        Public Property ApplicationCloseCommand As ICommand
        Public Property ClubNewCommand As ICommand
        Public Property ClubOpenCommand As ICommand
        Public Property ClubSaveCommand As ICommand
        Public Property ClubSaveAsCommand As ICommand
        Public Property ClubInfoPrintCommand As ICommand
        Public Property ClubCloseCommand As ICommand
        Public Property OpenHomepageCommand As ICommand

        ' Einteilung Commands
        Public Property EinteilungsuebersichtAnzeigenCommand As ICommand
        Public Property EinteilungErstellenCommand As ICommand
        Public ReadOnly Property EinteilungCopyToCommand As RelayCommand(Of Einteilung)
            Get
                If _einteilungCopyToCommand Is Nothing Then
                    _einteilungCopyToCommand = New RelayCommand(Of Einteilung)(
                Sub(target)
                    If target Is Nothing OrElse SelectedEinteilung Is Nothing Then Return
                    Dim svc As New EinteilungService()
                    svc.EinteilungKopieren(SelectedEinteilung, target)
                    ' Auswahl auf Ziel setzen und UI refreshen
                    SelectedEinteilung = target
                    If AlleEinteilungenCV IsNot Nothing Then AlleEinteilungenCV.Refresh()
                End Sub,
                Function(target)
                    Dim Esvc As New EinteilungService()
                    Return Esvc.CanEinteilungKopieren(SelectedEinteilung, target)
                End Function)
                End If
                Return _einteilungCopyToCommand
            End Get
        End Property

        ' Gruppen Commands
        Public Property GruppenuebersichtAnzeigenCommand As ICommand
        Public Property GruppeErstellenCommand As ICommand
        Public Property GruppeAusEinteilungEntfernenCommand As ICommand
        Public Property GruppeCopyToCommand As RelayCommand(Of Einteilung)

        ' Teilnehmer Commands
        Public Property TeilnehmerInGruppeEinteilenCommand As ICommand
        Public Property TeilnehmerAusGruppeEntfernenCommand As ICommand
        Public Property TeilnehmerAusEinteilungEntfernenCommand As ICommand
        Public Property TeilnehmeruebersichtAnzeigenCommand As ICommand
        Public Property TeilnehmerErstellenCommand As ICommand
        Public Property TeilnehmerSuchenCommand As ICommand

        ' Trainer Commands
        Public Property TrainerInGruppeEinteilenCommand As ICommand
        Public Property TrainerAusGruppeEntfernenCommand As ICommand
        Public Property TrainerAusEinteilungEntfernenCommand As ICommand
        Public Property TraineruebersichtAnzeigenCommand As ICommand
        Public Property TrainerErstellenCommand As ICommand

        ' Leistungsstufen Commands
        Public Property LeistungsstufenuebersichtAnzeigenCommand As ICommand
        Public Property LeistungsstufeErstellenCommand As ICommand

        ' Faehigkeiten Commands
        Public Property FaehigkeitenuebersichtAnzeigenCommand As ICommand
        Public Property FaehigkeitErstellenCommand As ICommand

        ' Menu Properties
        Public Property MostRecentlyUsedMenuItem As New ObservableCollection(Of MenuEintragViewModel)

        ' Window / View-spezifische Properties
        Public Property WindowTitleText As String
            Get
                Return _WindowTitleText
            End Get
            Set(value As String)
                _WindowTitleText = value
            End Set
        End Property

        Public ReadOnly DefaultWindowTitleText As String = "Groupies - Ski Club Management"
        Public Property WindowTitleIcon As String = "pack://application:,,,/Images/icons8-ski-resort-48.png"

        ' Daten- und Auswahl-Properties
        Public Property AlleEinteilungenCV As CollectionView
            Get
                Return _AlleEinteilungenCV
            End Get
            Set(value As CollectionView)
                _AlleEinteilungenCV = value
            End Set
        End Property

        Public Property SelectedEinteilung As Einteilung
            Get
                Return _SelectedEinteilung
            End Get
            Set(value As Einteilung)
                _SelectedEinteilung = value
                SelectedGruppe = Nothing
                SelectedTeilnehmer = Nothing
                OnPropertyChanged(NameOf(SelectedEinteilung))
                If TeilnehmerAusGruppeEntfernenCommand IsNot Nothing Then
                    DirectCast(TeilnehmerAusGruppeEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                End If
                RaiseTransferCommandsCanExecute()
            End Set
        End Property

        Public Property SelectedGruppe As Gruppe
            Get
                Return _SelectedGruppe
            End Get
            Set(value As Gruppe)
                _GruppendetailViewModel.Gruppe = value
                _SelectedGruppe = value
                SelectedTeilnehmer = Nothing
                OnPropertyChanged(NameOf(SelectedGruppe))
                If TeilnehmerAusGruppeEntfernenCommand IsNot Nothing Then
                    DirectCast(TeilnehmerAusGruppeEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                End If
                RaiseTransferCommandsCanExecute()
            End Set
        End Property

        Public Property SelectedTeilnehmer As Teilnehmer
            Get
                Return _SelectedTeilnehmer
            End Get
            Set(value As Teilnehmer)
                _SelectedTeilnehmer = value
                OnPropertyChanged(NameOf(SelectedTeilnehmer))
            End Set
        End Property

        ''' <summary>
        ''' Alle ausgewählten Teilnehmer der gruppenlose Teilnehmerliste.
        ''' </summary>
        Public Property SelectedAlleGruppenloserTeilnehmer As New TeilnehmerCollection

        ''' <summary>
        ''' Alle ausgewählten Teilnehmer der aktuellen Gruppe.
        ''' </summary>
        Public Property SelectedAlleMitglieder As TeilnehmerCollection
            Get
                Return _SelectedAlleMitglieder
            End Get
            Set(value As TeilnehmerCollection)
                If _SelectedAlleMitglieder IsNot Nothing Then
                    RemoveHandler CType(_SelectedAlleMitglieder, INotifyCollectionChanged).CollectionChanged, AddressOf OnSelectedAlleMitgliederCollectionChanged
                End If

                _SelectedAlleMitglieder = If(value, New TeilnehmerCollection())
                AddHandler CType(_SelectedAlleMitglieder, INotifyCollectionChanged).CollectionChanged, AddressOf OnSelectedAlleMitgliederCollectionChanged

                OnPropertyChanged(NameOf(SelectedAlleMitglieder))
                If TeilnehmerAusGruppeEntfernenCommand IsNot Nothing Then
                    DirectCast(TeilnehmerAusGruppeEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                End If
            End Set
        End Property

        Public Property SelectedGruppenloserTrainer As Trainer
            Get
                Return _SelectedGruppenloserTrainer
            End Get
            Set(value As Trainer)
                _SelectedGruppenloserTrainer = value
            End Set
        End Property

        ' Bindbare SelectedItems-Property (vom Attached-Behavior gesetzt)
        Public Property SelectedAlleGruppenloserTrainer As IList
            Get
                Return _selectedAlleGruppenloserTrainer
            End Get
            Set(value As IList)
                If _selectedAlleGruppenloserTrainer IsNot value Then
                    _selectedAlleGruppenloserTrainer = value
                    OnPropertyChanged(NameOf(SelectedAlleGruppenloserTrainer))
                End If
            End Set
        End Property

        Public Property GruppendetailViewModel As GruppendetailViewModel
            Get
                Return _GruppendetailViewModel
            End Get
            Set(value As GruppendetailViewModel)
                _GruppendetailViewModel = value
            End Set
        End Property

        Public Property VerfuegbareTrainerListe As TrainerCollection
        Public Property SelectedVerfuegbareTrainerListe As TrainerCollection
        Public Property NichtZugewieseneTeilnehmerListe As TeilnehmerCollection
        Public Property SelectedNichtZugewiesenerTeilnehmerListe As New TeilnehmerCollection

#End Region

#Region "Initialisierung (Commands / Handler)"
        Private Sub InitializeCommands()
            ' Window Commands
            WindowClosingCommand = New RelayCommand(Of CancelEventArgs)(AddressOf OnWindowClosing)
            WindowClosedCommand = New RelayCommand(Of Object)(AddressOf OnWindowClosed)

            ' Application Commands
            ApplicationCloseCommand = New RelayCommand(Of Object)(AddressOf OnWindowClose)

            ' Club Commands
            ClubNewCommand = New RelayCommand(Of Object)(AddressOf OnClubNew)
            ClubOpenCommand = New RelayCommand(Of Object)(AddressOf OnClubOpen)
            ClubSaveCommand = New RelayCommand(Of Object)(AddressOf OnClubSave, Function() CanClubSave())
            ClubSaveAsCommand = New RelayCommand(Of Object)(AddressOf OnClubSaveAs, Function() CanClubSaveAs())
            ClubInfoPrintCommand = New RelayCommand(Of Printversion)(AddressOf OnClubInfoPrint, Function() CanClubInfoPrint())
            ClubCloseCommand = New RelayCommand(Of Object)(AddressOf OnClubClose, Function() CanClubClose())

            ' Einteilung Commands
            EinteilungsuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnEinteilungsuebersichtAnzeigen, Function() CanEinteilungsuebersichtAnzeigen())
            EinteilungErstellenCommand = New RelayCommand(Of Object)(AddressOf OnEinteilungErstellen, Function() CanEinteilungErstellen())


            ' Gruppen Commands
            GruppenuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnGruppenuebersichtAnzeigen, Function() CanGruppenuebersichtAnzeigen())
            GruppeErstellenCommand = New RelayCommand(Of Object)(AddressOf OnGruppeErstellen, Function() CanGruppeErstellen())
            GruppeAusEinteilungEntfernenCommand = New RelayCommand(Of Object)(AddressOf OnGruppeAusEinteilungEntfernen, Function() CanGruppeAusEinteilungEntfernen())
            GruppeCopyToCommand = CopyCommandFactory.CreateGruppeCopyCommand(Function() SelectedGruppe, Sub()
                                                                                                            ' UI refresh / CanExecute neu auswerten
                                                                                                            RaiseTransferCommandsCanExecute()
                                                                                                            If AlleEinteilungenCV IsNot Nothing Then AlleEinteilungenCV.Refresh()
                                                                                                        End Sub)


            ' Teilnehmer Commands
            TeilnehmeruebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmeruebersichtAnzeigen, Function() CanTeilnehmeruebersichtAnzeigen())
            TeilnehmerErstellenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerErstellen, Function() CanTeilnehmerErstellen())
            TeilnehmerAusGruppeEntfernenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerAusGruppeEntfernen, Function() CanTeilnehmerAusGruppeEntfernen())
            TeilnehmerInGruppeEinteilenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerInGruppeEinteilen, Function() CanTeilnehmerInGruppeEinteilen())
            TeilnehmerAusEinteilungEntfernenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerAusEinteilungEntfernen, Function() CanTeilnehmerAusEinteilungEntfernen())
            TeilnehmerSuchenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerSuchen, Function() CanTeilnehmerSuchen())

            ' Trainer Commands
            TraineruebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnTraineruebersichtAnzeigen, Function() CanTraineruebersichtAnzeigen())
            TrainerAusGruppeEntfernenCommand = New RelayCommand(Of Object)(AddressOf OnTrainerAusGruppeEntfernen, Function() CanTrainerAusGruppeEntfernen())
            TrainerInGruppeEinteilenCommand = New RelayCommand(Of Object)(AddressOf OnTrainerInGruppeEinteilen, Function() CanTrainerInGruppeEinteilen())
            TrainerAusEinteilungEntfernenCommand = New RelayCommand(Of Object)(AddressOf OnTrainerAusEinteilungEntfernen, Function() CanTrainerAusEinteilungEntfernen())
            TrainerErstellenCommand = New RelayCommand(Of Object)(AddressOf OnTrainerErstellen, Function() CanTrainerErstellen())

            ' Leistungsstufen Commands
            LeistungsstufenuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnLeistungsstufenuebersichtAnzeigen, Function() CanLeistungsstufenuebersichtAnzeigen())
            LeistungsstufeErstellenCommand = New RelayCommand(Of Object)(AddressOf OnLeistungsstufeErstellen, Function() CanLeistungsstufeErstellen())

            ' Faehigkeiten Commands
            FaehigkeitenuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnFaehigkeitenuebersichtAnzeigen, Function() CanFaehigkeitenuebersichtAnzeigen())
            FaehigkeitErstellenCommand = New RelayCommand(Of Object)(AddressOf OnFaehigkeitErstellen, Function() CanFaehigkeitErstellen())
        End Sub


        Private Function CanFaehigkeitErstellen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Sub OnFaehigkeitErstellen(obj As Object)
            Dim FS As New FaehigkeitenService
            FS.FaehigkeitErstellen()
        End Sub

        Private Function CanFaehigkeitenuebersichtAnzeigen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanLeistungsstufeErstellen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Sub OnLeistungsstufeErstellen(obj As Object)
            Dim LS As New LeistungsstufenService
            LS.LeistungsstufeErstellen()
        End Sub

        Private Function CanLeistungsstufenuebersichtAnzeigen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanTrainerErstellen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanTraineruebersichtAnzeigen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanTeilnehmerErstellen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Sub OnTeilnehmerErstellen(obj As Object)
            Dim ts As New TeilnehmerService
            ts.TeilnehmerErstellen()
        End Sub

        Private Function CanTeilnehmeruebersichtAnzeigen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanGruppeAusEinteilungEntfernen() As Boolean
            Return SelectedEinteilung IsNot Nothing
        End Function

        Private Sub OnGruppeAusEinteilungEntfernen(obj As Object)
            Dim GS As New GruppenService
            GS.GruppeAusEinteilungEntfernen(SelectedGruppe, SelectedEinteilung)
        End Sub

        Private Function CanGruppeErstellen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Sub OnGruppeErstellen(obj As Object)
            Dim GS As New GruppenService
            GS.GruppeErstellen()
        End Sub

        Private Function CanGruppenuebersichtAnzeigen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanEinteilungErstellen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Sub OnEinteilungErstellen(obj As Object)
            Dim ES As New EinteilungService
            ES.EinteilungErstellen()
        End Sub

        Private Function CanEinteilungsuebersichtAnzeigen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanClubClose() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanClubInfoPrint() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing AndAlso SelectedEinteilung IsNot Nothing
        End Function

        Private Function CanClubSaveAs() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Function CanClubSave() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        'Private Function CanEinteilungTransferTo(target As Einteilung) As Boolean
        '    If target Is Nothing Then Return False
        '    If SelectedEinteilung Is Nothing Then Return False
        '    Dim club = DateiService.AktuellerClub
        '    If club Is Nothing OrElse club.Einteilungsliste Is Nothing Then Return False

        '    ' Quelle ermitteln (Einteilung, die die aktuell ausgewählte Gruppe enthält)
        '    Dim source = SelectedEinteilung
        '    If source Is Nothing Then Return False
        '    If target.GruppenIDListe.Count > 0 OrElse target.NichtZugewieseneTeilnehmerIDListe.Count > 0 OrElse target.VerfuegbareTrainerIDListe.Count > 0 Then Return False
        '    Return source.Ident <> target.Ident
        'End Function

        'Private Sub OnEinteilungTransferTo(target As Einteilung)
        '    If target Is Nothing OrElse SelectedEinteilung Is Nothing Then Return
        '    Dim club = DateiService.AktuellerClub
        '    If club Is Nothing Then Return

        '    Dim source = SelectedEinteilung
        '    If source Is Nothing Then Return
        '    If source.Ident = target.Ident Then Return

        '    SelectedEinteilung.Gruppenliste.ToList.ForEach(Sub(g)
        '                                                       If target.Gruppenliste Is Nothing Then
        '                                                           target.Gruppenliste = New GruppeCollection()
        '                                                       End If
        '                                                       target.Gruppenliste.Add(g)
        '                                                       target.GruppenIDListe.Add(g.Ident)
        '                                                   End Sub)

        '    SelectedEinteilung.NichtZugewieseneTeilnehmerListe.ToList.ForEach(Sub(t)
        '                                                                          If target.NichtZugewieseneTeilnehmerListe Is Nothing Then
        '                                                                              target.NichtZugewieseneTeilnehmerListe = New TeilnehmerCollection
        '                                                                              target.NichtZugewieseneTeilnehmerIDListe = New ObservableCollection(Of Guid)
        '                                                                          End If
        '                                                                          target.NichtZugewieseneTeilnehmerListe.Add(t)
        '                                                                          target.NichtZugewieseneTeilnehmerIDListe.Add(t.Ident)
        '                                                                      End Sub)

        '    SelectedEinteilung.VerfuegbareTrainerListe.ToList.ForEach(Sub(t)
        '                                                                  If target.VerfuegbareTrainerListe Is Nothing Then
        '                                                                      target.VerfuegbareTrainerListe = New TrainerCollection
        '                                                                      target.VerfuegbareTrainerIDListe = New ObservableCollection(Of Guid)
        '                                                                  End If
        '                                                                  target.VerfuegbareTrainerListe.Add(t)
        '                                                                  target.VerfuegbareTrainerIDListe.Add(t.TrainerID)
        '                                                              End Sub)

        '    ' Auswahl aktualisieren: wähle Ziel-Einteilung und Gruppe
        '    SelectedEinteilung = target
        '    ' SelectedGruppe bleibt auf der gleichen Instanz; UI-Refresh erzwingen
        '    OnPropertyChanged(NameOf(SelectedEinteilung))

        '    ' CollectionViews / Menüs aktualisieren
        '    If AlleEinteilungenCV IsNot Nothing Then AlleEinteilungenCV.Refresh()
        '    ' Optional: weitere Views refreshen (Gruppenliste-View etc.)

        'End Sub

        Private Function CanGruppeTransferTo(target As Einteilung) As Boolean
            If target Is Nothing Then Return False
            If SelectedGruppe Is Nothing Then Return False
            Dim club = DateiService.AktuellerClub
            If club Is Nothing OrElse club.Einteilungsliste Is Nothing Then Return False

            ' Quelle ermitteln (Einteilung, die die aktuell ausgewählte Gruppe enthält)
            Dim source = club.Einteilungsliste.FirstOrDefault(Function(e) e.Gruppenliste IsNot Nothing AndAlso e.Gruppenliste.Contains(SelectedGruppe))
            If source Is Nothing Then Return False
            If target.Gruppenliste.Contains(target.Gruppenliste.Where(Function(G) G.Ident = SelectedGruppe.Ident).SingleOrDefault) Then Return False
            Return source.Ident <> target.Ident
        End Function

        Private Sub OnGruppeTransferTo(target As Einteilung)
            If target Is Nothing OrElse SelectedGruppe Is Nothing Then Return
            Dim club = DateiService.AktuellerClub
            If club Is Nothing Then Return

            Dim source = club.Einteilungsliste.FirstOrDefault(Function(e) e.Gruppenliste IsNot Nothing AndAlso e.Gruppenliste.Contains(SelectedGruppe))
            If source Is Nothing Then Return
            If source.Ident = target.Ident Then Return

            ' Verschieben: Entfernen aus Quelle, Hinzufügen zum Ziel
            'source.Gruppenliste.Remove(SelectedGruppe)
            If target.Gruppenliste Is Nothing Then
                target.Gruppenliste = New GruppeCollection()
            End If
            target.Gruppenliste.Add(SelectedGruppe)
            target.GruppenIDListe.Add(SelectedGruppe.Ident)

            ' Auswahl aktualisieren: wähle Ziel-Einteilung und Gruppe
            SelectedEinteilung = target
            ' SelectedGruppe bleibt auf der gleichen Instanz; UI-Refresh erzwingen
            OnPropertyChanged(NameOf(SelectedGruppe))

            ' CollectionViews / Menüs aktualisieren
            If AlleEinteilungenCV IsNot Nothing Then AlleEinteilungenCV.Refresh()
            ' Optional: weitere Views refreshen (Gruppenliste-View etc.)
        End Sub

        Private Sub InitializeHandlers()
            AddHandler PropertyChanged, AddressOf HandlerPropertyChanged
            AddHandler DateiService.DateiGeoeffnet, AddressOf HandlerSetProperties
            AddHandler DateiService.DateiGeschlossen, AddressOf HandlerResetProperties
            AddHandler DateiService.DateiOeffnenIstFehlgeschlagen, AddressOf HandlerZeigeFehlerMeldung
            AddHandler DateiService.PropertyChanged, AddressOf HandlerDateiServicePropertyChanged
            AddHandler TrainerService.TrainerGeaendert, AddressOf HandlerTrainerServiceTrainerGeaendert
            ' CollectionChanged-Handler für SelectedAlleMitglieder vorbereiten
            AddHandler CType(_SelectedAlleMitglieder, INotifyCollectionChanged).CollectionChanged, AddressOf OnSelectedAlleMitgliederCollectionChanged
            AddHandler TeilnehmerService.TeilnehmerGeaendert, AddressOf OnMitgliederlisteGeaendert
            AddHandler TeilnehmerSuchErgebnisViewModel.OpenTargetRequested, AddressOf OnTeilnehmerSuchErgebnisOpenTargetRequested
        End Sub

        ' Aufruf in Setter von SelectedGruppe/SelectedEinteilung, damit CanExecute sofort neu evaluiert wird:
        Private Sub RaiseTransferCommandsCanExecute()
            If _GruppeCopyToCommand IsNot Nothing Then _GruppeCopyToCommand.RaiseCanExecuteChanged()
            If _EinteilungCopyToCommand IsNot Nothing Then _EinteilungCopyToCommand.RaiseCanExecuteChanged()
        End Sub

#End Region

#Region "Window Lifecycle"
        Private Sub OnWindowLoaded(obj As Object)
            InitializeCommands()
            InitializeHandlers()

            DateiService.LadeMeistVerwendeteDateienInSortedList()

            If (Environment.GetCommandLineArgs().Length = 2) Then
                Dim args = Environment.GetCommandLineArgs
                Dim filename = args(1)
                DateiService.DateiLaden(New FileInfo(filename))
            Else
                Dim LetzteDatei = DateiService.LiesZuletztGeoeffneteDatei
                If LetzteDatei IsNot Nothing AndAlso Not String.IsNullOrEmpty(LetzteDatei) Then
                    DateiService.DateiLaden(New FileInfo(LetzteDatei))
                End If
            End If

            If DateiService.AktuellerClub IsNot Nothing Then
                RefreshMostRecentMenu()
                RefreshJumpListInWinTaskbar()
                HandlerSetProperties(Me, EventArgs.Empty)
            Else
                HandlerResetProperties(Me, EventArgs.Empty)
            End If
        End Sub

        Private Sub OnWindowClosing(e As CancelEventArgs)
            Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schließen?", "Achtung", MessageBoxButton.YesNo)
            e.Cancel = (result = MessageBoxResult.No)
        End Sub

        Private Sub OnWindowClosed(obj As Object)
            DateiService.SpeicherZuletztVerwendeteDateiInsIolatedStorage()
            DateiService.SpeicherZuletztVerwendeteDateienSortedList()
        End Sub

        Private Sub OnWindowClose(obj As Object)
            _windowService.CloseWindow()
        End Sub
#End Region

#Region "Menu / Navigation Handlers"
        Private Sub OnLeistungsstufenuebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Leistungsstufe),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Leistungsstufenliste

            fenster.DataContext = mvw
            fenster.Show()
        End Sub

        Private Sub OnFaehigkeitenuebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Faehigkeit),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Faehigkeitenliste

            fenster.DataContext = mvw
            fenster.Show()
        End Sub

        Private Sub OnEinteilungsuebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Einteilung),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Einteilungsliste

            fenster.DataContext = mvw
            fenster.Show()
        End Sub

        Private Sub OnGruppenuebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppe),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Gruppenliste

            fenster.DataContext = mvw
            fenster.Show()
        End Sub

        Private Sub OnTraineruebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Trainer),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Trainerliste

            fenster.DataContext = mvw
            fenster.Show()
        End Sub

        Private Sub OnTeilnehmeruebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Teilnehmer),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Teilnehmerliste

            fenster.DataContext = mvw
            fenster.Show()
        End Sub
        Private Sub OnTeilnehmerSuchErgebnisOpenTargetRequested(sender As Object, e As NavigationRequest)
            Me.SelectedEinteilung = e.ZielEinteilung
            Me.SelectedGruppe = e.ZielGruppe
            Me.SelectedTeilnehmer = e.Teilnehmer
        End Sub

#End Region

#Region "Club / File Handling"
        Private Sub OnClubNew(obj As Object)
            DateiService.NeueDateiErstellen()
            DirectCast(ClubCloseCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            HandlerSetProperties(Me, EventArgs.Empty)
        End Sub

        Private Sub OnClubOpen(obj As Object)
            DateiService.DateiOeffnen()
        End Sub

        Private Sub OnClubSave(obj As Object)
            DateiService.DateiSpeichern()
            MsgBox($"Der Club {Services.DateiService.AktuellerClub.ClubName} wurde gespeichert")
        End Sub

        Private Sub OnClubSaveAs(obj As Object)
            DateiService.DateiSpeichernAls()
            HandlerSetProperties(Me, EventArgs.Empty)
        End Sub

        Private Sub OnClubClose(obj As Object)
            DateiService.DateiSchliessen()
        End Sub

        Private Sub OnClubInfoPrint(obj As Printversion)
            Dim dlg = New PrintDialog()
            If dlg.ShowDialog = True Then
                Dim doc As FixedDocument
                Dim printArea = New Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight)
                Dim pageMargin = New Thickness(30, 30, 30, 60)
                doc = PrintoutInfo(SelectedEinteilung, obj, printArea, pageMargin)
                dlg.PrintDocument(doc.DocumentPaginator, obj)
            End If
        End Sub

        Public Shared Function PrintoutInfo(Einteilung As Einteilung, Printversion As Printversion, pageSize As Size, pageMargin As Thickness) As FixedDocument
            Dim printFriendHeight As Double = 1000
            Dim printFriendWidth As Double = 730
            Dim availablePageHeight As Double = pageSize.Height - pageMargin.Top - pageMargin.Bottom
            Dim availablePageWidth As Double = pageSize.Width - pageMargin.Left - pageMargin.Right

            Dim rowsPerPage As Integer = CType(Math.Floor(availablePageHeight / printFriendHeight), Integer)
            Dim columnsPerPage As Integer = CType(Math.Floor(availablePageWidth / printFriendWidth), Integer)
            If rowsPerPage = 0 Then rowsPerPage = 1
            If columnsPerPage = 0 Then columnsPerPage = 1

            Dim participantsPerPage As Integer = rowsPerPage * columnsPerPage
            Dim vMarginBetweenFriends As Double = 0
            If rowsPerPage > 1 Then
                Dim vLeftOverSpace As Double = availablePageHeight - (printFriendHeight * rowsPerPage)
                vMarginBetweenFriends = vLeftOverSpace / (rowsPerPage - 1)
            End If

            Dim hMarginBetweenFriends As Double = 0
            If columnsPerPage > 1 Then
                Dim hLeftOverSpace As Double = availablePageWidth - (printFriendWidth * columnsPerPage)
                hMarginBetweenFriends = hLeftOverSpace / (columnsPerPage - 1)
            End If

            Dim doc = New FixedDocument()
            doc.DocumentPaginator.PageSize = pageSize
            Dim sortedGroupView = New ListCollectionView(Einteilung.Gruppenliste)
            sortedGroupView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Descending))

            Dim skikursgruppe As Gruppe
            Dim page As FixedPage = Nothing

            For i As Integer = 0 To sortedGroupView.Count - 1
                sortedGroupView.MoveCurrentToPosition(i)
                skikursgruppe = CType(sortedGroupView.CurrentItem, Gruppe)

                If i Mod participantsPerPage = 0 Then
                    page = New FixedPage
                    If page IsNot Nothing Then
                        Dim content = New PageContent()
                        TryCast(content, IAddChild).AddChild(page)
                        doc.Pages.Add(content)
                    End If
                End If

                Dim pSkikursgruppe As IPrintableNotice
                If Printversion = Printversion.TeilnehmerInfo Then
                    pSkikursgruppe = New TeilnehmerAusdruckUserControl
                Else
                    pSkikursgruppe = New TrainerausdruckUserControl
                End If

                DirectCast(pSkikursgruppe, UserControl).Height = printFriendHeight
                DirectCast(pSkikursgruppe, UserControl).Width = printFriendWidth

                If String.IsNullOrWhiteSpace(Einteilung.Benennung) Then
                    Einteilung.Benennung = InputBox("Bitte diese Einteilung benennen")
                End If

                pSkikursgruppe.InitPropsFromGroup(skikursgruppe, Einteilung.Benennung)
                Dim currentRow As Integer = (i Mod participantsPerPage) / columnsPerPage
                Dim currentColumn As Integer = i Mod columnsPerPage

                FixedPage.SetTop(pSkikursgruppe, pageMargin.Top + ((DirectCast(pSkikursgruppe, UserControl).Height + vMarginBetweenFriends) * currentRow))
                FixedPage.SetLeft(pSkikursgruppe, pageMargin.Left + ((DirectCast(pSkikursgruppe, UserControl).Width + hMarginBetweenFriends) * currentColumn))
                page.Children.Add(pSkikursgruppe)
            Next

            Return doc
        End Function
#End Region

#Region "Teilnehmer-Command-Handler"
        Private Function CanTeilnehmerInGruppeEinteilen() As Boolean
            Return SelectedEinteilung IsNot Nothing AndAlso SelectedGruppe IsNot Nothing
        End Function

        Private Sub OnTeilnehmerInGruppeEinteilen(obj As Object)
            Dim selectedList As New List(Of Teilnehmer)

            If obj IsNot Nothing Then
                Dim asEnumerable = TryCast(obj, System.Collections.IEnumerable)
                If asEnumerable IsNot Nothing Then
                    For Each item In asEnumerable
                        Dim t = TryCast(item, Teilnehmer)
                        If t IsNot Nothing Then selectedList.Add(t)
                    Next
                Else
                    Dim [single] = TryCast(obj, Teilnehmer)
                    If [single] IsNot Nothing Then selectedList.Add([single])
                End If
            End If

            If selectedList.Count = 0 AndAlso SelectedAlleMitglieder IsNot Nothing Then
                For Each t In SelectedAlleMitglieder
                    selectedList.Add(t)
                Next
            End If

            If selectedList.Count = 0 OrElse SelectedGruppe Is Nothing OrElse SelectedEinteilung Is Nothing Then
                Return
            End If

            Dim TService As New TeilnehmerService
            TService.TeilnehmerInGruppeEinteilen(selectedList, SelectedGruppe, SelectedEinteilung)
        End Sub

        Private Function CanTeilnehmerAusGruppeEntfernen() As Boolean
            Return SelectedGruppe IsNot Nothing AndAlso SelectedEinteilung IsNot Nothing
        End Function

        Private Sub OnTeilnehmerAusGruppeEntfernen(obj As Object)
            Dim selectedList As New List(Of Teilnehmer)

            If obj IsNot Nothing Then
                Dim asEnumerable = TryCast(obj, System.Collections.IEnumerable)
                If asEnumerable IsNot Nothing Then
                    For Each item In asEnumerable
                        Dim t = TryCast(item, Teilnehmer)
                        If t IsNot Nothing Then selectedList.Add(t)
                    Next
                Else
                    Dim [single] = TryCast(obj, Teilnehmer)
                    If [single] IsNot Nothing Then selectedList.Add([single])
                End If
            End If

            If selectedList.Count = 0 AndAlso SelectedAlleMitglieder IsNot Nothing Then
                For Each t In SelectedAlleMitglieder
                    selectedList.Add(t)
                Next
            End If

            If selectedList.Count = 0 OrElse SelectedGruppe Is Nothing OrElse SelectedEinteilung Is Nothing Then
                Return
            End If

            Dim TnS As New TeilnehmerService()
            TnS.TeilnehmerAusGruppeEntfernen(selectedList, SelectedGruppe, SelectedEinteilung)
        End Sub

        Private Function CanTeilnehmerAusEinteilungEntfernen() As Boolean
            Return SelectedEinteilung IsNot Nothing AndAlso SelectedGruppe IsNot Nothing
        End Function
        Private Function CanTeilnehmerSuchen() As Boolean
            Return DateiService.AktuellerClub IsNot Nothing
        End Function

        Private Sub OnTeilnehmerSuchen(obj As Object)
            Dim dlg As New UserControls.InputDialog()
            Dim vm As New ViewModels.InputDialogViewModel()
            Dim Ergebnisliste As New List(Of TeilnehmerSuchErgebnisItem)
            dlg.DataContext = vm
            dlg.Owner = _windowService.Window
            If dlg.ShowDialog() = True Then
                Dim suchname = vm.ResponseText
                If Not String.IsNullOrWhiteSpace(suchname) Then
                    Dim ts As New TeilnehmerService()
                    Ergebnisliste = ts.TeilnehmerSuchen(suchname) ' Implementieren Sie die Suche entsprechend
                End If
            End If

            Dim vmr As TeilnehmerSuchErgebnisViewModel = Nothing
            If Ergebnisliste IsNot Nothing Then
                ' Ergebnis-VM erzeugen und anzeigen (wie zuvor)
                vmr = New TeilnehmerSuchErgebnisViewModel(Ergebnisliste)
            End If
            Dim view = New TeilnehmerSuchErgebnis()
            view.DataContext = vmr
            view.Owner = _windowService.Window
            view.Show()

        End Sub

        Private Sub OnTeilnehmerAusEinteilungEntfernen(obj As Object)

            Dim selectedList As New List(Of Teilnehmer)

            If obj IsNot Nothing Then
                Dim asEnumerable = TryCast(obj, System.Collections.IEnumerable)
                If asEnumerable IsNot Nothing Then
                    For Each item In asEnumerable
                        Dim t = TryCast(item, Teilnehmer)
                        If t IsNot Nothing Then selectedList.Add(t)
                    Next
                Else
                    Dim [single] = TryCast(obj, Teilnehmer)
                    If [single] IsNot Nothing Then selectedList.Add([single])
                End If
            End If

            If selectedList.Count = 0 AndAlso SelectedAlleMitglieder IsNot Nothing Then
                For Each t In SelectedAlleMitglieder
                    selectedList.Add(t)
                Next
            End If

            If selectedList.Count = 0 OrElse SelectedGruppe Is Nothing OrElse SelectedEinteilung Is Nothing Then
                Return
            End If

            Dim ts As New TeilnehmerService
            ts.TeilnehmerAusEinteilungEntfernen(selectedList, SelectedEinteilung)
        End Sub
#End Region

#Region "Trainer-Command-Handler"
        Private Sub OnTrainerErstellen(obj As Object)
            Dim dialog = New BasisDetailWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Trainer),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)
            }
            mvw.AktuellesViewModel.Model = New Trainer
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                DateiService.AktuellerClub.Trainerliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Trainer).Alias} wurde gespeichert")
            End If
        End Sub

        Private Function CanTrainerInGruppeEinteilen() As Boolean
            Return SelectedGruppe IsNot Nothing AndAlso SelectedEinteilung IsNot Nothing
        End Function

        Private Sub OnTrainerInGruppeEinteilen(obj As Object)
            Dim selectedList As New List(Of Trainer)

            If obj IsNot Nothing Then
                Dim asEnumerable = TryCast(obj, System.Collections.IEnumerable)
                If asEnumerable IsNot Nothing Then
                    For Each item In asEnumerable
                        Dim t = TryCast(item, Trainer)
                        If t IsNot Nothing Then selectedList.Add(t)
                    Next
                Else
                    Dim [single] = TryCast(obj, Trainer)
                    If [single] IsNot Nothing Then selectedList.Add([single])
                End If
            End If

            If selectedList.Count = 0 AndAlso SelectedAlleGruppenloserTrainer IsNot Nothing Then
                For Each t In SelectedAlleGruppenloserTrainer
                    selectedList.Add(t)
                Next
            End If

            If selectedList.Count = 0 OrElse SelectedGruppe Is Nothing OrElse SelectedEinteilung Is Nothing Then
                Return
            End If

            Dim TrainerService As New TrainerService()
            TrainerService.TrainerInGruppeEinteilen(selectedList, SelectedGruppe, SelectedEinteilung)
        End Sub

        Private Function CanTrainerAusGruppeEntfernen() As Boolean
            Return SelectedGruppe IsNot Nothing AndAlso SelectedGruppe.Trainer IsNot Nothing
        End Function

        Private Sub OnTrainerAusGruppeEntfernen(obj As Object)
            Dim TrainerService As New TrainerService()
            TrainerService.TrainerAusGruppeEntfernen(SelectedGruppe, SelectedEinteilung)
        End Sub

        Private Function CanTrainerInEinteilungHinzufuegen() As Boolean
            Return False
        End Function

        Private Sub OnTrainerInEinteilungHinzufuegen(obj As Object)
            Dim TS As New TrainerService()
            TS.TrainerEinteilungHinzufuegen(New TrainerCollection, New Einteilung)
        End Sub

        Private Function CanTrainerAusEinteilungEntfernen() As Boolean
            Return SelectedEinteilung IsNot Nothing AndAlso SelectedEinteilung IsNot Nothing
        End Function

        Private Sub OnTrainerAusEinteilungEntfernen(obj As Object)

            Dim selectedList As New List(Of Trainer)

            If obj IsNot Nothing Then
                Dim asEnumerable = TryCast(obj, System.Collections.IEnumerable)
                If asEnumerable IsNot Nothing Then
                    For Each item In asEnumerable
                        Dim t = TryCast(item, Trainer)
                        If t IsNot Nothing Then selectedList.Add(t)
                    Next
                Else
                    Dim [single] = TryCast(obj, Trainer)
                    If [single] IsNot Nothing Then selectedList.Add([single])
                End If
            End If

            If selectedList.Count = 0 AndAlso SelectedAlleMitglieder IsNot Nothing Then
                For Each t In SelectedAlleGruppenloserTrainer
                    selectedList.Add(t)
                Next
            End If

            If selectedList.Count = 0 OrElse SelectedGruppe Is Nothing OrElse SelectedEinteilung Is Nothing Then
                Return
            End If

            Dim TrainerService As New TrainerService()
            TrainerService.TrainerAusEinteilungEntfernen(selectedList, SelectedEinteilung)

        End Sub
#End Region

#Region "Event Handler / PropertyChanged"
        Private Sub HandlerPropertyChanged(sender As Object, e As PropertyChangedEventArgs)
            DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()

            DirectCast(EinteilungsuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(EinteilungErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()

            DirectCast(GruppenuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(GruppeErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(GruppeAusEinteilungEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()

            DirectCast(TeilnehmerInGruppeEinteilenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TeilnehmerAusGruppeEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TeilnehmerAusEinteilungEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TeilnehmeruebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TeilnehmerErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()

            DirectCast(TrainerInGruppeEinteilenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TrainerAusGruppeEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TrainerAusEinteilungEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TraineruebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(TrainerErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()

            DirectCast(LeistungsstufenuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(LeistungsstufeErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()

            DirectCast(FaehigkeitenuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(FaehigkeitErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
        End Sub

        Private Sub HandlerTrainerServiceTrainerGeaendert(sender As Object, e As TrainerEventArgs)
            OnPropertyChanged(NameOf(VerfuegbareTrainerListe))
        End Sub

        Private Sub HandlerDateiServicePropertyChanged(sender As Object, e As PropertyChangedEventArgs)
            If e.PropertyName = NameOf(DateiService.AktuellerClub) Then
                DirectCast(ClubCloseCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                DirectCast(ClubSaveCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                DirectCast(ClubSaveAsCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                DirectCast(ClubOpenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()
            End If
        End Sub

        Private Sub HandlerSetProperties(sender As Object, e As EventArgs)
            WindowTitleText = DefaultWindowTitleText & " - " & DateiService.AktuellerClub.ClubName
            AlleEinteilungenCV = CollectionViewSource.GetDefaultView(DateiService.AktuellerClub.Einteilungsliste)

            DirectCast(ClubCloseCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubSaveCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubSaveAsCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubOpenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()

            ZeigeErfolgsMeldung(Me, e)
        End Sub

        Private Sub HandlerResetProperties(sender As Object, e As EventArgs)
            WindowTitleText = DefaultWindowTitleText
            AlleEinteilungenCV = Nothing

            DirectCast(ClubCloseCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubSaveCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubSaveAsCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubOpenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()
        End Sub

        Private Sub OnSelectedAlleMitgliederCollectionChanged(sender As Object, e As NotifyCollectionChangedEventArgs)
            OnPropertyChanged(NameOf(SelectedAlleMitglieder))
            If TeilnehmerAusGruppeEntfernenCommand IsNot Nothing Then
                DirectCast(TeilnehmerAusGruppeEntfernenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            End If
        End Sub

        Private Sub OnMitgliederlisteGeaendert(sender As Object, e As EventArgs)
            OnPropertyChanged(NameOf(SelectedAlleMitglieder))
        End Sub

#End Region

#Region "Helpers / Utilities"
        Private Sub HandlerZeigeFehlerMeldung(sender As Object, e As DateiEventArgs)
            MessageBox.Show(e.DateiPfad, "Fehler beim Öffnen der Datei", MessageBoxButton.OK, MessageBoxImage.Error)
        End Sub

        Private Sub ZeigeErfolgsMeldung(sender As Object, e As DateiEventArgs)
            MessageBox.Show(e.DateiPfad, "Datei öffnen erfolgreich", MessageBoxButton.OK, MessageBoxImage.Information)
        End Sub

        Private Sub ZeigeErfolgsMeldung(sender As Object, e As EventArgs)
            ' Platzhalter
        End Sub

        Private Sub RefreshMostRecentMenu()
            MostRecentlyUsedMenuItem.Clear()
            RefreshMenuInApplication()
            RefreshJumpListInWinTaskbar()
        End Sub

        Private Sub RefreshMenuInApplication()
            For i = DateiService.ZuletztVerwendeteDateienSortedList.Values.Count - 1 To 0 Step -1
                Dim mi As New MenuEintragViewModel() With {
                    .Titel = DateiService.ZuletztVerwendeteDateienSortedList.Values(i),
                    .Sortierung = i,
                    .Befehl = New RelayCommand(Of String)(AddressOf HandleMostRecentClick)}
                MostRecentlyUsedMenuItem.Add(mi)
            Next

            If MostRecentlyUsedMenuItem.Count = 0 Then
                Dim mi = New MenuEintragViewModel With {.Titel = "keine", .Sortierung = 1, .Befehl = Nothing}
                MostRecentlyUsedMenuItem.Add(mi)
            End If
        End Sub

        Private Sub RefreshJumpListInWinTaskbar()
            Dim jumplist = New JumpList With {
                .ShowFrequentCategory = False,
                .ShowRecentCategory = False}

            Dim jumptask = New JumpTask With {
                .CustomCategory = "Release Notes",
                .Title = "SkikursReleaseNotes",
                .Description = "Zeigt die ReleaseNotes zu Skikurse an",
                .ApplicationPath = "C:\Windows\notepad.exe",
                .IconResourcePath = "C:\Windows\notepad.exe",
                .WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                .Arguments = "SkikursReleaseNotes.txt"}

            jumplist.JumpItems.Add(jumptask)

            For i = DateiService.ZuletztVerwendeteDateienSortedList.Count - 1 To 0 Step -1
                Dim jumpPath = New JumpPath With {
                    .CustomCategory = "Zuletzt geöffnet",
                    .Path = $"!Pfad{i}"}
                jumplist.JumpItems.Add(jumpPath)
            Next

            JumpList.SetJumpList(Application.Current, jumplist)
        End Sub

        Private Sub HandleMostRecentClick(sender As Object)
            MessageBox.Show(DateiService.DateiLaden(sender))
        End Sub

        Public Function KopiereListeMitNeuenObjekten(Of T)(originalList As List(Of T), copyConstructor As Func(Of T, T)) As List(Of T)
            Dim copiedList As New List(Of T)
            For Each item In originalList
                copiedList.Add(copyConstructor(item))
            Next
            Return copiedList
        End Function

        Public Function GetLeistungsstufenliste() As LeistungsstufeCollection
            Return DateiService.AktuellerClub.Leistungsstufenliste
        End Function
#End Region

    End Class
End Namespace
