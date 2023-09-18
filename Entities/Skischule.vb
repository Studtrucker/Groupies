Imports Skischule.Entities

Namespace Entities


    Public Class Skischule

#Region "Fields"

        Public Property Teilnehmerliste() As TeilnehmerCollection
        Public Property Skikursliste() As SkikursCollection
        Public Property Levelliste() As LevelCollection
        Public Property Skilehrerliste() As UebungsleiterCollection

#End Region

#Region "Constructor"
        Public Sub New()
            _Skikursliste = New SkikursCollection
            _Teilnehmerliste = New TeilnehmerCollection
            _Levelliste = New LevelCollection
            _Skilehrerliste = New UebungsleiterCollection
        End Sub

        Public Sub New(Teilnehmerliste As TeilnehmerCollection)
            _Skikursliste = New SkikursCollection
            _Teilnehmerliste = New TeilnehmerCollection
            _Levelliste = New LevelCollection
            readTeilnehmerliste(Teilnehmerliste)
        End Sub

#End Region

        Private Sub readTeilnehmerliste(Teilnehmer As TeilnehmerCollection)
            Teilnehmerliste = Teilnehmer
            'Skikursgruppenliste = Teilnehmer.ToList.ForEach()
            ' Levelsliste = Teilnehmer.ToList.ForEach()
        End Sub

        Public Function GetAktualisierungen() As Skischule
            Skikursliste.ToList.ForEach(AddressOf GetAktualisierungen)
            Return Me
        End Function

        Private Sub GetAktualisierungen(Skikurs As Skikurs)
            Skikurs.Skilehrer = Skilehrerliste.Where(Function(x) x.SkilehrerID = Skikurs.Skilehrer.SkilehrerID).First
            For i = 0 To Skikurs.Mitgliederliste.Count - 1
                Skikurs.Mitgliederliste.Item(i) = GetAktualisierungen(Skikurs.Mitgliederliste.Item(i))
            Next
        End Sub

        Private Function GetAktualisierungen(Mitglied As Teilnehmer) As Teilnehmer
            Return Teilnehmerliste.Where(Function(x) x.TeilnehmerID = Mitglied.TeilnehmerID).First
        End Function


    End Class

End Namespace
