Imports System.Text
Imports Groupies.Entities

Namespace Services

    Module PresetService

#Region "Fields"
        Private _levelCollection As LevelCollection
        Private ReadOnly _CountOfGroups As Dictionary(Of Leistungsstufe, Integer)
#End Region

#Region "Public"
        'C:\Users\studt_era90oc\Source\Repos\Skischule\Services\GroupLevelDistribution.xlsx
        Public Function CreateLevels() As LevelCollection

            Dim Anfaenger = New Leistungsstufe(True) With {
                .SortNumber = "010",
                .Benennung = "Anfänger",
                .LeistungsstufeID = Guid.NewGuid,
                .Faehigkeiten = SkillsAnfaenger(),
                .Beschreibung = DescriptionAnfaenger()}

            Dim Fortgeschrittener = New Leistungsstufe(True) With {
                .SortNumber = "020",
                .Benennung = "Fortgeschritten",
                .Beschreibung = DescriptionFortgeschritten(),
                .Faehigkeiten = SkillsFortgeschritten(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Geniesser = New Leistungsstufe(True) With {
                .SortNumber = "030",
                .Benennung = "Genießer",
                .Beschreibung = DescriptionGeniesser(),
                .Faehigkeiten = SkillsGeniesser(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Koenner = New Leistungsstufe(True) With {
                .SortNumber = "040",
                .Benennung = "Könner",
                .Beschreibung = DescriptionKoenner(),
                .Faehigkeiten = SkillsKoenner(),
                .LeistungsstufeID = Guid.NewGuid}

            Dim Experte = New Leistungsstufe(True) With {
                .SortNumber = "050",
                .Benennung = "Experte",
                .Beschreibung = DescriptionExperte(),
                .Faehigkeiten = SkillsExperte(),
                .LeistungsstufeID = Guid.NewGuid}

            _levelCollection = New LevelCollection From {Anfaenger, Fortgeschrittener, Geniesser, Koenner, Experte}
            Return _levelCollection

        End Function

        Public Function CreateGroups(NumberOfGroups As Integer) As GroupCollection

            Dim groupCol = New GroupCollection

            Dim IndexGruppenName As Integer
            For i = 0 To NumberOfGroups - 1
                groupCol.Add(New Group With {
                        .GroupNaming = "Genießer",
                        .GroupPrintNaming = GroupPrintNames.Item(IndexGruppenName),
                        .GroupSort = GroupSorting.Item(IndexGruppenName)})
                IndexGruppenName += 1
            Next


            Return groupCol

        End Function

#End Region

#Region "Private"

        Private Function SkillsAnfaenger() As SkillCollection

            Dim sc = New SkillCollection From {
                New Faehigkeit With {
                   .Benennung = "Schneepflug",
                   .Sortierung = "110",
                   .Beschreibung = "Stoppen auf flachem Gelände"},
                   New Faehigkeit With {
                   .Benennung = "Schneepflug",
                   .Sortierung = "120",
                   .Beschreibung = "Kurvern fahren auf flachem Gelände"}}

            Return sc

        End Function

        Private Function SkillsFortgeschritten() As SkillCollection

            Dim sc = New SkillCollection From {
                New Faehigkeit With {
                   .Benennung = "Skiführung",
                   .Sortierung = "210",
                   .Beschreibung = "Erste Kurven mit parallelen Skiern"},
                New Faehigkeit With {
                   .Benennung = "Blaue Piste",
                   .Sortierung = "220",
                   .Beschreibung = "Sicheres Befahren"},
                New Faehigkeit With {
                   .Benennung = "Rote Piste",
                   .Sortierung = "230",
                   .Beschreibung = "Sicheres Befahren"},
                New Faehigkeit With {
                   .Benennung = "Schwarze Piste",
                   .Sortierung = "240",
                   .Beschreibung = "Erste Erfahrungen"},
                New Faehigkeit With {
                   .Benennung = "Gelände",
                   .Sortierung = "250",
                   .Beschreibung = "Erste Erfahrungen"}}

            Return sc

        End Function

        Private Function SkillsGeniesser() As SkillCollection

            Dim sc = New SkillCollection From {
                New Faehigkeit With {
                   .Benennung = "Blaue Piste",
                   .Sortierung = "310",
                   .Beschreibung = "Zügiges und sicheres Befahren, stabile Grundposition"},
                New Faehigkeit With {
                   .Benennung = "Rote/schwarze Piste",
                   .Sortierung = "320",
                   .Beschreibung = "Sicheres Befahren bei geringem Tempo"},
                New Faehigkeit With {
                   .Benennung = "Gelände",
                   .Sortierung = "330",
                   .Beschreibung = "Weitgehend sicheres Befahren in flachem Gelände"}}

            Return sc

        End Function

        Private Function SkillsKoenner() As SkillCollection

            Dim sc = New SkillCollection From {
                New Faehigkeit With {
                   .Benennung = "Blaue Piste",
                   .Sortierung = "410",
                   .Beschreibung = "Kurze und mittlere Radien bei regulierender Grundposition, rhytmisch und tempokontrolliert"},
                New Faehigkeit With {
                   .Benennung = "Rote Piste",
                   .Sortierung = "420",
                   .Beschreibung = "Sicheres Befahren bei zügigem Tempo"},
                New Faehigkeit With {
                   .Benennung = "Schwarze Piste",
                   .Sortierung = "430",
                   .Beschreibung = "Sicheres Befahren bei zügigem Tempo"},
                New Faehigkeit With {
                   .Benennung = "Gelände",
                   .Sortierung = "440",
                   .Beschreibung = "Sicheres Bewegen in flachem Gelände bei regulierender Grundposition"}}

            Return sc

        End Function

        Private Function SkillsExperte() As SkillCollection

            Dim sc = New SkillCollection From {
                New Faehigkeit With {
                   .Benennung = "Blaue Piste",
                   .Sortierung = "510",
                   .Beschreibung = "Fahren einer geführten Kurve bei kurzem und mittlerem Radius unter Erfüllung der Grundmerkmale"},
                New Faehigkeit With {
                   .Benennung = "Rote Piste",
                   .Sortierung = "520",
                   .Beschreibung = "Mittlere und kurze Radien, tempokontrolliert, fließend, rythmisch"},
                New Faehigkeit With {
                   .Benennung = "Schwarze Piste",
                   .Sortierung = "530",
                   .Beschreibung = "Mittlere und kurze Radien, tempokontrolliert, fließend, rythmisch"},
                New Faehigkeit With {
                   .Benennung = "Gelände",
                   .Sortierung = "540",
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
            sb.AppendLine("Mittlere und kurze Radien, tempokontrolliert, fließend, rythmisch")
            sb.AppendLine("Gelände:")
            sb.AppendLine("Sicheres Bewegen in mittelsteilem Gelände bei regulierender Grundposition")
            Return sb.ToString

        End Function

        Private Function GroupPrintNames() As List(Of String)
            Return New List(Of String) From {"Zugspitze", "Großglockner", "Wildspitze", "Zuckerhütl", "Matterhorn", "K2", "Mount Everest", "Fernau", "Finsteraarhorn", "Piz Permina", "Hochkönig", "Hoher Dachstein", "Marmolata", "Monte Viso", "Ortler"}
        End Function

        Private Function GroupSorting() As List(Of String)
            Return New List(Of String) From {"010", "020", "030", "040", "050", "060", "070", "080", "090", "100", "110", "120", "130", "140", "150"}
        End Function

#End Region

    End Module

End Namespace
