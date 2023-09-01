Imports Skikurs.Entities

Namespace Entities


    Class Skischule

#Region "Fields"

        Public Property Teilnehmerliste() As TeilnehmerCollection
        Public Property Skikursgruppenliste() As SkikursgruppenCollection
        Public Property Levelsliste() As LevelsCollection

#End Region

        Public Sub initialisiereListen()
            _Skikursgruppenliste = New SkikursgruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            _Levelsliste = New LevelsCollection
        End Sub

        Public Sub erstelleLevels()

            Levelsliste = Standardelemente.erstelleLevels
            Skikursgruppenliste = Standardelemente.erstelleGruppen(10)

        End Sub

    End Class

End Namespace
