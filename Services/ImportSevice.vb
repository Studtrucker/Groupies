Imports System.Collections.ObjectModel
Imports System.Text
Imports System.Windows
Imports Groupies.Controller
Imports Groupies.Entities.Generation4
Imports Microsoft.Win32

Namespace Services

    ''' <summary>
    ''' Service für den Import von Trainer- und Teilnehmerdaten aus Excel/CSV-Dateien.
    ''' Alle Methoden mit UI-Interaktion (MessageBox) sollten aus ViewModel/Controller aufgerufen werden.
    ''' </summary>
    Public Class ImportService
        Implements IDisposable

#Region "Felder"
        Private ReadOnly _dateiService As DateiService
        Private _disposed As Boolean = False
#End Region

#Region "Konstruktor"
        Public Sub New(dateiService As DateiService)
            _dateiService = If(dateiService, ServiceProvider.DateiService)
            If _dateiService Is Nothing Then
                Throw New InvalidOperationException("DateiService ist nicht verfügbar.")
            End If
        End Sub

        Public Sub New()
            Me.New(ServiceProvider.DateiService)
        End Sub
#End Region

#Region "Öffentliche Methoden"

        ''' <summary>
        ''' Importiert Trainer aus einer Datei. Zeigt Dateiauswahldialog an.
        ''' </summary>
        Public Function ImportTrainerMitDialog() As ImportErgebnis
            Dim pfad = ZeigeDateiAuswahlDialog()
            If String.IsNullOrEmpty(pfad) Then
                Return New ImportErgebnis With {.Abgebrochen = True}
            End If

            Return ImportTrainer(pfad)
        End Function

        ''' <summary>
        ''' Importiert Trainer aus der angegebenen Datei.
        ''' </summary>
        Public Function ImportTrainer(pfad As String) As ImportErgebnis
            If String.IsNullOrWhiteSpace(pfad) Then
                Throw New ArgumentException("Pfad darf nicht leer sein.", NameOf(pfad))
            End If

            If Not System.IO.File.Exists(pfad) Then
                Throw New IO.FileNotFoundException("Datei nicht gefunden.", pfad)
            End If

            Dim ergebnis As New ImportErgebnis()

            Try
                Dim importListe = LeseTrainerAusDatasetKI(pfad)
                If importListe Is Nothing OrElse importListe.Count = 0 Then
                    ergebnis.Fehler = "Keine Trainer in der Datei gefunden."
                    Return ergebnis
                End If

                Dim club = _dateiService.AktuellerClub
                If club Is Nothing Then
                    ergebnis.Fehler = "Kein Club geladen."
                    Return ergebnis
                End If

                If club.Trainerliste Is Nothing Then
                    club.Trainerliste = New TrainerCollection()
                End If

                ' Verarbeitung mit Lookup-Strukturen für Performance
                VerarbeiteTrainerImport(club, importListe, ergebnis)

                Return ergebnis

            Catch ex As Exception
                ergebnis.Fehler = $"Fehler beim Import: {ex.Message}"
                If ex.InnerException IsNot Nothing Then
                    ergebnis.Fehler &= $"{vbNewLine}{ex.InnerException.Message}"
                End If
                Return ergebnis
            End Try
        End Function

        ''' <summary>
        ''' Importiert Teilnehmer aus einer Datei. Zeigt Dateiauswahldialog an.
        ''' </summary>
        Public Function ImportTeilnehmerMitDialog() As ImportErgebnis
            Dim pfad = ZeigeDateiAuswahlDialog()
            If String.IsNullOrEmpty(pfad) Then
                Return New ImportErgebnis With {.Abgebrochen = True}
            End If

            Return ImportTeilnehmer(pfad)
        End Function

        ''' <summary>
        ''' Importiert Teilnehmer aus der angegebenen Datei.
        ''' </summary>
        Public Function ImportTeilnehmer(pfad As String) As ImportErgebnis
            If String.IsNullOrWhiteSpace(pfad) Then
                Throw New ArgumentException("Pfad darf nicht leer sein.", NameOf(pfad))
            End If

            If Not System.IO.File.Exists(pfad) Then
                Throw New IO.FileNotFoundException("Datei nicht gefunden.", pfad)
            End If

            Dim ergebnis As New ImportErgebnis()

            Try
                Dim importListe = LeseTeilnehmerAusDatasetKI(pfad)
                If importListe Is Nothing OrElse importListe.Count = 0 Then
                    ergebnis.Fehler = "Keine Teilnehmer in der Datei gefunden."
                    Return ergebnis
                End If

                Dim club = _dateiService.AktuellerClub
                If club Is Nothing Then
                    ergebnis.Fehler = "Kein Club geladen."
                    Return ergebnis
                End If

                If club.Teilnehmerliste Is Nothing Then
                    club.Teilnehmerliste = New TeilnehmerCollection()
                End If

                If club.Leistungsstufenliste Is Nothing Then
                    club.Leistungsstufenliste = New LeistungsstufeCollection()
                End If

                VerarbeiteTeilnehmerImport(club, importListe, ergebnis)

                Return ergebnis

            Catch ex As Exception
                ergebnis.Fehler = $"Fehler beim Import: {ex.Message}"
                If ex.InnerException IsNot Nothing Then
                    ergebnis.Fehler &= $"{vbNewLine}{ex.InnerException.Message}"
                End If
                Return ergebnis
            End Try
        End Function

