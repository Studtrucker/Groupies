namespace GroupiesMSTestsCSharp
{
    using Groupies;
    [TestClass]
    public class UnitTest1
    {
  
        [TestMethod]
        public void TestMethod1()
        {
            //var appController = new AppController();

            Assert.AreEqual("Es wurde keine Datei geladen", AppController.LoadFromJson("Test"));
        }
    }
}