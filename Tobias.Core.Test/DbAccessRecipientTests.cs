using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tobias.Core.TestUtil.Db;

namespace Tobias.Core.Test
{
    [TestClass]
    public class DbAccessRecipientTests
    {
        private static TestDatabaseBuilder m_testDatabaseBuilder = null;
        private static DbAccess m_dbAccess;

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            m_testDatabaseBuilder = TestDatabaseBuilder.GetNewDatabaseBuilder();

            m_testDatabaseBuilder.TryDropScriptsAndDeleteDatabase();
            if(!m_testDatabaseBuilder.BuildDatabase())
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
        [DataRow("", "", "", "", "")]
        [DataRow(null, null, null, null, null)]
        public void CreateRecipientTest_EmptyInput_ShouldThrowException(string recipientFirstName, string recipientLastName, string recipientSSN, string recipientBloodGroupRh, string recipientBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateRecipient(recipientFirstName, recipientLastName, recipientSSN, recipientBloodGroupRh, recipientBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("123456", "****", "20010101", "Rh+", "A")]
        [DataRow("!#¤", "123", "20010101", "Rh+", "A")]
        [DataRow("First", "", "20010101", "Rh+", "A")]
        [DataRow("", "Last", "20010101", "Rh+", "A")]
        public void CreateRecipientTest_InvalidName_ShouldThrowException(string recipientFirstName, string recipientLastName, string recipientSSN, string recipientBloodGroupRh, string recipientBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateRecipient(recipientFirstName, recipientLastName, recipientSSN, recipientBloodGroupRh, recipientBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "200101015", "Rh+", "A")]
        [DataRow("First Name", "Last Name", "2001010", "Rh+", "A")]
        [DataRow("First Name", "Last Name", "", "Rh+", "A")]
        [DataRow("First Name", "Last Name", "abcdefgh", "Rh+", "A")]
        public void CreateRecipientTest_BadSSN_ShouldThrowException(string recipientFirstName, string recipientLastName, string recipientSSN, string recipientBloodGroupRh, string recipientBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateRecipient(recipientFirstName, recipientLastName, recipientSSN, recipientBloodGroupRh, recipientBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "20010101", "Rh/", "A")]
        public void CreateRecipientTest_BadInputBloodGroupRh_ShouldThrowException(string recipientFirstName, string recipientLastName, string recipientSSN, string recipientBloodGroupRh, string recipientBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateRecipient(recipientFirstName, recipientLastName, recipientSSN, recipientBloodGroupRh, recipientBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "20010101", "Rh+", "C")]
        public void CreateRecipientTest_BadInputBloodGroupAB0_ShouldThrowException(string recipientFirstName, string recipientLastName, string recipientSSN, string recipientBloodGroupRh, string recipientBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateRecipient(recipientFirstName, recipientLastName, recipientSSN, recipientBloodGroupRh, recipientBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "20010101", "Rh+", "O")]
        public void CreateRecipientTest_ValidInput_ShouldNotThrowException(string recipientFirstName, string recipientLastName, string recipientSSN, string recipientBloodGroupRh, string recipientBloodGroupAB0)
        {
            m_dbAccess.CreateRecipient(recipientFirstName, recipientLastName, recipientSSN, recipientBloodGroupRh, recipientBloodGroupAB0);
            Assert.IsTrue(true);
        }


        [TestMethod]
        public void CreateRecipientTest()
        {
            string firstName = "Ressippient First Name";
            string recipientLastName = "Last Name";

            var recipient = m_dbAccess.CreateRecipient(firstName, recipientLastName, "1876", "Rh+", "AB");

            Assert.IsNotNull(recipient);
            Assert.AreNotEqual(Guid.Empty, recipient.Guid);
            Assert.AreEqual(firstName, recipient.FirstName);
            Assert.AreEqual(recipientLastName, recipient.LastName);
        }

        [TestMethod]
        public void CreateAndUpdateRecipientTest()
        {
            string recipientFirstNameCreated = "Recipient Name Created";
            string recipientFirstNameUpdated = "Recipient Name Updated";
            string recipientLastNameCreated = "Recipient Name Created";
            string recipientLastNameUpdated = "Recipient Name Updated";
            string recipientSSNCreated = "18881201";
            string recipientSSNUpdated = "18880112";
            string recipientBloodGroupRhCreated = "Rh+";
            string recipientBloodGroupRhUpdated = "Rh-";
            string recipientBloodGroupAB0Created = "AB";
            string recipientBloodGroupAB0Updated = "AB0";

            var recipientAdded = m_dbAccess.CreateRecipient(recipientFirstNameCreated, recipientLastNameCreated, recipientSSNCreated, recipientBloodGroupRhCreated, recipientBloodGroupAB0Created);

            recipientAdded.FirstName = recipientFirstNameUpdated;
            recipientAdded.LastName = recipientLastNameUpdated;
            recipientAdded.SocialSecurityNumber = recipientSSNUpdated;
            recipientAdded.BloodGroupRh = recipientBloodGroupRhUpdated;
            recipientAdded.BloodGroupAB0 = recipientBloodGroupAB0Updated;

            m_dbAccess.UpdateRecipient(recipientAdded);

            Recipient recipientRetrieved = m_dbAccess.GetRecipient(recipientAdded.Guid);

            Assert.AreEqual(recipientAdded.Guid, recipientRetrieved.Guid);
            Assert.AreEqual(recipientFirstNameUpdated, recipientRetrieved.FirstName);
            Assert.AreEqual(recipientLastNameUpdated, recipientRetrieved.LastName);
            Assert.AreEqual(recipientSSNUpdated, recipientRetrieved.SocialSecurityNumber);
            Assert.AreEqual(recipientBloodGroupRhUpdated, recipientRetrieved.BloodGroupRh);
            Assert.AreEqual(recipientBloodGroupAB0Updated, recipientRetrieved.BloodGroupAB0);
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "Updated First", "", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "", "Updated Last", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "Updated First", "123", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", ",.", "Updated Last", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", null, "Updated Last", "18881201", "Rh+", "AB")]
        public void CreateAndUpdateRecipient_BadNameInputs_ShouldThrowException(string firstName, string firstNameUpdated, string lastName, string lastNameUpdated, string SSN, string bloodGroupRh, string bloodGroupAB0)
        {
            var recipientAdded = m_dbAccess.CreateDonor(firstName, lastName, SSN, bloodGroupRh, bloodGroupAB0);

            recipientAdded.FirstName = firstNameUpdated;
            recipientAdded.LastName = lastNameUpdated;

            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.UpdateDonor(recipientAdded));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "18881201", "", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", null, "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", "abcdefgh", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", ",._,._,.", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", "0", "Rh+", "AB")]
        public void CreateAndUpdateRecipient_BadSSNInputs_ShouldThrowException(string firstName, string lastName, string SSN, string SSNUpdated, string bloodGroupRh, string bloodGroupAB0)
        {
            var recipientAdded = m_dbAccess.CreateDonor(firstName, lastName, SSN, bloodGroupRh, bloodGroupAB0);

            recipientAdded.SocialSecurityNumber = SSNUpdated;

            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.UpdateDonor(recipientAdded));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "", "")]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", null, null)]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "0", "0")]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "A", "Rh+")]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "Rh/", "C")]
        public void CreateAndUpdateRecipient_BadBloodGroupInputs_ShouldThrowException(string firstName, string lastName, string SSN, string bloodGroupRh, string bloodGroupAB0, string bloodGroupRhUpdated, string bloodGroupAB0Updated)
        {
            var recipientAdded = m_dbAccess.CreateDonor(firstName, lastName, SSN, bloodGroupRh, bloodGroupAB0);

            recipientAdded.BloodGroupRh = bloodGroupRhUpdated;
            recipientAdded.BloodGroupAB0 = bloodGroupAB0Updated;

            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.UpdateDonor(recipientAdded));
        }

        [TestMethod]
        public void CreateAndDeleteRecipient_ShouldPass()
        {
            string firstName = "First Name";
            string lastName = "Last Name";
            string SSN = "18881201";
            string bloodGroupRh = "Rh+";
            string bloodGroupAB0 = "AB0";

            var recipientAdded = m_dbAccess.CreateRecipient(firstName, lastName, SSN, bloodGroupRh, bloodGroupAB0);

            Recipient recipientRetrieved = m_dbAccess.GetRecipient(recipientAdded.Guid);
            Assert.IsNotNull(recipientRetrieved);

            m_dbAccess.DeleteRecipient(recipientAdded.Guid);

            Assert.IsNull(m_dbAccess.GetRecipient(recipientAdded.Guid));
        }

        [TestMethod]
        public void DeleteRecipient_NonExistingRecipient_ShouldThrowException()
        {
            Assert.ThrowsException<Exception>(() => m_dbAccess.DeleteRecipient(Guid.NewGuid()));
        }
    }
}
