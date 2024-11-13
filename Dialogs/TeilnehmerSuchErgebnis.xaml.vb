
Imports Groupies.Entities

Public Class TeilnehmerSuchErgebnis

    Public Sub New(Suchergebnis As GruppeCollection)

        ' Dieser Aufruf ist für den Designer erforderlich.
        InitializeComponent()

        ' Fügen Sie Initialisierungen nach dem InitializeComponent()-Aufruf hinzu.

        'Dim x = Suchergebnis.ToList.Select(Function(Gr) Gr.Mitgliederliste.Select(Function(Tn) New With {.TeilnehmerID = Tn.TeilnehmerID, .Name = Tn.VorUndNachname, .GruppenID = Gr.GruppenID, .GruppenName = Gr.Benennung, .Trainer = Gr.Trainer.VorUndNachname, .Gruppenstufe = Gr.Leistungsstufe.Benennung}))

        Dim l As New List(Of Object)
        Suchergebnis.ToList.ForEach(Sub(Gr) Gr.Mitgliederliste.ToList.ForEach(Sub(Tn) l.Add(New With {.Name = Tn.VorUndNachname, .GruppenName = Gr.Benennung})))
        Dim a = New With {.Name = "Andreas", .Nachname = "Studtrucker"}
        Dim r = New With {.Name = "Rainer", .Nachname = "Studtrucker"}

        Dim x As List(Of Object)
        x = New List(Of Object)
        x.AddRange({a, r})

        DataContext = l

    End Sub



End Class
