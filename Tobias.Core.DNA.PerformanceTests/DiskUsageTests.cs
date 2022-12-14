using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobias.Core.DNA.Util;
using Tobias.Core.TestUtil.ConsoleDriver;
using Tobias.Core.TestUtil.DNA;
using Tobias.TestUtil.Performance;

namespace Tobias.Core.DNA.PerformanceTests
{
    [TestClass]
    [TestCategory("Non-Functional Tests")]
    [TestCategory("Performance Tests")]
    [TestCategory("Disk Usage Tests")]
    public class DiskUsageTests
    {
        #region Constants and readonly properties
        private const uint MaxAllowedDiskUsageKB = 2500;
        private const uint NOfDNAStringFiles = 10;
        private const uint rndSeed = 123456;
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
            var filePath = Directory.GetFiles(DNAFilesDirectory).Last();

            // 2. Stimulate SUT
            var diskUsageKB = DiskUsage.MeasureDiskUsageKB(() =>
            {
                string aminoSequence = File.ReadAllText(filePath);
                DNAString dnaString = new DNAString(aminoSequence);
                dnaString.Validate();
            });

            // 3. Assert
            Assert.IsTrue(diskUsageKB < MaxAllowedDiskUsageKB, $"Disk space used: {diskUsageKB} KB. Disk space limit: {MaxAllowedDiskUsageKB} KB");
        }

        [TestMethod]
        public void BatchJobTest()
        {
            // 1. Prepare
            List<long> diskUsageList = new List<long>();

            foreach (string filePath in Directory.GetFiles(DNAFilesDirectory))
            {
                // 2. Stimulate SUT
                var diskUsageKB = (long)DiskUsage.MeasureDiskUsageKB(() =>
                {
                    string aminoSequence = File.ReadAllText(filePath);
                    DNAString dnaString = new DNAString(aminoSequence);
                    dnaString.Validate();
                });
                diskUsageList.Add(diskUsageKB);
            }

            // 3. Post-processing: Remove outliers
            diskUsageList.Remove(diskUsageList.Max());
            diskUsageList.Remove(diskUsageList.Min());

            // 4. Assert
            Assert.IsTrue(diskUsageList.Average() < MaxAllowedDiskUsageKB);
        }
    }
}
