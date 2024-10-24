Imports Groupies

Namespace DataImport

    Public Class Teilnehmer
        Implements IEquatable(Of Entities.Teilnehmer)

        Public Property TeilnehmerID() As Guid
        Public WriteOnly Property TeilnehmerIDText() As String
            Set(value As String)
                Dim newGuid As Guid
                If Guid.TryParse(value, newGuid) Then
                    _TeilnehmerID = newGuid
                End If
            End Set
        End Property
        Public Property Vorname() As String
        Public Property Nachname() As String
        Public Property IstBekannt As Boolean = False

        Public Overloads Function Equals(other As Entities.Teilnehmer) As Boolean Implements IEquatable(Of Entities.Teilnehmer).Equals
            Return Vorname = other.Vorname AndAlso Nachname = other.Nachname
        End Function
    End Class

    Public Class Trainer
        Public Property Vorname() As String
        Public Property Nachname() As String
        Public Property IstBekannt As Boolean = False

    End Class

End Namespace
