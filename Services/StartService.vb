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

        Public Property SkiclubListFile As FileInfo

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
            'QueueMostRecentFilename(fileName)

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




        'Private Sub RefreshMostRecentMenu()

        '    mostrecentlyUsedMenuItem.Items.Clear()

        '    RefreshMenuInApplication()
        '    RefreshJumpListInWinTaskbar()
        'End Sub



        '    Private Sub HandleMostRecentClick(sender As Object, e As RoutedEventArgs)
        '        Throw New NotImplementedException()
        '    End Sub

        '    Public Sub RefreshJumpListInWinTaskbar()

        '        Dim jumplist = New JumpList With {
        '            .ShowFrequentCategory = False,
        '            .ShowRecentCategory = False}

        '        Dim jumptask = New JumpTask With {
        '            .CustomCategory = "Release Notes",
        '            .Title = "SkikursReleaseNotes",
        '            .Description = "Zeigt die ReleaseNotes zu Skikurse an",
        '            .ApplicationPath = "C:\Windows\notepad.exe",
        '            .IconResourcePath = "C:\Windows\notepad.exe",
        '            .WorkingDirectory = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
        '            .Arguments = "SkikursReleaseNotes.txt"}

        '        jumplist.JumpItems.Add(jumptask)

        '        ' Hinweis Die JumpPath - Elemente sind nur sichtbar, wenn die ".ski"-Dateiendung
        '        ' unter Windows mit Skikurs assoziiert wird (kann durch Installation via Setup-Projekt erreicht werden,
        '        ' das auch in den Beispielen enthalten ist, welches die dafür benötigten Werte in die Registry schreibt)

        '        For i = _mRuSortedList.Values.Count - 1 To 0 Step -1
        '            Dim jumpPath = New JumpPath With {
        '                .CustomCategory = "Zuletzt geöffnet",
        '                .Path = _mRuSortedList.Values(i)}

        '            jumplist.JumpItems.Add(jumpPath)
        '        Next

        '        JumpList.SetJumpList(Application.Current, jumplist)

        '    End Sub

    End Module

End Namespace

