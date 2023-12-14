Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization

Public Class Window1

    Private _levelListCollectionView As ICollectionView
    Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        'Groupies.Services.CurrentDataService.Skiclub = New Entities.Skiclub
        'Groupies.Services.CurrentDataService.Skiclub.Levellist = Groupies.Services.CreateLevels()

        LoadLastSkischule()

        _levelListCollectionView = New ListCollectionView(Groupies.Services.CurrentDataService.Skiclub.Levellist)
        DataContext = _levelListCollectionView

    End Sub

    Private Sub LoadLastSkischule()
        ' Die letze Skischule aus dem IsolatedStorage holen.
        Try
            Dim x = ""
            Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                Using stream = New IsolatedStorageFileStream("LastSkischule", FileMode.Open, iso)
                    Using reader = New StreamReader(stream)
                        x = reader.ReadLine
                    End Using
                End Using

            End Using
            If File.Exists(x) Then Me.OpenSkischule(x)
        Catch ex As FileNotFoundException
        End Try
    End Sub
    Private Sub OpenSkischule(fileName As String)
        Dim _skischuleListFile = New FileInfo(fileName)

        'If _skischuleListFile IsNot Nothing AndAlso fileName.Equals(_skischuleListFile.FullName) Then
        '    MessageBox.Show("Groupies " & fileName & " ist bereits geöffnet")
        '    Exit Sub
        'End If

        'If Not File.Exists(fileName) Then
        '    MessageBox.Show("Die Datei existiert nicht")
        '    Exit Sub
        'End If

        Dim loadedSkischule = OpenXML(fileName)
        'Dim loadedSkischule = OpenZIP(fileName)
        If loadedSkischule Is Nothing Then Exit Sub

        Groupies.Services.CurrentDataService.Skiclub = loadedSkischule

        '_skischuleListFile = New FileInfo(fileName)
        'QueueMostRecentFilename(fileName)
        'SetView(loadedSkischule)
        'Title = "Groupies - " & fileName

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


End Class
