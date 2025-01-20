Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Markup
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports Groupies.Commands
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.Interfaces
Imports Groupies.Services
Imports Groupies.UserControls
Imports Microsoft.Win32
Imports AppCon = Groupies.Controller.AppController

Public Class MainWindow

#Region "Felder"

    Private _LeistungsstufenListCollectionView As ICollectionView
    Private _gruppenloseTeilnehmerCollectionView As ICollectionView
    Private _gruppenloseTrainerCollectionView As ICollectionView
    Private _gruppenlisteCollectionView As ICollectionView
    Private _groupiesFile As FileInfo
    Private _mRuSortedList As SortedList(Of Integer, String)

#End Region

#Region "Konstruktor"

    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        ' DataContext Window
        _gruppenlisteCollectionView = New ListCollectionView(New GruppeCollection)
        ' DataContext participantDataGrid
        _gruppenloseTeilnehmerCollectionView = New ListCollectionView(New TeilnehmerCollection)
        ' DataContext GruppenleiterDataGrid
        _gruppenloseTrainerCollectionView = New ListCollectionView(New TrainerCollection)

    End Sub

#End Region

#Region "Window-Events"
    Private Sub HandleWindowLoaded(sender As Object, e As RoutedEventArgs)

        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden

        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf Handle_New_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf Handle_Close_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf Handle_Open_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save,
                                               AddressOf Handle_SaveClub_Execute, AddressOf Handle_SaveClub_CanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs,
                                               AddressOf Handle_ClubSaveAs_Execute, AddressOf Handle_SaveClub_CanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Print,
                                               AddressOf Handle_PrintClub_Execute, AddressOf Handle_PrintClub_CanExecute))

        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerlisteImportieren,
                                               AddressOf Handle_TeilnehmerlisteImportieren_Execute,
                                               AddressOf Handle_TeilnehmerlisteImportieren_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerlisteExportierenXl,
                                               AddressOf Handle_TeilnehmerlisteExportierenXl_Execute,
                                               AddressOf Handle_TeilnehmerlisteExportierenXl_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerInGruppeEinteilen,
                                               AddressOf Handle_TeilnehmerInGruppeEinteilen_Execute,
                                               AddressOf Handle_TeilnehmerInGruppeEinteilen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerNeuErstellen,
                                               AddressOf Handle_TeilnehmerNeuErstellen_Execute,
                                               AddressOf Handle_TeilnehmerNeuErstellen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerAusGruppeEntfernen,
                                               AddressOf Handle_TeilnehmerAusGruppeEntfernen_Execute,
                                               AddressOf Handle_TeilnehmerAusGruppeEntfernen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerSuchen,
                                               AddressOf Handle_TeilnehmerSuchen_Execute,
                                               AddressOf Handle_TeilnehmerSuchen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerArchivieren,
                                               AddressOf Handle_TeilnehmerArchivieren_Execute,
                                               AddressOf Handle_TeilnehmerArchivieren_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerBearbeiten,
                                               AddressOf Handle_TeilnehmerBearbeiten_Execute,
                                               AddressOf Handle_TeilnehmerBearbeiten_CanExecuted))


        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerlisteImportieren,
                                               AddressOf Handle_TrainerlisteImportieren_Execute,
                                               AddressOf Handle_TrainerlisteImportieren_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerlisteExportierenXl,
                                               AddressOf Handle_TrainerlisteExportierenXl_Execute,
                                               AddressOf Handle_TrainerlisteExportierenXl_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerInGruppeEinteilen,
                                               AddressOf Handle_TrainerInGruppeEinteilen_Execute,
                                               AddressOf Handle_TrainerInGruppeEinteilen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerNeuErstellen,
                                               AddressOf Handle_TrainerNeuErstellen_Execute,
                                               AddressOf Handle_TrainerNeuErstellen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerBearbeiten,
                                               AddressOf Handle_TrainerBearbeiten_Execute,
                                               AddressOf Handle_TrainerBearbeiten_CanExecuted))

        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerAusGruppeEntfernen,
                                               AddressOf Handle_TrainerAusGruppeEntfernen_Execute,
                                               AddressOf Handle_TrainerAusGruppeEntfernen_CanExecute))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerArchivieren,
                                               AddressOf Handle_TrainerArchivieren_Execute,
                                               AddressOf Handle_TrainerArchivieren_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppeNeuErstellen,
                                               AddressOf Handle_GruppeNeuErstellen_Execute,
                                               AddressOf Handle_GruppeNeuErstellen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppeLoeschen,
                                               AddressOf Handle_GruppeLoeschen_Execute,
                                               AddressOf Handle_GruppeLoeschen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.GruppeSortieren,
                                               AddressOf Handle_GruppeSortieren_Execute))

        'CommandBindings.Add(New CommandBinding(SkiclubCommands.LeistungsstufeNeuErstellen,
        '                                       AddressOf Handle_LeistungsstufeNeuErstellen_Execute,
        '                                       AddressOf Handle_LeistungsstufeNeuErstellen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.LeistungsstufeNeuErstellen,
                                               AddressOf Handle_LeistungsstufeNeuErstellen_Execute,
                                               AddressOf Handle_LeistungsstufeNeuErstellen_CanExecuted))


        ' 2. SortedList für meist genutzte Skischulen (Most Recently Used) initialisieren
        _mRuSortedList = New SortedList(Of Integer, String)

        ' 3. SortedList für meist genutzte Skischulen befüllen
        LoadmRUSortedListMenu()

        ' 4. Die zuletzt verwendete Skischulen laden, falls nicht eine .ski-Datei doppelgeklickt wurde
        If (Environment.GetCommandLineArgs().Length = 2) Then
            Dim args = Environment.GetCommandLineArgs
            Dim filename = args(1)
            OpenSkischule(filename)
        Else
            LoadLastSkischule()
        End If


        RefreshJumpListInWinTaskbar()

        If AppController.CurrentClub IsNot Nothing Then
            _LeistungsstufenListCollectionView = New CollectionView(AppController.CurrentClub.LeistungsstufenTextliste)
            GruppeUserControl.GruppenleistungsstufeComboBox.ItemsSource = _LeistungsstufenListCollectionView
            GruppeUserControl.TeilnehmerLeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView
            TeilnehmerLeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView
        End If

    End Sub


    Private Sub HandleMainWindowClosing(sender As Object, e As CancelEventArgs)
        Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schließen?", "Achtung", MessageBoxButton.YesNo)
        e.Cancel = result = MessageBoxResult.No
    End Sub

    Private Sub HandleMainWindowClosed(sender As Object, e As EventArgs)

        ' 1. Den Pfad der letzten Liste ins IsolatedStorage speichern.
        If _groupiesFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(_groupiesFile.FullName)
                    End Using
                End Using
            End Using
        End If

        ' 2. Die meist genutzten Listen ins Isolated Storage speichern
        If _mRuSortedList.Count > 0 Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("_mRUSortedList", FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        For Each kvp As KeyValuePair(Of Integer, String) In _mRuSortedList
                            writer.WriteLine(kvp.Key.ToString() & ";" & kvp.Value)
                        Next
                    End Using
                End Using
            End Using
        End If

    End Sub

#End Region

#Region "Methoden zum Laden der meist genutzten Groupies und der letzten Groupies Datei"
    Public Sub LoadmRUSortedListMenu()
        Try
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                Using stream = New IsolatedStorageFileStream("LastGroupies", System.IO.FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        Dim i = 0
                        While reader.Peek <> -1
                            Dim line = reader.ReadLine().Split(";")
                            If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not _mRuSortedList.ContainsKey(Integer.Parse(line(0))) Then
                                If File.Exists(line(1)) Then
                                    i += 1
                                    _mRuSortedList.Add(i, line(1))
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

    Private Sub LoadLastSkischule()
        ' Die letzte Skischule aus dem IsolatedStorage holen.
        Try
            Dim x = ""
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        x = reader.ReadLine
                    End Using
                End Using

            End Using
            If File.Exists(x) Then OpenSkischule(x)
        Catch ex As FileNotFoundException
        End Try
    End Sub

#End Region

#Region "Methoden zum Pinnen und Ein-/Ausblenden der Explorer"

#End Region

#Region "EventHandler der CommandBindings"

#Region "Allgemein"

    Private Sub Handle_New_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If AppCon.CurrentClub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Skischule noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        UnsetView()

        AppCon.NeuenClubErstellen("Club")
        SetView(AppCon.CurrentClub)

    End Sub

    Private Sub Handle_Open_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        If dlg.ShowDialog = True Then
            UnsetView()
            OpenSkischule(dlg.FileName)
        End If
    End Sub

    Private Sub Handle_SaveClub_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        If _groupiesFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveSkischule(_groupiesFile.FullName)
        End If
    End Sub

    Private Sub Handle_SaveClub_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing
    End Sub

    Private Sub Handle_ClubSaveAs_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If _groupiesFile IsNot Nothing Then
            dlg.FileName = _groupiesFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveSkischule(dlg.FileName)
            _groupiesFile = New FileInfo(dlg.FileName)
        End If
    End Sub

    Private Sub Handle_Close_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Close()
    End Sub

    Private Sub Handle_PrintClub_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New PrintDialog()
        If dlg.ShowDialog = True Then
            Dim doc As FixedDocument
            Dim printArea = New Size(dlg.PrintableAreaWidth, dlg.PrintableAreaHeight)
            Dim pageMargin = New Thickness(30, 30, 30, 60)
            If e.Parameter = "InstructorInfo" Then
                doc = PrintoutInfo(Printversion.Instructor, printArea, pageMargin)
            Else
                doc = PrintoutInfo(Printversion.Participant, printArea, pageMargin)
            End If
            dlg.PrintDocument(doc.DocumentPaginator, e.Parameter)
        End If

    End Sub

    Private Sub Handle_PrintClub_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing AndAlso AppCon.CurrentClub.Gruppenliste.Count > 0
    End Sub

#End Region

#Region "Teilnehmer"
    Private Sub Handle_TeilnehmerlisteImportieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ImportService.ImportTeilnehmer()
    End Sub

    Private Sub Handle_TeilnehmerlisteImportieren_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing AndAlso AppCon.CurrentClub.GruppenloseTeilnehmer IsNot Nothing
    End Sub

    Private Sub Handle_TeilnehmerlisteExportierenXl_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing AndAlso AppCon.CurrentClub.AlleTeilnehmer.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerlisteExportierenXl_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ExportService.ExportTeilnehmer()
    End Sub

    Private Sub Handle_TeilnehmerNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub.GruppenloseTeilnehmer IsNot Nothing
    End Sub

    Private Sub Handle_TeilnehmerNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New TeilnehmerDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        If dlg.ShowDialog = True Then
            AppCon.CurrentClub.GruppenloseTeilnehmer.Add(dlg.Teilnehmer)
        End If
    End Sub

    Private Sub Handle_TeilnehmerInGruppeEinteilen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppenloseTeilnehmerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerInGruppeEinteilen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppCon.CurrentClub.TeilnehmerInGruppeEinteilen(GruppenloseTeilnehmerDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
        Next
    End Sub

    Private Sub Handle_TeilnehmerAusGruppeEntfernen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerAusGruppeEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppCon.CurrentClub.TeilnehmerAusGruppeEntfernen(GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Item(i), DirectCast(DataContext, ICollectionView).CurrentItem)
        Next
    End Sub

    Private Sub Handle_TeilnehmerBearbeiten_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count = 1
    End Sub

    Private Sub Handle_TeilnehmerBearbeiten_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New TeilnehmerDialog(GruppeUserControl.MitgliederlisteDataGrid.CurrentItem) With {
            .Owner = Me,
            .Modus = New Fabriken.ModusBearbeiten,
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        dlg.ShowDialog()

    End Sub

    Private Sub Handle_TeilnehmerSuchen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub Handle_TeilnehmerSuchen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppCon.TeilnehmerSuchen()
    End Sub

    Private Sub Handle_TeilnehmerArchivieren_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppenloseTeilnehmerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerArchivieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppCon.CurrentClub.TeilnehmerArchivieren(GruppenloseTeilnehmerDataGrid.SelectedItems.Item(i))
        Next
    End Sub

    Private Sub GruppenloseTeilnehmer_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppCon.CurrentClub.TeilnehmerInGruppeEinteilen(GruppenloseTeilnehmerDataGrid.SelectedItems.Item(i), DirectCast(GruppenlisteDataGrid.DataContext, ICollectionView).CurrentItem)
        Next
    End Sub

#End Region

#Region "Trainer"

    Private Sub Handle_TrainerlisteImportieren_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing AndAlso AppCon.CurrentClub.GruppenloseTrainer IsNot Nothing
    End Sub

    Private Sub Handle_TrainerlisteImportieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ImportService.ImportTrainer()
    End Sub

    Private Sub Handle_TrainerlisteExportierenXl_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub IsNot Nothing AndAlso AppCon.CurrentClub.AlleTrainer.Count > 0
    End Sub

    Private Sub Handle_TrainerlisteExportierenXl_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ExportService.ExportTrainer()
    End Sub

    Private Sub Handle_TrainerNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub.GruppenloseTrainer IsNot Nothing
    End Sub

    Private Sub Handle_TrainerNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New TrainerDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        If dlg.ShowDialog = True Then
            AppCon.CurrentClub.GruppenloseTrainer.Add(dlg.Trainer)
        End If
    End Sub

    Private Sub Handle_TrainerBearbeiten_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        'e.CanExecute = DirectCast(GruppenloseTrainerDataGrid.SelectedItem, Trainer) IsNot Nothing
        e.CanExecute = e.OriginalSource.DataContext IsNot Nothing
    End Sub

    Private Sub Handle_TrainerBearbeiten_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        ' Trainer ermitteln
        Dim Trainer As Trainer
        If e.OriginalSource.GetType.Equals(GetType(DataGridCell)) Then
            If e.Source.Name.ToString.Equals("GruppenloseTrainerDataGrid") Then
                Trainer = DirectCast(GruppenloseTrainerDataGrid.SelectedItem, Trainer)
            Else
                Trainer = DirectCast(DirectCast(GruppenlisteDataGrid.SelectedItem, Gruppe).Trainer, Trainer)
            End If
        ElseIf e.OriginalSource.GetType.Equals(GetType(MenuItem)) Then
            Trainer = DirectCast(DirectCast(GruppenlisteDataGrid.SelectedItem, Gruppe).Trainer, Trainer)
        ElseIf e.OriginalSource.GetType.Equals(GetType(ContextMenu)) Then
            Trainer = DirectCast(GruppenloseTrainerDataGrid.SelectedItem, Trainer)
        Else
            Trainer = Nothing
        End If

        If Trainer IsNot Nothing Then TrainerBearbeiten(Trainer)

    End Sub

    Private Sub TrainerBearbeiten(Trainer As Trainer)

        If Trainer IsNot Nothing Then
            Dim dlg = New TrainerDialog With {
                .Owner = Me,
                .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten),
                .WindowStartupLocation = WindowStartupLocation.CenterOwner}

            dlg.ModusEinstellen()
            dlg.Bearbeiten(Trainer)

            If dlg.ShowDialog = True Then
                Dim unused As Trainer = dlg.Trainer
            End If
        End If
    End Sub

    Private Sub Handle_TrainerInGruppeEinteilen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppCon.CurrentClub.TrainerEinerGruppeZuweisen(GruppenloseTrainerDataGrid.SelectedItems.Item(0), DirectCast(GruppenlisteDataGrid.DataContext, ICollectionView).CurrentItem)
    End Sub

    Private Sub Handle_TrainerInGruppeEinteilen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        Dim HatKeinTrainer = DirectCast(DirectCast(GruppenlisteDataGrid.DataContext, ICollectionView).CurrentItem, Gruppe).Trainer Is Nothing
        e.CanExecute = GruppenloseTrainerDataGrid.SelectedItems.Count > 0 AndAlso HatKeinTrainer
    End Sub
    Private Sub Handle_TrainerAusGruppeEntfernen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = DirectCast(DirectCast(DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing
    End Sub

    Private Sub Handle_TrainerAusGruppeEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppCon.CurrentClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
    End Sub

    Private Sub Handle_TrainerArchivieren_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppenloseTrainerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TrainerArchivieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTrainerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppCon.CurrentClub.TrainerArchivieren(GruppenloseTrainerDataGrid.SelectedItems.Item(i))
        Next
    End Sub

    Private Sub GruppenloseTrainer_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If DirectCast(DirectCast(GruppenlisteDataGrid.DataContext, ICollectionView).CurrentItem, Gruppe).Trainer IsNot Nothing Then
            MessageBox.Show("Es muss zuerst der aktuelle Trainer aus der Gruppe entfernt werden")
            Exit Sub
        End If
        AppCon.CurrentClub.TrainerEinerGruppeZuweisen(GruppenloseTrainerDataGrid.SelectedItems.Item(0), DirectCast(GruppenlisteDataGrid.DataContext, ICollectionView).CurrentItem)
    End Sub

#End Region

#Region "Gruppe"
    Private Sub Handle_GruppeNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppCon.CurrentClub.Gruppenliste IsNot Nothing
    End Sub

    Private Sub Handle_GruppeNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New NeueGruppeDialog With {.Owner = Me, .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        If dlg.ShowDialog = True Then
            AppCon.CurrentClub.Gruppenliste.Add(dlg.Group)
        End If
    End Sub

    Private Sub Handle_GruppeLoeschen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = DirectCast(_gruppenlisteCollectionView.CurrentItem, Gruppe).Trainer Is Nothing AndAlso DirectCast(_gruppenlisteCollectionView.CurrentItem, Gruppe).Mitgliederliste.Count = 0
    End Sub

    Private Sub Handle_GruppeLoeschen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppCon.CurrentClub.Gruppenliste.Remove(_gruppenlisteCollectionView.CurrentItem)
    End Sub

    Private Sub Handle_GruppeSortieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        _gruppenlisteCollectionView.Refresh()
    End Sub

#End Region

#Region "Leistungsstufe"
    Private Sub Handle_LeistungsstufeNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
        ' e.CanExecute = AppCon.CurrentClub.Leistungsstufenliste IsNot Nothing
    End Sub

    Private Sub Handle_LeistungsstufeNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New LeistungsstufeDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        If dlg.ShowDialog = True Then
            Try
                AppCon.CurrentClub.Leistungsstufenliste.Add(dlg.Leistungsstufe)
            Catch ex As Exception
                MessageBox.Show($"{ex.InnerException}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub


#End Region

#End Region

#Region "Sonstige Eventhandler"

    ' Handles für Drag and Drop

    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        OpenSkischule(TryCast(sender, MenuItem).Header.ToString())
    End Sub

    Private Sub ZeigeLeistungsstufenuebersicht(sender As Object, e As RoutedEventArgs)
        Dim Leistungsstufenuebericht As New Leistungsstufenuebersicht
        Leistungsstufenuebericht.Show()
    End Sub

    Private Sub ZeigeTraineruebersicht(sender As Object, e As RoutedEventArgs)
        Dim Traineruebersicht As New Traineruebersicht
        Traineruebersicht.Show()
    End Sub

#End Region

#Region "Helper-Methoden"

    Private Sub OpenSkischule(fileName As String)

        If _groupiesFile IsNot Nothing AndAlso fileName.Equals(_groupiesFile.FullName) Then
            MessageBox.Show("Groupies " & fileName & " ist bereits geöffnet")
            Exit Sub
        End If

        If Not File.Exists(fileName) Then
            MessageBox.Show("Die Datei existiert nicht")
            Exit Sub
        End If

        Dim loadedClub = OpenXML(fileName)
        Dim loadedSkiclub = New Veraltert.Skiclub
        If loadedClub Is Nothing Then
            loadedSkiclub = OpenAltesXML(fileName)
            If loadedSkiclub Is Nothing Then
                Exit Sub
            End If
        End If

        _groupiesFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)

        AppCon.CurrentClub = Nothing

        ' Eintrag in CurrentDataService
        If loadedClub Is Nothing Then
            AppCon.CurrentClub = MapSkiClub2Club(loadedSkiclub)
        Else
            AppCon.CurrentClub = loadedClub
        End If

        SetView(AppCon.CurrentClub)

        Title = "Groupies - " & fileName

    End Sub

    Private Function OpenAltesXML(fileName As String) As Veraltert.Skiclub

        Dim serializer = New XmlSerializer(GetType(Veraltert.Skiclub))
        Dim loadedSkiclub As Veraltert.Skiclub = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                loadedSkiclub = TryCast(serializer.Deserialize(fs), Veraltert.Skiclub)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedSkiclub

    End Function

    Private Function OpenXML(fileName As String) As Club

        Dim serializer = New XmlSerializer(GetType(Club))
        Dim loadedClub As Club = Nothing

        ' Datei deserialisieren
        Using fs = New FileStream(fileName, FileMode.Open)
            Try
                ' Todo: Doppelte Teilnehmer und Skilehrer - siehe Mapping altes Format!
                loadedClub = TryCast(serializer.Deserialize(fs), Club)
            Catch ex As InvalidOperationException
                Return Nothing
            Catch ex As InvalidDataException
                MessageBox.Show("Datei ungültig: " & ex.Message)
                Return Nothing
            End Try
        End Using
        Return loadedClub
    End Function


    Private Sub SaveSkischule(fileName As String)
        ' Ewige Liste schreiben

        'AppCon.CurrentClub.AlleTeilnehmer.ToList.ForEach(Sub(Tn) AppCon.CurrentClub.EwigeTeilnehmerliste.Add(Tn, Now.Date))

        ' 1. Skischule serialisieren und gezippt abspeichern
        SaveXML(fileName)
        'SaveZIP(fileName)
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = "Groupies - " & fileName
        QueueMostRecentFilename(fileName)
        MessageBox.Show("Groupies gespeichert!")
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Entities.Club))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, AppCon.CurrentClub)
        End Using
    End Sub

    Private Sub QueueMostRecentFilename(fileName As String)

        Dim max As Integer = 0
        For Each i In _mRuSortedList.Keys
            If i > max Then max = i
        Next

        Dim keysToRemove As New List(Of Integer)()
        For Each kvp In _mRuSortedList
            If kvp.Value.Equals(fileName) Then keysToRemove.Add(kvp.Key)
        Next
        For Each i In keysToRemove
            _mRuSortedList.Remove(i)
        Next

        _mRuSortedList.Add(max + 1, fileName)

        If _mRuSortedList.Count > 5 Then
            Dim min = Integer.MaxValue
            For Each i In _mRuSortedList.Keys
                If i < min Then min = i
            Next

            _mRuSortedList.Remove(min)
        End If

        RefreshMostRecentMenu()
    End Sub

    Private Sub RefreshMostRecentMenu()
        mostrecentlyUsedMenuItem.Items.Clear()

        RefreshMenuInApplication()
        RefreshJumpListInWinTaskbar()
    End Sub

    Private Sub RefreshMenuInApplication()

        For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
            Dim mi As New MenuItem() With {.Header = _mRuSortedList.Values(i)}
            AddHandler mi.Click, AddressOf HandleMostRecentClick
            mostrecentlyUsedMenuItem.Items.Add(mi)
        Next

        If mostrecentlyUsedMenuItem.Items.Count = 0 Then
            Dim mi = New MenuItem With {.Header = "keine"}
            mostrecentlyUsedMenuItem.Items.Add(mi)
        End If
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

    Private Sub SetGroupView(sender As Object, e As SelectedCellsChangedEventArgs)
        GruppeUserControl.setView(sender, New RoutedEventArgs)
    End Sub

    Private Sub SetView(Club As Club)

        ' Hier wird der DataContext gesetzt!

        UnsetView()
        _LeistungsstufenListCollectionView = New ListCollectionView(Club.LeistungsstufenTextliste.ToList)
        If _LeistungsstufenListCollectionView.CanSort Then
            _LeistungsstufenListCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        End If
        GruppeUserControl.GruppenleistungsstufeComboBox.ItemsSource = _LeistungsstufenListCollectionView
        GruppeUserControl.TeilnehmerLeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView
        TeilnehmerLeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView

        SetView(Club.Gruppenliste)
        SetView(Club.GruppenloseTeilnehmer)
        SetView(Club.GruppenloseTrainer)

    End Sub

    Private Sub SetView(GruppenloseTeilnehmer As TeilnehmerCollection)
        _gruppenloseTeilnehmerCollectionView = New ListCollectionView(GruppenloseTeilnehmer)
        If _gruppenloseTeilnehmerCollectionView.CanSort Then
            _gruppenloseTeilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Leistungsstufe", ListSortDirection.Ascending))
            _gruppenloseTeilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
            _gruppenloseTeilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Vorname", ListSortDirection.Ascending))
        End If
        GruppenloseTeilnehmerDataGrid.DataContext = _gruppenloseTeilnehmerCollectionView
    End Sub

    Private Sub SetView(Gruppenliste As GruppeCollection)
        _gruppenlisteCollectionView = New ListCollectionView(Gruppenliste)
        If _gruppenlisteCollectionView.CanSort Then
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Descending))
        End If
        DataContext = _gruppenlisteCollectionView
    End Sub

    Private Sub SetView(GruppenloseTrainer As TrainerCollection)
        _gruppenloseTrainerCollectionView = New ListCollectionView(GruppenloseTrainer)
        If _gruppenloseTrainerCollectionView.CanSort Then
            _gruppenloseTrainerCollectionView.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
        End If
        GruppenloseTrainerDataGrid.DataContext = _gruppenloseTrainerCollectionView
    End Sub



    Private Sub UnsetView()

        _groupiesFile = Nothing
        Me.Title = "Groupies"

        _gruppenlisteCollectionView = New ListCollectionView(New GruppeCollection)
        _gruppenloseTeilnehmerCollectionView = New ListCollectionView(New TeilnehmerCollection)
        _gruppenloseTrainerCollectionView = New ListCollectionView(New TrainerCollection)
        _LeistungsstufenListCollectionView = New ListCollectionView(New LeistungsstufeCollection)

        DataContext = Nothing
        GruppenloseTeilnehmerDataGrid.DataContext = Nothing
        GruppenloseTrainerDataGrid.DataContext = Nothing


    End Sub

    Private Function PrintoutInfo(Printversion As Printversion, pageSize As Size, pageMargin As Thickness) As FixedDocument

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
        Dim sortedGroupView = New ListCollectionView(AppCon.CurrentClub.Gruppenliste)
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
            If Printversion = Printversion.Participant Then
                pSkikursgruppe = New TeilnehmerAusdruckUserControl
            Else
                pSkikursgruppe = New TrainerausdruckUserControl
            End If

            DirectCast(pSkikursgruppe, UserControl).Height = printFriendHeight
            DirectCast(pSkikursgruppe, UserControl).Width = printFriendWidth

            pSkikursgruppe.InitPropsFromGroup(skikursgruppe)
            Dim currentRow As Integer = (i Mod participantsPerPage) / columnsPerPage
            Dim currentColumn As Integer = i Mod columnsPerPage

            FixedPage.SetTop(pSkikursgruppe, pageMargin.Top + ((DirectCast(pSkikursgruppe, UserControl).Height + vMarginBetweenFriends) * currentRow))
            FixedPage.SetLeft(pSkikursgruppe, pageMargin.Left + ((DirectCast(pSkikursgruppe, UserControl).Width + hMarginBetweenFriends) * currentColumn))
            page.Children.Add(pSkikursgruppe)
        Next

        Return doc

    End Function


#End Region

#Region "Enum für Printversion"

    Enum Printversion
        Instructor
        Participant
    End Enum



#End Region

End Class
