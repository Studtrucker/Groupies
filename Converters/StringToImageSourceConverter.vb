Imports System
Imports System.Globalization
Imports System.Windows.Data
Imports System.Windows.Media.Imaging

Namespace Converters

    Public Class StringToImageSourceConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.Convert
            Try
                If value Is Nothing Then
                    Return Nothing
                End If

                Dim s = Convert.ToString(value)
                If String.IsNullOrWhiteSpace(s) Then
                    Return Nothing
                End If

                Dim uri As Uri
                If Uri.TryCreate(s, UriKind.RelativeOrAbsolute, uri) Then
                    Dim bi As New BitmapImage()
                    bi.BeginInit()
                    bi.UriSource = uri
                    bi.CacheOption = BitmapCacheOption.OnLoad
                    bi.EndInit()
                    bi.Freeze()
                    Return bi
                End If

                Return Nothing
            Catch
                Return Nothing
            End Try
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object, culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Return Binding.DoNothing
        End Function
    End Class

End Namespace
