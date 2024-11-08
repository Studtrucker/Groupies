Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Reflection
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports Groupies.Annotations



''' <summary>
''' Abstrakte Basisklasse für alle Modelle  
''' </summary>
Public MustInherit Class BaseModel
    Implements INotifyPropertyChanged
    Implements IDataErrorInfo

    'Implements INotifyDataErrorInfo

#Region "Felder"
    Private Shared _propertyInfos As List(Of PropertyInfo)

#End Region

#Region "Events"

    '    Occurs when a property value changes.
    ''' <summary>
    ''' Tritt auf, wenn sich ein Eigenschaftswert ändert.
    ''' </summary>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

    '''' <summary>
    '''' Tritt auf, wenn sich ein Fehler ändert
    '''' </summary>
    'Public Event ErrorsChanged As EventHandler(Of DataErrorsChangedEventArgs) Implements INotifyDataErrorInfo.ErrorsChanged

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

#Region "explicit interfaces"

    '''' <summary>
    '''' Ruft eine Fehlermeldung ab, die angibt, was mit diesem Objekt nicht stimmt.
    '''' </summary>
    '''' <returns>
    '''' Eine Fehlermeldung, die angibt, was mit diesem Objekt nicht in Ordnung ist. 
    '''' Der Standardwert ist eine leere Zeichenkette ("").
    '''' </returns>
    'Public ReadOnly Property [Error] As String
    '    Get
    '        Throw New NotImplementedException()
    '    End Get
    'End Property



#End Region

