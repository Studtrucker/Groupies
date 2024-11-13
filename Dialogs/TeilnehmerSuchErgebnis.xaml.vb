
Imports Groupies.Entities

Public Class TeilnehmerSuchErgebnis

    Public Sub New(Suchergebnis As GruppeCollection)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Dim x = Suchergebnis.ToList.Select(Function(Gr) Gr.Mitgliederliste.Select(Function(Tn) New With {.TeilnehmerID = Tn.TeilnehmerID, .Name = Tn.VorUndNachname, .GruppenID = Gr.GruppenID, .GruppenName = Gr.Benennung, .Trainer = Gr.Trainer.VorUndNachname, .Gruppenstufe = Gr.Leistungsstufe.Benennung}))
        Dim x = Suchergebnis.ToList.Select(Function(Gr) Gr.Mitgliederliste.Select(Function(Tn) New With {.Name = Tn.VorUndNachname, .GruppenName = Gr.Benennung})).ToList

        DataContext = x

    End Sub



End Class
