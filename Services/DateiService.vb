Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports Groupies.Controller
Imports Groupies.Entities
Imports Groupies.ViewModels
Imports Microsoft.Win32


Namespace Services

    ''' <summary>
    ''' Modus für die Dateiauswahl: Laden oder Speichern.
    ''' </summary>
    Public Enum GetFileInfoMode
        Laden
        Speichern
    End Enum

    Public Class DateiService
        Inherits BaseModel

#Region "Events und EventArgs"
        Public Event DateiGeoeffnet As EventHandler(Of OperationResultEventArgs)
        Public Event DateiOeffnenIstFehlgeschlagen As EventHandler(Of OperationResultEventArgs)
        Public Event DateiGeschlossen As EventHandler(Of OperationResultEventArgs)
        Public Event DateiGespeichert As EventHandler(Of OperationResultEventArgs)
        Public Event ClubSpeichernFehlgeschlagen As EventHandler(Of OperationResultEventArgs)
        Public Event ClubSpeichernErfolgreich As EventHandler(Of OperationResultEventArgs)
#End Region
        Protected Overridable Sub OnDateiGeoeffnet(e As OperationResultEventArgs)
            RaiseEvent DateiGeoeffnet(Me, e)
        End Sub

        Protected Overridable Sub OnDateiOeffnenIstFehlgeschlagen(e As OperationResultEventArgs)
            RaiseEvent DateiOeffnenIstFehlgeschlagen(Me, e)
        End Sub

        Protected Overridable Sub OnDateiGeschlossen(e As OperationResultEventArgs)
            RaiseEvent DateiGeschlossen(Me, e)
        End Sub

        Protected Overridable Sub OnClubSpeichernErfolgreich(e As OperationResultEventArgs)
            RaiseEvent ClubSpeichernErfolgreich(Me, e)
        End Sub

        Protected Overridable Sub OnClubSpeichernFehlgeschlagen(e As OperationResultEventArgs)
            RaiseEvent ClubSpeichernFehlgeschlagen(Me, e)
        End Sub

        Private ReadOnly _msgService As IViewMessageService

        ''' <summary>
        ''' Konstruktor der DateiService Klasse.
        ''' Initialisiert die ZuletztVerwendeteDateienSortedList.
        ''' </summary>
        Public Sub New(Optional msgService As IViewMessageService = Nothing)
            ZuletztVerwendeteDateienSortedList = New SortedList(Of Integer, String)()
            _msgService = If(msgService, New DefaultViewMessageService())
        End Sub

        ''' <summary>
        ''' Gibt die Liste der zuletzt verwendeten Dateien zurück.
        ''' </summary>
        Public ReadOnly Property ZuletztVerwendeteDateienSortedList As New SortedList(Of Integer, String)

        ''' <summary>
        ''' Die aktuell geladene Datei (Instanz-Eigenschaft).
        ''' </summary>
        Public Property AktuelleDatei As FileInfo

        ''' <summary>
        ''' Der aktuell geladene Club (Instanz-Eigenschaft).
        ''' </summary>
        Public Property AktuellerClub As Generation4.Club


