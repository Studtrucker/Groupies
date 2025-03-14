Imports Groupies.Entities

Public Module MappingGeneration2

    Private NeuerClub As AktuelleVersion.Club
    Private _Gruppenliste As GruppeCollection

    Public Function MapSkiClub2Club(Skiclub As Generation2.Club) As AktuelleVersion.Club

        NeuerClub = New AktuelleVersion.Club
        ' Neue Einteilung erstellen
        NeuerClub.Einteilungsliste.Add(New Einteilung With {.Benennung = "Tag1", .Sortierung = 1})

        NeuerClub.Leistungsstufenliste = Skiclub.Leistungsstufenliste
        '' Jede Group aus dem Skiclub mappen und in die Gruppenliste des Clubs hängen
        NeuerClub.Einteilungsliste(0).Gruppenliste = Skiclub.Gruppenliste
        '' Gruppenlose Teilnehmer und Trainer mappen
        NeuerClub.Einteilungsliste(0).GruppenloseTeilnehmer = Skiclub.GruppenloseTeilnehmer
        NeuerClub.Einteilungsliste(0).GruppenloseTrainer = Skiclub.GruppenloseTrainer
        ' Trainer, die bereits in Gruppen eingeteilt wurden, aus den Gruppenlosen entfernen
        'NeuerClub.Einteilungsliste(0).Gruppenliste.ToList.ForEach(Sub(G) NeuerClub.Einteilungsliste(0).GruppenloseTrainer.RemoveByTrainerID(G.Trainer.TrainerID))

        Return NeuerClub

    End Function

End Module
