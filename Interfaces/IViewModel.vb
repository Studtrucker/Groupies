Public Interface IViewModel

    Property DatenObjekt As Object
    ReadOnly Property WindowTitle As String
    ReadOnly Property WindowImage As String
    ReadOnly Property WindowCaption As String

    Event RequestClose As EventHandler(Of Boolean)

    Event Close As EventHandler
    Property OkCommand As ICommand
    Property CancelCommand As ICommand
    Property CloseCommand As ICommand

End Interface
