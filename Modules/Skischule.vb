Imports Skikurs.Entities

Namespace BasicObjects

    Module Skischule

#Region "Fields"

        Private _HatCustomKoennenstufen As Boolean
        Public Property Teilnehmerliste() As TeilnehmerCollection
        Public Property Skikursgruppenliste() As SkikursgruppenCollection
        Public Property Koennenstufenliste() As LevelsCollection

#End Region


        Public ReadOnly Property HatCustomKoennenstufen() As Boolean
            Get
                Return _HatCustomKoennenstufen
            End Get
        End Property

        Public Sub initialisiereFixeListen()
            _skikursgruppenliste = New SkikursgruppenCollection
            _teilnehmerListe = New TeilnehmerCollection
            _Koennenstufenliste = New LevelsCollection
        End Sub

        Public Sub erstelleKoennenstufen()

            Dim Neuling = New Level With {
                .Benennung = "Neuling",
                .LevelID = Guid.NewGuid,
                .Beschreibung = "Das erste Mal auf Skiern oder 1-2 Tage Skischule"}

            Dim Anfaenger = New Level With {
                .Benennung = "Anfänger",
                .Beschreibung = "1-wöchiger Skikurs und kann eigenständig Bremsen, blaue Pisten",
                .LevelID = Guid.NewGuid}

            Dim Fortgeschritten = New Level With {
                .Benennung = "Fortgeschritten",
                .Beschreibung = "Rote Pisten, Kurven, eigenständiges Liftfahren",
                .LevelID = Guid.NewGuid}

            Dim Koenner = New Level With {
                .Benennung = "Könner",
                .Beschreibung = "Paralleles Skifahren, Stockeinsatz, kürzere Radien, schwarze Pisten",
                .LevelID = Guid.NewGuid}

            Dim Topfahrer = New Level With {
                .Benennung = "Topfahrer",
                .Beschreibung = "Jede Piste, jedes Gelände sowie alle Arten Schnee werden sicher gemeistert",
                .LevelID = Guid.NewGuid}

            Koennenstufenliste = New LevelsCollection From {Neuling, Anfaenger, Fortgeschritten, Koenner, Topfahrer}

        End Sub

    End Module

    Module Gruppen
        '.AngezeigteBenennung = "Gruppe Zugspitze",
        '.AngezeigteBenennung = "Gruppe Großglockner",
        '.AngezeigteBenennung = "Gruppe Wildspitze",
        '.AngezeigteBenennung = "Gruppe Zuckerhütl",
        '.AngezeigteBenennung = "Gruppe Matterhorn",
    End Module

End Namespace
