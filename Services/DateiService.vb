Imports System.IO
Imports System.IO.IsolatedStorage
Imports System.Xml.Serialization
Imports Groupies.Entities
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

        Public Event ClubGeladen As EventHandler(Of OperationResultEventArgs)
        Public Event ClubNichtGeladen As EventHandler(Of OperationResultEventArgs)
        Public Event ClubNeuErstellt As EventHandler(Of OperationResultEventArgs)
        Public Event ClubGeschlossen As EventHandler(Of OperationResultEventArgs)
        Public Event ClubGespeichert As EventHandler(Of OperationResultEventArgs)
        Public Event ClubNichtGespeichert As EventHandler(Of OperationResultEventArgs)

        Protected Overridable Sub OnClubGeladen(e As OperationResultEventArgs)
            If e.Success Then
                RaiseEvent ClubGeladen(Me, e)
            Else
                RaiseEvent ClubNichtGeladen(Me, e)
            End If
        End Sub

        Protected Overridable Sub OnClubGeschlossen(e As OperationResultEventArgs)
            RaiseEvent ClubGeschlossen(Me, e)
        End Sub

        Protected Overridable Sub OnDateiSpeichern(e As OperationResultEventArgs)
            If e.Success Then
                RaiseEvent ClubGespeichert(Me, e)
            Else
                RaiseEvent ClubNichtGespeichert(Me, e)
            End If
        End Sub


#End Region

#Region "Datei Funktionen"

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



        Public Function DateipfadAuswaehlen() As FileInfo
            Return GetFileInfo(String.Empty, "Club öffnen", GetFileInfoMode.Laden)
        End Function

#End Region

#Region "Club Funktionen"

        Public Sub ClubSpeichernAls()
            Dim sicherungAktuelleDatei = AktuelleDatei
            Dim neu = GetFileInfo(String.Empty, "Club speichern als", GetFileInfoMode.Speichern)

            If neu Is Nothing Then
                AktuelleDatei = sicherungAktuelleDatei
                OnDateiSpeichern(New OperationResultEventArgs(False, String.Empty))
                Return
            End If

            AktuelleDatei = neu
            ClubSpeichern()
            SchreibeZuletztVerwendeteDateienSortedList(AktuelleDatei.FullName)
        End Sub

        Public Sub ClubSpeichern()

            If AktuelleDatei Is Nothing OrElse String.IsNullOrWhiteSpace(AktuelleDatei.FullName) Then
                ClubSpeichernAls()
            End If

            If AktuellerClub Is Nothing Then
                OnDateiSpeichern(New OperationResultEventArgs(False, "Es ist kein Club geladen. Bitte laden Sie einen Club, bevor Sie speichern."))
                Return
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
                    Me.AktuellerClub.ClubName = Path.GetFileNameWithoutExtension(AktuelleDatei.Name)
                    serializer.Serialize(fs, AktuellerClub)
                End Using
                OnDateiSpeichern(New OperationResultEventArgs(True, $"DerClub '{AktuelleDatei.Name}' wurde erfolgreich gespeichert.", Nothing, AktuellerClub))
            Catch ex As UnauthorizedAccessException
                OnDateiSpeichern(New OperationResultEventArgs(False, $"Fehler beim Speichern: Zugriff verweigert ({ex.Message})"))
            Catch ex As IOException
                OnDateiSpeichern(New OperationResultEventArgs(False, $"Fehler beim Speichern: Ein-/Ausgabefehler ({ex.Message})"))
            Catch ex As Exception
                OnDateiSpeichern(New OperationResultEventArgs(False, $"Fehler beim Speichern: {ex.Message}"))
            End Try

        End Sub

        Public Sub NeuenClubErstellen()
            IstEinClubGeoffnet(Me, New OperationResultEventArgs(True, "Möchten Sie den aktuellen Club speichern, bevor Sie einen neuen Club erstellen."))
            AktuellerClub = New Generation4.Club() With {.ClubName = If(AktuelleDatei IsNot Nothing, Path.GetFileNameWithoutExtension(AktuelleDatei.Name), "Neuer Club")}
            OnClubGeladen(New OperationResultEventArgs(True, $"Der Club '{AktuellerClub.ClubName}' wurde erfolgreich erstellt.", Nothing, AktuellerClub))
        End Sub

        Public Sub ClubLaden()
            IstEinClubGeoffnet(Me, New OperationResultEventArgs(True, "Möchten Sie den aktuellen Club speichern, bevor Sie einen anderen Club öffnen."))
            Dim ausgewaehlterPfad = DateipfadAuswaehlen()
            ClubLaden(ausgewaehlterPfad.FullName)
        End Sub

        Public Sub ClubLaden(Path As String)
            IstEinClubGeoffnet(Me, New OperationResultEventArgs(True, "Möchten Sie den aktuellen Club speichern, bevor Sie einen anderen Club öffnen."))
            ClubLaden(New FileInfo(Path))
        End Sub

        Private Sub ClubLaden(File As FileInfo)

            If File Is Nothing OrElse String.IsNullOrEmpty(File.FullName) Then
                OnClubGeladen(New OperationResultEventArgs(False, "Die Datei wurde nicht ausgewählt oder existiert nicht."))
            End If

            If AktuelleDatei IsNot Nothing AndAlso String.Equals(File.FullName, AktuelleDatei.FullName, StringComparison.OrdinalIgnoreCase) Then
                OnClubGeladen(New OperationResultEventArgs(False, $"Der Club '{If(AktuellerClub?.ClubName, Path.GetFileNameWithoutExtension(File.Name))}' ist bereits geöffnet"))
            End If

            Try
                AktuelleDatei = New FileInfo(File.FullName)
                AktuellerClub = SkiDateienService.IdentifiziereDateiGeneration(AktuelleDatei.FullName).LadeGroupies(AktuelleDatei.FullName)
                SchreibeZuletztVerwendeteDateienSortedList(AktuelleDatei.FullName)
                ' Erfolgs-Event mit Payload (der geladene Club)
                OnClubGeladen(New OperationResultEventArgs(True, $"Die Datei '{AktuellerClub.ClubName}' wurde erfolgreich geladen.", Nothing, AktuellerClub))
                Return
                'OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, "Die Datei konnte nicht geladen werden."))
            Catch ex As Exception
                ' Fehler-Ereignis mit Exception-Objekt
                'OnDateiOeffnenIstFehlgeschlagen(New OperationResultEventArgs(False, "Die Datei konnte nicht geladen werden."))
                OnClubGeladen(New OperationResultEventArgs(False, $"Fehler beim Laden: {ex.Message}", ex))
            End Try

        End Sub

        Public Sub ClubSchliessen()
            AktuellerClub = Nothing
            AktuelleDatei = Nothing
            OnClubGeschlossen(New OperationResultEventArgs(True, String.Empty))
        End Sub

        Public Sub IstEinClubGeoffnet(sender As Object, e As OperationResultEventArgs)
            If ServiceProvider.DateiService.AktuellerClub IsNot Nothing Then
                Dim msg = New DefaultViewMessageService
                Dim result = msg.ShowConfirmation(e.Message, "Neuen Club erstellen")
                If result Then
                    ServiceProvider.DateiService.ClubSpeichern()
                End If
                ClubSchliessen()
            End If
        End Sub

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
