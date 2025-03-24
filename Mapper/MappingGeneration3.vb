Imports Groupies.Entities

Public Module MappingGeneration3

    Private NeuerClub As Generation3.Club

    Public Function MapSkiClub2Club(Skiclub As Generation3.Club) As Generation3.Club

        NeuerClub = New Generation3.Club
        NeuerClub = Skiclub
        NeuerClub.ClubName = If(Skiclub.ClubName, "Club")

        Return NeuerClub

    End Function

End Module
