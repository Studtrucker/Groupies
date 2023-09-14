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

    Public Function erstelleGruppen(Anzahl As Integer, Levelliste As LevelCollection) As SkikursgruppeCollection
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

    Private Function erstelle5Gruppen(Levelliste As LevelCollection) As SkikursgruppeCollection

        Dim GA1 = New Skikursgruppe With {
            .Gruppenname = "Anfänger",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(0),
            .Gruppenlevel = Levelliste.Item(0)}
        Dim GF1 = New Skikursgruppe With {
            .Gruppenname = "Fortgeschrittene",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(1),
            .Gruppenlevel = Levelliste.Item(1)}
        Dim GG1 = New Skikursgruppe With {
            .Gruppenname = "Genießer",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(2),
            .Gruppenlevel = Levelliste.Item(2)}

        Dim GK1 = New Skikursgruppe With {
            .Gruppenname = "Könner",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(3),
            .Gruppenlevel = Levelliste.Item(3)}

        Dim GE1 = New Skikursgruppe With {
            .Gruppenname = "Experten",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(4),
            .Gruppenlevel = Levelliste.Item(4)}

        Return New SkikursgruppeCollection From {GA1, GF1, GG1, GK1, GE1}

    End Function

    Private Function erstelle10Gruppen(Levelliste As LevelCollection) As SkikursgruppeCollection

        Dim GA1 = New Skikursgruppe With {
            .Gruppenname = "Anfänger",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(0),
            .Gruppenlevel = Levelliste.Item(0)}

        Dim GF1 = New Skikursgruppe With {
            .Gruppenname = "Fortgeschrittene 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(1),
            .Gruppenlevel = Levelliste.Item(1)}
        Dim GF2 = New Skikursgruppe With {
            .Gruppenname = "Fortgeschrittene 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(2),
            .Gruppenlevel = Levelliste.Item(1)}

        Dim GG1 = New Skikursgruppe With {
            .Gruppenname = "Genießer 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(3),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG2 = New Skikursgruppe With {
            .Gruppenname = "Genießer 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(4),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG3 = New Skikursgruppe With {
            .Gruppenname = "Genießer 3",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(5),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG4 = New Skikursgruppe With {
            .Gruppenname = "Genießer 4",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(6),
            .Gruppenlevel = Levelliste.Item(2)}

        Dim GK1 = New Skikursgruppe With {
            .Gruppenname = "Könner 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(7),
            .Gruppenlevel = Levelliste.Item(3)}
        Dim GK2 = New Skikursgruppe With {
            .Gruppenname = "Könner 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(8),
            .Gruppenlevel = Levelliste.Item(3)}

        Dim GE1 = New Skikursgruppe With {
            .Gruppenname = "Experten",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(9),
            .Gruppenlevel = Levelliste.Item(4)}

        Return New SkikursgruppeCollection From {GA1, GF1, GF2, GG1, GG2, GG3, GG4, GK1, GK2, GE1}
    End Function

    Private Function erstelle15Gruppen(Levelliste As LevelCollection) As SkikursgruppeCollection

        Dim GA1 = New Skikursgruppe With {
            .Gruppenname = "Anfänger 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(0),
            .Gruppenlevel = Levelliste.Item(0)}
        Dim GA2 = New Skikursgruppe With {
            .Gruppenname = "Anfänger2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(1),
            .Gruppenlevel = Levelliste.Item(0)}

        Dim GF1 = New Skikursgruppe With {
            .Gruppenname = "Fortgeschrittene 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(2),
            .Gruppenlevel = Levelliste.Item(1)}
        Dim GF2 = New Skikursgruppe With {
            .Gruppenname = "Fortgeschrittene 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(3),
            .Gruppenlevel = Levelliste.Item(1)}
        Dim GF3 = New Skikursgruppe With {
            .Gruppenname = "Fortgeschrittene 3",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(4),
            .Gruppenlevel = Levelliste.Item(1)}

        Dim GG1 = New Skikursgruppe With {
            .Gruppenname = "Genießer 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(5),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG2 = New Skikursgruppe With {
            .Gruppenname = "Genießer 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(6),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG3 = New Skikursgruppe With {
            .Gruppenname = "Genießer 3",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(7),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG4 = New Skikursgruppe With {
            .Gruppenname = "Genießer 4",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(8),
            .Gruppenlevel = Levelliste.Item(2)}
        Dim GG5 = New Skikursgruppe With {
            .Gruppenname = "Genießer 5",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(9),
            .Gruppenlevel = Levelliste.Item(2)}

        Dim GK1 = New Skikursgruppe With {
            .Gruppenname = "Könner 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(10),
            .Gruppenlevel = Levelliste.Item(3)}
        Dim GK2 = New Skikursgruppe With {
            .Gruppenname = "Könner 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(11),
            .Gruppenlevel = Levelliste.Item(3)}
        Dim GK3 = New Skikursgruppe With {
            .Gruppenname = "Könner 3",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(12),
            .Gruppenlevel = Levelliste.Item(3)}

        Dim GE1 = New Skikursgruppe With {
            .Gruppenname = "Experten 1",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(13),
            .Gruppenlevel = Levelliste.Item(4)}
        Dim GE2 = New Skikursgruppe With {
            .Gruppenname = "Experten 2",
            .AngezeigterGruppenname = AngezeigterGruppenname.Item(14),
            .Gruppenlevel = Levelliste.Item(4)}

        Return New SkikursgruppeCollection From {GA1, GA2, GF1, GF2, GF3, GG1, GG2, GG3, GG4, GG5, GK1, GK2, GK3, GE1, GE2}

    End Function

    Private Function AngezeigterGruppenname() As List(Of String)
        Return New List(Of String) From {"Zugspitze", "Großglockner", "Wildspitze", "Zuckerhütl", "Matterhorn", "K2", "Everest", "Fernau", "Finsteraarhorn", "Piz Permina", "Hochkönig", "Hoher Dachstein", "Marmolata", "Monte Viso", "Ortler"}
    End Function

End Module
