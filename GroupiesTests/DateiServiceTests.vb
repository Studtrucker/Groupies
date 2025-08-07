Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Groupies
Imports Groupies.Services
Imports System.IO


<TestClass>
Public Class DateiServiceTests

    <TestMethod>
    Sub NeueDateiErstellenTest()
        DateiService.AktuellerClub = Nothing
        DateiService.AktuelleDatei = Nothing

        DateiService.NeueDateiErstellen()
        Assert.AreEqual("MeinClub", DateiService.AktuellerClub.ClubName)
        Assert.IsNotNull(DateiService.AktuelleDatei)
    End Sub


    <TestMethod>
    Sub DateiSpeichernTest()

        DateiService.AktuellerClub = Nothing
        DateiService.AktuelleDatei = Nothing

        DateiService.DateiLaden()
        DateiService.DateiSpeichern()

        Assert.IsTrue(DateiService.AktuelleDatei.Exists)

    End Sub

    <TestMethod>
    Sub DateiSpeichernAlsTest()

        DateiService.AktuellerClub = Nothing
        DateiService.AktuelleDatei = Nothing

        DateiService.DateiLaden()
        Dim Filename = "MeinClub1.ski"
        DateiService.DateiSpeichernAls(Filename)

        Assert.AreEqual(Filename, DateiService.AktuelleDatei.Name)
        Assert.IsTrue(DateiService.AktuelleDatei.Exists)

    End Sub

    <TestMethod>
    Sub DateiLadenTest()

        Dim mFileInfo = DateiService.GetFileInfo(String.Empty, "Test Datei laden", GetFileInfoMode.Laden)

        DateiService.DateiLaden(mFileInfo.FullName)
        DateiService.DateiSchliessen()

        Assert.IsNull(DateiService.AktuelleDatei)
        Assert.IsNull(DateiService.AktuellerClub)

    End Sub

    <TestMethod>
    Sub DateiSchliessenTest()

        Dim mFileInfo = DateiService.GetFileInfo(String.Empty, "Test Datei schliessen", GetFileInfoMode.Laden)

        DateiService.DateiLaden(mFileInfo.FullName)

        Assert.AreEqual(mFileInfo, DateiService.AktuelleDatei)

    End Sub

    <TestMethod>
    Sub LoadmRUSortedListMenuTest()

        DateiService.LadeMeistVerwendeteDateienInSortedList()
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei1.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei5.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei3.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei9.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei13.ski")
        DateiService.SpeicherZuletztVerwendeteDateienSortedList()
        DateiService.LadeMeistVerwendeteDateienInSortedList()
        Assert.AreEqual("c:\User\Datei13.ski", DateiService.ZuletztVerwendeteDateienSortedList(5))
        Assert.AreEqual("c:\User\Datei1.ski", DateiService.ZuletztVerwendeteDateienSortedList(1))
        Assert.AreEqual("c:\User\Datei3.ski", DateiService.ZuletztVerwendeteDateienSortedList(3))

    End Sub

    <TestMethod>
    Sub QueueMostRecentFilenameTest()
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei1.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei5.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei3.ski")
        DateiService.SchreibeZuletztVerwendeteDateienSortedList("c:\User\Datei9.ski")
        Assert.AreEqual(4, DateiService.ZuletztVerwendeteDateienSortedList.Count)
    End Sub

End Class


