Imports System.ComponentModel
Imports System.Globalization
Namespace Converters


    Public Class Anzahl2TextConverter
        Inherits TypeConverter

        Public Text As String

        'Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
        '    If TypeOf value IsNot Integer Then Return DependencyProperty.UnsetValue
        '    Return If(value < 5, Text = "Kleiner5")
        'End Function

        'Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
        '    If TypeOf value IsNot String Then Return DependencyProperty.UnsetValue
        '    Return If(value = "Kleiner5", True, False)
        'End Function
        Public Overrides Function ConvertFrom(context As ITypeDescriptorContext, culture As CultureInfo, value As Object) As Object
            Return MyBase.ConvertFrom(context, culture, value)
        End Function
    End Class

End Namespace
