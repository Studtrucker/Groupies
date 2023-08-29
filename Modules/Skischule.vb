Imports Skikurs.Entities

Namespace BasicObjects

    Module Skischule

#Region "Fields"

        Private _HatCustomKoennenstufen As Boolean
        Public Property Teilnehmerliste() As TeilnehmerCollection
        Public Property Skikursgruppenliste() As SkikursgruppenCollection
        Public Property Koennenstufenliste() As KoennenstufenCollection

#End Region


        Public ReadOnly Property HatCustomKoennenstufen() As Boolean
            Get
                Return _HatCustomKoennenstufen
            End Get
        End Property

        Public Sub initialisiereFixeListen()
            _skikursgruppenliste = New SkikursgruppenCollection
            _teilnehmerListe = New TeilnehmerCollection
            _Koennenstufenliste = New KoennenstufenCollection
        End Sub

        Public Sub erstelleKoennenstufen()

            Dim Neuling = New Koennenstufe With {
                .Benennung = "Neuling",
                .KoennenstufeID = Guid.NewGuid,
                .Beschreibung = "Das erste Mal auf Skiern oder 1-2 Tage Skischule"}

            Dim Anfaenger = New Koennenstufe With {
                .Benennung = "Anfänger",
                .Beschreibung = "1-wöchiger Skikurs und kann eigenständig Bremsen, blaue Pisten",
                .KoennenstufeID = Guid.NewGuid}

            Dim Fortgeschritten = New Koennenstufe With {
                .Benennung = "Fortgeschritten",
                .Beschreibung = "Rote Pisten, Kurven, eigenständiges Liftfahren",
                .KoennenstufeID = Guid.NewGuid}

            Dim Koenner = New Koennenstufe With {
                .Benennung = "Könner",
                .Beschreibung = "Paralleles Skifahren, Stockeinsatz, kürzere Radien, schwarze Pisten",
                .KoennenstufeID = Guid.NewGuid}

            Dim Topfahrer = New Koennenstufe With {
                .Benennung = "Topfahrer",
                .Beschreibung = "Jede Piste, jedes Gelände sowie alle Arten Schnee werden sicher gemeistert",
                .KoennenstufeID = Guid.NewGuid}

            Koennenstufenliste = New KoennenstufenCollection From {Neuling, Anfaenger, Fortgeschritten, Koenner, Topfahrer}

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
