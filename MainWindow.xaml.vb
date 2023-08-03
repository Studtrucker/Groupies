Imports Microsoft.Win32
Imports Skireisen.Entities
Imports System.Reflection
Imports System.ComponentModel
Imports System.IO
Imports System.IO.Compression
Imports System.Windows.Shell
Imports System.Xml.Serialization

Class MainWindow

#Region "Fields"

    Private _teilnehmerList As TeilnehmerCollection
    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow
    Private _skireiseListFile As FileInfo
    Private _mRUSortedList As SortedList(Of Integer, String)

#End Region

#Region "Events"
    Private Sub HandleMainWindowLoaded(sender As Object, e As RoutedEventArgs)

        ' 0. Zur InputBindings ein MouseBinding hinzufügen. Nur als Beispiel,
        '    um mit Strg und Doppelklick eine neue Liste anlegen zu können
        'Dim mg = New MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control)
        'Dim m = New MouseBinding(ApplicationCommands.[New], mg)
        'InputBindings.Add(m)


        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden
        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleListNewExecuted))
        CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleListOpenExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleListSaveExecuted, AddressOf HandleListSaveCanExecute))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleListSaveAsExecuted, AddressOf HandleListSaveCanExecute))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Help, AddressOf HandleHelpExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))

        CommandBindings.Add(New CommandBinding(SkireisenBefehle.ImportTeilnehmerliste, AddressOf HandleImportTeilnehmerExecuted, AddressOf HandleImportTeilnehmerCanExecute))

    End Sub

#End Region

#Region "EventHandler der CommandBindings"
    Private Sub HandleListNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        If _teilnehmerList IsNot Nothing Then
            Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie die aktuelle Liste noch speichern?", "", MessageBoxButton.YesNoCancel)
            If rs = MessageBoxResult.Yes Then
                ApplicationCommands.Save.Execute(Nothing, Me)
            ElseIf rs = MessageBoxResult.Cancel Then
                Exit Sub
            End If
        End If

        _teilnehmerList = Nothing

        Title = "Skireisen"
        SetView(New TeilnehmerCollection)
        If MessageBoxResult.Yes = MessageBox.Show("Neue Reise erstellt. Jetzt gleich die Bearbeitung beginnen?", "Achtung", MessageBoxButton.YesNo) Then
            ' Todo: wie soll die Bearbeitung der neuen Skireise beginnen?
            'NewFriend.Execute(Nothing, Me)
        End If

    End Sub

    Private Sub HandleListOpenExecuted(sender As Object, e As ExecutedRoutedEventArgs)
        Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
        If dlg.ShowDialog = True Then
            OpenSkireiseList(dlg.FileName)
        End If
    End Sub

    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        OpenSkireiseList(TryCast(sender, MenuItem).Header.ToString())
    End Sub

    Private Sub HandleImportTeilnehmerExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        DatenImport.ImportTeilnehmerListe()

    End Sub

    Private Sub HandleImportTeilnehmerCanExecute(sender As Object, e As CanExecuteRoutedEventArgs)
        e.CanExecute = _teilnehmerList IsNot Nothing
    End Sub


#End Region

