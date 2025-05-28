Imports Groupies.Entities

Public Class EinteilungViewModel
    Inherits MasterDetailViewModel(Of Faehigkeit)
    Implements IViewModelSpecial

    Public Property Model As IModel Implements IViewModelSpecial.Model
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As IModel)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property UserControlLoaded As ICommand Implements IViewModelSpecial.UserControlLoaded
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As ICommand)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Sub OnLoaded(obj As Object) Implements IViewModelSpecial.OnLoaded
        Throw New NotImplementedException()
    End Sub

    Private Sub IViewModelSpecial_OnOk(obj As Object) Implements IViewModelSpecial.OnOk
        OnOk(obj)
    End Sub
End Class
