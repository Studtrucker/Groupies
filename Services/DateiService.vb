Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports Groupies.Controller
Imports Groupies.Entities
Imports Microsoft.Win32


Namespace Services

    Public Module DateiService

        Public Enum GetFileInfoMode
            Laden
            Speichern
        End Enum

        ''' <summary>
        ''' Gibt die Liste der zuletzt verwendeten Dateien zurück.
        ''' </summary>
        Public ReadOnly Property MeistVerwendeteDateienSortedList As New SortedList(Of Integer, String)

        ''' <summary>
        ''' Die aktuell geladene Datei.
        ''' Wird verwendet, um den aktuellen Club im Dateisystem als XML zu speichern.
        ''' Wenn keine Datei geladen ist, ist dieser Wert Nothing.
        ''' </summary>
        ''' <returns></returns>
        Public Property AktuelleDatei As FileInfo

        ''' <summary>
        ''' Ist der aktuell geladene Club.
        ''' Wird verwendet, um den aktuellen Club zu speichern oder zu laden.
        ''' Wenn kein Club geladen ist, ist dieser Wert Nothing.
        ''' </summary>
        ''' <returns></returns>
        Public Property AktuellerClub As Generation4.Club

#Region "Datei Funktionen"

        Public Function DateiLaden() As String

            Dim mAktuelleDatei = GetFileInfo(String.Empty, "Club laden", GetFileInfoMode.Laden)

            Return DateiLaden(mAktuelleDatei)

        End Function

        Public Function DateiLaden(FileInfo As FileInfo) As String

            If FileInfo Is Nothing Then
                Return "Die Datei wurde nicht ausgewählt oder existiert nicht."
            End If

            If FileInfo.Equals(AktuelleDatei) OrElse
                AktuellerClub?.ClubName.Equals(Path.GetFileNameWithoutExtension(FileInfo.FullName)) Then
                Return "Der Club " & AktuellerClub.ClubName & " ist bereits geöffnet"
            End If

            AktuelleDatei = FileInfo
            AktuellerClub = SkiDateienService.IdentifiziereDateiGeneration(AktuelleDatei.FullName).LadeGroupies(AktuelleDatei.FullName)

            Return $"Der Club '{AktuellerClub.ClubName}' wurde erfolgreich geladen."

        End Function


        ''' <summary>
        ''' Erstellt eine neue Datei und initialisiert den aktuellen Club.
        ''' Wenn bereits eine Datei geöffnet ist, wird der Benutzer gefragt, ob er sie speichern möchte.
        ''' </summary>
        ''' <remarks>Die Datei wird im aktuellen Arbeitsverzeichnis erstellt.</remarks>
        Public Sub NeueDateiErstellen()

            ' Ist aktuell eine Datei geöffnet?
            If AktuellerClub IsNot Nothing Then
                Dim rs As MessageBoxResult = MessageBox.Show("Möchten Sie den aktuellen Club noch speichern?", "", MessageBoxButton.YesNoCancel)
                If rs = MessageBoxResult.Yes Then
                    DateiSpeichern()
                ElseIf rs = MessageBoxResult.Cancel Then
                    Exit Sub
                End If
            End If

            AktuelleDatei = GetFileInfo("MeinClub.ski", "Club speichern", GetFileInfoMode.Speichern)

            If AktuelleDatei Is Nothing Then
                MessageBox.Show("Die Datei wurde nicht erstellt, da kein Dateiname angegeben wurde.")
                Return
            End If

            NeuenClubErstellen()

            DateiSpeichern()

        End Sub

        Public Function DateiSpeichernAls(Dateiname As String) As String

            If AktuellerClub Is Nothing Then
                Return ("Es ist keine Club geladen. Bitte laden Sie einen Club, bevor Sie speichern.")
            End If

            Dim mFileInfo = Path.Combine(Path.GetDirectoryName(AktuelleDatei.FullName), Dateiname)

            AktuelleDatei = New FileInfo(mFileInfo)

            DateiSpeichern()

            Return $"Die Datei '{AktuelleDatei.Name}' wurde erfolgreich gespeichert."

        End Function

        Public Function DateiSpeichernAls() As String


            AktuelleDatei = GetFileInfo(String.Empty, "Club speichern als", GetFileInfoMode.Speichern)

            Return DateiSpeichernAls(AktuelleDatei.FullName)

        End Function

        Public Function DateiSpeichern() As String

            If AktuellerClub Is Nothing Then
                Return ("Es ist keine Club geladen. Bitte laden Sie einen Club, bevor Sie speichern.")
            End If

            ' 1. Skischule serialisieren und gezippt abspeichern
            Dim serializer = New XmlSerializer(GetType(Groupies.Entities.Generation4.Club))
            Using fs = New FileStream(AktuelleDatei.FullName, FileMode.Create)
                serializer.Serialize(fs, AktuellerClub)
            End Using

            Return $"Die Datei '{AktuelleDatei.Name}' wurde erfolgreich gespeichert."

        End Function

        Public Sub DateiSchliessen()
            AktuellerClub = Nothing
            AktuelleDatei = Nothing
        End Sub

#End Region

