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
Imports Groupies.Entities.Generation3
Imports Groupies.Interfaces
Imports Groupies.Services
Imports Groupies.UserControls
Imports Microsoft.Win32
Imports Groupies.Controller.AppController

Public Class MainWindow

#Region "Felder"

    Private _LeistungsstufenListCollectionView As ICollectionView
    Private _einteilungslisteCollectionView As ICollectionView
    Private _gruppenloseTeilnehmerCollectionView As ICollectionView
    Private _gruppenloseTrainerCollectionView As ICollectionView
    Private _gruppenlisteCollectionView As ICollectionView
    Private _mRuSortedList As SortedList(Of Integer, String)

#End Region

#Region "Properties"

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
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TeilnehmerLoeschen,
                                               AddressOf Handle_TeilnehmerLoeschen_Execute,
                                               AddressOf Handle_TeilnehmerLoeschen_CanExecuted))


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
        CommandBindings.Add(New CommandBinding(SkiclubCommands.TrainerLoeschen,
                                               AddressOf Handle_TrainerLoeschen_Execute,
                                               AddressOf Handle_TrainerLoeschen_CanExecuted))

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

        CommandBindings.Add(New CommandBinding(SkiclubCommands.EinteilungNeuErstellen,
                                               AddressOf Handle_EinteilungNeuErstellen_Execute,
                                               AddressOf Handle_EinteilungNeuErstellen_CanExecuted))
        CommandBindings.Add(New CommandBinding(SkiclubCommands.EinteilungKopieren,
                                               AddressOf Handle_EinteilungKopieren_Execute,
                                               AddressOf Handle_EinteilungKopieren_CanExecuted))

        CommandBindings.Add(New CommandBinding(SkiclubCommands.LeistungsstufeNeuErstellen,
                                               AddressOf Handle_LeistungsstufeNeuErstellen_Execute,
                                               AddressOf Handle_LeistungsstufeNeuErstellen_CanExecuted))

        '1. CommandBindings, die geprüft sind und funktionieren

        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New],
                                               AddressOf Handle_New_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Close,
                                               AddressOf Handle_Close_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open,
                                               AddressOf Handle_Open_Execute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Save,
                                               AddressOf Handle_SaveClub_Execute,
                                               AddressOf Handle_SaveClub_CanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs,
                                               AddressOf Handle_ClubSaveAs_Execute,
                                               AddressOf Handle_SaveClub_CanExecute))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Print,
                                               AddressOf Handle_PrintClub_Execute,
                                               AddressOf Handle_PrintClub_CanExecute))

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
            LoadLastSkischule()
        End If


        RefreshJumpListInWinTaskbar()

    End Sub


    Private Sub HandleMainWindowClosing(sender As Object, e As CancelEventArgs)
        Dim result = MessageBox.Show("Möchten Sie die Anwendung wirklich schließen?", "Achtung", MessageBoxButton.YesNo)
        e.Cancel = result = MessageBoxResult.No
    End Sub

    Private Sub HandleMainWindowClosed(sender As Object, e As EventArgs)

        ' 1. Den Pfad der letzten Liste ins IsolatedStorage speichern.
        If AppController.GroupiesFile IsNot Nothing Then
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.OpenOrCreate, iso)
                    Using writer = New StreamWriter(stream)
                        writer.WriteLine(AppController.GroupiesFile.FullName)
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
#End Region

#Region "Methoden zum Pinnen und Ein-/Ausblenden der Explorer"

#End Region

#Region "EventHandler der CommandBindings"

#Region "Allgemein"

    Private Sub Handle_New_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        ' Ist aktuell eine Skischuldatei geöffnet?
        If AppController.AktuellerClub IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie den aktuellen Club noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        UnsetView()
        AppController.NeuenClubErstellen()
        SetView()

    End Sub

    Private Sub Handle_Open_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        If SkiDateienService.OpenSkiDatei() Then
            'UnsetView()
            SetView()
        End If
    End Sub

    Private Sub Handle_SaveClub_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        If AppController.GroupiesFile Is Nothing Then
            ApplicationCommands.SaveAs.Execute(Nothing, Me)
        Else
            SaveGroupies(AppController.GroupiesFile)
        End If
    End Sub

    Private Sub Handle_SaveClub_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub IsNot Nothing
    End Sub

    Private Sub Handle_ClubSaveAs_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New SaveFileDialog With {.Filter = "*.ski|*.ski"}
        If AppController.GroupiesFile IsNot Nothing Then
            dlg.FileName = AppController.GroupiesFile.Name
        End If

        If dlg.ShowDialog = True Then
            SaveGroupies(dlg.FileName)
            AppController.GroupiesFile = New FileInfo(dlg.FileName)
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
        e.CanExecute = AppController.AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.Count > 0
    End Sub