#Region "Datei Funktionen"

        Public Sub DateiOeffnen()
            Dim ausgewaehlterPfad = DateipfadAuswaehlen()
            DateiOeffnen(ausgewaehlterPfad)
        End Sub

        Public Sub DateiOeffnen(File As FileInfo)

            If File Is Nothing OrElse String.IsNullOrEmpty(File.FullName) Then
                OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, "Die Datei wurde nicht ausgewählt oder existiert nicht."))
            End If

            If AktuelleDatei IsNot Nothing AndAlso String.Equals(File.FullName, AktuelleDatei.FullName, StringComparison.OrdinalIgnoreCase) Then
                OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, $"Der Club '{If(AktuellerClub?.ClubName, Path.GetFileNameWithoutExtension(File.Name))}' ist bereits geöffnet"))
            End If

            Try
                AktuelleDatei = New FileInfo(File.FullName)
                AktuellerClub = SkiDateienService.IdentifiziereDateiGeneration(AktuelleDatei.FullName).LadeGroupies(AktuelleDatei.FullName)
                SchreibeZuletztVerwendeteDateienSortedList(AktuelleDatei.FullName)
                ' Erfolgs-Event mit Payload (der geladene Club)
                'OnDateiGeoeffnet(New OperationResultEventArgs(True, $"Die Datei '{AktuellerClub.ClubName}' wurde erfolgreich geladen.", Nothing, AktuellerClub))
                Return
                'OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, "Die Datei konnte nicht geladen werden."))
            Catch ex As Exception
                ' Fehler-Ereignis mit Exception-Objekt
                'OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, "Die Datei konnte nicht geladen werden."))
                'OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, $"Fehler beim Laden: {ex.Message}", ex))
            End Try

        End Sub
        Private Function DateiPruefenUndOeffnen(dateipfad As FileInfo, ByRef meldung As String) As Boolean

            If dateipfad Is Nothing Then
                meldung = "Datei öffnen wurde abgebrochen"
                Return False
            End If

            If String.IsNullOrEmpty(dateipfad.FullName) OrElse Not File.Exists(dateipfad.FullName) Then
                meldung = "Datei existiert nicht"
                Return False
            End If

            Dim ext = Path.GetExtension(dateipfad.FullName)
            If Not String.Equals(ext, ".ski", StringComparison.OrdinalIgnoreCase) Then
                meldung = "Datei ist keine Groupies Datei"
                Return False
            End If

            If AktuelleDatei IsNot Nothing AndAlso String.Equals(dateipfad.FullName, AktuelleDatei.FullName, StringComparison.OrdinalIgnoreCase) Then
                Dim clubname = If(AktuellerClub?.ClubName, Path.GetFileNameWithoutExtension(dateipfad.Name))
                meldung = $"Der Club '{clubname}' ist bereits geöffnet"
                Return False
            End If

            'meldung = $"Die Datei {dateipfad.Name} wurde erfolgreich geladen"
            Return True
        End Function

        Public Function DateipfadAuswaehlen() As FileInfo
            Return GetFileInfo(String.Empty, "Club öffnen", GetFileInfoMode.Laden)
        End Function

        Public Sub ClubSpeichern()

            If AktuellerClub Is Nothing Then
                OnClubSpeichernFehlgeschlagen(New OperationResultEventArgs(False, "Es ist kein Club geladen. Bitte laden Sie einen Club, bevor Sie speichern."))
            End If

            If AktuelleDatei Is Nothing OrElse String.IsNullOrWhiteSpace(AktuelleDatei.FullName) Then
                OnClubSpeichernFehlgeschlagen(New OperationResultEventArgs(False, "Keine Zieldatei angegeben. Bitte 'Speichern unter' verwenden."))
            End If

            Try
                Dim directory As String = Path.GetDirectoryName(AktuelleDatei.FullName)

                If String.IsNullOrWhiteSpace(directory) Then
                    ' Falls kein Verzeichnis ermittelt werden kann, auf aktuelles Verzeichnis zurückfallen
                    directory = Environment.CurrentDirectory
                    AktuelleDatei = New FileInfo(Path.Combine(directory, AktuelleDatei.Name))
                End If


                Dim serializer = New XmlSerializer(GetType(Groupies.Entities.Generation4.Club))
                Using fs = New FileStream(AktuelleDatei.FullName, FileMode.Create, FileAccess.Write, FileShare.None)
                    serializer.Serialize(fs, AktuellerClub)
                End Using

                OnClubSpeichernErfolgreich(New OperationResultEventArgs(True, $"Die Datei '{AktuelleDatei.Name}' wurde erfolgreich gespeichert."))
            Catch ex As UnauthorizedAccessException
                OnClubSpeichernFehlgeschlagen(New OperationResultEventArgs(False, $"Fehler beim Speichern: Zugriff verweigert ({ex.Message})"))
            Catch ex As IOException
                onclubspeichernfehlgeschlagen(New OperationResultEventArgs(False, $"Fehler beim Speichern: Ein-/Ausgabefehler ({ex.Message})"))
            Catch ex As Exception
                onclubspeichernfehlgeschlagen(New OperationResultEventArgs(False, $"Fehler beim Speichern: {ex.Message}"))
            End Try
        End Sub

        Public Function DateiSpeichern() As String
            If AktuellerClub Is Nothing Then
                Return "Es ist kein Club geladen. Bitte laden Sie einen Club, bevor Sie speichern."
            End If

            If AktuelleDatei Is Nothing OrElse String.IsNullOrWhiteSpace(AktuelleDatei.FullName) Then
                Return "Keine Zieldatei angegeben. Bitte 'Speichern unter' verwenden."
            End If

            Try
                Dim directory As String = Path.GetDirectoryName(AktuelleDatei.FullName)

                If String.IsNullOrWhiteSpace(directory) Then
                    ' Falls kein Verzeichnis ermittelt werden kann, auf aktuelles Verzeichnis zurückfallen
                    directory = Environment.CurrentDirectory
                    AktuelleDatei = New FileInfo(Path.Combine(directory, AktuelleDatei.Name))
                End If


                Dim serializer = New XmlSerializer(GetType(Groupies.Entities.Generation4.Club))
                Using fs = New FileStream(AktuelleDatei.FullName, FileMode.Create, FileAccess.Write, FileShare.None)
                    serializer.Serialize(fs, AktuellerClub)
                End Using

                OnClubSpeichernErfolgreich(New OperationResultEventArgs(True, $"Die Datei '{AktuelleDatei.Name}' wurde erfolgreich gespeichert."))
                Return $"Die Datei '{AktuelleDatei.Name}' wurde erfolgreich gespeichert."
            Catch ex As UnauthorizedAccessException
                Return $"Fehler beim Speichern: Zugriff verweigert ({ex.Message})"
            Catch ex As IOException
                Return $"Fehler beim Speichern: Ein-/Ausgabefehler ({ex.Message})"
            Catch ex As Exception
                Return $"Fehler beim Speichern: {ex.Message}"
            End Try
        End Function

        Public Function DateiSpeichernAls() As String
            Dim sicherungAktuelleDatei = AktuelleDatei
            Dim neu = GetFileInfo(String.Empty, "Club speichern als", GetFileInfoMode.Speichern)

            If neu Is Nothing Then
                AktuelleDatei = sicherungAktuelleDatei
                Return "Speichern als wurde abgebrochen"
            End If

            AktuelleDatei = neu
            Return DateiSpeichern()
        End Function

        Public Function DateiSpeichernAls(Dateiname As String) As String
            If AktuellerClub Is Nothing Then
                Return "Es ist kein Club geladen. Bitte laden Sie einen Club, bevor Sie speichern."
            End If

            Dim targetPath As String
            If Path.IsPathRooted(Dateiname) Then
                targetPath = Dateiname
            Else
                Dim baseDir = If(AktuelleDatei IsNot Nothing AndAlso Not String.IsNullOrEmpty(AktuelleDatei.FullName),
                                 Path.GetDirectoryName(AktuelleDatei.FullName),
                                 Environment.CurrentDirectory)
                targetPath = Path.Combine(baseDir, Dateiname)
            End If

            AktuelleDatei = New FileInfo(targetPath)
            AktuellerClub.ClubName = AktuelleDatei.Name

            Dim res = DateiSpeichern()
            If Not res.StartsWith("Fehler", StringComparison.OrdinalIgnoreCase) Then
                SchreibeZuletztVerwendeteDateienSortedList(AktuelleDatei.FullName)
            End If
            Return res
        End Function

        Public Sub DateiSchliessen()
            AktuellerClub = Nothing
            AktuelleDatei = Nothing
            OnDateiGeschlossen(OperationResultEventArgs.Empty)
        End Sub

