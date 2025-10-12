Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Markup
Imports System.Windows.Shell
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Interfaces
Imports Groupies.MainWindow
Imports Groupies.Services
Imports Groupies.UserControls
Imports Microsoft.Win32

Namespace ViewModels

    ''' <summary>
    ''' ViewModel für die Hauptansicht der Anwendung.
    ''' Es verwaltet die Einteilungen, Gruppen, Trainer und Teilnehmer.
    ''' </summary>
    Public Class MainViewModel
        Inherits BaseModel

#Region "Felder"
        Private ReadOnly _windowService As IWindowService
        Private ReadOnly DateiService As DateiService
#End Region

#Region "Konstruktor"

        Private Sub New()
            DateiService = New DateiService
            WindowLoadedCommand = New RelayCommand(Of Object)(AddressOf OnWindowLoaded)
        End Sub

        Public Sub New(windowService As IWindowService)
            MyBase.New()
            _windowService = windowService
            DateiService = New DateiService
            WindowLoadedCommand = New RelayCommand(Of Object)(AddressOf OnWindowLoaded)
            GruppendetailViewModel = New GruppendetailViewModel()
        End Sub
#End Region

#Region "Window Events"

        Private Sub OnWindowLoaded(obj As Object)

            AddHandler DateiService.PropertyChanged, Sub(sender, e)
                                                         If e.PropertyName = NameOf(DateiService.AktuellerClub) Then
                                                             DirectCast(ClubCloseCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(ClubSaveCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(ClubSaveAsCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()
                                                             DirectCast(EinteilungsuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(GruppenuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(LeistungsstufenuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(FaehigkeitenuebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(TraineruebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(TeilnehmeruebersichtAnzeigenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(EinteilungErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(GruppeErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(LeistungsstufeErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(FaehigkeitErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(TrainerErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(TeilnehmerErstellenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                             DirectCast(TeilnehmerEinteilenCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
                                                         End If
                                                     End Sub
            AddHandler PropertyChanged, Sub(sender, e)
                                            If e.PropertyName = NameOf(SelectedEinteilung) Then
                                                DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()
                                            End If
                                        End Sub
            'AddHandler DateiService.PropertyChanged, Sub(sender, e)
            '                                             If e.PropertyName = NameOf(DateiService.AktuellerClub.SelectedEinteilung.EinteilungAlleGruppen) Then
            '                                                 DirectCast(ClubInfoPrintCommand, RelayCommand(Of Printversion)).RaiseCanExecuteChanged()
            '                                             End If
            '                                         End Sub

            InitializeCommands()


            ' 3. SortedList für meist genutzte Skischulen befüllen
            DateiService.LadeMeistVerwendeteDateienInSortedList()

            ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
            If (Environment.GetCommandLineArgs().Length = 2) Then
                Dim args = Environment.GetCommandLineArgs
                Dim filename = args(1)
                DateiService.DateiLaden(filename)
            Else
                Dim LetzteDatei = DateiService.LiesZuletztGeoeffneteDatei
                If LetzteDatei IsNot Nothing AndAlso Not String.IsNullOrEmpty(LetzteDatei) Then
                    DateiService.DateiLaden(LetzteDatei)
                End If
            End If

            If DateiService.AktuellerClub IsNot Nothing Then
                RefreshMostRecentMenu()
                RefreshJumpListInWinTaskbar()
                SetProperties()
            Else
                ResetProperties()
            End If

        End Sub

        Private Sub InitializeCommands()

            ApplicationCloseCommand = New RelayCommand(Of Object)(AddressOf OnWindowClose)
            WindowClosingCommand = New RelayCommand(Of CancelEventArgs)(AddressOf OnWindowClosing)
            WindowClosedCommand = New RelayCommand(Of Object)(AddressOf OnWindowClosed)
            ClubNewCommand = New RelayCommand(Of Object)(AddressOf OnClubNew)
            ClubOpenCommand = New RelayCommand(Of Object)(AddressOf OnClubOpen)
            ClubSaveCommand = New RelayCommand(Of Object)(AddressOf OnClubSave, Function() CanClubSave())
            ClubSaveAsCommand = New RelayCommand(Of Object)(AddressOf OnClubSaveAs, Function() CanClubSaveAs())
            ClubCloseCommand = New RelayCommand(Of Object)(AddressOf OnClubClose, Function() CanClubClose())
            ClubInfoPrintCommand = New RelayCommand(Of Printversion)(AddressOf OnClubInfoPrint, Function() CanClubInfoPrint())

            EinteilungsuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnEinteilungsuebersichtAnzeigen, Function() CanEinteilungsuebersichtAnzeigen())
            GruppenuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnGruppenuebersichtAnzeigen, Function() CanGruppenuebersichtAnzeigen())
            LeistungsstufenuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnLeistungsstufenuebersichtAnzeigen, Function() CanLeistungsstufenuebersichtAnzeigen())
            FaehigkeitenuebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnFaehigkeitenuebersichtAnzeigen, Function() CanFaehigkeitenuebersichtAnzeigen())
            TraineruebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnTraineruebersichtAnzeigen, Function() CanTraineruebersichtAnzeigen())
            TeilnehmeruebersichtAnzeigenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmeruebersichtAnzeigen, Function() CanTeilnehmeruebersichtAnzeigen())

            EinteilungErstellenCommand = New RelayCommand(Of Object)(AddressOf OnEinteilungErstellen, Function() CanEinteilungErstellen())
            GruppeErstellenCommand = New RelayCommand(Of Object)(AddressOf OnGruppeErstellen, Function() CanGruppeErstellen())
            LeistungsstufeErstellenCommand = New RelayCommand(Of Object)(AddressOf OnLeistungsstufeErstellen, Function() CanLeistungsstufeErstellen())
            FaehigkeitErstellenCommand = New RelayCommand(Of Object)(AddressOf OnFaehigkeitErstellen, Function() CanFaehigkeitErstellen())
            TrainerErstellenCommand = New RelayCommand(Of Object)(AddressOf OnTrainerErstellen, Function() CanTrainerErstellen())
            TeilnehmerErstellenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerErstellen, Function() CanTeilnehmerErstellen())
            TeilnehmerEinteilenCommand = New RelayCommand(Of Object)(AddressOf OnTeilnehmerEinteilen, Function() CanTeilnehmerEinteilen())

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


        Public Shared Function PrintoutInfo(Printversion As Printversion, pageSize As Size, pageMargin As Thickness) As FixedDocument

            ' ein paar Variablen setzen
            Dim printFriendHeight As Double = 1000 ' Breite einer Gruppe
            Dim printFriendWidth As Double = 730 '  Höhe einer Gruppe

            ' ermitteln der tatsächlich verfügbaren Seitengröße
            Dim availablePageHeight As Double = pageSize.Height - pageMargin.Top - pageMargin.Bottom
            Dim availablePageWidth As Double = pageSize.Width - pageMargin.Left - pageMargin.Right

            ' ermitteln der Anzahl Spalten und Zeilen
            Dim rowsPerPage As Integer = CType(Math.Floor(availablePageHeight / printFriendHeight), Integer)
            Dim columnsPerPage As Integer = CType(Math.Floor(availablePageWidth / printFriendWidth), Integer)

            ' mindestens eine Zeile und Spalte verwenden, damit beim späteren Loop keine Endlos-Schleife entsteht
            If rowsPerPage = 0 Then rowsPerPage = 1
            If columnsPerPage = 0 Then columnsPerPage = 1

            Dim participantsPerPage As Integer = rowsPerPage * columnsPerPage


            ' ermitteln der vertikalen und horizontalen Abstände zwischen Freunden
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

            'Todo: Berechnen, wie viele Teilnehmer auf einer Seite gedruckt werden können
            Dim doc = New FixedDocument()
            doc.DocumentPaginator.PageSize = pageSize
            ' Objekte in der Skischule neu lesen, falls etwas geändert wurde


            ' nach AngezeigterName sortierte Liste verwenden
            Dim sortedGroupView = New ListCollectionView()
            sortedGroupView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Descending))

            Dim skikursgruppe As Gruppe
            Dim page As FixedPage = Nothing

            ' durch die Gruppen loopen und Seiten generieren
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


                ' Printable-Control mit Group-Objekt initialisieren und zur Page hinzufügen
                Dim pSkikursgruppe As IPrintableNotice
                If Printversion = Printversion.TeilnehmerInfo Then
                    pSkikursgruppe = New TeilnehmerAusdruckUserControl
                Else
                    pSkikursgruppe = New TrainerausdruckUserControl
                End If

                DirectCast(pSkikursgruppe, UserControl).Height = printFriendHeight
                DirectCast(pSkikursgruppe, UserControl).Width = printFriendWidth

                If String.IsNullOrWhiteSpace(SelectedEinteilung.Benennung) Then
                    SelectedEinteilung.Benennung = InputBox("Bitte diese Einteilung benennen")
                End If

                pSkikursgruppe.InitPropsFromGroup(skikursgruppe, SelectedEinteilung.Benennung)
                Dim currentRow As Integer = (i Mod participantsPerPage) / columnsPerPage
                Dim currentColumn As Integer = i Mod columnsPerPage

                FixedPage.SetTop(pSkikursgruppe, pageMargin.Top + ((DirectCast(pSkikursgruppe, UserControl).Height + vMarginBetweenFriends) * currentRow))
                FixedPage.SetLeft(pSkikursgruppe, pageMargin.Left + ((DirectCast(pSkikursgruppe, UserControl).Width + hMarginBetweenFriends) * currentColumn))
                page.Children.Add(pSkikursgruppe)
            Next

            Return doc

        End Function

        Public Shared Function PrintoutInfo(Einteilung As Einteilung, Printversion As Printversion, pageSize As Size, pageMargin As Thickness) As FixedDocument

            ' ein paar Variablen setzen
            Dim printFriendHeight As Double = 1000 ' Breite einer Gruppe
            Dim printFriendWidth As Double = 730 '  Höhe einer Gruppe

            ' ermitteln der tatsächlich verfügbaren Seitengröße
            Dim availablePageHeight As Double = pageSize.Height - pageMargin.Top - pageMargin.Bottom
            Dim availablePageWidth As Double = pageSize.Width - pageMargin.Left - pageMargin.Right

            ' ermitteln der Anzahl Spalten und Zeilen
            Dim rowsPerPage As Integer = CType(Math.Floor(availablePageHeight / printFriendHeight), Integer)
            Dim columnsPerPage As Integer = CType(Math.Floor(availablePageWidth / printFriendWidth), Integer)

            ' mindestens eine Zeile und Spalte verwenden, damit beim späteren Loop keine Endlos-Schleife entsteht
            If rowsPerPage = 0 Then rowsPerPage = 1
            If columnsPerPage = 0 Then columnsPerPage = 1

            Dim participantsPerPage As Integer = rowsPerPage * columnsPerPage


            ' ermitteln der vertikalen und horizontalen Abstände zwischen Freunden
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

            'Todo: Berechnen, wie viele Teilnehmer auf einer Seite gedruckt werden können
            Dim doc = New FixedDocument()
            doc.DocumentPaginator.PageSize = pageSize
            ' Objekte in der Skischule neu lesen, falls etwas geändert wurde


            ' nach AngezeigterName sortierte Liste verwenden
            Dim sortedGroupView = New ListCollectionView(Einteilung.EinteilungAlleGruppen)
            'Dim sortedGroupView = New ListCollectionView(AppController.AktuellerClub.SelectedEinteilung.EinteilungAlleGruppen)
            sortedGroupView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Descending))

            Dim skikursgruppe As Gruppe
            Dim page As FixedPage = Nothing

            ' durch die Gruppen loopen und Seiten generieren
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


                ' Printable-Control mit Group-Objekt initialisieren und zur Page hinzufügen
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


        Private Sub OnWindowClose(obj As Object)
            _windowService.CloseWindow()
        End Sub

        Private Sub OnWindowClosing(e As CancelEventArgs)
            Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schließen?", "Achtung", MessageBoxButton.YesNo)
            e.Cancel = (result = MessageBoxResult.No)
        End Sub

        Private Sub OnWindowClosed(obj As Object)

            ' 1. Den Pfad der letzten Liste ins IsolatedStorage speichern.
            DateiService.SpeicherZuletztVerwendeteDateiInsIolatedStorage()

            ' 2. Die meist genutzten Listen ins Isolated Storage speichern
            DateiService.SpeicherZuletztVerwendeteDateienSortedList()

        End Sub

#End Region

#Region "Command Properties"
        Public Property ApplicationCloseCommand As ICommand
        Public Property WindowLoadedCommand As ICommand
        Public Property WindowClosedCommand As ICommand
        Public Property WindowClosingCommand As ICommand
        Public Property ClubNewCommand As ICommand
        Public Property ClubOpenCommand As ICommand
        Public Property ClubSaveCommand As ICommand
        Public Property ClubSaveAsCommand As ICommand
        Public Property ClubInfoPrintCommand As ICommand
        Public Property ClubCloseCommand As ICommand

        Public Property EinteilungErstellenCommand As ICommand
        Public Property EinteilungsuebersichtAnzeigenCommand As ICommand

        Public Property GruppeErstellenCommand As ICommand
        Public Property GruppeLoeschenCommand As ICommand
        Public Property GruppenuebersichtAnzeigenCommand As ICommand

        Public Property LeistungsstufeErstellenCommand As ICommand
        Public Property LeistungsstufenuebersichtAnzeigenCommand As ICommand

        Public Property FaehigkeitErstellenCommand As ICommand
        Public Property FaehigkeitenuebersichtAnzeigenCommand As ICommand

        Public Property TeilnehmerErstellenCommand As ICommand
        Public Property TeilnehmerEinteilenCommand As ICommand
        Public Property TeilnehmerBearbeitenCommand As ICommand
        Public Property TeilnehmerLoeschenCommand As ICommand
        Public Property TeilnehmeruebersichtAnzeigenCommand As ICommand


        Public Property TrainerErstellenCommand As ICommand
        Public Property TrainerEinteilenCommand As ICommand
        Public Property TrainerLoeschenCommand As ICommand
        Public Property TrainerBearbeitenCommand As ICommand
        Public Property TraineruebersichtAnzeigenCommand As ICommand




#End Region

#Region "Properties"
        Public Property SelectedAlleGruppenloserTeilnehmer As New ObservableCollection(Of Teilnehmer)
        Public Property MostRecentlyUsedMenuItem As New ObservableCollection(Of MenuEintragViewModel)

        Public Property WindowTitleIcon As String = "pack://application:,,,/Images/icons8-ski-resort-48.png"

        Private ReadOnly DefaultWindowTitleText As String = "Groupies - Ski Club Management"

        Private _WindowTitleText As String

        Public Property WindowTitleText As String
            Get
                Return _WindowTitleText
            End Get
            Set(value As String)
                _WindowTitleText = value
            End Set
        End Property

        Private _AlleEinteilungenCV As CollectionView
        ''' <summary>
        ''' Alle Einteilungen des aktuellen Clubs.
        ''' </summary>
        ''' <remarks>Diese Property wird in der View für die Anzeige der Einteilungen verwendet.</remarks>
        Public Property AlleEinteilungenCV As CollectionView
            Get
                Return _AlleEinteilungenCV
            End Get
            Set(value As CollectionView)
                _AlleEinteilungenCV = value
            End Set
        End Property

        ''' <summary>
        ''' Alle Leistungsstufen des aktuellen Clubs.
        ''' </summary>
        ''' <remarks>Diese Property wird in der View für die Anzeige der Einteilungen verwendet.</remarks>
        Public ReadOnly Property LeistungsstufenListe As LeistungsstufeCollection
            Get
                Return GetLeistungsstufen()
            End Get
        End Property

        Public Shared Function GetLeistungsstufen() As ObservableCollection(Of Leistungsstufe)
            If DateiService.AktuellerClub IsNot Nothing Then
                'Return New ObservableCollection(Of Leistungsstufe)(DateiService.AktuellerClub.Leistungsstufenliste)
                Return DateiService.AktuellerClub.Leistungsstufenliste.Sortieren
            End If
            Return Nothing
        End Function

        Private _SelectedEinteilung As Einteilung

        Public Property SelectedEinteilung As Einteilung
            Get
                Return _SelectedEinteilung
            End Get
            Set(value As Einteilung)
                _SelectedEinteilung = value
                SelectedGruppe = Nothing
                OnPropertyChanged(NameOf(LeistungsstufenListe))
            End Set
        End Property


        Private _SelectedGruppe As Gruppe
        Public Property SelectedGruppe As Gruppe
            Get
                Return _SelectedGruppe
            End Get
            Set(value As Gruppe)
                _GruppendetailViewModel.Gruppe = value
                _SelectedGruppe = value
                OnPropertyChanged(NameOf(SelectedGruppe))
            End Set
        End Property

        Private _GruppendetailViewModel As GruppendetailViewModel
        Public Property GruppendetailViewModel As GruppendetailViewModel
            Get
                Return _GruppendetailViewModel
            End Get
            Set(value As GruppendetailViewModel)
                _GruppendetailViewModel = value
            End Set
        End Property

        Public Property GruppenloseTrainer As TrainerCollection

        Public Property GruppenloseTeilnehmer As TeilnehmerCollection


        Public Property CanClubClose() As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanClubClose))
            End Set
        End Property

        Private Property CanClubSave() As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanClubSave))
            End Set
        End Property

        Private Property CanClubSaveAs() As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanClubSaveAs))
            End Set
        End Property

        Private Property CanClubInfoPrint() As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing _
                    AndAlso SelectedEinteilung IsNot Nothing
                '_
                '    AndAlso DateiService.AktuellerClub.SelectedEinteilung.EinteilungAlleGruppen.Count > 0
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanClubInfoPrint))
            End Set
        End Property
        Public Property CanEinteilungsuebersichtAnzeigen() As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanEinteilungsuebersichtAnzeigen))
            End Set
        End Property

        Private Property CanFaehigkeitenuebersichtAnzeigen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanFaehigkeitenuebersichtAnzeigen))
            End Set
        End Property

        Private Property CanLeistungsstufenuebersichtAnzeigen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanLeistungsstufenuebersichtAnzeigen))
            End Set
        End Property

        Private Property CanGruppenuebersichtAnzeigen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanGruppenuebersichtAnzeigen))
            End Set
        End Property

        Private Property CanTraineruebersichtAnzeigen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanTraineruebersichtAnzeigen))
            End Set
        End Property

        Private Property CanTeilnehmeruebersichtAnzeigen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanTeilnehmeruebersichtAnzeigen))
            End Set
        End Property

        Private Property CanFaehigkeitErstellen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanFaehigkeitErstellen))
            End Set
        End Property

        Private Property CanLeistungsstufeErstellen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanLeistungsstufeErstellen))
            End Set
        End Property

        Private Property CanGruppeErstellen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanGruppeErstellen))
            End Set
        End Property

        Private Property CanEinteilungErstellen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanEinteilungErstellen))
            End Set
        End Property

        Private Property CanTrainerErstellen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanTrainerErstellen))
            End Set
        End Property

        Private Property CanTeilnehmerErstellen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanTeilnehmerErstellen))
            End Set
        End Property

        Private Property CanTeilnehmerEinteilen As Boolean
            Get
                Return DateiService.AktuellerClub IsNot Nothing
            End Get
            Set(value As Boolean)
                OnPropertyChanged(NameOf(CanTeilnehmerEinteilen))
            End Set
        End Property