#Region "Helper-Methoden"
    Private Sub OpenSkireiseList(fileName As String)
        If _skireiseListFile IsNot Nothing AndAlso fileName.Equals(_skireiseListFile.FullName) Then
            MessageBox.Show("Die Liste " & fileName & " ist bereits geöffnet")
            Exit Sub
        End If

        If Not File.Exists(fileName) Then
            MessageBox.Show("Die Datei existiert nicht")
            Exit Sub
        End If

        ' Datei enzippen und deserialisieren
        Dim serializer = New XmlSerializer(GetType(TeilnehmerCollection))
        Dim loadedFriendCollection As TeilnehmerCollection = Nothing
        Using fs = New FileStream(fileName, FileMode.Open)
            Using zipStream = New GZipStream(fs, CompressionMode.Decompress)
                Try
                    loadedFriendCollection = TryCast(serializer.Deserialize(zipStream), TeilnehmerCollection)
                Catch ex As InvalidDataException
                    MessageBox.Show("Datei ungültig: " & ex.Message)
                    Exit Sub
                End Try
            End Using
        End Using

        _teilnehmerList = Nothing

        _skireiseListFile = New FileInfo(fileName)
        QueueMostRecentFilename(fileName)
        SetView(loadedFriendCollection)
        Title = "Skireisen - " & fileName

    End Sub

    Private Sub QueueMostRecentFilename(fileName As String)
        Dim max As Integer = 0
        For Each i In _mRUSortedList.Keys
            If i > max Then max = i
        Next

        Dim keysToRemove = New List(Of Integer)
        For Each kvp As KeyValuePair(Of Integer, String) In _mRUSortedList
            If kvp.Value.Equals(fileName) Then keysToRemove.Add(kvp.Key)
        Next

        For Each i As Integer In keysToRemove
            _mRUSortedList.Remove(i)
        Next

        _mRUSortedList.Add(max + 1, fileName)

        If _mRUSortedList.Count > 5 Then
            Dim min As Integer = Integer.MaxValue
            For Each i As Integer In _mRUSortedList.Keys
                If i < min Then min = i
            Next
            _mRUSortedList.Remove(min)
        End If

        RefreshMostRecentMenu()

    End Sub

    Private Sub RefreshMostRecentMenu()
        mostrecentlyUsedMenuItem.Items.Clear()
        RefreshMenuInApplication()
        RefreshJumpListInWinTaskbar()
    End Sub

    Private Sub RefreshMenuInApplication()
        For i = _mRUSortedList.Values.Count - 1 To 0 Step -1
            Dim mi = New MenuItem With {.Header = _mRUSortedList.Values(i)}
            AddHandler mi.Click, AddressOf HandleMostRecentClick
            mostrecentlyUsedMenuItem.Items.Add(mi)
        Next

        If mostrecentlyUsedMenuItem.Items.Count = 0 Then
            Dim mi = New MenuItem With {.Header = "keine"}
            mostrecentlyUsedMenuItem.Items.Add(mi)
        End If
    End Sub
    Private Sub RefreshJumpListInWinTaskbar()

        Dim jumplist = New JumpList With {
            .ShowFrequentCategory = False,
            .ShowRecentCategory = False}

        Dim jumptask = New JumpTask With {
            .CustomCategory = "Release Notes",
            .Title = "FriendStorageReleaseNotes",
            .Description = "Zeigt die ReleaseNotes zu FriendStorage an",
            .ApplicationPath = "C:\Windows\notepad.exe",
            .IconResourcePath = "C:\Windows\notepad.exe",
            .WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
            .Arguments = "ReleaseNotes.txt"}

        jumplist.JumpItems.Add(jumptask)

        ' Hinweis Die JumpPath - Elemente sind nur sichtbar, wenn die ".friends"-Dateiendung
        ' unter Windows mit FriendStorage assoziiert wird (kann durch Installation via Setup-Projekt erreicht werden,
        ' das auch in den Beispielen enthalten ist, welches die dafür benötigten Werte in die Registry schreibt)

        For i = _mRUSortedList.Values.Count - 1 To 0 Step -1
            Dim jumpPath = New JumpPath With {
                .CustomCategory = "Zuletzt geöffnet",
                .Path = _mRUSortedList.Values(i)}

            jumplist.JumpItems.Add(jumpPath)
        Next

        JumpList.SetJumpList(Application.Current, jumplist)
    End Sub

#End Region

#Region "weitere Eventhandler"

    Sub _teilnehmerListCollectionView_CurrentChanged(sender As Object, e As EventArgs)
        RefreshTaskBarItemOverlay()
    End Sub

    Private Sub RefreshTaskBarItemOverlay()
        Dim currentTeilnehmer = DirectCast(_teilnehmerListCollectionView.CurrentItem, Teilnehmer)

        'Todo: Aufbereiten für die Skilehrer Bilder

        'If currentTeilnehmer IsNot Nothing AndAlso currentTeilnehmer.Image IsNot Nothing Then
        '    Dim bi As New BitmapImage
        '    bi.BeginInit()
        '    bi.StreamSource = New MemoryStream(currentFriend.Image)
        '    bi.EndInit()
        '    TaskbarItemInfo.Overlay = bi
        'Else
        '    TaskbarItemInfo.Overlay = Nothing
        'End If
    End Sub

#End Region

    Private Sub SetView(TeilnehmerListe As TeilnehmerCollection)
        _teilnehmerList = TeilnehmerListe
        _teilnehmerListCollectionView = New ListCollectionView(TeilnehmerListe)
        ' Hinweis AddHandler Seite 764
        AddHandler _teilnehmerListCollectionView.CurrentChanged, AddressOf _teilnehmerListCollectionView_CurrentChanged
        ' DataContext wird gesetzt Inhalt = CollectionView, diese kennt sein CurrentItem
        DataContext = _teilnehmerListCollectionView
    End Sub

End Class
