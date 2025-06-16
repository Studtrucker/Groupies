Public Interface IViewModelSpecial

    Event ObjektChangedEvent As EventHandler(Of Boolean)
    Property Model As IModel
    Property Daten As IEnumerable(Of IModel)
    ReadOnly Property IstEingabeGueltig As Boolean
    ReadOnly Property UserControlLoaded As ICommand
    ReadOnly Property DataGridSortingCommand As ICommand
    Sub OnOk(obj As Object)
    Sub OnLoaded(obj As Object)

End Interface
