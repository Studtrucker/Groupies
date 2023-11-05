Imports System.Text
Imports Skischule.Entities

Namespace DataService

    Module CreateDefaultService

#Region "Fields"
        Private _levelCollection As LevelCollection
        Private _CountOfGroups As Dictionary(Of Level, Integer)
#End Region

#Region "Public"

        Public Function CreateLevels() As LevelCollection

            Dim Anfaenger = New Level With {
                .LevelNaming = "Anfänger",
                .LevelID = Guid.NewGuid,
                .LevelDescription = DescriptionAnfaenger()}

            Dim Fortgeschrittener = New Level With {
                .LevelNaming = "Fortgeschritten",
                .LevelDescription = DescriptionFortgeschritten(),
                .LevelID = Guid.NewGuid}

            Dim Geniesser = New Level With {
                .LevelNaming = "Genießer",
                .LevelDescription = DescriptionGeniesser(),
                .LevelID = Guid.NewGuid}

            Dim Koenner = New Level With {
                .LevelNaming = "Könner",
                .LevelDescription = DescriptionKoenner(),
                .LevelID = Guid.NewGuid}

            Dim Experte = New Level With {
                .LevelNaming = "Experte",
                .LevelDescription = DescriptionExperte(),
                .LevelID = Guid.NewGuid}

            _levelCollection = New LevelCollection From {Anfaenger, Fortgeschrittener, Geniesser, Koenner, Experte}
            Return _levelCollection

        End Function

        Public Function CreateGroups(Anzahl As Integer) As GroupCollection
            'Todo: Gruppenlevel einbauen

            Dim uri = New Uri("GroupLevelDistribution", UriKind.Relative)

            Dim dic = ExcelService.ReadLevelDistribution(Anzahl, _levelCollection)

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
            Return New List(Of String) From {"Zugspitze", "Großglockner", "Wildspitze", "Zuckerhütl", "Matterhorn", "K2", "Everest", "Fernau", "Finsteraarhorn", "Piz Permina", "Hochkönig", "Hoher Dachstein", "Marmolata", "Monte Viso", "Ortler"}
        End Function

#End Region

    End Module
End Namespace
