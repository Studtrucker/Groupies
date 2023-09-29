Imports System.Text
Imports Skischule.Entities

Module Standardelemente

    Public Function erstelleLevels() As LevelCollection

        Dim Anfaenger = New Level With {
            .Benennung = "Anfänger",
            .LevelID = Guid.NewGuid,
            .Beschreibung = BeschreibungNeuling()}

        Dim Fortgeschrittener = New Level With {
            .Benennung = "Fortgeschrittener",
            .Beschreibung = BeschreibungFortgeschritten(),
            .LevelID = Guid.NewGuid}

        Dim Geniesser = New Level With {
            .Benennung = "Genießer",
            .Beschreibung = BeschreibungGeniesser(),
            .LevelID = Guid.NewGuid}

        Dim Koenner = New Level With {
            .Benennung = "Könner",
            .Beschreibung = BeschreibungKoenner(),
            .LevelID = Guid.NewGuid}

        Dim Experte = New Level With {
            .Benennung = "Experte",
            .Beschreibung = BeschreibungExperte(),
            .LevelID = Guid.NewGuid}

        Return New LevelCollection From {Anfaenger, Fortgeschrittener, Geniesser, Koenner, Experte}

    End Function

    Private Function BeschreibungNeuling() As String
        Dim sb = New StringBuilder
        sb.AppendLine("Das erste Mal auf Skiern oder")
        sb.AppendLine("1-2 Tage Skischule")
        Return sb.ToString
    End Function

    Private Function BeschreibungFortgeschritten() As String
        Dim sb = New StringBuilder
        sb.AppendLine("Blaue Piste:")
        sb.AppendLine("Sicheres Befahren")
        sb.AppendLine("Rote/schwarze Piste:")
        sb.AppendLine("Weitgehend sicheres Befahren")
        sb.AppendLine("Gelände:")
        sb.AppendLine("Erste Erfahrungen")
        Return sb.ToString
    End Function
    Private Function BeschreibungGeniesser() As String
        Dim sb = New StringBuilder
        sb.AppendLine("Blaue Piste:")
        sb.AppendLine("Zügiges und sicheres Befahren, stabile Grundposition")
        sb.AppendLine("Rote/schwarze Piste:")
        sb.AppendLine("Sicheres Befahren bei geringem Tempo")
        sb.AppendLine("Gelände:")
        sb.AppendLine("weitgehenmd sicheres Befahren in flachem Gelände")
        Return sb.ToString
    End Function
    Private Function BeschreibungKoenner() As String
        Dim sb = New StringBuilder
        sb.AppendLine("Blaue Piste:")
        sb.AppendLine("Kurze und mittlere Radien bei regulierender Grundposition, rhytmisch und tempokontrolliert")
        sb.AppendLine("Rote/schwarze Piste:")
        sb.AppendLine("Sicheres Befahren bei zügigem Tempo")
        sb.AppendLine("Gelände:")
        sb.AppendLine("Sicheres Bewegen in flachem Gelände bei regulierender Grundposition")
        Return sb.ToString
    End Function
    Private Function BeschreibungExperte() As String
        Dim sb = New StringBuilder
        sb.AppendLine("Blaue Piste:")
        sb.AppendLine("Fahren einer geführten Kurve bei kurzem und mittlerem Radius unter Erfüllung der Grundmerkmale")
        sb.AppendLine("Rote/schwarze Piste:")
        sb.AppendLine("Mittlere und kurze Radien, tempokontrolliert, fließend, rythmisch")
        sb.AppendLine("Gelände:")
        sb.AppendLine("Sicheres Bewegen in mittelsteilem Gelände bei regulierender Grundposition")
        Return sb.ToString
    End Function

    Public Function erstelleGruppen(Anzahl As Integer, Levelliste As LevelCollection) As SkikursCollection
        'Todo: Gruppenlevel einbauen

        If Anzahl = 15 Then
            Return erstelle15Gruppen(Levelliste)
        ElseIf Anzahl = 10 Then
            Return erstelle10Gruppen(Levelliste)
            'ElseIf Anzahl = 5 Then
        Else
            Return erstelle5Gruppen(Levelliste)
        End If

    End Function

    Private Function erstelle5Gruppen(Levelliste As LevelCollection) As SkikursCollection

        Dim GA1 = New Skikurs With {
            .Kurs = "Anfänger",
            .KursPrintName = AngezeigterGruppenname.Item(0),
            .KurslevelID = Levelliste.Item(0).LevelID}
        Dim GF1 = New Skikurs With {
            .Kurs = "Fortgeschrittene",
            .KursPrintName = AngezeigterGruppenname.Item(1),
            .KurslevelID = Levelliste.Item(1).LevelID}
        Dim GG1 = New Skikurs With {
            .Kurs = "Genießer",
            .KursPrintName = AngezeigterGruppenname.Item(2),
            .KurslevelID = Levelliste.Item(2).LevelID}

        Dim GK1 = New Skikurs With {
            .Kurs = "Könner",
            .KursPrintName = AngezeigterGruppenname.Item(3),
            .KurslevelID = Levelliste.Item(3).LevelID}

        Dim GE1 = New Skikurs With {
            .Kurs = "Experten",
            .KursPrintName = AngezeigterGruppenname.Item(4),
            .KurslevelID = Levelliste.Item(4).LevelID}

        Return New SkikursCollection From {GA1, GF1, GG1, GK1, GE1}

    End Function

    Private Function erstelle10Gruppen(Levelliste As LevelCollection) As SkikursCollection

        Dim GA1 = New Skikurs With {
            .Kurs = "Anfänger",
            .KursPrintName = AngezeigterGruppenname.Item(0),
            .KurslevelID = Levelliste.Item(0).LevelID}

        Dim GF1 = New Skikurs With {
            .Kurs = "Fortgeschrittene 1",
            .KursPrintName = AngezeigterGruppenname.Item(1),
            .KurslevelID = Levelliste.Item(1).LevelID}
        Dim GF2 = New Skikurs With {
            .Kurs = "Fortgeschrittene 2",
            .KursPrintName = AngezeigterGruppenname.Item(2),
            .KurslevelID = Levelliste.Item(1).LevelID}

        Dim GG1 = New Skikurs With {
            .Kurs = "Genießer 1",
            .KursPrintName = AngezeigterGruppenname.Item(3),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG2 = New Skikurs With {
            .Kurs = "Genießer 2",
            .KursPrintName = AngezeigterGruppenname.Item(4),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG3 = New Skikurs With {
            .Kurs = "Genießer 3",
            .KursPrintName = AngezeigterGruppenname.Item(5),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG4 = New Skikurs With {
            .Kurs = "Genießer 4",
            .KursPrintName = AngezeigterGruppenname.Item(6),
            .KurslevelID = Levelliste.Item(2).LevelID}

        Dim GK1 = New Skikurs With {
            .Kurs = "Könner 1",
            .KursPrintName = AngezeigterGruppenname.Item(7),
            .KurslevelID = Levelliste.Item(3).LevelID}
        Dim GK2 = New Skikurs With {
            .Kurs = "Könner 2",
            .KursPrintName = AngezeigterGruppenname.Item(8),
            .KurslevelID = Levelliste.Item(3).LevelID}

        Dim GE1 = New Skikurs With {
            .Kurs = "Experten",
            .KursPrintName = AngezeigterGruppenname.Item(9),
            .KurslevelID = Levelliste.Item(4).LevelID}

        Return New SkikursCollection From {GA1, GF1, GF2, GG1, GG2, GG3, GG4, GK1, GK2, GE1}
    End Function

    Private Function erstelle15Gruppen(Levelliste As LevelCollection) As SkikursCollection

        Dim GA1 = New Skikurs With {
            .Kurs = "Anfänger 1",
            .KursPrintName = AngezeigterGruppenname.Item(0),
            .KurslevelID = Levelliste.Item(0).LevelID}
        Dim GA2 = New Skikurs With {
            .Kurs = "Anfänger2",
            .KursPrintName = AngezeigterGruppenname.Item(1),
            .KurslevelID = Levelliste.Item(0).LevelID}

        Dim GF1 = New Skikurs With {
            .Kurs = "Fortgeschrittene 1",
            .KursPrintName = AngezeigterGruppenname.Item(2),
            .KurslevelID = Levelliste.Item(1).LevelID}
        Dim GF2 = New Skikurs With {
            .Kurs = "Fortgeschrittene 2",
            .KursPrintName = AngezeigterGruppenname.Item(3),
            .KurslevelID = Levelliste.Item(1).LevelID}
        Dim GF3 = New Skikurs With {
            .Kurs = "Fortgeschrittene 3",
            .KursPrintName = AngezeigterGruppenname.Item(4),
            .KurslevelID = Levelliste.Item(1).LevelID}

        Dim GG1 = New Skikurs With {
            .Kurs = "Genießer 1",
            .KursPrintName = AngezeigterGruppenname.Item(5),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG2 = New Skikurs With {
            .Kurs = "Genießer 2",
            .KursPrintName = AngezeigterGruppenname.Item(6),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG3 = New Skikurs With {
            .Kurs = "Genießer 3",
            .KursPrintName = AngezeigterGruppenname.Item(7),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG4 = New Skikurs With {
            .Kurs = "Genießer 4",
            .KursPrintName = AngezeigterGruppenname.Item(8),
            .KurslevelID = Levelliste.Item(2).LevelID}
        Dim GG5 = New Skikurs With {
            .Kurs = "Genießer 5",
            .KursPrintName = AngezeigterGruppenname.Item(9),
            .KurslevelID = Levelliste.Item(2).LevelID}

        Dim GK1 = New Skikurs With {
            .Kurs = "Könner 1",
            .KursPrintName = AngezeigterGruppenname.Item(10),
            .KurslevelID = Levelliste.Item(3).LevelID}
        Dim GK2 = New Skikurs With {
            .Kurs = "Könner 2",
            .KursPrintName = AngezeigterGruppenname.Item(11),
            .KurslevelID = Levelliste.Item(3).LevelID}
        Dim GK3 = New Skikurs With {
            .Kurs = "Könner 3",
            .KursPrintName = AngezeigterGruppenname.Item(12),
            .KurslevelID = Levelliste.Item(3).LevelID}

        Dim GE1 = New Skikurs With {
            .Kurs = "Experten 1",
            .KursPrintName = AngezeigterGruppenname.Item(13),
            .KurslevelID = Levelliste.Item(4).LevelID}
        Dim GE2 = New Skikurs With {
            .Kurs = "Experten 2",
            .KursPrintName = AngezeigterGruppenname.Item(14),
            .KurslevelID = Levelliste.Item(4).LevelID}

        Return New SkikursCollection From {GA1, GA2, GF1, GF2, GF3, GG1, GG2, GG3, GG4, GG5, GK1, GK2, GK3, GE1, GE2}

    End Function

    Private Function AngezeigterGruppenname() As List(Of String)
        Return New List(Of String) From {"Zugspitze", "Großglockner", "Wildspitze", "Zuckerhütl", "Matterhorn", "K2", "Everest", "Fernau", "Finsteraarhorn", "Piz Permina", "Hochkönig", "Hoher Dachstein", "Marmolata", "Monte Viso", "Ortler"}
    End Function

End Module
