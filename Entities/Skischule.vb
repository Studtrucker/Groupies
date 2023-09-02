Imports Skischule.Entities

Namespace Entities


    Public Class Skischule

#Region "Fields"

        Public Property Teilnehmerliste() As TeilnehmerCollection
        Public Property Skikursgruppenliste() As SkikursgruppenCollection
        Public Property Levelsliste() As LevelsCollection
        Public Property Skilehrerliste() As UebungsleiterCollection

#End Region

#Region "Constructor"
        Public Sub New()
            _Skikursgruppenliste = New SkikursgruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            _Levelsliste = New LevelsCollection
            _Skilehrerliste = New UebungsleiterCollection
        End Sub

        Public Sub New(Teilnehmerliste As TeilnehmerCollection)
            _Skikursgruppenliste = New SkikursgruppenCollection
            _Teilnehmerliste = New TeilnehmerCollection
            _Levelsliste = New LevelsCollection
            readTeilnehmerliste(Teilnehmerliste)
        End Sub

#End Region

        Private Sub erstelleBasisobjekte()
            Levelsliste = Standardelemente.erstelleLevels
            Skikursgruppenliste = Standardelemente.erstelleGruppen(10)
        End Sub

        Private Sub readTeilnehmerliste(Teilnehmer As TeilnehmerCollection)
            Teilnehmerliste = Teilnehmer
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub


    End Class

End Namespace
