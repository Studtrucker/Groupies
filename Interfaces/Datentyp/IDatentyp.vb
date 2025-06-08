Namespace Interfaces

    Public Interface IDatentyp

        ''' <summary>
        ''' Text für den singular Datentyp
        ''' </summary>
        ''' <returns></returns>
        Property DatentypText As String

        ''' <summary>
        ''' Text für den plural Datentyp
        ''' </summary>
        ''' <returns></returns>
        Property DatentypenText As String

        ''' <summary>
        ''' Bild für den Datentyp
        ''' </summary>
        ''' <returns></returns>
        Property DatentypIcon As String

        ''' <summary>
        ''' Das Usercontrol von dem Datentyp
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property DatentypDetailUserControl As UserControl

        ReadOnly Property AktuellesUebersichtViewModel As IViewModelSpecial
        ReadOnly Property AktuellesDetailViewModel As IViewModelSpecial



    End Interface


End Namespace
