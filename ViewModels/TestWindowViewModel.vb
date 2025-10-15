Imports System.Collections.ObjectModel
Imports Groupies.Entities.Generation4

Public Class TestWindowViewModel
    Public Property LeistungsstufenListe As ObservableCollection(Of Leistungsstufe)
    Public Property AlleGruppen As GruppeCollection
    Public Property SelectedGruppe As GruppeCollection

    Public Sub New()
        LeistungsstufenListe = Groupies.Services.DateiService.AktuellerClub.Leistungsstufenliste.Sortieren
        AlleGruppen = Groupies.Services.DateiService.AktuellerClub.Einteilungsliste.First.Gruppenliste
    End Sub

End Class
