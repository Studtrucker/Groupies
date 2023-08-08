Imports Skireisen.Entities

Namespace BasicObjects

    Module Skischueler


        Private _HatCustomKoennenstufen As Boolean
        Public ReadOnly Property HatCustomKoennenstufen() As Boolean
            Get
                Return _HatCustomKoennenstufen
            End Get
        End Property

        Public Property Koennenstufen As KoennenstufenCollection

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

            _Koennenstufen = New KoennenstufenCollection From {Neuling, Anfaenger, Fortgeschritten, Koenner, Topfahrer}

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
