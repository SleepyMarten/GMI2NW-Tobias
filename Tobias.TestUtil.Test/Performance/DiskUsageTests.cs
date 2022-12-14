using System;
using Tobias.TestUtil.Performance;
using Tobias.TestUtil.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace Tobias.Core.TestUtil.Test
{
    [TestClass]
    class DiskUsageTests
    {
        [TestMethod]
        void TestDiskUsageTest1()
        {
            // 1. Prepare
            uint sizePerTempFile = 1000;
            uint nOfTempFiles = 1000;
            byte[] buffer = new byte[sizePerTempFile];
            string folderName = FileSystemUtil.CreateTestFolder();
 
            var rnd = new Random(12345);

            // 2. Measure before
            var diskSpaceBefore = DiskUsage.GetDirectorySizeKB();

            // 3. Generate files
            for (uint i = 0; i < nOfTempFiles; i++)
            {
                var fileNameWithoutPath = String.Format("Tmp_{0}", i.ToString("D4"));
                var fileNameWithPath = Path.Combine(folderName, fileNameWithoutPath);
                rnd.NextBytes(buffer);
                File.WriteAllBytes(fileNameWithPath, buffer);
            }

            // 4. Measure after call
            var diskSpaceAfter = DiskUsage.GetDirectorySizeKB(folderName);
            var expectedDiskSpaceAfter = diskSpaceBefore + sizePerTempFile * nOfTempFiles;
            Assert.AreEqual(expectedDiskSpaceAfter, diskSpaceAfter);
        }

    }
}
