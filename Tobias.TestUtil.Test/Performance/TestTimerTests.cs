using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobias.TestUtil.Performance;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace Tobias.Core.TestUtil.Test
{

    [TestClass]
    public class WorkerThreadRunnerTests
    {

        #region Constants and readonly properties
        /// <summary>
        /// Max allowed time for any method call, in seconds
        /// </summary>
        public const uint MaxAllowedElapsedTimeValidationSeconds = 2;

        /// <summary>
        /// Max allowed time for any method call, in milliseconds
        /// </summary>
        public const uint MaxAllowedElapsedTimeValidationMs = MaxAllowedElapsedTimeValidationSeconds * 10000;


        //TODO: injected fault, wrong conversion factor

        /// <summary>
        /// Max allowed time for any method call, in milliseconds
        /// </summary>
        public const uint MaxAllowedElapsedTimeBatchMs = MaxAllowedElapsedTimeValidationMs * 10;
        #endregion

        [TestMethod]
        public void MeasureAverageTimeMs_TimePassed_ShouldPassUnderRequiredTime()
        {
            var averageElapsedTimeMs = TestTimer.MeasureAverageTimeMs(() =>
            {
                System.Threading.Thread.Sleep(1000);
            });

            Assert.IsTrue(averageElapsedTimeMs < MaxAllowedElapsedTimeValidationMs);
        }

    }
}
