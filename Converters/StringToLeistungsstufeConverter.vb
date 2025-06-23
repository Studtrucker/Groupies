Imports System.Globalization

Namespace Converters

    Public Class StringToLeistungsstufeConverter
        Implements IValueConverter

        Private ReadOnly AktuelleLeistungsstufen As List(Of String)

        Public Sub New()
            If AktuelleLeistungsstufen Is Nothing Then
                AktuelleLeistungsstufen = New List(Of String)
            Else
                AktuelleLeistungsstufen.Clear()
            End If
            Controller.AppController.AktuellerClub?.AlleLeistungsstufen.ToList.ForEach(Sub(Ls) AktuelleLeistungsstufen.Add(Ls.Benennung))
        End Sub

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If TypeOf value IsNot Entities.Leistungsstufe Then Return DependencyProperty.UnsetValue
            Return DirectCast(value, Entities.Leistungsstufe).Benennung
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            If TypeOf value IsNot String Then Return DependencyProperty.UnsetValue
            Dim obj = Controller.AppController.AktuellerClub.AlleLeistungsstufen.ToList.Where(Function(Ls) Ls.Benennung = value).DefaultIfEmpty(New Entities.Leistungsstufe("Level unbekannt")).First
            Return obj
        End Function
    End Class
End Namespace
