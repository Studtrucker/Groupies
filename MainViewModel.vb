Imports System.Collections.ObjectModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Shell
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Services

Namespace ViewModels

    ''' <summary>
    ''' ViewModel für die Hauptansicht der Anwendung.
    ''' Es verwaltet die Einteilungen, Gruppen, Trainer und Teilnehmer.
    ''' </summary>
    Public Class MainViewModel
        Inherits BaseModel

#Region "Felder"
        Private _mRuSortedList As SortedList(Of Integer, String)
        Private _neueSortedList As New ObservableCollection(Of MenuEintragViewModel)
#End Region

#Region "Konstruktor"

        Public Sub New()
            WindowLoadedCommand = New RelayCommand(Of Object)(AddressOf OnWindowLoaded)
        End Sub

#End Region

#Region "Window Events"
        Private Sub OnWindowLoaded(obj As Object)

            ApplicationNewCommand = New RelayCommand(Of Object)(AddressOf OnApplicationNew)
            ApplicationOpenCommand = New RelayCommand(Of Object)(AddressOf OnApplicationOpen)
            ApplicationCloseCommand = New RelayCommand(Of Object)(Sub(o) Application.Current.Shutdown())

            ' 2. SortedList für meist genutzte Skischulen (Most Recently Used) initialisieren
            _mRuSortedList = New SortedList(Of Integer, String)

            ' 3. SortedList für meist genutzte Skischulen befüllen
            LoadmRUSortedListMenu()

            ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
            If (Environment.GetCommandLineArgs().Length = 2) Then
                Dim args = Environment.GetCommandLineArgs
                Dim filename = args(1)
                OpenGroupies(filename)
            Else
                LoadLastGroupies()
            End If

            RefreshJumpListInWinTaskbar()

        End Sub

#End Region

        Private Sub OpenGroupies(fileName As String)
            'If SkiDateienService.OpenSkiDatei(fileName) Then
            '    SetView()
            'End If

            AktuellerClub = Services.GroupiesApplication.ClubLaden(fileName)


        End Sub

        Private Sub LoadLastGroupies()
            ' Die letzte Skischule aus dem IsolatedStorage holen.
            Try
                Dim x = String.Empty
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.Open, iso)
                        Using reader = New StreamReader(stream)
                            x = reader.ReadLine
                        End Using
                    End Using

                End Using

                'If File.Exists(x) Then OpenSkischule(x)
                OpenGroupies(x)
            Catch ex As FileNotFoundException
            End Try
        End Sub

        Private Sub LoadmRUSortedListMenu()
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                    Using stream = New IsolatedStorageFileStream("mRuSortedList", System.IO.FileMode.Open, iso)
                        Using reader = New StreamReader(stream)
                            Dim i = 0
                            While reader.Peek <> -1
                                Dim line = reader.ReadLine().Split(";")
                                If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not _mRuSortedList.ContainsKey(Integer.Parse(line(0))) Then
                                    If File.Exists(line(1)) Then
                                        i += 1
                                        _mRuSortedList.Add(i, line(1))
                                        _neueSortedList.Add(New MenuEintragViewModel With {.Titel = line(1), .Sortierung = i})
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
            'MostRecentlyUsedMenuItem.Items.Clear()

            RefreshMenuInApplication()
            RefreshJumpListInWinTaskbar()
        End Sub

        Private Sub RefreshMenuInApplication()

            For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
                Dim mi As New MenuItem() With {.Header = _mRuSortedList.Values(i)}
                AddHandler mi.Click, AddressOf HandleMostRecentClick
                'MostRecentlyUsedMenuItem.Items.Add(mi)
            Next

            'If MostRecentlyUsedMenuItem.Items.Count = 0 Then
            '    Dim mi = New MenuItem With {.Header = "keine"}
            '    'MostRecentlyUsedMenuItem.Items.Add(mi)
            'End If
        End Sub
        Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
            OpenGroupies(TryCast(sender, MenuItem).Header.ToString())
        End Sub

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

            For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
                Dim jumpPath = New JumpPath With {
                    .CustomCategory = "Zuletzt geöffnet",
                    .Path = _mRuSortedList.Values(i)}

                jumplist.JumpItems.Add(jumpPath)
            Next

            JumpList.SetJumpList(Application.Current, jumplist)

        End Sub

        Private Sub OnApplicationNew(obj As Object)

            ' Ist aktuell eine Skischuldatei geöffnet?
            If AktuellerClub IsNot Nothing Then
                Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie den aktuellen Groupies Club noch speichern?", "", MessageBoxButton.YesNoCancel)
                If rs = MessageBoxResult.Yes Then
                    ApplicationCommands.Save.Execute(Nothing, Me)
                ElseIf rs = MessageBoxResult.Cancel Then
                    Exit Sub
                End If
            End If

            AktuellerClub = Services.GroupiesApplication.NeuenClubErstellen()

        End Sub

        Private Sub OnApplicationOpen(obj As Object)
            Throw New NotImplementedException()
        End Sub


#Region "Properties"

        Public Property NeueSortedList As ObservableCollection(Of MenuEintragViewModel)
            Get
                Return _neueSortedList
            End Get
            Set(value As ObservableCollection(Of MenuEintragViewModel))
                _neueSortedList = value
            End Set
        End Property
        Public Property MostRecentlyUsedMenuItem

        Public Property WindowTitleIcon As String = "pack://application:,,,/Images/icons8-ski-resort-48.png"

        Public Property WindowTitleText As String = "Groupies - Ski Club Management - Neues MainWindow"

        Public Property Einteilungsliste As EinteilungCollection


        Public Property AktuellerClub As Generation4.Club

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

    End Class
End Namespace
