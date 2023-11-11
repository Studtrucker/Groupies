Imports System.Collections.ObjectModel
Imports Skiclub.Entities

Namespace Services

    Module CurrentDataService

        Public Property Skiclub As Skiclub.Entities.Skiclub

        Public Function SaveSkiclubObjects() As Entities.Skiclub
            Dim InstrList = New InstructorCollection
            Skiclub.Instructorlist.Where(Function(x) x.Save = True).ToList.ForEach(Sub(x) InstrList.Add(x))
            Skiclub.Instructorlist = InstrList
            Dim Levellist = New LevelCollection
            Skiclub.Levellist.Where(Function(x) x.Save = True).ToList.ForEach(Sub(x) Levellist.Add(x))
            Skiclub.Levellist = Levellist
            Return Skiclub
        End Function


    End Module

End Namespace
