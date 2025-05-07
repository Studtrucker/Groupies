Public Interface IModel

    ''' <summary>
    ''' Die ID des Models
    ''' </summary>
    ''' <returns></returns>
    Property Ident As Guid

    '''' <summary>
    '''' Speichert das Model
    '''' </summary>
    '''' <remarks>
    '''' Das Model wird in der Datenbank gespeichert.
    '''' </remarks>
    '''' <exception cref="NotImplementedException">Wenn die Methode nicht implementiert ist</exception>
    '''' <exception cref="InvalidOperationException">Wenn das Model nicht gespeichert werden kann</exception>
    'Sub save()
    '    ' Implement the logic to save the model
    'End Sub

    '''' <summary>
    '''' Löscht das Model
    '''' </summary>
    '''' <remarks>
    '''' Das Model wird aus der Datenbank gelöscht.
    '''' </remarks>
    '''' <exception cref="NotImplementedException">Wenn die Methode nicht implementiert ist</exception>
    '''' <exception cref="InvalidOperationException">Wenn das Model nicht geladen werden kann</exception>
    'Sub delete(Ident As Guid)
    '    ' Implement the logic to delete the model
    'End Sub

    '''' <summary>
    '''' Lädt das Model
    '''' </summary>
    '''' <remarks>
    '''' Das Model wird aus der Datenbank geladen.
    '''' </remarks>
    '''' <exception cref="NotImplementedException">Wenn die Methode nicht implementiert ist</exception>
    '''' <exception cref="InvalidOperationException">Wenn das Model nicht geladen werden kann</exception>
    'Sub load(Ident As Guid)
    '    ' Implement the logic to load the model
    'End Sub

End Interface
