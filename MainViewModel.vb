Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Shell
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Services
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
#End Region

#Region "Konstruktor"

        Private Sub New()
            WindowLoadedCommand = New RelayCommand(Of Object)(AddressOf OnWindowLoaded)
        End Sub

        Public Sub New(windowService As IWindowService)
            MyBase.New()
            _windowService = windowService
            WindowLoadedCommand = New RelayCommand(Of Object)(AddressOf OnWindowLoaded)
        End Sub
#End Region

#Region "Window Events"

        Private Sub OnWindowLoaded(obj As Object)

            ApplicationCloseCommand = New RelayCommand(Of Object)(AddressOf OnWindowClose)
            WindowClosingCommand = New RelayCommand(Of CancelEventArgs)(AddressOf OnWindowClosing)
            WindowClosedCommand = New RelayCommand(Of Object)(AddressOf OnWindowClosed)
            ApplicationNewCommand = New RelayCommand(Of Object)(AddressOf OnApplicationNew)
            ApplicationOpenCommand = New RelayCommand(Of Object)(AddressOf OnApplicationOpen)
            ApplicationSaveCommand = New RelayCommand(Of Object)(AddressOf OnApplicationSave, Function() CanApplicationSave())
            ApplicationSaveAsCommand = New RelayCommand(Of Object)(AddressOf OnApplicationSaveAs, Function() CanApplicationSaveAs())
            ApplicationPrintCommand = New RelayCommand(Of Object)(AddressOf OnApplicationPrint, Function() CanApplicationPrint())

            ' 3. SortedList für meist genutzte Skischulen befüllen
            DateiService.LadeMeistVerwendeteDateienInSortedList()

            ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
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

            RefreshMostRecentMenu()
            RefreshJumpListInWinTaskbar()

        End Sub

        Private Function CanApplicationPrint() As Boolean
            Return False
        End Function

        Private Sub OnApplicationPrint(obj As Object)
            Throw New NotImplementedException()
        End Sub

        Private Function CanApplicationSaveAs() As Boolean
            Return False
        End Function

        Private Function CanApplicationSave() As Boolean
            Return False
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
            DateiService.SpeicherMeistVerwendeteDateienSortedListInsIsolatedStorage()

        End Sub

#End Region

#Region "Properties"

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

        Public Property Einteilungsliste As EinteilungCollection

        Public Property AlleEinteilungen As EinteilungCollection
            Get
                Return AktuellerClub?.AlleEinteilungen
            End Get
            Set(value As EinteilungCollection)
                AktuellerClub.AlleEinteilungen = value
            End Set
        End Property


        Private _SelectedEinteilung As Einteilung

        Public Property SelectedEinteilung As Einteilung
            Get
                Return _SelectedEinteilung
            End Get
            Set(value As Einteilung)
                _SelectedEinteilung = value
                SelectedGruppe = Nothing
            End Set
        End Property


        Private _SelectedGruppe As Gruppe
        Public Property SelectedGruppe As Gruppe
            Get
                Return _SelectedGruppe
            End Get
            Set(value As Gruppe)
                _SelectedGruppe = value
            End Set
        End Property

        Public Property GruppenloseTrainer As TrainerCollection

        Public Property GruppenloseTeilnehmer As TeilnehmerCollection

#End Region

#Region "Command Properties"
        Public Property WindowLoadedCommand As ICommand
        Public Property WindowClosedCommand As ICommand
        Public Property WindowClosingCommand As ICommand
        Public Property ApplicationNewCommand As ICommand
        Public Property ApplicationOpenCommand As ICommand
        Public Property ApplicationSaveCommand As ICommand
        Public Property ApplicationSaveAsCommand As ICommand
        Public Property ApplicationPrintCommand As ICommand
        Public Property ApplicationCloseCommand As ICommand

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

