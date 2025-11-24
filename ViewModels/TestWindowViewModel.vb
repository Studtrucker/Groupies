Imports System.Collections.ObjectModel
Imports Groupies.Entities.Generation4
Imports Groupies.Services

Public Class TestWindowViewModel
    Public Property LeistungsstufenListe As ObservableCollection(Of Leistungsstufe)
    Public Property AlleGruppen As GruppeCollection
    Public Property SelectedGruppe As GruppeCollection

    Public Sub New()
        LeistungsstufenListe = ServiceProvider.DateiService.AktuellerClub.Leistungsstufenliste.Sortieren
        AlleGruppen = ServiceProvider.DateiService.AktuellerClub.Einteilungsliste.First.Gruppenliste
    End Sub

End Class
