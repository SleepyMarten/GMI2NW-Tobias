using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Tobias.Core.TestUtil.Db;

namespace Tobias.Core.TestUtil.Test.Db
{
    [TestClass]
    public class TestDatabaseBuilderTests
    {
        #region Initialize and Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            TryDeleteDbDirectory();
        }

        private static void TryDeleteDbDirectory(int timeOutMs = 5000)
        {
            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder();

            var watch = Stopwatch.StartNew();
            bool success = false;

            //By experience, we may not be able to delete the directory immediately
            //if we previously connected to the database in the folder.
            while (!success && watch.ElapsedMilliseconds < timeOutMs)
            {
                try
                {
                    if (Directory.Exists(SUT.LocalDbDirectory))
                    {
                        Directory.Delete(SUT.LocalDbDirectory, true);
                    }
                    success = true;
                }
                catch 
                {
                    Thread.Sleep(10);
                }
            }
        }

        #endregion

        [TestMethod]
        public void GetTestDatabaseBuilder_WithExplicitDirName()
        {
            string expectedDirName = "MyOwnName";

            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder(expectedDirName);

            //Assertion
            //Extract given name from path
            var directories = SUT.LocalDbDirectory.Split(@"\");
            var actualDirName = directories[directories.Length - 1];
            Assert.AreEqual(expectedDirName, actualDirName);
        }

        [TestMethod]
        public void GetTestDatabaseBuilder_OmittingDirName()
        {
            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder();

            string expectedDirName = GetType().Name + ".cs";

            //Assertion
            //Extract given name from path
            var directories = SUT.LocalDbDirectory.Split(@"\");
            var actualDirName = directories[directories.Length - 1];
            Assert.AreEqual(expectedDirName, actualDirName);
        }

        [TestMethod]
        public void GetTestDatabaseBuilder_WithEmptyStringAsDirName()
        {
            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder("");

            string localDbDirectoryRoot= Path.GetFullPath(TestDatabaseBuilder.LocalDbDirectoryRoot);

            //Assertion
            //Extract given name from path
            Assert.AreEqual(localDbDirectoryRoot, SUT.LocalDbDirectory);
        }

        [TestMethod]
        public void GetTestDatabaseBuilder_WithNullAsDirName()
        {
            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder(null);

            string localDbDirectoryRoot = Path.GetFullPath(TestDatabaseBuilder.LocalDbDirectoryRoot);

            //Assertion
            //Extract given name from path
            Assert.AreEqual(localDbDirectoryRoot, SUT.LocalDbDirectory);
        }

        [TestMethod]
        public void DeleteDb_WithoutErrors()
        {
            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder();

            //Setup
            if (!Directory.Exists(SUT.LocalDbDirectory))
            {
                Directory.CreateDirectory(SUT.LocalDbDirectory);
            }

            //Stimulation
            var success = SUT.DeleteDatabase();

            //Assertion
            Assert.IsTrue(success);
            Assert.IsTrue(!Directory.Exists(SUT.LocalDbDirectory));
        }

        [TestMethod]
        public void Scenario_CreateDb_DeleteDb_WithoutErrors()
        {
            TestDatabaseBuilder SUT = TestDatabaseBuilder.GetNewDatabaseBuilder();

            //Observation/Assertion before stimulation
            Assert.IsTrue(!Directory.Exists(SUT.LocalDbDirectory));

            //Stimulation-->Assertion #1
            var createSuccess = SUT.BuildDatabase();
            
            Assert.IsTrue(createSuccess);
            Assert.IsTrue(Directory.Exists(SUT.LocalDbDirectory));
            Assert.AreEqual(1, Directory.GetFiles(SUT.LocalDbDirectory, "*.mdf").Length);
            Assert.AreEqual(1, Directory.GetFiles(SUT.LocalDbDirectory, "*.ldf").Length);
            Assert.IsTrue(Directory.GetFiles(SUT.LocalDbDirectory, "*.sql").Length > 0);

            //Stimulation-->Assertion #2
            var deleteSuccess = SUT.DeleteDatabase();
            
            Assert.IsTrue(deleteSuccess);
            Assert.IsTrue(!Directory.Exists(SUT.LocalDbDirectory));
        }
    }
}
