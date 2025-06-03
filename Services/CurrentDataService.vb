Imports System.Collections.ObjectModel
Imports System.Windows.Shell
Imports Groupies.Entities
Imports Groupies.Entities.Generation3

Namespace Services

    ' Hier werden alle Daten der aktuellen Groupies Datei gesammelt

    Public Module CurrentDataService

        Public Property Club As Club


        Public Property Skiclub As Generation1.Skiclub

        Public ReadOnly Property availableInstructors As TrainerCollection
            Get
                Return InstructorsAvailable()
            End Get
        End Property

        Private Function InstructorsAvailable() As TrainerCollection
            Dim available = New TrainerCollection
            Club.SelectedEinteilung.GruppenloseTrainer.Where(Function(y) y.IstEinerGruppeZugewiesen = False).ToList.ForEach(Sub(i) available.Add(i))
            Return available
        End Function

        Public Sub CreateNewSkiclub()
            Club = New Club("Club") With {.AlleLeistungsstufen = TemplateService.StandardLeistungsstufenErstellen()}
            If MessageBoxResult.Yes = MessageBox.Show("Neuen Skiclub erstellt, gleich neue Gruppen hinzufügen?", "Achtung", MessageBoxButton.YesNo) Then
                Dim dlg = New CountOfGroupsDialog
                If dlg.ShowDialog Then
                    Club.SelectedEinteilung.Gruppenliste = TemplateService.StandardGruppenErstellen(dlg.Count.Text)
                End If
            End If
        End Sub

    End Module

End Namespace
