Public Class ImageTypes
    Public Const BMP As String = ".bmp"
    Public Const GIF As String = ".gif"
    Public Const JPEG As String = "jpeg"
    Public Const JPG As String = ".jpg"
    Public Const PNG As String = ".png"
    Public Const TIFF As String = ".tiff"

    Public Shared ReadOnly Iterator Property AllImageTypes As IEnumerable(Of String)
        Get
            Yield BMP
            Yield GIF
            Yield JPEG
            Yield JPG
            Yield PNG
            Yield TIFF
        End Get
    End Property
End Class
