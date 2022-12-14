using Microsoft.Data.SqlClient;
using System.IO;
using Tobias.DatabaseMgmt;
using System.Diagnostics;
using System.Threading;
using System;
using System.Runtime.CompilerServices;
using Tobias.Core.TestUtil.ConsoleDriver;
using System.Collections.Generic;

namespace Tobias.Core.TestUtil.Db
{
    /// <summary>
    /// Class that initializes a test database with tables.
    /// </summary>
    public class TestDatabaseBuilder
    {
        #region Constants
        private const uint DefaultTimeoutMs = 10000;

        public const string LocalDbDirectoryRoot = @".\Db\";

        public readonly string DatabaseName;
        public readonly string LocalDbDirectory; 
        public string LocalScriptDirectory => LocalDbDirectory;
        public string LocalDbFilePath => Path.Combine(LocalDbDirectory, @"Tobias.mdf");
        public string ConnectionString =>
            @"Server=(LocalDb)\MSSQLLocalDb;"
            + @"Integrated Security = true;"
            + @"AttachDbFileName = " + LocalDbFilePath + ";";

        #endregion

        /// <summary>
        /// Private constructor - can only 
        /// </summary>
        private TestDatabaseBuilder(string subDirectoryName)
        {
            DatabaseName = subDirectoryName;
            if(String.IsNullOrEmpty(DatabaseName))
            {
                DatabaseName = Guid.NewGuid().ToString();
            }

            LocalDbDirectory = Path.GetFullPath(Path.Combine(LocalDbDirectoryRoot, subDirectoryName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subDirectoryName">
        /// One of: 
        /// (1) A name you decide yourself.
        /// (2) Null or empty string: no additional level of subdirectory will be used.
        /// (3) Omitted: the caller's <see cref="Type"/> to be used in the path.
        /// </param>
        /// <returns></returns>
        public static TestDatabaseBuilder GetNewDatabaseBuilder([CallerFilePath] string subDirectoryName = null)
        {
            if (subDirectoryName is null)
            {
                subDirectoryName = "";
            }

            if (subDirectoryName.Contains(@"\"))
            {
                //A path...
                var directories = subDirectoryName.Split(@"\");
                subDirectoryName = directories[directories.Length - 1];
            }
 
            return new TestDatabaseBuilder(subDirectoryName);
        }

        #region Public methods
        /// <summary>
        /// Builds a test database, by executing the original SQL update scripts.
        /// </summary>
        /// <param name="timeOutMs">Maximum waiting time (ms) before exiting</param>
        /// <returns>True if database was created successfully; false otherwise</returns>
        public bool BuildDatabase(uint timeOutMs = DefaultTimeoutMs)
        {
            Console.WriteLine($"TestDatabaseBuilder.CreateDatabase() entered.");
            Console.WriteLine($"  Local Db directory: {LocalDbDirectory}.");
            Console.WriteLine($"  Database name: {DatabaseName}.");

            // 1. Collect/construct various folder paths and file names, 
            //    in order to call Tobias.Console in the correct directory
            //    and with the correct parameters.
            string curDir = Directory.GetCurrentDirectory();
            string fourStepsUp = string.Format(@"..\..\..\..\");

            string databaseMgmtProjectFolder = @"Tobias.DatabaseMgmt";
            string databaseMgmtDbFolderName = @"Db";
            string databaseMgmtScriptFolderName = @"Script";
            string databaseMgmtFullDbFolderName = Path.Combine(curDir,
                                                               fourStepsUp,
                                                               databaseMgmtProjectFolder,
                                                               databaseMgmtDbFolderName);
            string databaseMgmtFullScriptFolderName = Path.Combine(curDir,
                                                               fourStepsUp,
                                                               databaseMgmtProjectFolder,
                                                               databaseMgmtScriptFolderName);

            //2. Construct argument list
            List<string> arguments = new List<string>();
            arguments.Add("-command build");
            arguments.Add($"-origin {databaseMgmtFullDbFolderName}");
            arguments.Add("-filetype mdf");
            arguments.Add("-filetype ldf");
            arguments.Add( $"-origin {databaseMgmtFullScriptFolderName}");
            arguments.Add("-filetype sql");
            arguments.Add($"-destination {LocalDbDirectory}");

            //3. Create and start the process
            return ConsoleUtil.ExecuteTobiasConsole(arguments, timeOutMs);
        }

        /// <summary>
        /// Deletes the test database, by deleting the folder where it resides.
        /// </summary>
        /// <param name="timeOutMs">Maximum waiting time (ms) before exiting</param>
        /// <returns>True if database was deleted successfully; false otherwise</returns>
        public bool DeleteDatabase(uint timeOutMs = DefaultTimeoutMs)
        {
            var watch = Stopwatch.StartNew();
            bool success = false;

            Console.WriteLine($"TestDatabaseBuilder.DeleteDatabase() entered.");

            //4. Timeout loop
            while (!success && watch.ElapsedMilliseconds < timeOutMs)
            {
                try
                {
                    if (Directory.Exists(LocalDbDirectory))
                    {
                        Directory.Delete(LocalDbDirectory, true);
                    }
                    Console.WriteLine($"Success!");
                    success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Exception caught: {e.Message}");
                    Thread.Sleep(10);
                }
            }

            return success;
        }

        /// <summary>
        /// Builds a test database, by executing the original SQL update scripts.
        /// </summary>
        public void ExecuteUpdateScripts()
        {
            DbObjectManager.CreateDbObjects(ConnectionString, LocalScriptDirectory);
        }


        /// <summary>
        /// Tears down the test database, by executing the original SQL drop scripts.
        /// </summary>
        public void ExecuteDropScripts()
        {
            DbObjectManager.DropDbObjects(ConnectionString, LocalScriptDirectory);
        }

        #region Combined methods
        /// <summary>
        /// Performs <see cref="ExecuteDropScripts()"/> followed by <see cref="DeleteDatabase(uint)"/>,
        /// ignoring any exceptions.
        /// </summary>
        /// <param name="timeOutMs">Maximum waiting time (ms) before exiting</param>
        public void TryDropScriptsAndDeleteDatabase(uint timeOutMs = DefaultTimeoutMs)
        {
            try
            {
                ExecuteDropScripts();
            }
            catch { }

            try
            {
                DeleteDatabase();
            }
            catch { }
        }
        #endregion
        #endregion
    }
}
