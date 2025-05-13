Public Interface IViewModelSpecial

    Sub OnOk(obj As Object)
    Sub OnLoaded()

    Property Model As IModel
    Property UserControlLoaded As ICommand

End Interface