#End Region

#Region "Private Hilfsmethoden"

        Private Sub VerarbeiteTrainerImport(club As Club, importListe As List(Of DataImport.Trainer), ergebnis As ImportErgebnis)
            ' Reset Import-Flags
            For Each t In club.Trainerliste
                t.IstImImport = False
                t.IstNeuImClub = False
                t.IstRueckkehrer = False
            Next

            ' Lookup-Strukturen
            Dim byId As New Dictionary(Of Guid, Trainer)
            Dim byName As New Dictionary(Of String, Trainer)(StringComparer.OrdinalIgnoreCase)

            For Each t In club.Trainerliste
                If t.TrainerID <> Guid.Empty AndAlso Not byId.ContainsKey(t.TrainerID) Then
                    byId.Add(t.TrainerID, t)
                End If
                Dim nameKey = NormalisiereName(t.Vorname, t.Nachname)
                If Not String.IsNullOrEmpty(nameKey) AndAlso Not byName.ContainsKey(nameKey) Then
                    byName.Add(nameKey, t)
                End If
            Next

            Dim neuTrainer As New List(Of Trainer)()

            ' Verarbeite importierte Trainer
            For Each imported In importListe
                If imported Is Nothing Then Continue For

                If imported.TrainerID <> Guid.Empty Then
                    ' Versuch Match per ID
                    Dim exist As Trainer = Nothing
                    If byId.TryGetValue(imported.TrainerID, exist) Then
                        ' Update
                        AktualisiereTrainer(exist, imported)
                        ergebnis.Aktualisiert += 1
                    Else
                        ' Rückkehrer
                        Dim rueck = ErstelleTrainer(imported, istRueckkehrer:=True)
                        club.Trainerliste.Add(rueck)
                        neuTrainer.Add(rueck)
                        byId.Add(rueck.TrainerID, rueck)
                        ergebnis.NeuHinzugefuegt += 1
                        ergebnis.Rueckkehrer += 1
                    End If
                Else
                    ' Kein ID: per Name
                    Dim nameKey = NormalisiereName(imported.Vorname, imported.Nachname)
                    Dim known As Trainer = Nothing
                    If Not String.IsNullOrEmpty(nameKey) AndAlso byName.TryGetValue(nameKey, known) Then
                        AktualisiereTrainer(known, imported)
                        ergebnis.Aktualisiert += 1
                    Else
                        ' Neu
                        Dim neu = ErstelleTrainer(imported, istRueckkehrer:=False)
                        club.Trainerliste.Add(neu)
                        neuTrainer.Add(neu)
                        byId.Add(neu.TrainerID, neu)
                        byName.Add(nameKey, neu)
                        ergebnis.NeuHinzugefuegt += 1
                    End If
                End If
            Next

            ergebnis.NeueTrainer = neuTrainer
            'ergebnis.NichtImportiert = club.Trainerliste.Where(Function(t) Not t.IstImImport).ToList()
        End Sub

        Private Sub VerarbeiteTeilnehmerImport(club As Club, importListe As List(Of DataImport.Teilnehmer), ergebnis As ImportErgebnis)
            ' Reset Import-Flags
            For Each t In club.Teilnehmerliste
                t.IstImImport = False
                t.IstNeuImClub = False
                t.IstRueckkehrer = False
            Next

            ' Lookup-Strukturen
            Dim byId As New Dictionary(Of Guid, Teilnehmer)
            Dim byName As New Dictionary(Of String, Teilnehmer)(StringComparer.OrdinalIgnoreCase)

            For Each t In club.Teilnehmerliste
                If t.Ident <> Guid.Empty AndAlso Not byId.ContainsKey(t.Ident) Then
                    byId.Add(t.Ident, t)
                End If
                Dim nameKey = NormalisiereName(t.Vorname, t.Nachname)
                If Not String.IsNullOrEmpty(nameKey) AndAlso Not byName.ContainsKey(nameKey) Then
                    byName.Add(nameKey, t)
                End If
            Next

            Dim neuTeilnehmer As New List(Of Teilnehmer)()

            For Each imported In importListe
                If imported Is Nothing Then Continue For

                If imported.TeilnehmerID <> Guid.Empty Then
                    Dim exist As Teilnehmer = Nothing
                    If byId.TryGetValue(imported.TeilnehmerID, exist) Then
                        AktualisiereTeilnehmer(exist, imported, club.Leistungsstufenliste)
                        ergebnis.Aktualisiert += 1
                    Else
                        Dim rueck = ErstelleTeilnehmer(imported, club.Leistungsstufenliste, istRueckkehrer:=True)
                        club.Teilnehmerliste.Add(rueck)
                        neuTeilnehmer.Add(rueck)
                        byId.Add(rueck.Ident, rueck)
                        ergebnis.NeuHinzugefuegt += 1
                        ergebnis.Rueckkehrer += 1
                    End If
                Else
                    Dim nameKey = NormalisiereName(imported.Vorname, imported.Nachname)
                    Dim known As Teilnehmer = Nothing
                    If Not String.IsNullOrEmpty(nameKey) AndAlso byName.TryGetValue(nameKey, known) Then
                        AktualisiereTeilnehmer(known, imported, club.Leistungsstufenliste)
                        ergebnis.Aktualisiert += 1
                    Else
                        Dim neu = ErstelleTeilnehmer(imported, club.Leistungsstufenliste, istRueckkehrer:=False)
                        club.Teilnehmerliste.Add(neu)
                        neuTeilnehmer.Add(neu)
                        byId.Add(neu.Ident, neu)
                        byName.Add(nameKey, neu)
                        ergebnis.NeuHinzugefuegt += 1
                    End If
                End If
            Next

            ergebnis.NeueTeilnehmer = neuTeilnehmer
            'ergebnis.NichtImportiert = club.Teilnehmerliste.Where(Function(t) Not t.IstImImport).ToList()
        End Sub

        Private Function NormalisiereName(vorname As String, nachname As String) As String
            Dim v = If(vorname, String.Empty).Trim()
            Dim n = If(nachname, String.Empty).Trim()
            If String.IsNullOrEmpty(v) AndAlso String.IsNullOrEmpty(n) Then Return String.Empty
            Return (v & "|" & n).ToLowerInvariant()
        End Function

        Private Sub AktualisiereTrainer(ziel As Trainer, quelle As DataImport.Trainer)
            ziel.Vorname = quelle.Vorname
            ziel.Nachname = quelle.Nachname
            ziel.Alias = quelle.Spitzname
            ziel.Telefonnummer = quelle.Telefonnummer
            ziel.EMail = quelle.eMail
            ziel.IstImImport = True
        End Sub

        Private Function ErstelleTrainer(quelle As DataImport.Trainer, istRueckkehrer As Boolean) As Trainer
            Return New Trainer With {
                .TrainerID = If(quelle.TrainerID <> Guid.Empty, quelle.TrainerID, Guid.NewGuid()),
                .Vorname = quelle.Vorname,
                .Nachname = quelle.Nachname,
                .Alias = quelle.Spitzname,
                .Telefonnummer = quelle.Telefonnummer,
                .EMail = quelle.eMail,
                .Foto = Nothing,
                .IstImImport = True,
                .IstRueckkehrer = istRueckkehrer,
                .IstNeuImClub = True
            }
        End Function

        Private Sub AktualisiereTeilnehmer(ziel As Teilnehmer, quelle As DataImport.Teilnehmer, leistungsstufen As LeistungsstufeCollection)
            ziel.Vorname = quelle.Vorname
            ziel.Nachname = quelle.Nachname
            ziel.Telefonnummer = quelle.Telefonnummer
            ziel.Geburtsdatum = quelle.Geburtsdatum
            ziel.Leistungsstufe = FindeLeistungsstufe(quelle.Leistungsstand, leistungsstufen)
            ziel.LeistungsstufeID = If(ziel.Leistungsstufe?.Ident, Guid.Empty)
            ziel.IstImImport = True
        End Sub

        Private Function ErstelleTeilnehmer(quelle As DataImport.Teilnehmer, leistungsstufen As LeistungsstufeCollection, istRueckkehrer As Boolean) As Teilnehmer
            Dim ls = FindeLeistungsstufe(quelle.Leistungsstand, leistungsstufen)
            Return New Teilnehmer With {
                .Ident = If(quelle.TeilnehmerID <> Guid.Empty, quelle.TeilnehmerID, Guid.NewGuid()),
                .Vorname = quelle.Vorname,
                .Nachname = quelle.Nachname,
                .Telefonnummer = quelle.Telefonnummer,
                .Geburtsdatum = quelle.Geburtsdatum,
                .Leistungsstufe = ls,
                .LeistungsstufeID = If(ls?.Ident, Guid.Empty),
                .IstImImport = True,
                .IstRueckkehrer = istRueckkehrer,
                .IstNeuImClub = True
            }
        End Function

        Private Function FindeLeistungsstufe(benennung As String, liste As LeistungsstufeCollection) As Leistungsstufe
            If String.IsNullOrWhiteSpace(benennung) OrElse liste Is Nothing Then Return Nothing
            Return liste.FirstOrDefault(Function(ls) String.Equals(ls.Benennung, benennung.Trim(), StringComparison.OrdinalIgnoreCase))
        End Function

        Private Function ZeigeDateiAuswahlDialog(Optional initialDirectory As String = Nothing) As String
            Dim ofd As New OpenFileDialog With {
                .InitialDirectory = If(initialDirectory, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)),
                .Filter = "Excel Dateien (*.xlsx, *.xls)|*.xlsx;*.xls|CSV (Trennzeichen-getrennt) (*.csv)|*.csv",
                .FilterIndex = 1,
                .RestoreDirectory = True
            }

            If ofd.ShowDialog() = True Then
                Return ofd.FileName
            Else
                Return Nothing
            End If
        End Function

        ' Platzhalter für die Methoden, die Excel/CSV lesen (müssen implementiert werden)
        Private Function LeseTrainerAusDatasetKI(pfad As String) As List(Of DataImport.Trainer)
            ' TODO: Implementierung der Excel/CSV-Lesemethode
            Throw New NotImplementedException("LeseTrainerAusDatasetKI muss noch implementiert werden.")
        End Function

        Private Function LeseTeilnehmerAusDatasetKI(pfad As String) As List(Of DataImport.Teilnehmer)
            ' TODO: Implementierung der Excel/CSV-Lesemethode
            Throw New NotImplementedException("LeseTeilnehmerAusDatasetKI muss noch implementiert werden.")
        End Function

