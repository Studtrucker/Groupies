Imports System.Windows

Namespace Services
    Public Interface IViewMessageService
        Function Show(message As String, caption As String, buttons As MessageBoxButton, icon As MessageBoxImage) As MessageBoxResult
        Function ShowInformation(message As String, Optional caption As String = "") As MessageBoxResult
        Function ShowWarning(message As String, Optional caption As String = "") As MessageBoxResult
        Function ShowError(message As String, Optional caption As String = "") As MessageBoxResult
        Function ShowConfirmation(message As String, Optional caption As String = "") As Boolean
        Function PromptForText(prompt As String, Optional title As String = "", Optional defaultValue As String = "") As String

        ' Convenience: spezifische Bestätigung für Datei-Überschreiben
        Function ConfirmOverwrite(fileName As String, Optional caption As String = "Datei überschreiben") As Boolean
    End Interface
End Namespace