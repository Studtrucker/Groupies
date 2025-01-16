Imports Groupies

Namespace DataImport

    Public Class Teilnehmer
        Implements IEquatable(Of Entities.Teilnehmer)

        Public Property TeilnehmerID As Guid
        Public Property Vorname() As String
        Public Property Nachname() As String
        Public Property Geburtsdatum() As Date

        Public Property IstBekannt As Boolean = False

        Public Overloads Function Equals(other As Entities.Teilnehmer) As Boolean Implements IEquatable(Of Entities.Teilnehmer).Equals
            Return Vorname = other.Vorname AndAlso Nachname = other.Nachname
        End Function
    End Class

    Public Class Trainer
        Implements IEquatable(Of Entities.Trainer)
        Public Property TrainerID() As Guid
        Public Property Vorname() As String
        Public Property Nachname() As String
        Public Property IstBekannt As Boolean = False

        Public Overloads Function Equals(other As Entities.Trainer) As Boolean Implements IEquatable(Of Entities.Trainer).Equals
            Return Vorname = other.Vorname AndAlso Nachname = other.Nachname
        End Function

    End Class
End Namespace
