Imports Skireisen.Entities
Imports System.ComponentModel

Class MainWindow

#Region "Fields"
    Private _teilnehmerList As TeilnehmerCollection
    Private _teilnehmerListCollectionView As ICollectionView '... DataContext für das MainWindow


#End Region

#Region "Events"
    Private Sub HandleMainWindowLoaded(sender As Object, e As RoutedEventArgs)

        ' 0. Zur InputBindings ein MouseBinding hinzufügen. Nur als Beispiel,
        '    um mit Strg und Doppelklick eine neue Liste anlegen zu können
        Dim mg = New MouseGesture(MouseAction.LeftDoubleClick, ModifierKeys.Control)
        Dim m = New MouseBinding(ApplicationCommands.[New], mg)
        InputBindings.Add(m)


        ' 1. CommandBindings zur CommandBindings-Property des Window
        '    hinzufügen, um die Commands mit den entsprechenden Eventhandler zu verbinden
        CommandBindings.Add(New CommandBinding(ApplicationCommands.[New], AddressOf HandleListNewExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Open, AddressOf HandleListOpenExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Save, AddressOf HandleListSaveExecuted, AddressOf HandleListSaveCanExecute))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.SaveAs, AddressOf HandleListSaveAsExecuted, AddressOf HandleListSaveCanExecute))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Close, AddressOf HandleCloseExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Help, AddressOf HandleHelpExecuted))
        'CommandBindings.Add(New CommandBinding(ApplicationCommands.Print, AddressOf HandleListPrintExecuted, AddressOf HandleListPrintCanExecute))

        'CommandBindings.Add(New CommandBinding(NewFriend, AddressOf HandleFriendNewExecuted, AddressOf HandleFriendNewCanExecute))

    End Sub

#End Region

#Region "EventHandler der CommandBindings"
    Private Sub HandleListNewExecuted(sender As Object, e As ExecutedRoutedEventArgs)

        MessageBox.Show("Hallo")
        Exit Sub

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
