Imports System.Collections.ObjectModel
Imports Skiclub.Entities

Namespace Services

    Module CurrentDataService

        Public Property Skiclub As Skiclub.Entities.Skiclub

        Public Function SaveSkiclubObjects() As Entities.Skiclub
            Dim InstrList = New InstructorCollection
            Skiclub.Instructorlist.Where(Function(x) x.SaveOrDisplay = True).ToList.ForEach(Sub(x) InstrList.Add(x))
            Skiclub.Instructorlist = InstrList
            Dim Levellist = New LevelCollection
            Skiclub.Levellist.Where(Function(x) x.SaveOrDisplay = True).ToList.ForEach(Sub(x) Levellist.Add(x))
            Skiclub.Levellist = Levellist
            Return Skiclub
        End Function

        Public Function SortedInstructorsDisplayable() As InstructorCollection
            Dim sorted = New InstructorCollection
            Dim a = Skiclub.Instructorlist.Where(Function(y) y.SaveOrDisplay = True).ToList
            Dim b = a.OrderBy(Of String)(Function(x) x.InstructorFullName).ToList
            b.ForEach(Sub(i) sorted.Add(i))
            Return sorted
        End Function

        Public Function SortedInstructorsAvailable() As InstructorCollection
            Dim sorted = New InstructorCollection
            Skiclub.Instructorlist.Where(Function(y) y.IsAvailable).OrderBy(Of String)(Function(x) x.InstructorFullName).ToList.ForEach(Sub(i) sorted.Add(i))
            Return sorted
        End Function

        Public Function SortedLevels() As LevelCollection
            Dim Sorted = New LevelCollection
            Skiclub.Levellist.OrderBy(Function(x) x.SortNumber).ToList.ForEach(Sub(i) Sorted.Add(i))
            Return Sorted
        End Function

        Public Function SortedLevelsDisplayables() As LevelCollection
            Dim Sorted = New LevelCollection
            Skiclub.Levellist.Where(Function(d) d.SaveOrDisplay = True).OrderBy(Of String)(Function(x) x.SortNumber).ToList.ForEach(Sub(i) Sorted.Add(i))
            Return Sorted
        End Function

    End Module

End Namespace
