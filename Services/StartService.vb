Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Reflection
Imports System.Windows.Shell
Imports System.Xml.Serialization
Imports Microsoft.Win32
Imports CDS = Groupies.Services.CurrentDataService


Namespace Services

    Module StartService

        Public Property mRuSortedList As SortedList(Of Integer, String)
        Public Property SkiclubListFile As FileInfo
        Public Property mostrecentlyUsedMenuItem As MenuItem

        Public Sub OpenFile()
            Dim dlg = New OpenFileDialog With {.Filter = "*.ski|*.ski"}
            If dlg.ShowDialog = True Then
                OpenSkischule(dlg.FileName)
            End If
        End Sub


        Public Sub OpenSkischule(fileName As String)

            If _SkiclubListFile IsNot Nothing AndAlso fileName.Equals(_SkiclubListFile.FullName) Then
                MessageBox.Show("Groupies " & fileName & " ist bereits geöffnet")
                Exit Sub
            End If

            If Not File.Exists(fileName) Then
                MessageBox.Show("Die Datei existiert nicht")
                Exit Sub
            End If

            Dim loadedSkischule = OpenXML(fileName)
            'Dim loadedSkischule = OpenZIP(fileName)
            If loadedSkischule Is Nothing Then Exit Sub


            CDS.Skiclub = Nothing

            _SkiclubListFile = New FileInfo(fileName)
            QueueMostRecentFilename(fileName)

            ' Eintrag in CurrentDataService
            CDS.Skiclub = loadedSkischule
            CDS.SkiclubFileName = fileName

        End Sub


        Private Function OpenXML(fileName As String) As Entities.Skiclub
            Dim serializer = New XmlSerializer(GetType(Entities.Skiclub))
            Dim loadedSkiclub As Entities.Skiclub = Nothing

            ' Datei deserialisieren
            Using fs = New FileStream(fileName, FileMode.Open)
                Try
                    loadedSkiclub = TryCast(serializer.Deserialize(fs), Entities.Skiclub)
                Catch ex As InvalidDataException
                    MessageBox.Show("Datei ungültig: " & ex.Message)
                    Return Nothing
                End Try
            End Using
            Return loadedSkiclub
        End Function

        Public Sub LoadLastSkischule()
            ' Die letze Skischule aus dem IsolatedStorage holen.
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

        Public Sub LoadmRUSortedListMenu()
            _mRuSortedList = New SortedList(Of Integer, String)
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                    Using stream = New IsolatedStorageFileStream("mRUSortedList", System.IO.FileMode.Open, iso)
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
                'Throw ex
            End Try
        End Sub

        Public Sub QueueMostRecentFilename(fileName As String)

            Dim max As Integer = 0
            For Each i In _mRuSortedList.Keys
                If i > max Then max = i
            Next

            Dim keysToRemove = New List(Of Integer)
            For Each kvp As KeyValuePair(Of Integer, String) In _mRuSortedList
                If kvp.Value.Equals(fileName) Then keysToRemove.Add(kvp.Key)
            Next

            For Each i As Integer In keysToRemove
                _mRuSortedList.Remove(i)
            Next

            _mRuSortedList.Add(max + 1, fileName)

            If _mRuSortedList.Count > 5 Then
                Dim min As Integer = Integer.MaxValue
                For Each i As Integer In _mRuSortedList.Keys
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

        Public Sub RefreshMenuInApplication()
            For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
                Dim mi As MenuItem = New MenuItem()
                mi.Header = _mRuSortedList.Values(i)
                AddHandler mi.Click, AddressOf HandleMostRecentClick
                mostrecentlyUsedMenuItem.Items.Add(mi)
            Next

            If mostrecentlyUsedMenuItem.Items.Count = 0 Then
                Dim mi = New MenuItem With {.Header = "keine"}
                mostrecentlyUsedMenuItem.Items.Add(mi)
            End If
        End Sub

        Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
            Throw New NotImplementedException()
        End Sub

        Public Sub RefreshJumpListInWinTaskbar()

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

    End Module

End Namespace

