Imports Skireisen.Entities

Module StandardKoennenstufen
    Public Sub erstellen()

        Dim Neuling = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Zugspitze",
            .Benennung = "Neuling",
            .KoennenstufeID = New Guid,
            .Beschreibung = "Das erste Mal auf Skiern oder 1-2 Tage Skischule"}

        Dim Anfaenger = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Großglockner",
            .Benennung = "Anfänger",
            .Beschreibung = "1-wöchiger Skikurs und kann eigenständig Bremsen, blaue Pisten",
            .KoennenstufeID = New Guid}

        Dim Fortgeschritten = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Wildspitze",
            .Benennung = "Anfänger",
            .Beschreibung = "Rote Pisten, Kurven, eigenständiges Liftfahren",
            .KoennenstufeID = New Guid}


        Dim Koenner = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Zuckerhütl",
            .Benennung = "Könner",
            .Beschreibung = "Paralleles Skifahren, Stockeinsatz, kürzere Radien, schwarze Pisten",
            .KoennenstufeID = New Guid}


        Dim Topfahrer = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Matterhorn",
            .Benennung = "Topfahrer",
            .Beschreibung = "Jede Piste, jedes Gelände sowie alle Arten Schnee werden sicher gemeistert",
            .KoennenstufeID = New Guid}

        Dim Koennenstufenliste = New KoennenstufenCollection
        Koennenstufenliste.ToList.AddRange({Neuling, Anfaenger, Fortgeschritten, Koenner, Topfahrer})


    End Sub
End Module
