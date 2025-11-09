Imports System
Imports System.Globalization
Imports System.Windows.Data

Namespace Converters

    Public Class SelectedItemsAndTargetConverter
        Implements IMultiValueConverter

        Public Function Convert(values As Object(), targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            If values Is Nothing Then Return Nothing
            Dim a(1) As Object
            a(0) = If(values.Length > 0, values(0), Nothing)
            a(1) = If(values.Length > 1, values(1), Nothing)
            Return a
        End Function

        Public Function ConvertBack(value As Object, targetTypes As Type(), parameter As Object, culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function
    End Class

End Namespace
