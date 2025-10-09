Imports Groupies.Entities

Namespace Interfaces


    Public Interface IClub
        Function LadeGroupies(Datei As String) As Generation4.Club
        Function Map2AktuelleGeneration(Skiclub As IClub) As Generation4.Club
        Property Name As String
        ReadOnly Property DateiGeneration As String


    End Interface

End Namespace
