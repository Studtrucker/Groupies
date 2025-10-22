Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports Groupies.Annotations
Imports Microsoft.Office.Interop.Excel

''' <summary>
''' Abstrakte Basisklasse für alle Modelle 
''' </summary>
Public MustInherit Class BaseModel
    Implements INotifyPropertyChanged

#Region "Events"

    '    Occurs when a property value changes.
    ''' <summary>
    ''' Tritt auf, wenn sich ein Eigenschaftswert ändert.
    ''' </summary>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region "Konstruktoren"
    '   Default constructor.
    ''' <summary>
    ''' Standard-Konstruktor.
    ''' </summary>
    Public Sub New()
        InitCommands()
    End Sub
#End Region

#Region "Methoden"

    '   Override this method in derived types to initialize command logic.
    ''' <summary>
    ''' Überschreiben Sie diese Methode in abgeleiteten Typen, um die Befehlslogik zu initialisieren.
    ''' </summary>
    Protected Overridable Sub InitCommands()
    End Sub


    '   Raises the <see cref="PropertyChanged"/> event.
    '   The name of the property which value has changed.
    ''' <summary>
    ''' Löst das Ereignis <see cref="PropertyChanged"/> aus.
    ''' </summary>
    ''' <param name="propertyName">
    ''' Der Name der Eigenschaft, deren Wert sich geändert hat.
    ''' </param>
    <NotifyPropertyChangedInvocator>
    Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

#End Region


#Region "Dekativiert: ExceptionValidationRule"
    ' Um Validierungen mit der ExceptionValidationRule zu prüfen, muss die Property in der
    ' Modell-Klasse eine Exception auslösen
    ' xaml: ValidatesOnExceptions=True oder ExceptionValidationRule
    ' Seite 710
#End Region

#Region "Dekativiert: DataErrorValidationRule"
    ' Um Validierungen mit der DataErrorValidationRule zu prüfen,
    ' muss die Modell-Klasse das IDataErrorInfo implementieren
    ' xaml: ValidatesOnDataErrors=True oder DataErrorValidationRule
    ' Seite 712

    'Friend _Errors As New Dictionary(Of String, String)

#End Region

#Region "Dekativiert NotifyDataErrorValidationRule"
    ' Um Validierungen mit der NotifyDataErrorValidationRule zu prüfen,
    ' muss die Modell-Klasse das INotifyDataErrorInfo implementieren
    ' xaml: ValidatesOnNotifyDataErrors=True oder NotifyDataErrorValidationRule
    ' Seite 713
#End Region

End Class

