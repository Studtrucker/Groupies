Namespace Interfaces

    Public Interface IModus

        ''' <summary>
        ''' Der Modustext für das Window
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property Titel As String

        ''' <summary>
        ''' Das Modusicon für das Window
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property WindowIcon As String

        ''' <summary>
        ''' Die Sichtbarkeit für den Close-Button
        ''' </summary>
        ''' <returns></returns>
        Property CloseButtonVisibility As Visibility

        ''' <summary>
        ''' Die Sichtbarkeit für den Ok-Button
        ''' </summary>
        ''' <returns></returns>
        Property OkButtonVisibility As Visibility

        ''' <summary>
        ''' Die Sichtbarkeit für den Cancel-Button
        ''' </summary>
        ''' <returns></returns>
        Property CancelButtonVisibility As Visibility


    End Interface


End Namespace