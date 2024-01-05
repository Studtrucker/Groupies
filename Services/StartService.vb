Imports System.ComponentModel
Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports CDS = Groupies.Services.CurrentDataService


Namespace Services

    Module StartService

        Public Property RuSortedList As SortedList(Of Integer, String)
        Private _skischuleListFile As FileInfo

        Public Sub LoadLastSkischule()
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
                If File.Exists(x) Then OpenSkischule(x)
            Catch ex As FileNotFoundException
            End Try
        End Sub


        Public Sub OpenSkischule(fileName As String)

            If _skischuleListFile IsNot Nothing AndAlso fileName.Equals(_skischuleListFile.FullName) Then
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

            CDS.Skiclub = loadedSkischule

            _skischuleListFile = New FileInfo(fileName)


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


        Public Sub LoadmRUSortedListMenu()
            RuSortedList = New SortedList(Of Integer, String)
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly
                    Using stream = New IsolatedStorageFileStream("RUSortedList", System.IO.FileMode.Open, iso)
                        Using reader = New StreamReader(stream)
                            Dim i = 0
                            While reader.Peek <> -1
                                Dim line = reader.ReadLine().Split(";")
                                If line.Length = 2 AndAlso line(0).Length > 0 AndAlso Not RuSortedList.ContainsKey(Integer.Parse(line(0))) Then
                                    If File.Exists(line(1)) Then
                                        i += 1
                                        RuSortedList.Add(i, line(1))
                                    End If
                                End If
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As FileNotFoundException
                'Throw ex
            End Try

        End Sub

    End Module

End Namespace

