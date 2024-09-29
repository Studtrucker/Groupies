Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel

Namespace Entities

    ''' <summary>
    ''' Fähigkeit zur Beschreibung von Leistungsstufen 
    ''' </summary>
    Public Class Faehigkeit
        Inherits BaseModel

#Region "Konstruktor"
        ''' <summary>
        ''' Erstellt eine neue Fähigkeit
        ''' </summary>
        <Obsolete>
        Public Sub New()
            _FaehigkeitID = Guid.NewGuid()
        End Sub

        ''' <summary>
        ''' Erstellt eine neue Fähigkeit unter Angabe der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Sub New(Benennung As String)
            _Benennung = Benennung
        End Sub

        ''' <summary>
        ''' Erstellt eine neue Fähigkeit unter Angabe der Benennung und Sortierungskennzahl
        ''' </summary>
        ''' <param name="Benennung"></param>
        Sub New(Benennung As String, Sortierung As Integer)
            _Benennung = Benennung
            _Sortierung = Sortierung
        End Sub
#End Region

#Region "Eigenschaften"
        ''' <summary>
        ''' Eindeutige Kennzeichnung der Fähigkeit
        ''' </summary>
        ''' <returns></returns>
        Public Property FaehigkeitID As Guid

        ''' <summary>
        ''' Benennung der Fähigkeit
        ''' </summary>
        ''' <returns></returns>

        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Benennung ist eine Pflichtangabe")>
        Public Property Benennung As String

        ''' <summary>
        ''' Sortierungszahl für die Ausgabeinformationen 
        ''' </summary>
        ''' <returns></returns>
        Public Property Sortierung As Integer?


        ''' <summary>
        ''' Beschreibung der Fähigkeit
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Die Beschreibung ist ein Pflichtfeld")>
        Public Property Beschreibung As String

#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Gibt die Benennung und Beschreibung für die Trainerinformation zurück
        ''' </summary>
        ''' <returns></returns>
        Public Function AusgabeAnTrainerinfo() As String
            If Sortierung Is Nothing And Beschreibung Is Nothing Then
                Return $"{Benennung}"
            ElseIf Sortierung Is Nothing Then
                Return $"{Benennung}{Environment.NewLine}{Beschreibung}."
            ElseIf Beschreibung Is Nothing Then
                Return $"{Sortierung}. {Benennung}"
            Else
                Return $"{Sortierung}. {Benennung}{Environment.NewLine}{Beschreibung}."
            End If
        End Function

        Public Overrides Function ToString() As String
            Return Benennung
        End Function
#End Region

    End Class
End Namespace
