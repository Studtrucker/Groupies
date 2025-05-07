Public Interface IViewModel

#Region "Events"

    Event RequestClose As EventHandler(Of Boolean)

    Event Close As EventHandler

#End Region

#Region "Properties"

    ''' <summary>
    ''' Das Usercontrol, das angezeigt werden soll
    ''' </summary>
    ''' <returns></returns>
    Property DatenObjekt As Object

    ''' <summary>
    ''' Der Titel für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowTitleText As String

    ''' <summary>
    ''' Das Icon für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowTitelIcon As String

    ''' <summary>
    ''' Das Bild in der Überschrift für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowHeaderImage As String

    ''' <summary>
    ''' Der Text in der Überschrift für das Window
    ''' </summary>
    ''' <returns></returns>
    ReadOnly Property WindowHeaderText As String

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
