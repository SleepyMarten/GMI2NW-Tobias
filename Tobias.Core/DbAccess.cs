using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace Tobias.Core
{
    /// <summary>
    /// Class responsible for database access.
    /// </summary>
    public class DbAccess
    {
        #region Constants: database names
        #region Table names
        private const string DonorTableName = "DONOR";
        private const string RecipientTableName = "RECIPIENT";
        #endregion
        #region Column names
        private const string PrimaryKeyColumnName = "PK";

        #region Column names for Donor
        private const string DonorFirstNameColumnName = "FIRST_NAME";
        private const string DonorLastNameColumnName = "LAST_NAME";
        private const string DonorSocialSecurityNumberColumnName = "SOCIAL_SECURITY_NUMBER";
        private const string DonorBloodGroupAB0ColumnName = "BLOOD_GROUP_AB0";
        private const string DonorBloodGroupRhColumnName = "BLOOD_GROUP_RH";
        #endregion

        #region Column names for Recipient
        private const string RecipientFirstNameColumnName = "FIRST_NAME";
        private const string RecipientLastNameColumnName = "LAST_NAME";
        private const string RecipientSocialSecurityNumberColumnName = "SOCIAL_SECURITY_NUMBER";
        private const string RecipientBloodGroupAB0ColumnName = "BLOOD_GROUP_AB0";
        private const string RecipientBloodGroupRhColumnName = "BLOOD_GROUP_RH";
        #endregion


        #region Values used only for my own developer tests
        private const int TestingShoeSize = 37;
        #endregion
        #endregion
        #endregion

        #region Members
        private SqlConnection m_connection = null;
        #endregion

        #region Ctor(s)
        public DbAccess(string connectionString = null)
        {
            ConnectionString = connectionString;
        }
        #endregion
        #region Public properties
        /// <summary>
        /// The connection string to the database. 
        /// By setting the connection string, any previous connection will be closed, and a new connection will be opened.
        /// By setting the connection string to null or String.Empty, the connection (if any) will be closed.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return m_connection == null ? String.Empty : m_connection.ConnectionString;
            }
            set
            {
                m_connection?.Close();
                m_connection = ((value == null) ? null : new SqlConnection(value));
                m_connection?.Open();
            }
        }
        #endregion

        #region Public methods

        #region Donor
        /// <summary>
        /// Creates a new Donor in the database.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="bloodGroupRh"></param>
        /// <param name="bloodGroupAB0"></param>
        /// <returns>The created <c>Donor</c>.</returns>
        public Donor CreateDonor(string firstName, string lastName,
                                        string socialSecurityNumber,
                                        string bloodGroupRh, string bloodGroupAB0)
        {
            return CreateDonor(Guid.NewGuid(), firstName, lastName,
                               socialSecurityNumber,
                               bloodGroupRh, bloodGroupAB0);
        }

        /// <summary>
        /// Creates a new Donor in the database.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="bloodGroupRh"></param>
        /// <param name="bloodGroupAB0"></param>
        /// <returns>The created <c>Donor</c>.</returns>
        public Donor CreateDonor(Guid guid, string firstName, string lastName, 
                                 string socialSecurityNumber,
                                 string bloodGroupRh, string bloodGroupAB0)
        {
            if (guid == Guid.Empty)
            {
                guid = Guid.NewGuid();
            }
            Donor newDonor = new Donor(guid, firstName, lastName,
                                       socialSecurityNumber,
                                       bloodGroupRh, bloodGroupAB0);

            var transaction = m_connection.BeginTransaction();

            try
            {
                string commandString = String.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}) VALUES('{7}', '{8}', '{9}', '{10}', '{11}', '{12}')",
                    DonorTableName,
                    PrimaryKeyColumnName, DonorFirstNameColumnName, DonorLastNameColumnName,
                    DonorSocialSecurityNumberColumnName, DonorBloodGroupRhColumnName, DonorBloodGroupAB0ColumnName,
                    guid, firstName, lastName,
                    socialSecurityNumber,
                    bloodGroupRh, bloodGroupAB0);

                var command = new SqlCommand(commandString, m_connection, transaction);

                var result = command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();
            return newDonor;
        }

        /// <summary>
        /// Creates a new Donor in the database.
        /// </summary>
        /// <param name="donor">A <c>Donor</c> with members assigned.</param>
        /// <remarks>The <c>Donor</c> will be assigned a Guid; the Guid of the input parameter will be ignored.</remarks>
        /// <returns>The created <c>Donor</c>.</returns>
        public Donor CreateDonor(Donor donor)
        {
            return CreateDonor(donor.Guid, donor.FirstName, donor.LastName, 
                               donor.SocialSecurityNumber,
                               donor.BloodGroupRh, donor.BloodGroupAB0);
        }

        /// <summary>
        /// Gets a <c>Donor</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="transaction">A valid transaction, if the get occurs inside another transaction, or null if there is no outer transaction.</param>
        /// <returns>The <c>Donor</c> retreived from the database.</returns>
        private Donor GetDonor(Guid guid, SqlTransaction transaction)
        {
            Donor donor;

            string commandString = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'",
                                                DonorTableName, PrimaryKeyColumnName, guid);

            var command = transaction == null ? new SqlCommand(commandString, m_connection)
                                              : new SqlCommand(commandString, m_connection, transaction);
            var donorDataReader = command.ExecuteReader();
            if (donorDataReader.Read())
            {
                string donorFirstName = donorDataReader.GetString(1);
                string donorLastName = donorDataReader.GetString(2);
                string donorSocialSecurityNumber = donorDataReader.GetString(3);
                string donorBloodGroupRh = donorDataReader.GetString(4);
                string donorBloodGroupAB0 = donorDataReader.GetString(5);

                donor = new Donor(guid, donorFirstName, donorLastName,
                                  donorSocialSecurityNumber,
                                  donorBloodGroupRh, donorBloodGroupAB0);
            }
            else
            {
                donor = null;
            }
            donorDataReader.Close();

            return donor;
        }

        /// <summary>
        /// Gets a <c>Donor</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="transaction">A valid transaction, if the get occurs inside another transaction, or null if there is no outer transaction.</param>
        /// <returns>true if a <c>Donor</c> with Guid = <paramref name="guid"/> exists in the database; false otherwise.</returns>
        private bool DonorExists(Guid guid, SqlTransaction transaction)
        {
            string commandString = String.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'",
                                                DonorTableName, PrimaryKeyColumnName, guid);

            var command = transaction == null ? new SqlCommand(commandString, m_connection)
                                              : new SqlCommand(commandString, m_connection, transaction);
            var nOfDonors = (int) command.ExecuteScalar();

            return (nOfDonors > 0);
        }

        /// <summary>
        /// Gets a <c>Donor</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if a <c>Donor</c> with Guid = <paramref name="guid"/> exists in the database; false otherwise.</returns>
        public bool DonorExists(Guid guid)
        {
            return DonorExists(guid, null);
        }

        /// <summary>
        /// Gets a <c>Donor</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>The <c>Donor</c> retreived from the database.</returns>
        public Donor GetDonor(Guid guid)
        {
            return GetDonor(guid, null);
        }


        /// <summary>
        /// Gets all <c>Donor</c>s from the database
        /// </summary>
        /// <param name="transaction">A valid transaction, if the get occurs inside another transaction, or null if there is no outer transaction.</param>
        /// <returns>The <c>Donor</c> retreived from the database.</returns>
        private List<Donor> GetDonorList(SqlTransaction transaction)
        {
            List<Donor> donors = new List<Donor>();

            string commandString = String.Format("SELECT * FROM {0}", DonorTableName);

            var command = transaction == null ? new SqlCommand(commandString, m_connection)
                                              : new SqlCommand(commandString, m_connection, transaction);
            var donorDataReader = command.ExecuteReader();
            while(donorDataReader.Read())
            {
                Donor donor;

                Guid guid = donorDataReader.GetGuid(0);
                string donorFirstName = donorDataReader.GetString(1);
                string donorLastName = donorDataReader.GetString(2);
                string donorSocialSecurityNumber = donorDataReader.GetString(3);
                string donorBloodGroupRh = donorDataReader.GetString(4);
                string donorBloodGroupAB0 = donorDataReader.GetString(5);

                donor = new Donor(guid, donorFirstName, donorLastName,
                                  donorSocialSecurityNumber,
                                  donorBloodGroupRh, donorBloodGroupAB0);

                donors.Add(donor);
            }
            donorDataReader.Close();

            return donors;
        }

        /// <summary>
        /// Gets all <c>Donor</c>s from the database
        /// </summary>
        /// <returns>The <c>Donor</c> retreived from the database.</returns>
        public List<Donor> GetDonorList()
        {
            return GetDonorList(null);
        }

        /// <summary>
        /// Updates a <c>Donor</c> in the database with the new values of <paramref name="donor"/>.
        /// </summary>
        /// <param name="donor"></param>
        /// <returns>The <c>Donor</c>'s updated values, as read again from the database.</returns>
        public Donor UpdateDonor(Donor donor)
        {
            var transaction = m_connection.BeginTransaction();

            try
            {
                string commandString = String.Format("UPDATE {0} SET {1} = '{2}', {3} = '{4}', {5} = '{6}', {7} = '{8}', {9} = '{10}' WHERE {11} = '{12}'",
                    DonorTableName,
                    DonorFirstNameColumnName, donor.FirstName,
                    DonorLastNameColumnName, donor.LastName,
                    DonorSocialSecurityNumberColumnName, donor.SocialSecurityNumber,
                    DonorBloodGroupRhColumnName, donor.BloodGroupRh,
                    DonorBloodGroupAB0ColumnName, donor.BloodGroupAB0,
                    PrimaryKeyColumnName, donor.Guid);

                var command = new SqlCommand(commandString, m_connection, transaction);

                var result = command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();

            donor = GetDonor(donor.Guid);
            return donor;
        }

        /// <summary>
        /// Deletes the <c>Donor</c> with guid == <paramref name="guid"/> from the database.
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteDonor(Guid guid)
        {
            var transaction = m_connection.BeginTransaction();

            try
            {
                if (GetDonor(guid, transaction) == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(guid), $"No Donor with guid = '{guid}' exist in the database.");
                }

                string commandString = String.Format("DELETE FROM {0} WHERE {1} = '{2}'",
                    DonorTableName,
                    PrimaryKeyColumnName, guid);

                var command = new SqlCommand(commandString, m_connection, transaction);

                var result = command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();
        }

        #endregion


        #region Recipient
        /// <summary>
        /// Creates a new Recipient in the database.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="bloodGroupRh"></param>
        /// <param name="bloodGroupAB0"></param>
        /// <returns>The created <c>Recipient</c>.</returns>
        public Recipient CreateRecipient(string firstName, string lastName,
                                                string socialSecurityNumber,
                                                string bloodGroupRh, string bloodGroupAB0)
        {
            return CreateRecipient(Guid.NewGuid(), firstName, lastName,
                                   socialSecurityNumber,
                                   bloodGroupRh, bloodGroupAB0);
        }

        /// <summary>
        /// Creates a new Recipient in the database.
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="socialSecurityNumber"></param>
        /// <param name="bloodGroupRh"></param>
        /// <param name="bloodGroupAB0"></param>
        /// <returns>The created <c>Recipient</c>.</returns>
        public Recipient CreateRecipient(Guid guid, string firstName, string lastName,
                                                string socialSecurityNumber,
                                                string bloodGroupRh, string bloodGroupAB0)
        {
            if (guid == Guid.Empty)
            {
                guid = Guid.NewGuid();
            }
            Recipient newRecipient = new Recipient(guid, firstName, lastName,
                                                   socialSecurityNumber,
                                                   bloodGroupRh, bloodGroupAB0);

            var transaction = m_connection.BeginTransaction();

            try
            {
                string commandString = String.Format("INSERT INTO {0} ({1}, {2}, {3}, {4}, {5}, {6}) VALUES('{7}', '{8}', '{9}', '{10}', '{11}', '{12}')",
                    RecipientTableName,
                    PrimaryKeyColumnName, RecipientFirstNameColumnName, RecipientLastNameColumnName,
                    DonorSocialSecurityNumberColumnName, DonorBloodGroupRhColumnName, DonorBloodGroupAB0ColumnName,
                    guid, firstName, lastName,
                    socialSecurityNumber,
                    bloodGroupRh, bloodGroupAB0);

                var command = new SqlCommand(commandString, m_connection, transaction);

                var result = command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();
            return newRecipient;
        }

        /// <summary>
        /// Creates a new Recipient in the database.
        /// </summary>
        /// <param name="recipient">A <c>Recipient</c> with members assigned.</param>
        /// <remarks>The <c>Recipient</c> will be assigned a Guid; the Guid of the input parameter will be ignored.</remarks>
        /// <returns>The created <c>Recipient</c>.</returns>
        public Recipient CreateRecipient(Recipient recipient)
        {
            return CreateRecipient(recipient.Guid, recipient.FirstName, recipient.LastName,
                                   recipient.SocialSecurityNumber, 
                                   recipient.BloodGroupRh, recipient.BloodGroupAB0);
        }

        /// <summary>
        /// Gets a <c>Recipient</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="transaction">A valid transaction, if the get occurs inside another transaction, or null if there is no outer transaction.</param>
        /// <returns>The <c>Recipient</c> retreived from the database.</returns>
        private Recipient GetRecipient(Guid guid, SqlTransaction transaction)
        {
            Recipient recipient;

            string commandString = String.Format("SELECT * FROM {0} WHERE {1} = '{2}'",
                                                RecipientTableName, PrimaryKeyColumnName, guid);

            var command = transaction == null ? new SqlCommand(commandString, m_connection)
                                              : new SqlCommand(commandString, m_connection, transaction);
            var recipientDataReader = command.ExecuteReader();
            if (recipientDataReader.Read())
            {
                string recipientFirstName = recipientDataReader.GetString(1);
                string recipientLastName = recipientDataReader.GetString(2);
                string recipientSocialSecurityNumber = recipientDataReader.GetString(3);
                string recipientBloodGroupRh = recipientDataReader.GetString(4);
                string recipientBloodGroupAB0 = recipientDataReader.GetString(5);

                recipient = new Recipient(guid, recipientFirstName, recipientLastName,
                                          recipientSocialSecurityNumber,
                                          recipientBloodGroupRh, recipientBloodGroupAB0);
            }
            else
            {
                recipient = null;
            }
            recipientDataReader.Close();

            return recipient;
        }

        /// <summary>
        /// Gets a <c>Recipient</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="transaction">A valid transaction, if the get occurs inside another transaction, or null if there is no outer transaction.</param>
        /// <returns>true if a <c>Recipient</c> with Guid = <paramref name="guid"/> exists in the database; false otherwise.</returns>
        private bool RecipientExists(Guid guid, SqlTransaction transaction)
        {
            string commandString = String.Format("SELECT COUNT(*) FROM {0} WHERE {1} = '{2}'",
                                                RecipientTableName, PrimaryKeyColumnName, guid);

            var command = transaction == null ? new SqlCommand(commandString, m_connection)
                                              : new SqlCommand(commandString, m_connection, transaction);
            var nOfRecipients = (int)command.ExecuteScalar();

            return (nOfRecipients > 0);
        }

        /// <summary>
        /// Gets a <c>Recipient</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>true if a <c>Recipient</c> with Guid = <paramref name="guid"/> exists in the database; false otherwise.</returns>
        public bool RecipientExists(Guid guid)
        {
            return RecipientExists(guid, null);
        }

        /// <summary>
        /// Gets a <c>Recipient</c> from the database
        /// </summary>
        /// <param name="guid"></param>
        /// <returns>The <c>Recipient</c> retreived from the database.</returns>
        public Recipient GetRecipient(Guid guid)
        {
            return GetRecipient(guid, null);
        }


        /// <summary>
        /// Gets all <c>Recipient</c>s from the database
        /// </summary>
        /// <param name="transaction">A valid transaction, if the get occurs inside another transaction, or null if there is no outer transaction.</param>
        /// <returns>The <c>Recipient</c> retreived from the database.</returns>
        private List<Recipient> GetRecipientList(SqlTransaction transaction)
        {
            List<Recipient> recipients = new List<Recipient>();

            string commandString = String.Format("SELECT * FROM {0}", RecipientTableName);

            var command = transaction == null ? new SqlCommand(commandString, m_connection)
                                              : new SqlCommand(commandString, m_connection, transaction);
            var recipientDataReader = command.ExecuteReader();
            while (recipientDataReader.Read())
            {
                Recipient recipient;

                Guid guid = recipientDataReader.GetGuid(0);
                string firstName = recipientDataReader.GetString(1);
                string lastName = recipientDataReader.GetString(2);
                string socialSecurityNumber = recipientDataReader.GetString(3);
                string bloodGroupRh = recipientDataReader.GetString(4);
                string bloodGroupAB0 = recipientDataReader.GetString(5);

                recipient = new Recipient(guid, firstName, lastName,
                                          socialSecurityNumber,
                                          bloodGroupRh, bloodGroupAB0);
                recipients.Add(recipient);
            }
            recipientDataReader.Close();

            return recipients;
        }

        /// <summary>
        /// Gets all <c>Recipient</c>s from the database
        /// </summary>
        /// <returns>The <c>Recipient</c> retreived from the database.</returns>
        public List<Recipient> GetRecipientList()
        {
            return GetRecipientList(null);
        }

        /// <summary>
        /// Updates a <c>Recipient</c> in the database with the new values of <paramref name="recipient"/>.
        /// </summary>
        /// <param name="recipient"></param>
        /// <returns>The <c>Recipient</c>'s updated values, as read again from the database.</returns>
        public Recipient UpdateRecipient(Recipient recipient)
        {
            var transaction = m_connection.BeginTransaction();

            try
            {
                string commandString = String.Format("UPDATE {0} SET {1} = '{2}', {3} = '{4}', {5} = '{6}', {7} = '{8}', {9} = '{10}' WHERE {11} = '{12}'",
                    RecipientTableName,
                    RecipientFirstNameColumnName, recipient.FirstName,
                    RecipientLastNameColumnName, recipient.LastName,
                    RecipientSocialSecurityNumberColumnName, recipient.SocialSecurityNumber,
                    RecipientBloodGroupRhColumnName, recipient.BloodGroupRh,
                    RecipientBloodGroupAB0ColumnName, recipient.BloodGroupAB0,
                    PrimaryKeyColumnName, recipient.Guid);

                var command = new SqlCommand(commandString, m_connection, transaction);

                var result = command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();

            recipient = GetRecipient(recipient.Guid);
            return recipient;
        }

        /// <summary>
        /// Deletes the <c>Recipient</c> with guid == <paramref name="guid"/> from the database.
        /// </summary>
        /// <param name="guid"></param>
        public void DeleteRecipient(Guid guid)
        {
            var transaction = m_connection.BeginTransaction();

            //System.Threading.Thread.Sleep(5000);

            try
            {
                if (GetRecipient(guid, transaction) == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(guid), $"No Recipient with guid = '{guid}' exist in the database.");
                }

                string commandString = String.Format("DELETE FROM {0} WHERE {1} = '{2}'",
                    RecipientTableName,
                    PrimaryKeyColumnName, guid);

                var command = new SqlCommand(commandString, m_connection, transaction);

                var result = command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

            transaction.Commit();
        }

        #endregion
        
        #endregion
    }
}