#Region "Functions"

        Public Function KopiereListeMitNeuenObjekten(Of T)(originalList As List(Of T), copyConstructor As Func(Of T, T)) As List(Of T)
            Dim copiedList As New List(Of T)
            For Each item In originalList
                copiedList.Add(copyConstructor(item))
            Next
            Return copiedList
        End Function


        '''' <summary>
        '''' Lädt eine XML-Datei und erstellt daraus einen Club
        '''' </summary>
        'Public Sub DateiLaden()
        '    Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}

        '    If dlg.ShowDialog = True Then
        '        Club = Controller.SkiDatenLaden.SkiDateiLesen(dlg.FileName)
        '        Dim Einteilungen = Controller.SkiDatenLaden.EinteilungenLesen(Club)

        '        If Club IsNot Nothing Then
        '            Einteilungen.ToList.ForEach(Sub(E) Club.AddEinteilung(E))
        '        End If
        '    End If
        'End Sub

        '''' <summary>
        '''' Lädt aus einer XML-Datei eine Einteilung 
        '''' </summary>
        'Public Sub EinteilungAusDateiLaden()

        '    Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        '    If dlg.ShowDialog = True Then
        '        Dim lokalerClub = Controller.SkiDatenLaden.SkiDateiLesen(dlg.FileName)

        '        If lokalerClub IsNot Nothing Then
        '            Dim Einteilungsname = BestimmeEinteilungsbenennung()
        '            Trainerliste = lokalerClub.AlleTrainer
        '            If Club.Einteilungsliste IsNot Nothing AndAlso Club.Einteilungsliste.Count > 0 Then
        '            Else
        '                Einteilungsliste = New EinteilungCollection From {New Einteilung With {.Benennung = Einteilungsname, .Gruppenliste = Club.Gruppenliste}}
        '            End If
        '        End If
        '    End If
        'End Sub

        'Public Function BestimmeEinteilungsbenennung(Einteilungsliste As EinteilungCollection) As String
        '    Dim Einteilungsname = String.Empty
        '    Dim Zaehler As Integer
        '    If Club.Einteilungsliste.Count > 0 Then
        '        Einteilungsname = Club.Einteilungsliste.OrderByDescending(Function(e) e.Sortierung).First.Benennung
        '        Zaehler = Val(Einteilungsname.Last) + 1
        '        Einteilungsname &= Zaehler
        '    Else
        '        Einteilungsname = "Tag1"
        '    End If
        '    Return Einteilungsname
        'End Function

        'Public Function BestimmeEinteilungsbenennung() As String
        '    Dim Einteilungsname = String.Empty
        '    Dim Zaehler As Integer
        '    If Einteilungsliste.Count > 0 Then
        '        Einteilungsname = Club.Einteilungsliste.OrderByDescending(Function(e) e.Sortierung).First.Benennung
        '        Zaehler = Val(Einteilungsname.Last) + 1
        '        Einteilungsname &= Zaehler
        '    Else
        '        Einteilungsname = "Tag1"
        '    End If
        '    Return Einteilungsname
        'End Function

#End Region