#End Region

#Region "IDisposable"
        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not _disposed Then
                If disposing Then
                    ' Managed Ressourcen freigeben
                End If
                _disposed = True
            End If
        End Sub
#End Region

    End Class

    ''' <summary>
    ''' Ergebnis eines Importvorgangs.
    ''' </summary>
    Public Class ImportErgebnis
        Public Property Abgebrochen As Boolean
        Public Property Fehler As String
        Public Property NeuHinzugefuegt As Integer
        Public Property Aktualisiert As Integer
        Public Property Rueckkehrer As Integer
        Public Property NeueTrainer As List(Of Trainer)
        Public Property NeueTeilnehmer As List(Of Teilnehmer)
        Public Property NichtImportiert As List(Of Object) ' Trainer oder Teilnehmer

        Public ReadOnly Property Erfolgreich As Boolean
            Get
                Return String.IsNullOrEmpty(Fehler) AndAlso Not Abgebrochen
            End Get
        End Property

        Public Function GetZusammenfassung() As String
            If Abgebrochen Then Return "Import abgebrochen."
            If Not String.IsNullOrEmpty(Fehler) Then Return $"Fehler: {Fehler}"

            Dim sb As New Text.StringBuilder()
            sb.AppendLine($"Import erfolgreich abgeschlossen:")
            sb.AppendLine($"  Neu hinzugefügt: {NeuHinzugefuegt}")
            sb.AppendLine($"  Aktualisiert: {Aktualisiert}")
            If Rueckkehrer > 0 Then sb.AppendLine($"  Rückkehrer: {Rueckkehrer}")
            If NichtImportiert IsNot Nothing AndAlso NichtImportiert.Count > 0 Then
                sb.AppendLine($"  Nicht im Import: {NichtImportiert.Count}")
            End If
            Return sb.ToString()
        End Function
    End Class

End Namespace
