Imports System.Text
Imports Groupies.Entities
Imports Groupies.Controller

Namespace Services

    Module TemplateService

#Region "Fields"
        Private _levelCollection As LeistungsstufeCollection
        Private ReadOnly _CountOfGroups As Dictionary(Of Leistungsstufe, Integer)
#End Region

#Region "Public"
        'C:\Users\studt_era90oc\Source\Repos\Skischule\Services\GroupLevelDistribution.xlsx
        Public Function StandardLeistungsstufenErstellen() As LeistungsstufeCollection

            Dim Empty = New Leistungsstufe() With {
                .Benennung = String.Empty,
                .Sortierung = -1,
                .LeistungsstufeID = Guid.Empty,
                .Faehigkeiten = Nothing,
                .Beschreibung = String.Empty}

            Dim Anfaenger = New Leistungsstufe() With {
                .Benennung = "Anfänger",
                .Sortierung = 10,
                .LeistungsstufeID = Guid.NewGuid,
                .Faehigkeiten = SkillsAnfaenger(),
                .Beschreibung = DescriptionAnfaenger()}

            Dim Fortgeschrittener = New Leistungsstufe() With {
                .Benennung = "Fortgeschritten",
                .Sortierung = 20,
                .Beschreibung = DescriptionFortgeschritten(),
                .Faehigkeiten = SkillsFortgeschritten(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Geniesser = New Leistungsstufe("") With {
                .Benennung = "Genießer",
                .Sortierung = 30,
                .Beschreibung = DescriptionGeniesser(),
                .Faehigkeiten = SkillsGeniesser(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Koenner = New Leistungsstufe("") With {
                .Benennung = "Könner",
                .Sortierung = 40,
                .Beschreibung = DescriptionKoenner(),
                .Faehigkeiten = SkillsKoenner(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Experte = New Leistungsstufe("") With {
                .Benennung = "Experte",
                .Sortierung = 50,
                .Beschreibung = DescriptionExperte(),
                .Faehigkeiten = SkillsExperte(),
                .LeistungsstufeID = Guid.NewGuid}

            _levelCollection = New LeistungsstufeCollection From {Empty, Anfaenger, Fortgeschrittener, Geniesser, Koenner, Experte}
            Return _levelCollection

        End Function

        Public Function StandardEinteilungenErstellen() As EinteilungCollection
            Dim einteilungen = New EinteilungCollection From {
                New Einteilung With {
                    .Benennung = "Tag 1",
                    .Sortierung = 10,
                    .Gruppenliste = Nothing}}
            Return einteilungen
        End Function

        Public Function StandardFaehigkeitenErstellen() As FaehigkeitCollection
            Dim faehigkeiten = New FaehigkeitCollection From {
                New Faehigkeit(String.Empty, -1) With {
                    .Beschreibung = String.Empty},
                New Faehigkeit("Technik: Kurvensteuerung", 100) With {
                    .Beschreibung = DescriptionAnfaenger()},
                New Faehigkeit("Technik: Belastung", 110) With {
                    .Beschreibung = DescriptionAnfaenger()},
                New Faehigkeit("Technik: Stockeinsatz", 120) With {
                    .Beschreibung = DescriptionAnfaenger()},
                New Faehigkeit("Technik: Carving", 130) With {
                    .Beschreibung = DescriptionAnfaenger()},
                New Faehigkeit("Technik: Gedriftet", 140) With {
                    .Beschreibung = DescriptionAnfaenger()},
                New Faehigkeit("Technik: Position", 150) With {
                    .Beschreibung = DescriptionAnfaenger()},
                New Faehigkeit("Koordination: Gleichgewicht", 200) With {
                    .Beschreibung = DescriptionFortgeschritten()},
                New Faehigkeit("Koordination: Rhythmusgefühl", 210) With {
                    .Beschreibung = DescriptionFortgeschritten()},
                New Faehigkeit("Koordination: Reaktionsfähigkeit", 220) With {
                    .Beschreibung = DescriptionFortgeschritten()},
                New Faehigkeit("Koordination: Bewegungskopplung", 230) With {
                    .Beschreibung = DescriptionFortgeschritten()},
                New Faehigkeit("Koordination: Umstellungsfähigkeit", 240) With {
                    .Beschreibung = DescriptionFortgeschritten()},
                New Faehigkeit("Physisch: Bein- und Rumpfkraft", 300) With {
                    .Beschreibung = DescriptionGeniesser()},
                New Faehigkeit("Physisch: Schnelligkeit und Explosivität", 310) With {
                    .Beschreibung = DescriptionGeniesser()},
                New Faehigkeit("Physisch: Ausdauer", 320) With {
                    .Beschreibung = DescriptionGeniesser()},
                New Faehigkeit("Physisch: Beweglichkeit", 330) With {
                    .Beschreibung = DescriptionGeniesser()},
                New Faehigkeit("Taktisch-Analytisch: Linienwahl", 400) With {
                    .Beschreibung = DescriptionKoenner()},
                New Faehigkeit("Taktisch-analytische: Anpassung an Schneebedingungen", 410) With {
                    .Beschreibung = DescriptionExperte()},
                New Faehigkeit("Taktisch-analytische: Geländeeinschätzung", 420) With {
                    .Beschreibung = DescriptionExperte()},
                New Faehigkeit("Taktisch-analytische: Tempodosierung", 430) With {
                    .Beschreibung = DescriptionExperte()}}
            Return faehigkeiten
        End Function

        Public Function StandardGruppenErstellen(AnzahlGruppen As Integer) As GruppeCollection

            Dim groupCol = New GruppeCollection

            Dim IndexGruppenName As Integer
            For i = 0 To AnzahlGruppen - 1
                'groupCol.Add(New Gruppe("Genießer", 3) With {
                '        .Benennung = GroupPrintNames.Item(IndexGruppenName),
                '        .Sortierung = GroupSorting.Item(IndexGruppenName),
                '        .Leistungsstufe = AppController.AktuellerClub.AlleLeistungsstufen.Where(Function(L) L.Sortierung = -1).Single})
                groupCol.Add(New Gruppe("Genießer", 3) With {
                        .Benennung = GroupPrintNames.Item(IndexGruppenName),
                        .Sortierung = GroupSorting.Item(IndexGruppenName)})

                IndexGruppenName += 1
            Next

            Return groupCol

        End Function

#End Region

#Region "Private"

        Private Function SkillsAnfaenger() As FaehigkeitCollection

            Dim sc = New FaehigkeitCollection From {
                New Faehigkeit("Schneepflug", 110) With {
                   .Beschreibung = "Stoppen auf flachem Gelände"},
                   New Faehigkeit("Schneepflug", 120) With {
                   .Beschreibung = "Kurven fahren auf flachem Gelände"}}

            Return sc

        End Function

        Private Function SkillsFortgeschritten() As FaehigkeitCollection

            Dim sc = New FaehigkeitCollection From {
                New Faehigkeit("Skiführung", 210) With {
                   .Beschreibung = "Erste Kurven mit parallelen Skiern"},
                New Faehigkeit("Blaue Piste", 220) With {
                   .Beschreibung = "Sicheres Befahren"},
                New Faehigkeit("Rote Piste", 230) With {
                   .Beschreibung = "Sicheres Befahren"},
                New Faehigkeit("Schwarze Piste", 240) With {
                   .Beschreibung = "Erste Erfahrungen"},
                New Faehigkeit("Gelände", 250) With {
                   .Beschreibung = "Erste Erfahrungen"}}

            Return sc

        End Function

        Private Function SkillsGeniesser() As FaehigkeitCollection

            Dim sc = New FaehigkeitCollection From {
                New Faehigkeit("Blaue Piste", 310) With {
                   .Beschreibung = "Zügiges und sicheres Befahren, stabile Grundposition"},
                New Faehigkeit("Rote/schwarze Piste", 320) With {
                   .Beschreibung = "Sicheres Befahren bei geringem Tempo"},
                New Faehigkeit("Gelände", 330) With {
                   .Beschreibung = "Weitgehend sicheres Befahren in flachem Gelände"}}

            Return sc

        End Function

        Private Function SkillsKoenner() As FaehigkeitCollection

            Dim sc = New FaehigkeitCollection From {
                New Faehigkeit("Blaue Piste", 410) With {
                   .Beschreibung = "Kurze und mittlere Radien bei regulierender Grundposition, rhythmisch und tempokontrolliert"},
                New Faehigkeit("Rote Piste", 420) With {
                   .Beschreibung = "Sicheres Befahren bei zügigem Tempo"},
                New Faehigkeit("Schwarze Piste", 430) With {
                   .Beschreibung = "Sicheres Befahren bei zügigem Tempo"},
                New Faehigkeit("Gelände", 440) With {
                   .Beschreibung = "Sicheres Bewegen in flachem Gelände bei regulierender Grundposition"}}

            Return sc

        End Function

        Private Function SkillsExperte() As FaehigkeitCollection

            Dim sc = New FaehigkeitCollection From {
                New Faehigkeit("Blaue Piste", 510) With {
                   .Beschreibung = "Fahren einer geführten Kurve bei kurzem und mittlerem Radius unter Erfüllung der Grundmerkmale"},
                New Faehigkeit("Rote Piste", 520) With {
                   .Beschreibung = "Mittlere und kurze Radien, tempokontrolliert, fließend, rhythmisch"},
                New Faehigkeit("Schwarze Piste", 530) With {
                   .Beschreibung = "Mittlere und kurze Radien, tempokontrolliert, fließend, rhythmisch"},
                New Faehigkeit("Gelände", 540) With {
                   .Beschreibung = "Sicheres Bewegen in mittelsteilem Gelände bei regulierender Grundposition"}}

            Return sc

        End Function

        Private Function DescriptionUnbekannt() As String

            Dim sb = New StringBuilder
            sb.AppendLine("Der Teilnehmer ist skifahrerisch unbekannt.")
            sb.AppendLine("Es liegen keine Informationen über die Leistungsstärke vor.")
            Return sb.ToString

        End Function

        Private Function DescriptionAnfaenger() As String

            Dim sb = New StringBuilder
            sb.AppendLine("Das erste Mal auf Skiern oder")
            sb.AppendLine("1-2 Tage Skischule")
            Return sb.ToString

        End Function

        Private Function DescriptionFortgeschritten() As String

            Dim sb = New StringBuilder
            sb.AppendLine("Blaue Piste:")
            sb.AppendLine("Sicheres Befahren")
            sb.AppendLine("Rote/schwarze Piste:")
            sb.AppendLine("Weitgehend sicheres Befahren")
            sb.AppendLine("Gelände:")
            sb.AppendLine("Erste Erfahrungen")
            Return sb.ToString

        End Function

        Private Function DescriptionGeniesser() As String

            Dim sb = New StringBuilder
            sb.AppendLine("Blaue Piste:")
            sb.AppendLine("Zügiges und sicheres Befahren, stabile Grundposition")
            sb.AppendLine("Rote/schwarze Piste:")
            sb.AppendLine("Sicheres Befahren bei geringem Tempo")
            sb.AppendLine("Gelände:")
            sb.AppendLine("weitgehend sicheres Befahren in flachem Gelände")
            Return sb.ToString

        End Function

        Private Function DescriptionKoenner() As String

            Dim sb = New StringBuilder
            sb.AppendLine("Blaue Piste:")
            sb.AppendLine("Kurze und mittlere Radien bei regulierender Grundposition, rhytmisch und tempokontrolliert")
            sb.AppendLine("Rote/schwarze Piste:")
            sb.AppendLine("Sicheres Befahren bei zügigem Tempo")
            sb.AppendLine("Gelände:")
            sb.AppendLine("Sicheres Bewegen in flachem Gelände bei regulierender Grundposition")
            Return sb.ToString

        End Function

        Private Function DescriptionExperte() As String

            Dim sb = New StringBuilder
            sb.AppendLine("Blaue Piste:")
            sb.AppendLine("Fahren einer geführten Kurve bei kurzem und mittlerem Radius unter Erfüllung der Grundmerkmale")
            sb.AppendLine("Rote/schwarze Piste:")
            sb.AppendLine("Mittlere und kurze Radien, tempokontrolliert, fließend, rhythmisch")
            sb.AppendLine("Gelände:")
            sb.AppendLine("Sicheres Bewegen in mittelsteilem Gelände bei regulierender Grundposition")
            Return sb.ToString

        End Function

        Private Function GruppenAusgabeNamenUndSortierung() As Dictionary(Of Integer, String)
            Dim Namen = New Dictionary(Of Integer, String) From {
                {10, "Zugspitze 2962"},
                {20, "Watzmann 2712"},
                {30, "Kreuzspitze 2656"},
                {40, "Hochwanner 2744"},
                {50, "Höllentalspitze 2743"},
                {60, "Alpenspitze 2751"},
                {70, "Falknerhorn 2655"},
                {80, "Brettfallspitze 2404"},
                {90, "Mont Blanc 4808"},
                {100, "Dufourspitze 4634"},
                {110, "Dom 4545"},
                {120, "Lyskamm 4527"},
                {130, "Weisshorn 4506"},
                {140, "Täschhorn 4491"},
                {150, "Matterhorn 4478"}}
            Return Namen
        End Function

        Private Function GruppenBennungen() As List(Of String)
            Return New List(Of String) From {"Skigruppe 1", "Skigruppe 2", "Skigruppe 3", "Skigruppe 4", "Skigruppe 5", "Skigruppe 6",
                "Skigruppe 7", "Skigruppe 8", "Skigruppe 9", "Skigruppe 10", "Skigruppe 11", "Skigruppe 12", "Skigruppe 13",
                "Skigruppe 14", "Skigruppe 15"}
        End Function

        Private Function GroupPrintNames() As List(Of String)
            Return New List(Of String) From {
                "Zugspitze 2962",
                "Watzmann 2712",
                "Kreuzspitze 2656",
                "Hochwanner 2744",
                "Höllentalspitze 2743",
                "Alpenspitze 2751",
                "Falknerhorn 2655",
                "Brettfallspitze 2404",
                "Mont Blanc 4808",
                "Dufourspitze 4634",
                "Dom 4545",
                "Lyskamm 4527",
                "Weisshorn 4506",
                "Täschhorn 4491",
                "Matterhorn 4478"}
        End Function

        Private Function GroupSorting() As List(Of Integer)
            Return New List(Of Integer) From {10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150}
        End Function

#End Region

    End Module

End Namespace
