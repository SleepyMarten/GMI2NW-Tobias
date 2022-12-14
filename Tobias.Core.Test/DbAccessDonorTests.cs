using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tobias.Core.TestUtil.Db;

namespace Tobias.Core.Test
{
    [TestClass]
    public class DbAccessDonorTests
    {
        private static TestDatabaseBuilder m_testDatabaseBuilder = null;
        private static DbAccess m_dbAccess = null;

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
        public void CreateDonorTest()
        {
            string donorFirstName = "Donor First Name";
            string donorLastName = "Donor Last Name";
            string donorSSN = "18881201";
            string donorBloodGroupRh = "Rh+";
            string donorBloodGroupAB0 = "AB0";

            //TODO: uppdatera inte hos studenterna
            var donorCreated = m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0);

            Assert.IsNotNull(donorCreated);
            Assert.AreNotEqual(Guid.Empty, donorCreated.Guid);
            Assert.AreEqual(donorFirstName, donorCreated.FirstName);
            Assert.AreEqual(donorLastName, donorCreated.LastName);
            Assert.AreEqual(donorSSN, donorCreated.SocialSecurityNumber);
            Assert.AreEqual(donorBloodGroupRh, donorCreated.BloodGroupRh);
            Assert.AreEqual(donorBloodGroupAB0, donorCreated.BloodGroupAB0);
        }

