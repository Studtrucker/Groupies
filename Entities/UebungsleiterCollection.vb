Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Data.SqlClient

Namespace Entities

    Public Class UebungsleiterCollection
        Inherits ObservableCollection(Of Uebungsleiter)

        Public Function GetPrintName(Instructor As Uebungsleiter) As String
            Return First(Function(x) x.UebungsleiterID = Instructor.UebungsleiterID).PrintName
        End Function

        Public Function GetHatFoto(Instructor As Uebungsleiter) As Boolean
            Return First(Function(x) x.UebungsleiterID = Instructor.UebungsleiterID).HatFoto
        End Function

        Public Function GetFoto(Instructor As Uebungsleiter) As Byte()
            Return First(Function(x) x.UebungsleiterID = Instructor.UebungsleiterID).Foto
        End Function

    End Class
End Namespace
