using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tobias.Core.TestUtil.Db;

namespace Tobias.Core.Test
{
    [TestClass]
    public class DbAccessConnectionTests
    {
        private static TestDatabaseBuilder m_testDatabaseBuilder;
        private static DbAccess m_dbAccess;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            m_testDatabaseBuilder = TestDatabaseBuilder.GetNewDatabaseBuilder();

            m_testDatabaseBuilder.BuildDatabase();
            m_dbAccess = new DbAccess(m_testDatabaseBuilder.ConnectionString);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            m_dbAccess.ConnectionString = null;

            m_testDatabaseBuilder.TryDropScriptsAndDeleteDatabase();
        }

        [TestMethod]
        public void CanConnect()
        {
            m_dbAccess.ConnectionString = m_testDatabaseBuilder.ConnectionString;

            //No exception in previous statement = success
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void CanConnectAndClose()
        {
            m_dbAccess.ConnectionString = m_testDatabaseBuilder.ConnectionString;
            Assert.AreEqual(m_testDatabaseBuilder.ConnectionString, m_dbAccess.ConnectionString);

            m_dbAccess.ConnectionString = null;
            Assert.IsTrue(String.IsNullOrEmpty(m_dbAccess.ConnectionString));

            //No exception in previous statement = success
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void CanConnectAndCloseTwice()
        {
            m_dbAccess.ConnectionString = m_testDatabaseBuilder.ConnectionString;
            m_dbAccess.ConnectionString = null;

            m_dbAccess.ConnectionString = m_testDatabaseBuilder.ConnectionString;
            m_dbAccess.ConnectionString = null;
            
            Assert.IsTrue(String.IsNullOrEmpty(m_dbAccess.ConnectionString));
        }

        [TestMethod]
        public void BadConnectionStringThrows()
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.ConnectionString = "BadConnectionString");
        }
    }
}
