using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Threading;
using System.Net;
using System.IO;
using Tobias.TestUtil.Load;

namespace Tobias.LoadTests
{
    [TestClass]
    [TestCategory("Non-Functional Tests")]
    [TestCategory("Performance Tests")]
    [TestCategory("Load and Stress Tests")]
    public class LoadTests
    {
        private const int TimeOutMs = 30 * 1000; //Thirty seconds

        private const string restInterfaceURI = "https://localhost:44357/";
        private const string restInterfaceURIDonor = restInterfaceURI + "donor";
        private const string restInterfaceURIRecipient = restInterfaceURI + "recipient";

        private static Process m_serverProcess = null;

        #region Helper functions
        private static void StartServerProcess()
        {
            // 1. Collect/construct various folder paths and file names, 
            //    in order to call Tobias.Console in the correct directory
            //    and with the correct parameters.

            string curDir = Directory.GetCurrentDirectory();
            string fourStepsUp = string.Format(@"..\..\..\..\");
            string[] directories = curDir.Split(@"\");
            int lastIndex = directories.Length - 1;
            string restInterfaceBinPath = Path.Combine(directories[lastIndex - 2],
                                                       directories[lastIndex - 1],
                                                       directories[lastIndex]);
            string restInterfaceProjectFolder = @"Tobias.RestInterface";
            string restInterfaceExeFileName = restInterfaceProjectFolder + ".exe";
            //string consoleExeFileName = @"Tobias.Console.exe";
            //string consoleExeFilePath = Path.Combine(curDir,
            //                                         fourStepsUp,
            //                                         restInterfaceProjectFolder,
            //                                         consoleBinaryPath,
            //                                         consoleExeFileName);


            string restInterfaceBinDir = Path.Combine(curDir,
                                                      fourStepsUp,
                                                      restInterfaceProjectFolder,
                                                      restInterfaceBinPath);
            string restInterfaceExePath = Path.Combine(restInterfaceBinDir,
                                                       restInterfaceExeFileName);

            m_serverProcess = new Process();
            m_serverProcess.StartInfo.WorkingDirectory = restInterfaceBinDir;
            m_serverProcess.StartInfo.CreateNoWindow = false;
            m_serverProcess.StartInfo.FileName = restInterfaceExePath;
            m_serverProcess.StartInfo.Arguments = restInterfaceBinDir;

            m_serverProcess.Start();


            const int waitTimeSeconds = 10;
            Console.WriteLine($"Server process with Id {m_serverProcess.Id} started.");
            Console.WriteLine($"Waiting {waitTimeSeconds} seconds for server to start properly.");

            Thread.Sleep(waitTimeSeconds * 1000);
        }

        private static void TryKillServerProcess()
        {
            try
            {
                if (m_serverProcess is not null)
                {
                    Console.WriteLine("Server process standard output:");
                    Console.WriteLine(m_serverProcess.StandardOutput.ReadToEnd());

                    if (m_serverProcess.HasExited)
                    {
                        Console.WriteLine($"Server process has already exited.");
                    }
                    else
                    {
                        Console.WriteLine($"Trying to kill server process with Id {m_serverProcess.Id}...");
                        m_serverProcess.Kill();
                        Console.WriteLine($"Process with Id {m_serverProcess.Id} killed successfully.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error when trying to kill server process.");
            }

        }
        #endregion

        #region Initialize and Cleanup methods

        [TestInitialize]
        public void TestInitialize()
        {
            //StartServerProcess();
            Console.WriteLine("Remember that the server first needs to be started separately.");
        }

        [TestCleanup]
        public void TestCleanup()
        {
            //TryKillServerProcess();
        }

        #endregion

        [DataTestMethod]
        [DataRow(1, restInterfaceURIDonor)] //This input data is to be able to debug the test itself
        [DataRow(2, restInterfaceURIDonor)] //This input data is to be able to debug the test itself
        [DataRow(10, restInterfaceURIDonor)] //This input data is to carefully increase the number of simultaneous clients
        [DataRow(10, restInterfaceURIRecipient)] //This input data is to carefully increase the number of simultaneous clients
        public void GetOverHttp(int nOfParallelClients, string uri)
        {
            // 1. Define what each thread shall do
            Action threadAction = () =>
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "GET";
                WebResponse response = httpWebRequest.GetResponse();

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    var responseString = reader.ReadToEnd();
                }
            };

            // 2. Execute threads in parallel
            WorkerThreadRunner workerThreadUtil = new WorkerThreadRunner();
            var workResult = workerThreadUtil.ExecuteThreads(threadAction, nOfParallelClients, TimeOutMs);

            // 3. Write info about the failed threads
            Console.WriteLine(workResult.ToString(true));

            // 4. Asserts
            //   Note: we assert on different conditions to be able to trace a failed test to the correct condition that failed.
            Assert.IsFalse(workResult.TimeOutOccurred && workResult.FailedThreadCount > 0, "Timeout occurred, AND the following threads failed: ");
            Assert.IsFalse(workResult.FailedThreadCount > 0, "One or more process(es) failed.");
            Assert.IsFalse(workResult.TimeOutOccurred, "Timeout occured.");
        }

        [DataTestMethod]
        [DataRow(1, restInterfaceURIDonor)] //This input data is to be able to debug the test itself
        [DataRow(2, restInterfaceURIDonor)] //This input data is to be able to debug the test itself
        [DataRow(10, restInterfaceURIDonor)] //This input data is to carefully increase the number of simultaneous clients
        [DataRow(10, restInterfaceURIRecipient)] //This input data is to carefully increase the number of simultaneous clients
        public void AddOverHttp(int nOfParallelClients, string uri)
        {
            // 1. Define what each thread shall do
            Action threadAction =
                () =>
            {
                // To perform sanity check after request/response
                const string firstName = "Pelle";

                // Prepare the web request
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = "application/json; charset=utf-8";

                string json = "{ \"guid\" : \"\", "
                + "\"firstName\" : \"" + firstName + "\", "
                + "\"lastName\" : \"Päron\", "
                + "\"socialSecurityNumber\": \"1888\", "
                + "\"bloodGroupRh\": \"Rh+\", "
                + "\"bloodGroupAB0\": \"AB\" }";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                    streamWriter.Flush();
                }

                // Send/receive
                WebResponse webResponse = httpWebRequest.GetResponse();

                // Sanity check of the response
                using (var streamReader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var responseContents = streamReader.ReadToEnd();
                    if (!responseContents.Contains(firstName))
                    {
                        throw new Exception("The web response does not contain the data sent.");
                    }
                }
            };

            // 2. Execute threads in parallel
            WorkerThreadRunner workerThreadUtil = new WorkerThreadRunner();
            var workResult = workerThreadUtil.ExecuteThreads(threadAction, nOfParallelClients, TimeOutMs);

            
            // 3. Write info about the failed threads
            Console.WriteLine(workResult.ToString(true));

            // 4. Asserts
            //   Note: we assert on different conditions to be able to trace a failed test to the correct condition that failed.
            Assert.IsFalse(workResult.TimeOutOccurred && workResult.FailedThreadCount > 0, "Timeout occurred, AND the following threads failed: ");
            Assert.IsFalse(workResult.FailedThreadCount > 0, "One or more process(es) failed.");
            Assert.IsFalse(workResult.TimeOutOccurred, "Timeout occured.");
        }
    }
}