#Region "Methoden"

    '   Override this method in derived types to initialize command logic.
    ''' <summary>
    ''' Überschreiben Sie diese Methode in abgeleiteten Typen, um die Befehlslogik zu initialisieren.
    ''' </summary>
    Protected Overridable Sub InitCommands()
    End Sub

    '   Can be overridden by derived types to react on the finisihing of error-collections.
    '''' <summary>
    '''' Kann von abgeleiteten Typen überschrieben werden, um auf die Beendigung von Fehlersammlungen zu reagieren.
    '''' </summary>
    'Protected Overridable Sub OnErrorsCollected()
    'End Sub

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

    ''   Raises the <see cref="PropertyChanged"/> event.
    ''   The name of the property which value has changed.
    '''' <summary>
    '''' Löst das Ereignis <see cref="ErrorsChanged"/> aus.
    '''' </summary>
    '''' <param name="propertyName">
    '''' Der Name der Eigenschaft, deren Fehlerliste sich geändert hat.
    '''' </param>
    '<NotifyPropertyChangedInvocator>
    'Protected Overridable Sub OnErrorsChanged(<CallerMemberName> Optional propertyName As String = Nothing)
    '    RaiseEvent ErrorsChanged(Me, New DataErrorsChangedEventArgs(propertyName))
    'End Sub


    ' Is called by the indexer to collect all errors and not only the one for a special field.
    ' Because <see cref="HasErrors"/> depends on the <see cref="Errors"/> dictionary this
    ' ensures that controls like buttons can switch their state accordingly.

    '''' <summary>
    '''' Wird vom Indexer aufgerufen, um alle Fehler zu sammeln und nicht nur die für ein bestimmtes Feld.
    '''' </summary>
    '''' <remarks>
    '''' Da <see cref="HasErrors"/> vom <see cref="Errors"/>-Wörterbuch abhängt, 
    '''' wird sichergestellt, dass Steuerelemente wie Schaltflächen ihren Zustand entsprechend ändern können.
    '''' </remarks>
    'Private Sub CollectErrors()
    '    Errors.Clear()
    '    PropertyInfos.ForEach(Sub(prop)
    '                              Dim currentValue = prop.GetValue(Me)
    '                              Dim requiredAttr = prop.GetCustomAttribute(Of RequiredAttribute)()
    '                              Dim maxLenAttr = prop.GetCustomAttribute(Of MaxLengthAttribute)()
    '                              If requiredAttr IsNot Nothing Then
    '                                  If String.IsNullOrEmpty(If(currentValue?.ToString(), String.Empty)) Then
    '                                      Errors.Add(prop.Name, requiredAttr.ErrorMessage)
    '                                  End If
    '                              End If
    '                              If maxLenAttr IsNot Nothing Then
    '                                  If If(currentValue?.ToString(), String.Empty).Length > maxLenAttr.Length Then
    '                                      Errors.Add(prop.Name, maxLenAttr.ErrorMessage)
    '                                  End If
    '                              End If
    '                              ' TODO further attributes
    '                          End Sub)
    '    ' we have to do this because the Dictionary does not implement INotifyPropertyChanged            
    '    OnPropertyChanged(NameOf(BaseModel.HasErrors))
    '    OnPropertyChanged(NameOf(BaseModel.IsOk))
    '    ' commands do not recognize property changes automatically
    '    OnErrorsCollected()
    'End Sub

    'Public Function GetErrors(PropertyName As String) As IEnumerable Implements INotifyDataErrorInfo.GetErrors
    '    If String.IsNullOrEmpty(PropertyName) Then Return Nothing
    '    If _Errors.ContainsKey(PropertyName) Then
    '        Return _Errors(PropertyName)
    '    End If
    '    Return Nothing
    'End Function

#End Region

#Region "Eigenschaften"

    '''' <summary>
    '''' Ein Wörterbuch der aktuellen Fehler mit dem Namen des Fehlerfeldes als Schlüssel und 
    '''' dem Fehlertext als Wert aufnimmt
    '''' </summary>
    Private Protected Property Errors As Dictionary(Of String, List(Of String)) = New Dictionary(Of String, List(Of String))

    '   Gets an error message indicating what is wrong with this object.
    '   An error message indicating what is wrong with this object. The default is an empty string ("").

    ''' <summary>
    ''' Ruft die Fehlermeldung für die Eigenschaft mit dem angegebenen Namen ab.
    ''' </summary>
    ''' <param name="propertyName">
    ''' Der Name der Eigenschaft, deren Fehlermeldung abgerufen werden soll.
    ''' </param>
    ''' <returns>
    ''' Die Fehlermeldung für die Eigenschaft. Der Standard ist eine leere Zeichenkette ("").
    ''' </returns>
    Default Public ReadOnly Property Item(propertyName As String) As String Implements IDataErrorInfo.Item
        Get
            'CollectErrors()
            Return If(Errors.ContainsKey(propertyName), Errors(propertyName), String.Empty)
        End Get
    End Property

    '   Indicates whether this instance has any errors.
    ''' <summary>
    ''' Zeigt an, ob diese Instanz Fehler aufweist.
    ''' </summary>
    Public ReadOnly Property HasErrors As Boolean 'Implements INotifyDataErrorInfo.HasErrors
        Get
            Return False
            '            Return Errors.Any()
        End Get
    End Property

    '   The opposite of <see cref="HasErrors"/>.
    '   Exists for convenient binding only.
    ''' <summary>
    ''' Das Gegenteil von <see cref="HasErrors"/>.
    ''' </summary>
    ''' <remarks>
    ''' Sie dient nur der bequemen Bindung.
    ''' </remarks>
    Public ReadOnly Property IsOk As Boolean
        Get
            Return Not HasErrors
        End Get
    End Property

    '   Retrieves a list of all properties with attributes required for <see cref="IDataErrorInfo"/> automation.
    ''' <summary>
    ''' Ruft eine Liste aller Eigenschaften mit Attributen ab, 
    ''' die für die <see cref="IDataErrorInfo"/>-Automatisierung erforderlich sind.
    ''' </summary>
    Protected ReadOnly Property PropertyInfos As List(Of PropertyInfo)
        Get
            Return If(_propertyInfos, Function()
                                          _propertyInfos = [GetType]().GetProperties(BindingFlags.Public Or BindingFlags.Instance).Where(Function(prop) prop.IsDefined(GetType(RequiredAttribute), True) OrElse prop.IsDefined(GetType(MaxLengthAttribute), True)).ToList()
                                          Return _propertyInfos
                                      End Function())
        End Get
    End Property

    Public ReadOnly Property [Error] As String Implements IDataErrorInfo.Error
        Get
            Throw New NotImplementedException()
        End Get
    End Property


#End Region

End Class
