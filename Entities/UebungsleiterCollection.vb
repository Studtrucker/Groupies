Imports System.Collections.ObjectModel

Namespace Entities

    Public Class UebungsleiterCollection
        Inherits ObservableCollection(Of Uebungsleiter)

        Public Function GetPrintName(Id As Guid) As String
            Return First(Function(x) x.UebungsleiterID = Id).PrintName
        End Function

        Public Function GetHatFoto(Id As Guid) As Boolean
            Return First(Function(x) x.UebungsleiterID = Id).HatFoto
        End Function

        Public Function GetFoto(Id As Guid) As Byte()
            Return First(Function(x) x.UebungsleiterID = Id).Foto
        End Function

    End Class
End Namespace
