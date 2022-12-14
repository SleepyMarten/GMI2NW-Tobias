using Microsoft.Data.SqlClient;
using System.Linq;
using System.IO;

namespace Tobias.DatabaseMgmt
{
    /// <summary>
    /// Class that initializes a database with tables.
    /// </summary>
    public static class DbObjectManager
    {
        #region Private helper methods
        private static void ExecuteSqlScripts(string filePattern, string connectionString, string sqlFolder)
        {
            var fileNames = Directory.EnumerateFiles(sqlFolder, filePattern).ToList();

            fileNames.Sort();

            foreach (var fileName in fileNames)
            {
                ExecuteSqlScript(fileName, connectionString);
            }
        }

        private static void ExecuteSqlScript(string fileName, string connectionString)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var transaction = connection.BeginTransaction();

            try
            {
                string sqlContent = File.ReadAllText(fileName);

                var command = new SqlCommand(sqlContent, connection, transaction);
                command.ExecuteNonQuery();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
            transaction.Commit();
            connection.Close();
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Builds a test database, by executing SQL update scripts.
        /// </summary>
        public static void CreateDbObjects(string connectionString, string sqlFolder)
        {
            ExecuteSqlScripts("UpdateDatabaseObjects*", connectionString, sqlFolder);
        }


        /// <summary>
        /// Tears down the test database, by executing the original SQL drop scripts.
        /// </summary>
        public static void DropDbObjects(string connectionString, string sqlFolder)
        {
            ExecuteSqlScripts("DropDatabaseObjects*", connectionString, sqlFolder);
        }
        #endregion
    }
}
