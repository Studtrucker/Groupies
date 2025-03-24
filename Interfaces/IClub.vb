Imports Groupies.Entities

Namespace Interfaces


    Public Interface IClub
        Function LadeGroupies(Datei As String) As Generation3.Club
        Function Map2AktuelleGeneration(Skiclub As IClub) As Generation3.Club
        Property Name As String

    End Interface

End Namespace
