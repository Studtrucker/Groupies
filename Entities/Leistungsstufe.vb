Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations


Namespace Entities

    <DefaultProperty("Benennung")>
    Public Class Leistungsstufe
        Inherits BaseModel

#Region "Konstruktor"

        ''' <summary>
        ''' Erstellt eine Leistungsstufe
        ''' </summary>
        Public Sub New()
            _LeistungsstufeID = Guid.NewGuid()
        End Sub


        <Obsolete>
        Public Sub New(SaveMe As Boolean)
            _LeistungsstufeID = Guid.NewGuid()
            SaveOrDisplay = SaveMe
        End Sub

        ''' <summary>
        ''' Erstellt eine Leistungsstufe mit Angabe der Benennung
        ''' </summary>
        ''' <param name="Benennung"></param>
        Public Sub New(Benennung As String)
            _Benennung = Benennung
        End Sub

#End Region

#Region "Eigenschaften"

        ''' <summary>
        ''' Eindeutige Kennzeichnung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property LeistungsstufeID As Guid


        ''' <summary>
        ''' Sortierungszahl für die Ausgabeinformationen
        ''' </summary>
        ''' <returns></returns>
        <StringLength(3)>
        Public Property SortNumber As String

        ''' <summary>
        ''' Die Benennung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        <Required(AllowEmptyStrings:=False, ErrorMessage:="Der Name ist eine Pflichtangabe")>
        Public Property Benennung As String

        ''' <summary>
        ''' Beschreibung der Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property Beschreibung As String

        ''' <summary>
        ''' Ein Liste von erforderlichen Fähigkeiten für diese Leistungsstufe
        ''' </summary>
        ''' <returns></returns>
        Public Property Faehigkeiten As SkillCollection

        <Obsolete>
        Property SaveOrDisplay As Boolean

#End Region

#Region "Funktionen und Methoden"

        ''' <summary>
        ''' Fügt der Leistungsstufe eine erforderliche Fähigkeit hinzu 
        ''' </summary>
        ''' <param name="skill"></param>
        Public Sub AddSkill(skill As Faehigkeit)
            _Faehigkeiten.Add(skill)
        End Sub

        ''' <summary>
        ''' Entfernt die Leistungsstufe aus den Fähigkeiten 
        ''' </summary>
        ''' <param name="skill"></param>
        Public Sub RemoveSkill(skill As Faehigkeit)
            _Faehigkeiten.Remove(skill)
        End Sub

        Public Overrides Function ToString() As String
            Return Benennung
        End Function

#End Region


    End Class
End Namespace
