Namespace Interfaces

    Public Interface IDatentyp

        ''' <summary>
        ''' Text für den Datentyp
        ''' </summary>
        ''' <returns></returns>
        Property DatentypText As String

        ''' <summary>
        ''' Bild für den Datentyp
        ''' </summary>
        ''' <returns></returns>
        Property DatentypIcon As String

        ''' <summary>
        ''' Das Usercontrol von dem Datentyp
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property DatentypUserControl As UserControl

    End Interface


End Namespace
