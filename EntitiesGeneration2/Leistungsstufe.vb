Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Runtime.InteropServices
Imports System.Text
Imports Microsoft.Office.Interop.Excel
Imports System.Collections.ObjectModel



Namespace Entities.Generation2

    Public Class Leistungsstufe

#Region "Properties"

        Public Property LeistungsstufeID As Guid

        Public Property Sortierung As Integer

        Public Property Benennung As String

        Public Property Beschreibung As String

        Public Property Faehigkeiten As List(Of Faehigkeit)

#End Region

    End Class
End Namespace
