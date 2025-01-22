Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Controller.AppController

Public Class TeilnehmerSuchErgebnis

    Private _teilnehmerCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        'Dim liste As New List(Of Object)

        ' in diese Teilnehmerliste kann ein anonymer Typ eingefügt werden
        Dim Teilnehmerliste As New List(Of Object)

        ' Die Teilnehmer und Gruppeninfo aus den einzelen Gruppen
        For Each TnL In CurrentClub.Gruppenliste
            Teilnehmerliste.AddRange(TnL.Mitgliederliste.Select(Function(Tn) New With {.Teilnehmer = Tn, .Gruppe = TnL}))
        Next

        ' Noch nicht eingeteilte Teilnehmer
        For Each Tn In CurrentClub.GruppenloseTeilnehmer
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