#End Region

#Region "Club Funktionen"
        Public Function NeuenClubErstellen() As String
            AktuellerClub = New Generation4.Club() With {
                .ClubName = If(AktuelleDatei IsNot Nothing, Path.GetFileNameWithoutExtension(AktuelleDatei.Name), "Neuer Club")}
            Return $"Der Club '{AktuellerClub.ClubName}' wurde erfolgreich erstellt."
        End Function
#End Region

#Region "IsolatedStorage"
        Public Sub LadeMeistVerwendeteDateienInSortedList()
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    If Not iso.FileExists("mRuSortedList") Then Return
                    Using stream = New IsolatedStorageFileStream("mRuSortedList", FileMode.Open, iso)
                        Using reader = New StreamReader(stream)
                            Dim i = 0
                            While reader.Peek <> -1
                                Dim line = reader.ReadLine()
                                If String.IsNullOrWhiteSpace(line) Then Continue While
                                Dim parts = line.Split(";"c)
                                Dim key As Integer
                                If parts.Length = 2 AndAlso Integer.TryParse(parts(0), key) Then
                                    Dim path = parts(1)
                                    If File.Exists(path) Then
                                        i += 1
                                        ZuletztVerwendeteDateienSortedList.Add(i, path)
                                    End If
                                End If
                            End While
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                ' fail silently — Liste bleibt leer; optional logging möglich
            End Try
        End Sub

        Public Sub SchreibeZuletztVerwendeteDateienSortedList(fileName As String)
            Dim max As Integer = 0
            For Each k In ZuletztVerwendeteDateienSortedList.Keys
                If k > max Then max = k
            Next

            Dim keysToRemove As New List(Of Integer)()
            For Each kvp In ZuletztVerwendeteDateienSortedList.ToList()
                If String.Equals(kvp.Value, fileName, StringComparison.OrdinalIgnoreCase) Then
                    keysToRemove.Add(kvp.Key)
                End If
            Next

            For Each i In keysToRemove
                ZuletztVerwendeteDateienSortedList.Remove(i)
            Next

            ZuletztVerwendeteDateienSortedList.Add(max + 1, fileName)

            If ZuletztVerwendeteDateienSortedList.Count > 5 Then
                Dim min = ZuletztVerwendeteDateienSortedList.Keys.Min()
                ZuletztVerwendeteDateienSortedList.Remove(min)
            End If
        End Sub

        Public Sub SpeicherZuletztVerwendeteDateienSortedList()
            If ZuletztVerwendeteDateienSortedList.Count = 0 Then Return
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    Using stream = New IsolatedStorageFileStream("mRUSortedList", FileMode.Create, iso)
                        Using writer = New StreamWriter(stream)
                            For Each kvp As KeyValuePair(Of Integer, String) In ZuletztVerwendeteDateienSortedList
                                writer.WriteLine(kvp.Key.ToString() & ";" & kvp.Value)
                            Next
                        End Using
                    End Using
                End Using
            Catch ex As Exception
                ' optional logging
            End Try
        End Sub

        Public Function LiesZuletztGeoeffneteDatei() As String
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    If Not iso.FileExists("LastGroupies") Then Return Nothing
                    Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.Open, iso)
                        Using reader = New StreamReader(stream)
                            Dim filestring = reader.ReadLine()
                            If Not String.IsNullOrWhiteSpace(filestring) AndAlso File.Exists(filestring) Then
                                Return filestring
                            End If
                        End Using
                    End Using
                End Using
            Catch ex As Exception
            End Try
            Return Nothing
        End Function

        Public Sub SpeicherZuletztVerwendeteDateiInsIolatedStorage()
            If AktuelleDatei Is Nothing Then Return
            Try
                Using iso = IsolatedStorageFile.GetUserStoreForAssembly()
                    Using stream = New IsolatedStorageFileStream("LastGroupies", FileMode.Create, iso)
                        Using writer = New StreamWriter(stream)
                            writer.WriteLine(AktuelleDatei.FullName)
                        End Using
                    End Using
                End Using
            Catch ex As Exception
            End Try
        End Sub
