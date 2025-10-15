Imports Groupies
Imports Groupies.Entities.Generation4

Namespace DataImport

    Public Class Teilnehmer
        Implements IEquatable(Of Teilnehmer)

        Public Property TeilnehmerID As Guid
        Public Property Vorname() As String
        Public Property Nachname() As String
        Public Property Geburtsdatum() As Date
        Public Property Telefonnummer() As String
        Public Property Leistungsstand() As String

        Public Property IstBekannt As Boolean = False

        Public Overloads Function Equals(other As Teilnehmer) As Boolean Implements IEquatable(Of Teilnehmer).Equals
            Return Vorname = other.Vorname AndAlso Nachname = other.Nachname
        End Function
    End Class

    Public Class Trainer
        Implements IEquatable(Of Trainer)
        Public Property TrainerID() As Guid
        Public Property Vorname() As String
        Public Property Nachname() As String
        Public Property Spitzname() As String
        Public Property Telefonnummer() As String
        Public Property eMail() As String
        Public Property IstBekannt As Boolean = False

        Public Overloads Function Equals(other As Trainer) As Boolean Implements IEquatable(Of Trainer).Equals
            Return Vorname = other.Vorname AndAlso Nachname = other.Nachname
        End Function

    End Class
End Namespace