#End Region

#Region "Teilnehmer"
    Private Sub Handle_TeilnehmerlisteImportieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ImportService.ImportTeilnehmer()
    End Sub

    Private Sub Handle_TeilnehmerlisteImportieren_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub IsNot Nothing AndAlso AppController.AktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer IsNot Nothing
    End Sub

    Private Sub Handle_TeilnehmerlisteExportierenXl_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub IsNot Nothing AndAlso AppController.AktuellerClub.SelectedEinteilung.AlleTeilnehmer.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerlisteExportierenXl_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ExportService.ExportTeilnehmer()
    End Sub

    Private Sub Handle_TeilnehmerNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso AktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer IsNot Nothing
    End Sub

    Private Sub Handle_TeilnehmerNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New TeilnehmerDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        If dlg.ShowDialog = True Then
            AppController.AktuellerClub.SelectedEinteilung.GruppenloseTeilnehmer.Add(dlg.Teilnehmer)
        End If
    End Sub

    Private Sub Handle_TeilnehmerInGruppeEinteilen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppenloseTeilnehmerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerInGruppeEinteilen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppController.AktuellerClub.SelectedEinteilung.TeilnehmerInGruppeEinteilen(GruppenloseTeilnehmerDataGrid.SelectedItems.Item(i), AppController.AktuellerClub.SelectedGruppe)
        Next
    End Sub

    Private Sub Handle_TeilnehmerAusGruppeEntfernen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerAusGruppeEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count - 1 To 0 Step -1
            DirectCast(DataContext, Generation3.Club).SelectedEinteilung.TeilnehmerAusGruppeEntfernen(GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Item(i), DirectCast(DataContext, Club).SelectedGruppe)
        Next
    End Sub
    Private Sub Handle_TeilnehmerLoeschen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count > 1
        e.CanExecute = AppController.AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso GruppenloseTrainerDataGrid.SelectedItems.Count > 0

    End Sub
    Private Sub Handle_TeilnehmerLoeschen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppController.AktuellerClub.SelectedEinteilung.TeilnehmerLoeschen(GruppenloseTeilnehmerDataGrid.SelectedItems(i))
        Next
    End Sub

    Private Sub Handle_TeilnehmerBearbeiten_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppeUserControl.MitgliederlisteDataGrid.SelectedItems.Count = 1
    End Sub

    Private Sub Handle_TeilnehmerBearbeiten_Execute(sender As Object, e As ExecutedRoutedEventArgs)

        Dim dlg = New TeilnehmerDialog() With {
            .Owner = Me,
            .Modus = New Fabriken.ModusBearbeiten,
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()


        ' Teilnehmer ermitteln
        Dim Teilnehmer As Teilnehmer
        If e.OriginalSource.GetType.Equals(GetType(DataGridCell)) Then
            If e.Source.Name.ToString.Equals("GruppenloseTeilnehmerDataGrid") Then
                Teilnehmer = DirectCast(GruppenloseTeilnehmerDataGrid.SelectedItem, Teilnehmer)
            Else
                Teilnehmer = DirectCast(GruppeUserControl.MitgliederlisteDataGrid.SelectedItem, Teilnehmer)
            End If
        ElseIf e.OriginalSource.GetType.Equals(GetType(MenuItem)) Then
            Teilnehmer = DirectCast(GruppeUserControl.MitgliederlisteDataGrid.SelectedItem, Teilnehmer)
        ElseIf e.OriginalSource.GetType.Equals(GetType(ContextMenu)) Then
            Teilnehmer = DirectCast(GruppenloseTrainerDataGrid.SelectedItem, Teilnehmer)
        Else
            Teilnehmer = Nothing
        End If

        If Teilnehmer IsNot Nothing Then dlg.Bearbeiten(Teilnehmer)

        dlg.ShowDialog()

    End Sub

    Private Sub Handle_TeilnehmerSuchen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = True
    End Sub

    Private Sub Handle_TeilnehmerSuchen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppController.TeilnehmerSuchen()
    End Sub

    Private Sub Handle_TeilnehmerArchivieren_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppenloseTeilnehmerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TeilnehmerArchivieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppController.AktuellerClub.TeilnehmerArchivieren(GruppenloseTeilnehmerDataGrid.SelectedItems.Item(i))
        Next
    End Sub

    Private Sub GruppenloseTeilnehmer_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        For i = GruppenloseTeilnehmerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            DirectCast(DataContext, Generation3.Club).SelectedEinteilung.TeilnehmerInGruppeEinteilen(GruppenloseTeilnehmerDataGrid.SelectedItems.Item(i), DirectCast(DataContext, Club).SelectedGruppe)
        Next
    End Sub

#End Region

#Region "Trainer"

    Private Sub Handle_TrainerlisteImportieren_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub IsNot Nothing AndAlso AppController.AktuellerClub.SelectedEinteilung.GruppenloseTrainer IsNot Nothing
    End Sub

    Private Sub Handle_TrainerlisteImportieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ImportService.ImportTrainer()
    End Sub

    Private Sub Handle_TrainerlisteExportierenXl_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub IsNot Nothing AndAlso AppController.AktuellerClub.SelectedEinteilung.AlleTrainer.Count > 0
    End Sub

    Private Sub Handle_TrainerlisteExportierenXl_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        ExportService.ExportTrainer()
    End Sub

    Private Sub Handle_TrainerNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso AktuellerClub.SelectedEinteilung.GruppenloseTrainer IsNot Nothing
    End Sub

    Private Sub Handle_TrainerNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        'Dim dlg = New TrainerDialog With {
        '    .Owner = Me,
        '    .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
        '    .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        'dlg.ModusEinstellen()

        'If dlg.ShowDialog = True Then
        '    AppController.AktuellerClub.SelectedEinteilung.GruppenloseTrainer.Add(dlg.Trainer)
        'End If

        Dim dialog = New DialogBasis(Of Trainer) With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}
    End Sub

    Private Sub Handle_TrainerLoeschen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub.SelectedEinteilung IsNot Nothing AndAlso GruppenloseTrainerDataGrid.SelectedItems.Count > 0
    End Sub
    Private Sub Handle_TrainerLoeschen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTrainerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppController.AktuellerClub.SelectedEinteilung.TrainerLoeschen(GruppenloseTrainerDataGrid.SelectedItems(i))
        Next
    End Sub

    Private Sub Handle_TrainerBearbeiten_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
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
        AppController.AktuellerClub.SelectedEinteilung.TrainerEinerGruppeZuweisen(GruppenloseTrainerDataGrid.SelectedItems.Item(0), AppController.AktuellerClub.SelectedGruppe)
    End Sub

    Private Sub Handle_TrainerInGruppeEinteilen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        Dim HatKeinTrainer = AppController.AktuellerClub.SelectedGruppe IsNot Nothing AndAlso AppController.AktuellerClub.SelectedGruppe.Trainer Is Nothing
        e.CanExecute = GruppenloseTrainerDataGrid.SelectedItems.Count > 0 AndAlso HatKeinTrainer
    End Sub
    Private Sub Handle_TrainerAusGruppeEntfernen_CanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub.SelectedGruppe IsNot Nothing AndAlso AppController.AktuellerClub.SelectedGruppe.Trainer IsNot Nothing
    End Sub

    Private Sub Handle_TrainerAusGruppeEntfernen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppController.AktuellerClub.TrainerAusGruppeEntfernen(DirectCast(DataContext, ICollectionView).CurrentItem)
    End Sub

    Private Sub Handle_TrainerArchivieren_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = GruppenloseTrainerDataGrid.SelectedItems.Count > 0
    End Sub

    Private Sub Handle_TrainerArchivieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        For i = GruppenloseTrainerDataGrid.SelectedItems.Count - 1 To 0 Step -1
            AppController.AktuellerClub.TrainerArchivieren(GruppenloseTrainerDataGrid.SelectedItems.Item(i))
        Next
    End Sub

    Private Sub GruppenloseTrainer_MouseDoubleClick(sender As Object, e As MouseButtonEventArgs)
        If DirectCast(DataContext, Generation3.Club).SelectedGruppe Is Nothing Then
            MessageBox.Show("Es muss erst eine Gruppe ausgewählt werden", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End If
        If DirectCast(DataContext, Generation3.Club).SelectedGruppe.Trainer IsNot Nothing Then
            MessageBox.Show("Es muss zuerst der aktuelle Trainer aus der Gruppe entfernt werden", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
            Exit Sub
        End If
        DirectCast(DataContext, Generation3.Club).SelectedEinteilung.TrainerEinerGruppeZuweisen(DirectCast(DataContext, Generation3.Club).SelectedEinteilung.SelectedGruppenloserTrainer, DirectCast(DataContext, Generation3.Club).SelectedGruppe)
    End Sub

#End Region

#Region "Gruppe"
    Private Sub Handle_GruppeNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub.SelectedEinteilung.Gruppenliste IsNot Nothing
    End Sub

    Private Sub Handle_GruppeNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New GruppeDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        If dlg.ShowDialog = True Then
            AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.Add(dlg.Gruppe)
        End If
    End Sub

    Private Sub Handle_GruppeLoeschen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _gruppenlisteCollectionView.CurrentItem IsNot Nothing AndAlso DirectCast(_gruppenlisteCollectionView.CurrentItem, Gruppe).Trainer Is Nothing AndAlso DirectCast(_gruppenlisteCollectionView.CurrentItem, Gruppe).Mitgliederliste.Count = 0
    End Sub

    Private Sub Handle_GruppeLoeschen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.Remove(_gruppenlisteCollectionView.CurrentItem)
    End Sub

    Private Sub Handle_GruppeSortieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        If _gruppenlisteCollectionView.SortDescriptions.Count = 0 AndAlso _gruppenlisteCollectionView.CanSort Then
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Descending))
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Leistungsstufe.Sortierung", ListSortDirection.Descending))
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Benennung", ListSortDirection.Ascending))
        End If
        _gruppenlisteCollectionView.Refresh()
    End Sub

