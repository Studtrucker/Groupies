Imports System.Collections.ObjectModel
Imports System.Windows.Shell
Imports Groupies.Entities

Namespace Services

    ' Hier werden alle Daten der aktuellen Groupies Datei gesammelt
    <Obsolete>
    Public Module CurrentDataService

        Public Property Club As Club


        Public Property Skiclub As Veraltert.Skiclub

        Public ReadOnly Property availableInstructors As TrainerCollection
            Get
                Return InstructorsAvailable()
            End Get
        End Property

        Private Function InstructorsAvailable() As TrainerCollection
            Dim available = New TrainerCollection
            Club.Trainerliste.Where(Function(y) y.IstEinerGruppeZugewiesen = False).ToList.ForEach(Sub(i) available.Add(i))
            Return available
        End Function

        Public Sub CreateNewSkiclub()
            Club = New Entities.Club("Club") With {.Leistungsstufenliste = PresetService.StandardLeistungsstufenErstellen()}
            If MessageBoxResult.Yes = MessageBox.Show("Neuen Skiclub erstellt, gleich neue Gruppen hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
                Dim dlg = New CountOfGroupsDialog
                If dlg.ShowDialog Then
                    Club.Gruppenliste = PresetService.StandardGruppenErstellen(dlg.Count.Text)
                End If
            End If
        End Sub

    End Module

End Namespace