#End Region

#Region "Functions"

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

#Region "Methoden"

        ''' <summary>
        ''' Wird aufgerufen, wenn im Menu Öffnen geklickt wird.
        ''' </summary>
        ''' <param name="obj"></param>
        Private Sub OnClubNew(obj As Object)
            DateiService.NeueDateiErstellen()
            DirectCast(ClubCloseCommand, RelayCommand(Of Object)).RaiseCanExecuteChanged()
            SetProperties()
        End Sub

        ''' <summary>
        ''' Wird aufgerufen, wenn im Menu Öffnen geklickt wird.
        ''' Zeigt einen Dialog zum Öffnen einer Groupies-Datei an.
        ''' </summary>
        ''' <param name="obj"></param>
        Private Sub OnClubOpen(obj As Object)
            MessageBox.Show(DateiService.DateiLaden())
            SetProperties()
        End Sub

        Private Sub OnClubSave(obj As Object)
            DateiService.DateiSpeichern()
        End Sub

        Private Sub OnClubSaveAs(obj As Object)
            DateiService.DateiSpeichernAls()
            SetProperties()
        End Sub
        Private Sub OnClubClose(obj As Object)
            DateiService.DateiSchliessen()
            ResetProperties()
        End Sub

        ''' <summary>
        ''' Handler für Eintrag aus 'Zuletzt geöffnet'
        ''' </summary>
        ''' <param name="sender"></param>
        Private Sub HandleMostRecentClick(sender As Object)
            MessageBox.Show(DateiService.DateiLaden(sender))
        End Sub

        Private Sub SetProperties()
            WindowTitleText = DefaultWindowTitleText & " - " & DateiService.AktuellerClub.ClubName
            AlleEinteilungenCV = CollectionViewSource.GetDefaultView(DateiService.AktuellerClub.Einteilungsliste)
        End Sub
        Private Sub ResetProperties()
            WindowTitleText = DefaultWindowTitleText
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

        Private Sub OnLeistungsstufenuebersichtAnzeigen(obj As Object)
            Dim fenster = New BasisUebersichtWindow() With {
            .Owner = _windowService.Window,
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(fenster)) With {
            .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Leistungsstufe),
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
            }
            mvw.AktuellesViewModel.Daten = DateiService.AktuellerClub.Leistungsstufenliste.Sortieren

            fenster.DataContext = mvw

            fenster.Show()
        End Sub

        Private Sub OnEinteilungsuebersichtAnzeigen()
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
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Anzeigen)
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

        Private Sub OnFaehigkeitErstellen(obj As Object)
            Dim dialog = New BasisDetailWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Faehigkeit),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)
            }
            mvw.AktuellesViewModel.Model = New Faehigkeit
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                DateiService.AktuellerClub.Faehigkeitenliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Faehigkeit).Benennung} wurde gespeichert")
            End If
        End Sub

        Private Sub OnLeistungsstufeErstellen(obj As Object)
            Dim dialog = New BasisDetailWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Leistungsstufe),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)
            }
            mvw.AktuellesViewModel.Model = New Leistungsstufe
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                DateiService.AktuellerClub.Leistungsstufenliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Leistungsstufe).Benennung} wurde gespeichert")
            End If
        End Sub

        Private Sub OnGruppeErstellen(obj As Object)

            Dim dialog = New BasisDetailWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Gruppe),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)
            }
            mvw.AktuellesViewModel.Model = New Gruppe
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                DateiService.AktuellerClub.Gruppenliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Gruppe).Benennung} wurde gespeichert")
            End If
        End Sub

        Private Sub OnEinteilungErstellen(obj As Object)
            Dim dialog = New BasisDetailWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Einteilung),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)
            }
            mvw.AktuellesViewModel.Model = New Einteilung
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                DateiService.AktuellerClub.Einteilungsliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Einteilung).Benennung} wurde gespeichert")
            End If
        End Sub

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
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                DateiService.AktuellerClub.Trainerliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Trainer).Spitzname} wurde gespeichert")
            End If
        End Sub

        Private Sub OnTeilnehmerErstellen(obj As Object)
            Dim dialog = New BasisDetailWindow() With {
                .Owner = _windowService.Window,
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            Dim mvw = New ViewModelWindow(New WindowService(dialog)) With {
                .Datentyp = New Fabriken.DatentypFabrik().ErzeugeDatentyp(Enums.DatentypEnum.Teilnehmer),
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen)
            }
            mvw.AktuellesViewModel.Model = New Teilnehmer
            dialog.DataContext = mvw

            Dim result As Boolean = dialog.ShowDialog()

            If result = True Then
                ' Todo: Das Speichern muss im ViewModel erledigt werden
                DateiService.AktuellerClub.Teilnehmerliste.Add(mvw.AktuellesViewModel.Model)
                MessageBox.Show($"{DirectCast(mvw.AktuellesViewModel.Model, Teilnehmer).VorUndNachname} wurde gespeichert")
            End If
        End Sub

        Private Sub OnTeilnehmerEinteilen(obj As Object)
            For i = SelectedAlleGruppenloserTeilnehmer.Count - 1 To 0 Step -1
                SelectedEinteilung.TeilnehmerInGruppeEinteilen(SelectedAlleGruppenloserTeilnehmer(i), SelectedGruppe)
            Next
        End Sub

#End Region

#Region "Methoden zum Laden der meist genutzten Groupies und der letzten Groupies Datei"

        Private Sub RefreshMostRecentMenu()
            MostRecentlyUsedMenuItem.Clear()

            RefreshMenuInApplication()
            RefreshJumpListInWinTaskbar()
        End Sub

        ''' <summary>
        ''' Aktualisiert das Menü in der Anwendung mit den zuletzt verwendeten Skischulen.
        ''' <remarks>Die Sortierung erfolgt in umgekehrter Reihenfolge, damit die zuletzt verwendete Skischule oben steht.</remarks>
        ''' </summary>
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


        ''' <summary>
        ''' Zur Aktualisierung der JumpList im Windows Taskbar
        ''' Diese Methode erstellt eine JumpList mit den zuletzt verwendeten Skischulen und dem Release Notes Eintrag.
        ''' </summary>
        Private Sub RefreshJumpListInWinTaskbar()
            'JumpList Klasse
            'Stellt eine Liste von Elementen und Aufgaben dar, die auf einer Windows 7-Taskleistenschaltfläche als Menü angezeigt werden.
            Dim jumplist = New JumpList With {
                .ShowFrequentCategory = False,
                .ShowRecentCategory = False}

            'JumpTask Klasse
            'Stellt eine Verknüpfung zu einer Anwendung in der Taskleisten-Sprungliste unter Windows 7 dar.
            Dim jumptask = New JumpTask With {
                .CustomCategory = "Release Notes",
                .Title = "SkikursReleaseNotes",
                .Description = "Zeigt die ReleaseNotes zu Skikurse an",
                .ApplicationPath = "C:\Windows\notepad.exe",
                .IconResourcePath = "C:\Windows\notepad.exe",
                .WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                .Arguments = "SkikursReleaseNotes.txt"}

            jumplist.JumpItems.Add(jumptask)

            ' Hinweis Die JumpPath - Elemente sind nur sichtbar, wenn die ".ski"-Dateiendung
            ' unter Windows mit Skikurs assoziiert wird (kann durch Installation via Setup-Projekt erreicht werden,
            ' das auch in den Beispielen enthalten ist, welches die dafür benötigten Werte in die Registry schreibt)

            For i = DateiService.ZuletztVerwendeteDateienSortedList.Count - 1 To 0 Step -1
                Dim jumpPath = New JumpPath With {
                    .CustomCategory = "Zuletzt geöffnet",
                    .Path = $"!Pfad{i}"}
                '_mRuSortedList.Select(Of String)(Function(M) M.Key = i).Titel}

                jumplist.JumpItems.Add(jumpPath)
            Next

            JumpList.SetJumpList(Application.Current, jumplist)

        End Sub

#End Region

    End Class

    Public Enum Printversion
        TrainerInfo
        TeilnehmerInfo
    End Enum
End Namespace