#End Region

#Region "Einteilung"
    Private Sub Handle_EinteilungNeuErstellen_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub.SelectedEinteilung IsNot Nothing
    End Sub

    Private Sub Handle_EinteilungNeuErstellen_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New EinteilungDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()

        If dlg.ShowDialog = True Then
            Try
                AppController.AktuellerClub.Einteilungsliste.Add(dlg.Einteilung)
                dlg.KopiereAktuelleGruppen(AppController.AktuellerClub.SelectedEinteilung.Gruppenliste)
            Catch ex As Exception
                MessageBox.Show($"{ex.InnerException}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
    End Sub

    Private Sub Handle_EinteilungKopieren_CanExecuted(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = AppController.AktuellerClub.SelectedEinteilung IsNot Nothing
    End Sub

    Private Sub Handle_EinteilungKopieren_Execute(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New EinteilungDialog With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dlg.ModusEinstellen()
        dlg.KopiereAktuelleGruppen(AppController.AktuellerClub.SelectedEinteilung.Gruppenliste)

        If dlg.ShowDialog = True Then
            Try
                'AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.ToList.ForEach(Sub() )
                'AppController.AktuellerClub.SelectedEinteilung.Gruppenliste.Co(neueGruppen)
                AppController.AktuellerClub.Einteilungsliste.Add(dlg.Einteilung)

            Catch ex As Exception
                MessageBox.Show($"{ex.InnerException}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error)
            End Try
        End If
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
                AppController.AktuellerClub.Leistungsstufenliste.Add(dlg.Leistungsstufe)
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
        OpenGroupies(TryCast(sender, MenuItem).Header.ToString())
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


    Private Sub LoadLastSkischule()
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

    Private Sub OpenGroupies(fileName As String)
        If SkiDateienService.OpenSkiDatei(fileName) Then
            SetView()
        End If
    End Sub


    Private Sub SaveGroupies(fileName As String)
        SaveGroupies(New FileInfo(fileName))
    End Sub

    Private Sub SaveGroupies(fileInfo As FileInfo)
        ' 1. Skischule serialisieren und gezippt abspeichern
        SaveXML(fileInfo.FullName)
        ' 2. Titel setzen und Datei zum MostRecently-Menü hinzufügen
        Title = $"Groupies - {AppController.AktuellerClub.ClubName} - {fileInfo.Name}"
        QueueMostRecentFilename(fileInfo.FullName)
        MessageBox.Show($"Die Datei {fileInfo.Name} wurde gespeichert!", AppController.AktuellerClub.ClubName, MessageBoxButton.OK, MessageBoxImage.Information)
    End Sub

    Private Sub SaveXML(fileName As String)
        Dim serializer = New XmlSerializer(GetType(Club))
        Using fs = New FileStream(fileName, FileMode.Create)
            serializer.Serialize(fs, AppController.AktuellerClub)
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

    'Private Sub SetGroupView(sender As Object, e As SelectedCellsChangedEventArgs)
    '    GruppeUserControl.setView(sender, New RoutedEventArgs)
    'End Sub

    Private Sub SetView()

        QueueMostRecentFilename(AppController.GroupiesFile.FullName)
        Title = "Groupies - " & AppController.AktuellerClub.ClubName & " - " & AppController.GroupiesFile.Name

        ' Die allgemeinen Leistungsstufen füllen
        _LeistungsstufenListCollectionView = New CollectionView(AppController.AktuellerClub.LeistungsstufenTextliste)
        GruppeUserControl.GruppenleistungsstufeComboBox.ItemsSource = _LeistungsstufenListCollectionView
        GruppeUserControl.TeilnehmerLeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView
        TeilnehmerLeistungsstandComboBox.ItemsSource = _LeistungsstufenListCollectionView

        ' Hier wird der DataContext gesetzt!
        'DataContext = AppController.AktuellerClub
        SetView(AppController.AktuellerClub)
        'SetView(AppController.AktuellerClub.SelectedEinteilung.Gruppenliste)
        'SetView(AppController.AktuellerClub.Einteilungsliste(0).GruppenloseTeilnehmer)
        'SetView(AppController.AktuellerClub.Einteilungsliste(0).GruppenloseTrainer)

    End Sub



    Private Sub SetView(Club As Generation3.Club)
        DataContext = Club
    End Sub
    Private Sub SetView(Einteilungsliste As EinteilungCollection)
        _einteilungslisteCollectionView = New ListCollectionView(Einteilungsliste)
        If _einteilungslisteCollectionView.CanSort Then
            '_gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
            '_gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Leistungsstufe.Sortierung", ListSortDirection.Ascending))
            '_gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Benennung", ListSortDirection.Ascending))
        End If
        _einteilungslisteCollectionView.MoveCurrentToFirst()
        DataContext = _einteilungslisteCollectionView
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
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Leistungsstufe.Sortierung", ListSortDirection.Ascending))
            _gruppenlisteCollectionView.SortDescriptions.Add(New SortDescription("Benennung", ListSortDirection.Ascending))
        End If
        GruppenlisteDataGrid.DataContext = _gruppenlisteCollectionView
    End Sub

    Private Sub SetView(GruppenloseTrainer As TrainerCollection)
        _gruppenloseTrainerCollectionView = New ListCollectionView(GruppenloseTrainer)
        If _gruppenloseTrainerCollectionView.CanSort Then
            _gruppenloseTrainerCollectionView.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
        End If
        GruppenloseTrainerDataGrid.DataContext = _gruppenloseTrainerCollectionView
    End Sub



    Private Sub UnsetView()

        'AppController.GroupiesFile = Nothing
        'Me.Title = "Groupies"

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
        Dim sortedGroupView = New ListCollectionView(AppController.AktuellerClub.SelectedEinteilung.Gruppenliste)
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

            If String.IsNullOrWhiteSpace(AppController.AktuellerClub.SelectedEinteilung.Benennung) Then
                AppController.AktuellerClub.SelectedEinteilung.Benennung = InputBox("Bitte diese Einteilung benennen")
            End If

            pSkikursgruppe.InitPropsFromGroup(skikursgruppe, AppController.AktuellerClub.SelectedEinteilung.Benennung)
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

    Private Sub MenuItem_Click(sender As Object, e As RoutedEventArgs)
        Dim fml = New Testfenster
        fml.Show()
    End Sub

    Private Sub HandleAboutButtonExecuted(sender As Object, e As RoutedEventArgs)
        Dim dialog = New DialogBasis(Of Trainer) With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Erstellen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dialog.ModusEinstellen()
        dialog.ShowDialog()
    End Sub

    Private Sub HandleHelpButtonExecuted(sender As Object, e As RoutedEventArgs)
        Dim dialog = New DialogBasis(Of Trainer) With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Bearbeiten),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dialog.ModusEinstellen()
        dialog.ShowDialog()
    End Sub

    Private Sub HandleFirstHelpButtonExecuted(sender As Object, e As RoutedEventArgs)
        Dim dialog = New DialogBasis(Of Trainer) With {
            .Owner = Me,
            .Modus = New Fabriken.ModusFabrik().ErzeugeModus(Enums.ModusEnum.Ansehen),
            .WindowStartupLocation = WindowStartupLocation.CenterOwner}

        dialog.ModusEinstellen()
        dialog.ShowDialog()
    End Sub

#End Region

End Class
