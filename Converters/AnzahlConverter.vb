Imports System.Globalization
Namespace Converters


    Public Class AnzahlKleiner5ToBooleanConverter
        Implements IValueConverter

        Public Anzahl As Integer

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If Not TypeOf value Is Integer Then Return DependencyProperty.UnsetValue
            Return If(value < 5, True, False)
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            If Not TypeOf value Is Integer Then Return DependencyProperty.UnsetValue
            Return If(value < 5, True, False)
        End Function
    End Class

End Namespace
