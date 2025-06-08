Public Interface IViewModelSpecial

    Property Model As IModel
    Property Daten As IEnumerable(Of IModel)
    ReadOnly Property UserControlLoaded As ICommand
    ReadOnly Property DataGridSortingCommand As ICommand
    Sub OnOk(obj As Object)
    Sub OnLoaded(obj As Object)

End Interface