#Region "Methoden"

        ''' <summary>
        ''' Wird aufgerufen, wenn im Menu Öffnen geklickt wird.
        ''' </summary>
        ''' <param name="obj"></param>
        Private Sub OnApplicationNew(obj As Object)
            DateiService.NeueDateiErstellen()
            DateiService.SchreibeFilenameInMeistVerwendeteDateienSortedList(DateiService.AktuelleDatei.FullName)
            SetProperties()
        End Sub

        ''' <summary>
        ''' Wird aufgerufen, wenn im Menu Öffnen geklickt wird.
        ''' Zeigt einen Dialog zum Öffnen einer Groupies-Datei an.
        ''' </summary>
        ''' <param name="obj"></param>
        Private Sub OnApplicationOpen(obj As Object)
            DateiService.DateiLaden()
            DateiService.SchreibeFilenameInMeistVerwendeteDateienSortedList(DateiService.AktuelleDatei.FullName)
            SetProperties()
        End Sub

        Private Sub OnApplicationSave(obj As Object)
            DateiService.DateiSpeichern()
        End Sub

        Private Sub OnApplicationSaveAs(obj As Object)
            DateiService.DateiSpeichernAls()
            DateiService.SchreibeFilenameInMeistVerwendeteDateienSortedList(DateiService.AktuelleDatei.FullName)
            SetProperties()
        End Sub


        ''' <summary>
        ''' Handler für Eintrag aus 'Zuletzt geöffnet'
        ''' </summary>
        ''' <param name="sender"></param>
        Private Sub HandleMostRecentClick(sender As Object)
            DateiService.DateiLaden(New FileInfo(sender))
            DateiService.SchreibeFilenameInMeistVerwendeteDateienSortedList(sender)
        End Sub

        Private Sub OpenGroupies(fileName As String)
            DateiService.DateiLaden(New FileInfo(fileName))
            DateiService.SchreibeFilenameInMeistVerwendeteDateienSortedList(fileName)
        End Sub

        Private Sub SetProperties()
            WindowTitleText = DefaultWindowTitleText & " - " & AppController.GroupiesFile.Name
        End Sub

#End Region

#Region "Methoden zum Laden der meist genutzten Groupies und der letzten Groupies Datei"

        ''' <summary>
        ''' Lädt die zuletzt verwendeten Skischulen in eine SortedList und aktualisiert das Menü.
        ''' </summary>
        Public Sub LoadmRUSortedListMenu()
            Try
                ' IsolatedStorage initialisiern
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                    ' Die Datei mRUSortedList in den Stream packen
                    Using stream = New IsolatedStorageFileStream("mRuSortedList", System.IO.FileMode.Open, iso)
                        ' Den Stream lesen
                        Using reader = New StreamReader(stream)
                            Dim i = 0
                            ' Gibt es zu lesende Zeichen in dem Reader
                            While reader.Peek <> -1
                                ' Die Zeilen aus dem Reader lesen und splitten
                                Dim line = reader.ReadLine().Split(";")
                                ' Prüfen, ob die Zeile gesplittet werden konnte UND 
                                ' Prüfen, ob der erste Teil (Key) größer als 0 ist UND
                                ' Prüfen, ob der Key Wert bereits in der Variablen _mRuSortedList vorhanden ist
                                If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not DateiService.MeistVerwendeteDateienSortedList.ContainsKey(Integer.Parse(line(0))) Then
                                    ' Prüfen, ob die Datei (Wert) auf dem Rechner vorhanden ist
                                    If File.Exists(line(1)) Then
                                        ' Key erhöhen
                                        i += 1
                                        ' Key-Value der Liste hinzufügen
                                        DateiService.MeistVerwendeteDateienSortedList.Add(i, line(1))
                                    End If
                                End If
                            End While
                        End Using
                    End Using
                End Using
                RefreshMostRecentMenu()
            Catch ex As FileNotFoundException
                Throw ex
            End Try
        End Sub


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

            For i = DateiService.MeistVerwendeteDateienSortedList.Values.Count - 1 To 0 Step -1
                Dim mi As New MenuEintragViewModel() With {
                    .Titel = DateiService.MeistVerwendeteDateienSortedList.Values(i),
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
        ''' Lädt die Datei der letzten App-Ausführung
        ''' </summary>
        Private Sub LoadLastGroupies()
            ' Die LastGroupies aus dem IsolatedStorage einlesen.
            Try
                Dim Filestring = String.Empty
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.Open, iso)
                        Using reader = New StreamReader(stream)
                            Filestring = reader.ReadLine
                        End Using
                    End Using
                End Using
                If File.Exists(Filestring) Then OpenGroupies(Filestring)
            Catch ex As FileNotFoundException
            End Try
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

            For i = DateiService.MeistVerwendeteDateienSortedList.Count - 1 To 0 Step -1
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
End Namespace
