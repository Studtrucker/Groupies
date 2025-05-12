Public Interface IViewModelBase

#Region "Events"

    Event RequestClose As EventHandler(Of Boolean)

    Event Close As EventHandler

#End Region

#Region "Properties"

    ''' <summary>
    ''' Der Titel für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowTitleText As String

    ''' <summary>
    ''' Das Icon für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowTitleIcon As String

    ''' <summary>
    ''' Das Bild in der Überschrift für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowHeaderImage As String

    ''' <summary>
    ''' Die Sichtbarkeit des Cancel-Buttons
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property CancelButtonVisibility As Visibility

    ''' <summary>
    ''' Die Sichtbarkeit des Ok-Buttons
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property OkButtonVisibility As Visibility

    ''' <summary>
    ''' Die Sichtbarkeit des Close-Buttons
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property CloseButtonVisibility As Visibility

#End Region

#Region "Commands"

    ''' <summary>
    ''' Der OK-Button Command
    ''' </summary>
    ''' <returns></returns>
    Property OkCommand As ICommand

    ''' <summary>
    ''' Der Cancel-Button Command
    ''' </summary>
    ''' <returns></returns>
    Property CancelCommand As ICommand

    ''' <summary>
    ''' Der Schliessen-Button Command
    ''' </summary>
    ''' <returns></returns>
    Property CloseCommand As ICommand

#End Region

End Interface
