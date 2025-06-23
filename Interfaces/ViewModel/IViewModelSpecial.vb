Public Interface IViewModelSpecial

    Event ModelChangedEvent As EventHandler(Of Boolean)
    Property Model As IModel
    Property Daten As IEnumerable(Of IModel)
    ReadOnly Property IstEingabeGueltig As Boolean
    ReadOnly Property UserControlLoaded As ICommand
    ReadOnly Property DataGridSortingCommand As ICommand
    ReadOnly Property LoeschenCommand As ICommand
    ReadOnly Property BearbeitenCommand As ICommand
    ReadOnly Property NeuCommand As ICommand
    Sub OnOk(obj As Object)
    Sub OnLoaded(obj As Object)

End Interface
