Imports System.Globalization

Namespace Converters

    Public Class InvertableBoolToVisibleConverter
        Implements IValueConverter

        Private _inverted As Boolean = False
        Public Property Inverted() As Boolean
            Get
                Return _inverted
            End Get
            Set(ByVal value As Boolean)
                _inverted = value
            End Set
        End Property

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            If Not TypeOf (value) Is Boolean Then Return DependencyProperty.UnsetValue
            Return If(CBool(value) Xor Inverted, Visibility.Visible, Visibility.Collapsed)
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            If Not TypeOf value Is Visibility Then Return DependencyProperty.UnsetValue
            Return value = Visibility.Visible
        End Function
    End Class

End Namespace
