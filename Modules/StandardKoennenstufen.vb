Imports Skireisen.Entities

Module StandardKoennenstufen
    Public Sub erstellen()

        Dim Neuling = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Edelweiss",
            .Benennung = "Neuling",
            .KoennenstufeID = New Guid,
            .Beschreibung = "Das erste Mal auf Skiern oder 1-2 Tage Skischule"}

        Dim Anfaenger = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Enzian",
            .Benennung = "Anfänger",
            .Beschreibung = "1-wöchiger Skikurs und kann eigenständig Bremsen",
            .KoennenstufeID = New Guid}

        Dim Fortgeschritten = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Alpenrose",
            .Benennung = "Anfänger",
            .Beschreibung = "Blaue Pisten, Kurven, eigenständiges Liftfahren",
            .KoennenstufeID = New Guid}


        Dim Koenner = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Alpenglöckchen",
            .Benennung = "Könner",
            .Beschreibung = "Paralleles Skifahren, Stockeinsatz, kürzere Radien, rote und schwarze Pisten",
            .KoennenstufeID = New Guid}


        Dim Topfahrer = New Koennenstufe With {
            .AngezeigteBenennung = "Gruppe Hahnenfuß",
            .Benennung = "Topfahrer",
            .Beschreibung = "Jede Piste, Gelände sowie alle Arten Schnee werden sicher gemeistert",
            .KoennenstufeID = New Guid}

        Dim Koennenstufenliste = New KoennenstufenCollection
        Koennenstufenliste.ToList.AddRange({Neuling, Anfaenger, Fortgeschritten, Koenner, Topfahrer})


    End Sub
End Module
