Imports System.ComponentModel
Imports Groupies.Entities
Imports Groupies.Controller.AppController

Public Class TeilnehmerSuchErgebnis

    Private _teilnehmerCollectionView As ICollectionView

    Public Sub New()

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.
        Dim liste As New List(Of Object)

        Dim Teilnehmerliste As New List(Of Object)

        For Each TnL In CurrentClub.Gruppenliste
            Teilnehmerliste.AddRange(TnL.Mitgliederliste.Select(Function(Tn) New With {.Teilnehmer = Tn, .Gruppe = TnL}))
        Next


        _teilnehmerCollectionView = New ListCollectionView(Teilnehmerliste)
        If _teilnehmerCollectionView.CanSort Then
            _teilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Nachname", ListSortDirection.Ascending))
            _teilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Vorname", ListSortDirection.Ascending))
            _teilnehmerCollectionView.SortDescriptions.Add(New SortDescription("Sortierung", ListSortDirection.Ascending))
        End If
        DataContext = _teilnehmerCollectionView

        'Dim geordneteliste = Teilnehmerliste.OrderBy(Function(x) x.Teilnehmer.Nachname).ToList.Select(Function(y) New With {y.Teilnehmer.Vorname, y.Teilnehmer.Nachname, .Gruppe = y.Gruppe.Benennung, y.Gruppe.Sortierung})
        'DataContext = geordneteliste

    End Sub

    Private Sub sort()


    End Sub


End Class
