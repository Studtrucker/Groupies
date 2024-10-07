Imports System.Text
Imports Groupies.Entities

Namespace Services

    Module PresetService

#Region "Fields"
        Private _levelCollection As LeistungsstufeCollection
        Private ReadOnly _CountOfGroups As Dictionary(Of Leistungsstufe, Integer)
#End Region

#Region "Public"
        'C:\Users\studt_era90oc\Source\Repos\Skischule\Services\GroupLevelDistribution.xlsx
        Public Function StandardLeistungsstufenErstellen() As LeistungsstufeCollection

            Dim Anfaenger = New Leistungsstufe("Anfänger") With {
                .Sortierung = "010",
                .LeistungsstufeID = Guid.NewGuid,
                .Faehigkeiten = SkillsAnfaenger(),
                .Beschreibung = DescriptionAnfaenger()}

            Dim Fortgeschrittener = New Leistungsstufe("Fortgeschritten") With {
                .Sortierung = "020",
                .Beschreibung = DescriptionFortgeschritten(),
                .Faehigkeiten = SkillsFortgeschritten(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Geniesser = New Leistungsstufe("Genießer") With {
                .Sortierung = "030",
                .Beschreibung = DescriptionGeniesser(),
                .Faehigkeiten = SkillsGeniesser(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Koenner = New Leistungsstufe("Könner") With {
                .Sortierung = "040",
                .Beschreibung = DescriptionKoenner(),
                .Faehigkeiten = SkillsKoenner(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Experte = New Leistungsstufe("Experte") With {
                .Sortierung = "050",
                .Beschreibung = DescriptionExperte(),
                .Faehigkeiten = SkillsExperte(),
                .LeistungsstufeID = Guid.NewGuid}

            _levelCollection = New LeistungsstufeCollection From {Anfaenger, Fortgeschrittener, Geniesser, Koenner, Experte}
            Return _levelCollection

        End Function

        Public Function StandardGruppenErstellen(AnzahlGruppen As Integer) As GruppeCollection

            Dim groupCol = New GruppeCollection

            Dim IndexGruppenName As Integer
            For i = 0 To AnzahlGruppen - 1
                groupCol.Add(New Gruppe("Genießer", 3) With {
                        .AusgabeTeilnehmerinfo = GroupPrintNames.Item(IndexGruppenName),
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
            Dim Namen = New Dictionary(Of Integer, String)
            Namen.Add(10, "Zugspitze")
            Namen.Add(20, "Großglockner")
            Namen.Add(30, "Wildspitze")
            Namen.Add(40, "Zuckerhütl")
            Namen.Add(50, "K2")
            Namen.Add(60, "Mount Everest")
            Namen.Add(70, "Fernau")
            Namen.Add(80, "Finsteraarhorn")
            Namen.Add(90, "Piz Permina")
            Namen.Add(100, "Hochkönig")
            Namen.Add(110, "Hoher Dachstein")
            Namen.Add(120, "Marmolata")
            Namen.Add(130, "Monte Viso")
            Namen.Add(140, "Ortler")
            Namen.Add(150, "Matterhorn")
            Return Namen
        End Function

        Private Function GroupPrintNames() As List(Of String)
            Return New List(Of String) From {"Zugspitze", "Großglockner", "Wildspitze", "Zuckerhütl", "Matterhorn", "K2",
                "Mount Everest", "Fernau", "Finsteraarhorn", "Piz Permina", "Hochkönig", "Hoher Dachstein", "Marmolata",
                "Monte Viso", "Ortler"}
        End Function

        Private Function GroupSorting() As List(Of String)
            Return New List(Of String) From {"010", "020", "030", "040", "050", "060", "070", "080", "090", "100", "110", "120", "130", "140", "150"}
        End Function

#End Region

    End Module

End Namespace
