using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;
using Tobias.Core.DNA;
using Tobias.Core.TestUtil.DNA;
using Tobias.TestUtil.Performance;

namespace Tobias.Core.DNA.PerformanceTests
{
    [TestClass]
    [TestCategory("Non-Functional Tests")]
    [TestCategory("Performance Tests")]
    [TestCategory("Timing Tests")]
    public class ExecutionTimeTests
    {
        #region Constants and readonly properties
        /// <summary>
        /// Max allowed time for any method call, in seconds
        /// </summary>
        public const uint MaxAllowedElapsedTimeValidationSeconds = 2;

        /// <summary>
        /// Max allowed time for any method call, in milliseconds
        /// </summary>
        public const uint MaxAllowedElapsedTimeValidationMs = MaxAllowedElapsedTimeValidationSeconds * 1000;

        //TODO: injected fault, wrong conversion factor FIXED

        /// <summary>
        /// Max allowed time for any method call, in milliseconds
        /// </summary>
        public const uint MaxAllowedElapsedTimeBatchMs = MaxAllowedElapsedTimeValidationMs * 10;

        private const uint NOfDNAStringFiles = 10;
        private const uint rndSeed = 12345678;
        private static string DNAFilesDirectory => Path.Combine(Directory.GetCurrentDirectory(), "DNAStrings");
        #endregion


        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            if (!DNAUtil.CreateDNAFiles(DNAFilesDirectory, NOfDNAStringFiles, rndSeed))
            {
                Assert.IsTrue(false, "ClassInitialize(): Couldn't create DNA string files. All tests will fail.");
            }
        }

        [TestMethod]
        public void DNAStringValidateTest()
        {
            // 1. Prepare
            var filePaths = Directory.GetFiles(DNAFilesDirectory);
            var filePath = filePaths[filePaths.Length / 2];

            // 2. Stimulate SUT
            var averageElapsedTimeMs = TestTimer.MeasureAverageTimeMs(() =>
            {
                string aminoSequence = File.ReadAllText(filePath);
                DNAString dnaString = new DNAString(aminoSequence);
                dnaString.Validate();
            });

            // 3. Assert
            Assert.IsTrue(averageElapsedTimeMs < MaxAllowedElapsedTimeValidationMs);
        }
    }
}