#End Region

#Region "Hilfsfunktionen"
        Public Function GetFileInfo(DefaultFilename As String, Titel As String, mode As GetFileInfoMode) As FileInfo
            If mode = GetFileInfoMode.Speichern Then
                Dim saveDialog As New SaveFileDialog() With {
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    .Title = Titel,
                    .FileName = DefaultFilename,
                    .Filter = "Groupies Dateien (*.ski)|*.ski",
                    .ValidateNames = True,
                    .OverwritePrompt = False
                }
                If saveDialog.ShowDialog() = True Then
                    Dim fileInfo As New FileInfo(saveDialog.FileName)
                    If fileInfo.Exists Then
                        If Not _msgService.ConfirmOverwrite(fileInfo.Name) Then
                            Return Nothing
                        End If
                    End If
                    Return fileInfo
                End If
                Return Nothing
            Else
                Dim openFileDialog As New OpenFileDialog() With {
                    .InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                    .Title = Titel,
                    .CheckFileExists = True,
                    .FileName = DefaultFilename,
                    .Filter = "Groupies Dateien (*.ski)|*.ski",
                    .ValidateNames = True}
                If openFileDialog.ShowDialog() = True Then
                    Return New FileInfo(openFileDialog.FileName)
                End If
                Return Nothing
            End If
        End Function
#End Region

    End Class
End Namespace
