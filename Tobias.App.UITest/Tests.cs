using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.Queries;

namespace Tobias.App.UITest
{
    [TestFixture(Platform.Android)]
//    [TestFixture(Platform.iOS)]
    public class Tests
    {
        IApp app;
        Platform platform;

        public Tests(Platform platform)
        {
            this.platform = platform;
        }

        [SetUp]
        public void BeforeEachTest()
        {
            app = AppInitializer.StartApp(platform);
        }


        [Test]
        public void AssertTrue()
        {
            Assert.IsTrue(true);
        }

        [Test]
        public void StartRepl()
        {
            app.Repl();
        }
    }
}
