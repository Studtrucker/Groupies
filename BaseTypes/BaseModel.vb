Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.Reflection
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports Skischule.Annotations



''' <summary>
''' Abstrakte Basisklasse für alle Modelle  
''' </summary>
Public MustInherit Class BaseModel
    Implements INotifyPropertyChanged, IDataErrorInfo

#Region "constants"
    Private Shared _propertyInfos As List(Of PropertyInfo)
#End Region

#Region "events"

    ''' <summary>
    ''' Occurs when a property value changes.
    ''' </summary>
    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

#End Region

#Region "constructors and destructors"
    ''' <summary>
    ''' Default constructor.
    ''' </summary>
    Public Sub New()
        InitCommands()
    End Sub
#End Region

#Region "explicit interfaces"

    ''' <summary>
    ''' Gets an error message indicating what is wrong with this object.
    ''' </summary>
    ''' <returns>
    ''' An error message indicating what is wrong with this object. The default is an empty string ("").
    ''' </returns>
    Public ReadOnly Property [Error] As String Implements IDataErrorInfo.Error
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    ''' <summary>
    ''' Gets the error message for the property with the given name.
    ''' </summary>
    ''' <param name="propertyName">
    ''' The name of the property whose error message to get.
    ''' </param>
    ''' <returns>
    ''' The error message for the property. The default is an empty string ("").
    ''' </returns>
    Default Public ReadOnly Property Item(propertyName As String) As String Implements IDataErrorInfo.Item
        Get
            CollectErrors()
            Return If(Errors.ContainsKey(propertyName), Errors(propertyName), String.Empty)
        End Get
    End Property

#End Region

#Region "methods"
    ''' <summary>
    ''' Override this method in derived types to initialize command logic.
    ''' </summary>
    Protected Overridable Sub InitCommands()
    End Sub

    ''' <summary>
    ''' Can be overridden by derived types to react on the finisihing of error-collections.
    ''' </summary>
    Protected Overridable Sub OnErrorsCollected()
    End Sub
    'Protected MustOverride Sub OnErrorsCollected()

    ''' <summary>
    ''' Raises the <see cref="PropertyChanged"/> event.
    ''' </summary>
    ''' <param name="propertyName">
    ''' The name of the property which value has changed.
    ''' </param>
    <NotifyPropertyChangedInvocator>
    Protected Overridable Sub OnPropertyChanged(<CallerMemberName> Optional propertyName As String = Nothing)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    ''' <summary>
    ''' Is called by the indexer to collect all errors and not only the one for a special field.
    ''' </summary>
    ''' <remarks>
    ''' Because <see cref="HasErrors"/> depends on the <see cref="Errors"/> dictionary this
    ''' ensures that controls like buttons can switch their state accordingly.
    ''' </remarks>
    Private Sub CollectErrors()
        Errors.Clear()
        PropertyInfos.ForEach(Sub(prop)
                                  Dim currentValue = prop.GetValue(Me)
                                  Dim requiredAttr = prop.GetCustomAttribute(Of RequiredAttribute)()
                                  Dim maxLenAttr = prop.GetCustomAttribute(Of MaxLengthAttribute)()
                                  If requiredAttr IsNot Nothing Then
                                      If String.IsNullOrEmpty(If(currentValue?.ToString(), String.Empty)) Then
                                          Errors.Add(prop.Name, requiredAttr.ErrorMessage)
                                      End If
                                  End If
                                  If maxLenAttr IsNot Nothing Then
                                      If If(currentValue?.ToString(), String.Empty).Length > maxLenAttr.Length Then
                                          Errors.Add(prop.Name, maxLenAttr.ErrorMessage)
                                      End If
                                  End If
                                  ' TODO further attributes
                              End Sub)
        ' we have to do this because the Dictionary does not implement INotifyPropertyChanged            
        OnPropertyChanged(NameOf(BaseModel.HasErrors))
        OnPropertyChanged(NameOf(BaseModel.IsOk))
        ' commands do not recognize property changes automatically
        OnErrorsCollected()
    End Sub


#End Region

#Region "properties"

    ''' <summary>
    ''' Indicates whether this instance has any errors.
    ''' </summary>
    Public ReadOnly Property HasErrors As Boolean
        Get
            Return Errors.Any()
        End Get
    End Property

    ''' <summary>
    ''' The opposite of <see cref="HasErrors"/>.
    ''' </summary>
    ''' <remarks>
    ''' Exists for convenient binding only.
    ''' </remarks>
    Public ReadOnly Property IsOk As Boolean
        Get
            Return Not HasErrors
        End Get
    End Property

    ''' <summary>
    ''' Retrieves a list of all properties with attributes required for <see cref="IDataErrorInfo"/> automation.
    ''' </summary>
    Protected ReadOnly Property PropertyInfos As List(Of PropertyInfo)
        Get
            Return If(_propertyInfos, Function()
                                          _propertyInfos = [GetType]().GetProperties(BindingFlags.Public Or BindingFlags.Instance).Where(Function(prop) prop.IsDefined(GetType(RequiredAttribute), True) OrElse prop.IsDefined(GetType(MaxLengthAttribute), True)).ToList()
                                          Return _propertyInfos
                                      End Function())
        End Get
    End Property

    ''' <summary>
    ''' A dictionary of current errors with the name of the error-field as the key and the error
    ''' text as the value.
    ''' </summary>
    Private ReadOnly Property Errors As Dictionary(Of String, String) = New Dictionary(Of String, String)()

#End Region

End Class
