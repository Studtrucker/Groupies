Imports System.Globalization

Namespace Converters

    Public Class StringToLeistungsstufeConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If TypeOf value IsNot String Then Return DependencyProperty.UnsetValue
            Return DirectCast(value, Entities.Leistungsstufe).Benennung
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return Groupies.Controller.AppController.CurrentClub.Leistungsstufenliste.ToList.Where(Function(Ls) Ls.Benennung = value).First
        End Function
    End Class
End Namespace
