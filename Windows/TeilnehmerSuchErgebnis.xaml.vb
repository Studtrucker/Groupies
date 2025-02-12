Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Controller.AppController

Public Class TeilnehmerSuchErgebnis

    Private _teilnehmerCollectionView As ICollectionView

    Private Property TeilnehmerAnzahl As String
        Get
            Return TeilnehmerAnzahlTextBlock.Text
        End Get
        Set(value As String)
            TeilnehmerAnzahlTextBlock.Text = value
        End Set
    End Property

    Private Property TrainerAnzahl As String
        Get
            Return TrainerAnzahlTextBlock.Text
        End Get
        Set(value As String)
            TrainerAnzahlTextBlock.Text = value
        End Set
    End Property

    Private Property GruppiertesAlter As String
        Get
            Return GruppiertesAlterTextBlock.Text
        End Get
        Set(value As String)
            GruppiertesAlterTextBlock.Text = value
        End Set
    End Property

    Private Property GruppierteLeistungsstufe As String
        Get
            Return GruppierteLeistungsstufeTextBlock.Text
        End Get
        Set(value As String)
            GruppierteLeistungsstufeTextBlock.Text = value
        End Set
    End Property

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        TeilnehmerAnzahl = AktuellerClub.AlleTeilnehmer.Count
        TrainerAnzahl = AktuellerClub.AlleTrainer.Count
        Dim x = AktuellerClub.AlleTeilnehmer.OrderByDescending(Function(Tn) Tn.Leistungsstand.Sortierung).GroupBy(Function(Tn) Tn.Leistungsstand.Benennung)
        Dim z = AktuellerClub.AlleTeilnehmer.OrderByDescending(Function(Tn) Tn.Alter).GroupBy(Function(Tn) Tn.Alter)
        Dim y As New System.Text.StringBuilder

        For Each Stufengruppe In x
            y.AppendLine($"{Stufengruppe.Count} Teilnehmer sind {Stufengruppe.Key}")
        Next
        GruppierteLeistungsstufe = y.ToString
        y.Clear()

        For Each Altersgruppe In z
            y.AppendLine($"{Altersgruppe.Count} Teilnehmer sind {Altersgruppe.Key} Jahre alt")
        Next
        GruppiertesAlter = y.ToString
        y.Clear()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        'Dim liste As New List(Of Object)

        ' in diese Teilnehmerliste kann ein anonymer Typ eingefügt werden
        Dim Teilnehmerliste As New List(Of Object)

        ' Die Teilnehmer und Gruppeninfo aus den einzelen Gruppen
        For Each TnL In AktuellerClub.Gruppenliste
            Teilnehmerliste.AddRange(TnL.Mitgliederliste.Select(Function(Tn) New With {.Teilnehmer = Tn, .Gruppe = TnL}))
        Next

        ' Noch nicht eingeteilte Teilnehmer
        For Each Tn In AktuellerClub.GruppenloseTeilnehmer
            Teilnehmerliste.Add(New With {.Teilnehmer = Tn, .Gruppe = Nothing})
        Next

        ' Alles in ein CollectionView
        _teilnehmerCollectionView = New ListCollectionView(Teilnehmerliste)
        ' Initiale Sortierung
        If _teilnehmerCollectionView.CanSort Then
            _teilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Teilnehmer.Nachname", ListSortDirection.Ascending))
            _teilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Teilnehmer.Vorname", ListSortDirection.Ascending))
            _teilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Gruppe.Sortierung", ListSortDirection.Ascending))
        End If
        ' Kann ein Filter eingebaut werden
        If _teilnehmerCollectionView.CanFilter Then

        End If
        DataContext = _teilnehmerCollectionView

    End Sub

End Class