        [TestMethod]
        [DataRow("", "", "", "", "")]
        [DataRow(null, null, null, null, null)]
        public void CreateDonorTest_EmptyInput_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("123456", "****", "20010101", "Rh+", "A")]
        [DataRow("!#¤", "123", "20010101", "Rh+", "A")]
        [DataRow("First", "", "20010101", "Rh+", "A")]
        [DataRow("", "Last", "20010101", "Rh+", "A")]
        public void CreateDonorTest_InvalidName_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "200101015", "Rh+", "A")]
        [DataRow("First Name", "Last Name", "2001010", "Rh+", "A")]
        [DataRow("First Name", "Last Name", "", "Rh+", "A")]
        [DataRow("First Name", "Last Name", "abcdefgh", "Rh+", "A")]
        public void CreateDonorTest_BadSSN_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "20010101", "", "A")]
        [DataRow("First Name", "Last Name", "20010101", null, "A")]
        [DataRow("First Name", "Last Name", "20010101", "0", "A")]
        [DataRow("First Name", "Last Name", "20010101", "Rh/", "A")]
        public void CreateDonorTest_BadInputBloodGroupRh_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "20010101", "Rh+", "")]
        [DataRow("First Name", "Last Name", "20010101", "Rh+", null)]
        [DataRow("First Name", "Last Name", "20010101", "Rh+", "1")]
        [DataRow("First Name", "Last Name", "20010101", "Rh+", "C")]
        public void CreateDonorTest_BadInputBloodGroupAB0_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0));
        }


        [TestMethod]
        public void CreateAndGetDonorTest()
        {
            string donorFirstName = "Donor Name 2";
            string donorLastName = "Donor Last Name";
            string donorSSN = "18881201";
            string donorBloodGroupRh = "Rh+";
            string donorBloodGroupAB0 = "AB0";

            var donorAdded = m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0);

            Donor donorRetrieved = m_dbAccess.GetDonor(donorAdded.Guid);

            Assert.AreEqual(donorAdded.Guid, donorRetrieved.Guid);
            Assert.AreEqual(donorFirstName, donorRetrieved.FirstName);
            Assert.AreEqual(donorLastName, donorRetrieved.LastName);
            Assert.AreEqual(donorSSN, donorRetrieved.SocialSecurityNumber);
            Assert.AreEqual(donorBloodGroupRh, donorRetrieved.BloodGroupRh);
            Assert.AreEqual(donorBloodGroupAB0, donorRetrieved.BloodGroupAB0);
        }

        [TestMethod]
        
        public void CreateAndUpdateDonorTest()
        {
            string donorFirstNameCreated = "Donor Name Created";
            string donorFirstNameUpdated = "Donor Name Updated";
            string donorLastNameCreated = "Donor Name Created";
            string donorLastNameUpdated = "Donor Name Updated";
            string donorSSNCreated = "18881201";
            string donorSSNUpdated = "18880112";
            string donorBloodGroupRhCreated = "Rh+";
            string donorBloodGroupRhUpdated = "Rh-";
            string donorBloodGroupAB0Created = "AB";
            string donorBloodGroupAB0Updated = "AB0";

            var donorAdded = m_dbAccess.CreateDonor(donorFirstNameCreated, donorLastNameCreated, donorSSNCreated, donorBloodGroupRhCreated, donorBloodGroupAB0Created);

            donorAdded.FirstName = donorFirstNameUpdated;
            donorAdded.LastName = donorLastNameUpdated;
            donorAdded.SocialSecurityNumber = donorSSNUpdated;
            donorAdded.BloodGroupRh = donorBloodGroupRhUpdated;
            donorAdded.BloodGroupAB0 = donorBloodGroupAB0Updated;

            m_dbAccess.UpdateDonor(donorAdded);

            Donor donorRetrieved = m_dbAccess.GetDonor(donorAdded.Guid);

            Assert.AreEqual(donorAdded.Guid, donorRetrieved.Guid);
            Assert.AreEqual(donorFirstNameUpdated, donorRetrieved.FirstName);
            Assert.AreEqual(donorLastNameUpdated, donorRetrieved.LastName);
            Assert.AreEqual(donorSSNUpdated, donorRetrieved.SocialSecurityNumber);
            Assert.AreEqual(donorBloodGroupRhUpdated, donorRetrieved.BloodGroupRh);
            Assert.AreEqual(donorBloodGroupAB0Updated, donorRetrieved.BloodGroupAB0);
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "Updated First", "", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "", "Updated Last", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "Updated First", "123", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", ",.", "Updated Last", "18881201", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", null, "Updated Last", "18881201", "Rh+", "AB")]
        public void CreateAndUpdateDonor_BadNameInputs_ShouldThrowException(string donorFirstNameCreated, string donorFirstNameUpdated, string donorLastNameCreated, string donorLastNameUpdated, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            var donorAdded = m_dbAccess.CreateDonor(donorFirstNameCreated, donorLastNameCreated, donorSSN, donorBloodGroupRh, donorBloodGroupAB0);

            donorAdded.FirstName = donorFirstNameUpdated;
            donorAdded.LastName = donorLastNameUpdated;

            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.UpdateDonor(donorAdded));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "18881201", "", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", null, "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", "abcdefgh", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", ",._,._,.", "Rh+", "AB")]
        [DataRow("First Name", "Last Name", "18881201", "0", "Rh+", "AB")]
        public void CreateAndUpdateDonor_BadSSNInputs_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorSSNUpdated, string donorBloodGroupRh, string donorBloodGroupAB0)
        {
            var donorAdded = m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0);

            donorAdded.SocialSecurityNumber = donorSSNUpdated;

            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.UpdateDonor(donorAdded));
        }

        [TestMethod]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "", "")]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", null, null)]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "0", "0")]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "A", "Rh+")]
        [DataRow("First Name", "Last Name", "18881201", "Rh+", "AB", "Rh/", "C")]
        public void CreateAndUpdateDonor_BadBloodGroupInputs_ShouldThrowException(string donorFirstName, string donorLastName, string donorSSN, string donorBloodGroupRh, string donorBloodGroupAB0, string donorBloodGroupRhUpdated, string donorBloodGroupAB0Updated)
        {
            var donorAdded = m_dbAccess.CreateDonor(donorFirstName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0);

            donorAdded.BloodGroupRh = donorBloodGroupRhUpdated;
            donorAdded.BloodGroupAB0= donorBloodGroupAB0Updated;

            Assert.ThrowsException<ArgumentException>(() => m_dbAccess.UpdateDonor(donorAdded));
        }

        [TestMethod]
        public void CreateAndDeleteDonor_ShouldPass()
        {
            string donorName = "Donor Name Created";
            string donorLastName = "Donor Name Created";
            string donorSSN = "18881201";
            string donorBloodGroupRh = "Rh+";
            string donorBloodGroupAB0 = "AB0";

            var donorAdded = m_dbAccess.CreateDonor(donorName, donorLastName, donorSSN, donorBloodGroupRh, donorBloodGroupAB0);

            Donor donorRetrieved = m_dbAccess.GetDonor(donorAdded.Guid);
            Assert.IsNotNull(donorRetrieved);

            m_dbAccess.DeleteDonor(donorAdded.Guid);

            Assert.IsNull(m_dbAccess.GetDonor(donorAdded.Guid));
        }

        [TestMethod]
        public void DeleteDonor_NonExistingDonor_ShouldThrowException()
        {
            Assert.ThrowsException<Exception>(() => m_dbAccess.DeleteDonor(Guid.NewGuid()));
        }

        [TestMethod]
        public void GetListOfDonorsTest()
        {
            //Preparation
            string donorName = "Donor Number ";

            for (int i = 0; i < 10; i++)
            {
                m_dbAccess.CreateDonor(donorName + i.ToString(), donorName + i.ToString(), "19750205", "Rh+", "AB");
            }

            //Invoke SUT
            var donorsRetrieved = m_dbAccess.GetDonorList();
            
            //Assert result: number of items in list
            //Note: since other tests have also added Donors, we assert that there are 13 Donors.
            Assert.AreEqual(13, donorsRetrieved.Count);
        }

        [TestMethod]
        public void DeleteNonexistingDonorThrows()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => m_dbAccess.DeleteDonor(Guid.Empty));
        }
    }
}
