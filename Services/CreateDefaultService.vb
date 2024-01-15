Imports System.Text
Imports Groupies.Entities

Namespace Services

    Module CreateDefaultService

#Region "Fields"
        Private _levelCollection As LevelCollection
        Private ReadOnly _CountOfGroups As Dictionary(Of Level, Integer)
#End Region

#Region "Public"

        Public Function CreateLevels() As LevelCollection

            Dim Anfaenger = New Level(True) With {
                .SortNumber = "010",
                .LevelNaming = "Anfänger",
                .LevelID = Guid.NewGuid,
                .LevelSkills = SkillsAnfaenger(),
                .LevelDescription = DescriptionAnfaenger()}

            Dim Fortgeschrittener = New Level(True) With {
                .SortNumber = "020",
                .LevelNaming = "Fortgeschritten",
                .LevelDescription = DescriptionFortgeschritten(),
                .LevelSkills = SkillsFortgeschritten,
                .LevelID = Guid.NewGuid}

            Dim Geniesser = New Level(True) With {
                .SortNumber = "030",
                .LevelNaming = "Genießer",
                .LevelDescription = DescriptionGeniesser(),
                .LevelSkills = SkillsGeniesser(),
                .LevelID = Guid.NewGuid}

            Dim Koenner = New Level(True) With {
                .SortNumber = "040",
                .LevelNaming = "Könner",
                .LevelDescription = DescriptionKoenner(),
                .LevelSkills = SkillsKoenner(),
                .LevelID = Guid.NewGuid}

            Dim Experte = New Level(True) With {
                .SortNumber = "050",
                .LevelNaming = "Experte",
                .LevelDescription = DescriptionExperte(),
                .LevelSkills = SkillsExperte(),
                .LevelID = Guid.NewGuid}

            _levelCollection = New LevelCollection From {Anfaenger, Fortgeschrittener, Geniesser, Koenner, Experte}
            Return _levelCollection

        End Function

        Public Function CreateGroups(Anzahl As Integer) As GroupCollection
            'Todo: Gruppenlevel einbauen

            Dim uri = New Uri("GroupLevelDistribution", UriKind.Relative)

            Dim dic = Services.ReadLevelDistribution(Anzahl, _levelCollection)

            Dim groupCol = New GroupCollection

            Dim IndexGruppenName As Integer
            For Each item In dic
                For i = 0 To item.Value - 1
                    groupCol.Add(New Group With {
                        .GroupNaming = String.Format("{0}{1}", item.Key.LevelNaming, i + 1),
                        .GroupPrintNaming = GroupPrintNames.Item(IndexGruppenName),
                        .GroupLevel = item.Key})
                    IndexGruppenName += 1
                Next
            Next

            Return groupCol

        End Function


#End Region

#Region "Private"

        Private Function SkillsAnfaenger() As SkillCollection

            Dim sc = New SkillCollection From {
                New Skill With {
                   .SkillNaming = "Schneepflug",
                   .SortNumber = "110",
                   .Description = "Stoppen auf flachem Gelände"},
                   New Skill With {
                   .SkillNaming = "Schneepflug",
                   .SortNumber = "120",
                   .Description = "Kurvern fahren auf flachem Gelände"}}

            Return sc

        End Function

        Private Function SkillsFortgeschritten() As SkillCollection

            Dim sc = New SkillCollection From {
                New Skill With {
                   .SkillNaming = "Skiführung",
                   .SortNumber = "210",
                   .Description = "Erste Kurven mit parallelen Skiern"},
                New Skill With {
                   .SkillNaming = "Blaue Piste",
                   .SortNumber = "220",
                   .Description = "Sicheres Befahren"},
                New Skill With {
                   .SkillNaming = "Rote Piste",
                   .SortNumber = "230",
                   .Description = "Sicheres Befahren"},
                New Skill With {
                   .SkillNaming = "Schwarze Piste",
                   .SortNumber = "240",
                   .Description = "Erste Erfahrungen"},
                New Skill With {
                   .SkillNaming = "Gelände",
                   .SortNumber = "250",
                   .Description = "Erste Erfahrungen"}}

            Return sc

        End Function

        Private Function SkillsGeniesser() As SkillCollection

            Dim sc = New SkillCollection From {
                New Skill With {
                   .SkillNaming = "Blaue Piste",
                   .SortNumber = "310",
                   .Description = "Zügiges und sicheres Befahren, stabile Grundposition"},
                New Skill With {
                   .SkillNaming = "Rote/schwarze Piste",
                   .SortNumber = "320",
                   .Description = "Sicheres Befahren bei geringem Tempo"},
                New Skill With {
                   .SkillNaming = "Gelände",
                   .SortNumber = "330",
                   .Description = "Weitgehend sicheres Befahren in flachem Gelände"}}

            Return sc

        End Function

        Private Function SkillsKoenner() As SkillCollection

            Dim sc = New SkillCollection From {
                New Skill With {
                   .SkillNaming = "Blaue Piste",
                   .SortNumber = "410",
                   .Description = "Kurze und mittlere Radien bei regulierender Grundposition, rhytmisch und tempokontrolliert"},
                New Skill With {
                   .SkillNaming = "Rote Piste",
                   .SortNumber = "420",
                   .Description = "Sicheres Befahren bei zügigem Tempo"},
                New Skill With {
                   .SkillNaming = "Schwarze Piste",
                   .SortNumber = "430",
                   .Description = "Sicheres Befahren bei zügigem Tempo"},
                New Skill With {
                   .SkillNaming = "Gelände",
                   .SortNumber = "440",
                   .Description = "Sicheres Bewegen in flachem Gelände bei regulierender Grundposition"}}

            Return sc

        End Function

        Private Function SkillsExperte() As SkillCollection

            Dim sc = New SkillCollection From {
                New Skill With {
                   .SkillNaming = "Blaue Piste",
                   .SortNumber = "510",
                   .Description = "Fahren einer geführten Kurve bei kurzem und mittlerem Radius unter Erfüllung der Grundmerkmale"},
                New Skill With {
                   .SkillNaming = "Rote Piste",
                   .SortNumber = "520",
                   .Description = "Mittlere und kurze Radien, tempokontrolliert, fließend, rythmisch"},
                New Skill With {
                   .SkillNaming = "Schwarze Piste",
                   .SortNumber = "530",
                   .Description = "Mittlere und kurze Radien, tempokontrolliert, fließend, rythmisch"},
                New Skill With {
                   .SkillNaming = "Gelände",
                   .SortNumber = "540",
                   .Description = "Sicheres Bewegen in mittelsteilem Gelände bei regulierender Grundposition"}}

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

#End Region

    End Module

End Namespace
