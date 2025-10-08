Imports System.Collections.ObjectModel
Imports Groupies.Entities

Public Class TestWindowViewModel
    Public Property LeistungsstufenListe As ObservableCollection(Of Entities.Leistungsstufe)
    Public Property AlleGruppen As Entities.GruppeCollection
    Public Property SelectedGruppe As Entities.GruppeCollection

    Public Sub New()
        LeistungsstufenListe = Groupies.Services.DateiService.AktuellerClub.Leistungsstufenliste.Sortieren
        AlleGruppen = Groupies.Services.DateiService.AktuellerClub.Einteilungsliste.First.EinteilungAlleGruppen
    End Sub

End Class