#Region "Club Funktionen"

        ''' <summary>
        ''' Erstellt einen neuen Club mit Standardwerten.
        ''' Der Clubname wird aus dem Dateinamen der aktuellen Datei generiert.
        ''' </summary>
        ''' <returns>Eine Erfolgsmeldung, dass der Club erstellt wurde.</returns>"
        Public Function NeuenClubErstellen() As String

            AktuellerClub = Nothing

            AktuellerClub = New Generation4.Club() With {
                .ClubName = Path.GetFileNameWithoutExtension(AktuelleDatei.Name),
                .AlleGruppen = TemplateService.StandardGruppenErstellen(15),
                .AlleFaehigkeiten = TemplateService.StandardFaehigkeitenErstellen,
                .AlleLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen,
                .AlleEinteilungen = TemplateService.StandardEinteilungenErstellen}

            Return ($"Der Club '{Path.GetFileNameWithoutExtension(AktuelleDatei.Name)}' wurde erfolgreich erstellt.")

        End Function

#End Region

#Region "IsolatedStorage"


        ''' <summary>
        ''' Der IsolatedStorage wird geladen, um die meist verwendeten Dateien zu laden und im Menu zu zeigen.
        ''' Diese Funktion wird aufgerufen, wenn das MainWindow geöffnet wird.
        ''' Die Dateien werden in der MeistVerwendeteDateienSortedList gespeichert.
        ''' </summary>
        Public Sub LadeMeistVerwendeteDateienInSortedList()
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
            Catch ex As FileNotFoundException
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Schreibt den Filename in die geordnete Liste der zuletzt verwendeten Dateien.
        ''' Wenn der Dateiname bereits in der Liste vorhanden ist, wird er aktualisiert.
        ''' Die Liste behält maximal 5 Einträge bei, wobei die ältesten entfernt werden.
        ''' </summary>
        ''' <param name="fileName"></param>
        Public Sub SchreibeFilenameInMeistVerwendeteDateienSortedList(fileName As String)
            ' Hier wird der maximale Zähler in der Variablen
            ' _mRUSortedList herausgefunden und in der lokalen
            ' Variablen max gespeichert
            Dim max As Integer = 0
            For Each i In MeistVerwendeteDateienSortedList.Keys
                If i > max Then max = i
            Next

            Dim keysToRemove As New List(Of Integer)()
            For Each kvp In MeistVerwendeteDateienSortedList
                ' Hier wird geprüft, ob der an die Methode übergebene
                ' filename einem Wert in der _mRUSortedList entspricht
                If kvp.Value.Equals(fileName) Then keysToRemove.Add(kvp.Key)
            Next

            ' Gibt es einen Eintrag in keysToRemove, dann wird dieser aus _mRUSortedList entfernt
            For Each i In keysToRemove
                MeistVerwendeteDateienSortedList.Remove(i)
            Next

            ' Hier wird der neue filename in die _mRUSortedList eingefügt
            MeistVerwendeteDateienSortedList.Add(max + 1, fileName)

            ' Wenn die Liste grösser als 5 ist, dann wird der kleinste Eintrag entfernt
            If MeistVerwendeteDateienSortedList.Count > 5 Then
                Dim min = Integer.MaxValue
                For Each i In MeistVerwendeteDateienSortedList.Keys
                    If i < min Then min = i
                Next
                MeistVerwendeteDateienSortedList.Remove(min)
            End If
        End Sub

        ''' <summary>
        ''' Die Dateien aus der MeistVerwendeteDateienSortedList werden ins IsolatedStorage gespeichert.
        ''' Diese Funktion wird aufgerufen, wenn das MainWindow geschlossen wird.
        ''' </summary>
        Public Sub SpeicherMeistVerwendeteDateienSortedListInsIsolatedStorage()
            ' 2. Die meist genutzten Listen ins Isolated Storage speichern
            If MeistVerwendeteDateienSortedList.Count > 0 Then
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    Using stream = New IsolatedStorageFileStream("mRUSortedList", FileMode.OpenOrCreate, iso)
                        Using writer = New StreamWriter(stream)
                            For Each kvp As KeyValuePair(Of Integer, String) In MeistVerwendeteDateienSortedList
                                writer.WriteLine(kvp.Key.ToString() & ";" & kvp.Value)
                            Next
                        End Using
                    End Using
                End Using
            End If
        End Sub

        ''' <summary>
        ''' Liest den zuletzt verwendeten Dateinamen aus dem IsolatedStorage.
        ''' </summary>
        Public Function LiesZuletztGeoeffneteDatei() As String
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
                If File.Exists(Filestring) Then
                    Return (Filestring)
                End If
            Catch ex As FileNotFoundException
            End Try
            Return Nothing
        End Function


        ''' <summary>
        ''' Der zuletzt verwendete Dateiname wird im IsolatedStorage gespeichert.
        ''' Diese Funktion wird aufgerufen, wenn das MainWindow geschlossen wird.
        ''' </summary>
        Public Sub SpeicherZuletztVerwendeteDateiInsIolatedStorage()
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
        End Sub


#End Region


#Region "Hilfsfunktionen"

        Public Function GetFileInfo(DefaultFilename As String, Titel As String, FileMode As GetFileInfoMode) As FileInfo

            ' CheckFileExists = False => Existierende Dateien dürfen überschrieben werden
            Dim openFileDialog As New OpenFileDialog() With {
                .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                .Title = Titel,
                .CheckFileExists = False,
                .FileName = DefaultFilename,
                .Filter = "Groupies Dateien (*.ski)|*.ski",
                .ValidateNames = True}

            If openFileDialog.ShowDialog() = True Then
                Dim fileInfo As New FileInfo(openFileDialog.FileName)
                If fileInfo.Exists AndAlso FileMode = GetFileInfoMode.Speichern Then
                    If MessageBox.Show($"Die Datei {fileInfo.Name} existiert bereits. Möchten Sie sie überschreiben?", "Datei überschreiben", MessageBoxButton.YesNo, MessageBoxImage.Question) = MessageBoxResult.No Then
                        Return Nothing
                    End If
                End If
                Return fileInfo
            Else
                Return Nothing
            End If

        End Function
#End Region

    End Module
End Namespace
