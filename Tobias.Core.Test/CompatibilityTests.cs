using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tobias.Core.TestUtil.Db;
using Tobias.Core.Compatibility;

namespace Tobias.Core.Test
{
    [TestClass]
    public class CompatibilityTests
    {
        private static TestDatabaseBuilder m_testDatabaseBuilder = null;
        private static DbAccess m_dbAccess = null;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            m_testDatabaseBuilder = TestDatabaseBuilder.GetNewDatabaseBuilder();

            m_testDatabaseBuilder.TryDropScriptsAndDeleteDatabase();
            if (!m_testDatabaseBuilder.BuildDatabase())
            {
                Assert.IsTrue(false, "ClassInitialize(): Couldn't build database. All tests will fail.");
            }

            m_dbAccess = new DbAccess(m_testDatabaseBuilder.ConnectionString);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            m_dbAccess.ConnectionString = null;

            m_testDatabaseBuilder.TryDropScriptsAndDeleteDatabase();
        }

        [TestMethod]
        #region Datarows
        [DataRow("0", "-", "AB", "+")]
        [DataRow("0", "-", "AB", "-")]
        [DataRow("0", "-", "A", "+")]
        [DataRow("0", "-", "A", "-")]
        [DataRow("0", "-", "B", "+")]
        [DataRow("0", "-", "B", "-")]
        [DataRow("0", "-", "0", "+")]
        [DataRow("0", "-", "0", "-")]
        [DataRow("0", "+", "AB", "+")]
        [DataRow("0", "+", "A", "+")]
        [DataRow("0", "+", "B", "+")]
        [DataRow("0", "+", "0", "+")]
        [DataRow("B", "-", "AB", "+")]
        [DataRow("B", "-", "AB", "-")]
        [DataRow("B", "-", "B", "+")]
        [DataRow("B", "-", "B", "-")]
        [DataRow("B", "+", "AB", "+")]
        [DataRow("B", "+", "B", "+")]
        [DataRow("A", "-", "AB", "+")]
        [DataRow("A", "-", "AB", "-")]
        [DataRow("A", "-", "A", "+")]
        [DataRow("A", "-", "A", "-")]
        [DataRow("A", "+", "AB", "+")]
        [DataRow("A", "+", "A", "+")]
        [DataRow("AB", "-", "AB", "+")]
        [DataRow("AB", "-", "AB", "-")]
        [DataRow("AB", "+", "AB", "+")]
        #endregion
        public void CompatibilityTest_MatchingBloodGroups_ShouldPass(string donorAB0, string donorRh, string recipientAB0, string recipientRh)
        {
            string fName = "First Name";
            string lName = "Last Name";
            string SSN = "20010101";
            Donor donor = m_dbAccess.CreateDonor(fName, lName, SSN, donorAB0, donorRh);
            Recipient recipient = m_dbAccess.CreateRecipient(fName, lName, SSN, recipientAB0, recipientRh);

            CompatibilityResult res = CompatibilityCalculator.ComputeCompatibilityScore(donor, recipient);
            Assert.AreEqual(100, res.CompatibilityScore);
        }

        [TestMethod]
        #region Datarows
        [DataRow("0", "+", "AB", "-")]
        [DataRow("0", "+", "A", "-")]
        [DataRow("0", "+", "B", "-")]
        [DataRow("0", "+", "0", "-")]
        [DataRow("B", "-", "A", "+")]
        [DataRow("B", "-", "A", "-")]
        [DataRow("B", "-", "0", "+")]
        [DataRow("B", "-", "0", "-")]
        [DataRow("B", "+", "AB", "-")]
        [DataRow("B", "+", "A", "+")]
        [DataRow("B", "+", "A", "-")]
        [DataRow("B", "+", "B", "-")]
        [DataRow("B", "+", "0", "+")]
        [DataRow("B", "+", "0", "-")]
        [DataRow("A", "-", "B", "+")]
        [DataRow("A", "-", "B", "-")]
        [DataRow("A", "-", "0", "+")]
        [DataRow("A", "-", "0", "-")]
        [DataRow("A", "+", "AB", "-")]
        [DataRow("A", "+", "A", "-")]
        [DataRow("A", "+", "B", "+")]
        [DataRow("A", "+", "B", "-")]
        [DataRow("A", "+", "0", "+")]
        [DataRow("A", "+", "0", "-")]
        [DataRow("AB", "-", "A", "+")]
        [DataRow("AB", "-", "A", "-")]
        [DataRow("AB", "-", "B", "+")]
        [DataRow("AB", "-", "B", "-")]
        [DataRow("AB", "-", "0", "+")]
        [DataRow("AB", "-", "0", "-")]
        [DataRow("AB", "+", "AB", "-")]
        [DataRow("AB", "+", "A", "+")]
        [DataRow("AB", "+", "A", "-")]
        [DataRow("AB", "+", "B", "+")]
        [DataRow("AB", "+", "B", "-")]
        [DataRow("AB", "+", "0", "+")]
        [DataRow("AB", "+", "0", "-")]
        #endregion
        public void CompatibilityTest_NonMatchingBloodGroups_ShouldFail(string donorAB0, string donorRh, string recipientAB0, string recipientRh)
        {
            string fName = "First Name";
            string lName = "Last Name";
            string SSN = "20010101";
            Donor donor = m_dbAccess.CreateDonor(fName, lName, SSN, donorAB0, donorRh);
            Recipient recipient = m_dbAccess.CreateRecipient(fName, lName, SSN, recipientAB0, recipientRh);

            CompatibilityResult res = CompatibilityCalculator.ComputeCompatibilityScore(donor, recipient);
            Assert.AreEqual(0, res.CompatibilityScore);
        }
    }
}
