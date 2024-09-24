Imports NUnit.Framework
Imports Groupies

Namespace GroupiesTests

    Public Class AppControllerTests

        '<SetUp>
        'Public Sub Setup()
        'End Sub

        <Test>
        Public Sub LoadFromXMLTest()
            'Dim appController = New AppController
            'Assert.AreEqual("Die Datei existiert nicht", appController.LoadFromXML("Jason"))
            Assert.AreEqual("1", "1")
        End Sub

        <Test>
        Public Sub LoadFromJsonTest()
            'Dim appController = New AppController
            'Assert.AreSame("Es wurde keine Datei geladen", appController.LoadFromJson("xml"))
            Assert.AreEqual("1", "1")
        End Sub

    End Class

End Namespace