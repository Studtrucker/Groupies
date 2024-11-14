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
        'Suchergebnis.ToList.ForEach(Sub(Gr) _
        '                                Gr.Mitgliederliste.ToList.
        '                                ForEach(Sub(Tn) _
        '                                            liste.Add(New With {.Vorname = Tn.Vorname,
        '                                                  .Nachname = Tn.Nachname,
        '                                                  .GruppenName = Gr.Benennung,
        '                                                  .Trainer = Gr.Trainer.VorUndNachname,
        '                                                  .Gruppenstufe = Gr.Leistungsstufe.Benennung})))

        'liste.OrderBy(Function(Tn) Tn.Vorname).ToList.OrderBy(Function(Tn) Tn.Nachname)

        'Groupies.Controller.AppController.CurrentClub.AlleTeilnehmer.(

        Dim Teilnehmerliste As New List(Of Object)

        For Each TnL In CurrentClub.Gruppenliste
            Teilnehmerliste.AddRange(TnL.Mitgliederliste.Select(Function(Tn) New With {.Teilnehmer = Tn, .Gruppe = TnL}))
        Next

        Dim geordneteliste = Teilnehmerliste.OrderBy(Function(x) x.Teilnehmer.Nachname).ToList.Select(Function(y) New With {.Vorname = y.Teilnehmer.Vorname, .Nachname = y.Teilnehmer.Nachname, .Gruppe = y.Gruppe.Benennung, .Sortierung = y.Gruppe.Sortierung})

        DataContext = geordneteliste

    End Sub




End Class
