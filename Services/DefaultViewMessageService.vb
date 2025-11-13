Imports System.Windows

Namespace Services
    Public Class DefaultViewMessageService
        Implements IViewMessageService


        Public Function Show(message As String, caption As String, buttons As MessageBoxButton, icon As MessageBoxImage) As MessageBoxResult Implements IViewMessageService.Show
            Return MessageBox.Show(message, caption, buttons, icon)
        End Function

        Public Function ShowInformation(message As String, Optional caption As String = "") As MessageBoxResult Implements IViewMessageService.ShowInformation
            Return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information)
        End Function

        Public Function ShowWarning(message As String, Optional caption As String = "") As MessageBoxResult Implements IViewMessageService.ShowWarning
            Return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Warning)
        End Function

        Public Function ShowError(message As String, Optional caption As String = "") As MessageBoxResult Implements IViewMessageService.ShowError
            Return Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error)
        End Function

        Public Function ShowConfirmation(message As String, Optional caption As String = "") As Boolean Implements IViewMessageService.ShowConfirmation
            Throw New NotImplementedException()
        End Function

        Public Function PromptForText(prompt As String, Optional title As String = "", Optional defaultValue As String = "") As String Implements IViewMessageService.PromptForText
            Throw New NotImplementedException()
        End Function

    End Class
End Namespace
