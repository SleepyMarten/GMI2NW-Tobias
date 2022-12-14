using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Tobias.TestUtil.Performance
{
    public static class TestTimer
    {
        /// <summary>
        /// Factor to level out sporadic outlier execution times
        /// </summary>
        public const uint AveragingFactor = 10;

        /// <summary>
        /// The default number of (each of) max and min values removed 
        /// before computing average. 
        /// </summary>
        public const uint DefaultNOfOutliers = 1;

        /// <summary>
        /// Performs <paramref name="action"/> <paramref name="nOfExecutions"/> times
        /// while measuring the time. Then removes the outliers, the <paramref name="nOfOutliers"/>
        /// highest and lowest values) and returns the average of the rest.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="nOfExecutions"></param>
        /// <returns></returns>
        public static long MeasureAverageTimeMs(Action action, uint nOfExecutions = AveragingFactor, uint nOfOutliers = DefaultNOfOutliers)
        {
            List<long> executionTimesMs = new List<long>();

            for(uint i = 0; i < nOfExecutions; i++)
            {
                var watch = Stopwatch.StartNew();

                action.Invoke();

                watch.Stop();
                executionTimesMs.Add(watch.ElapsedMilliseconds);
            }

            //Post-processing: remove outliers, then calculate and return the average
            executionTimesMs.Remove(executionTimesMs.Max());
            executionTimesMs.Remove(executionTimesMs.Min());

            return (long)executionTimesMs.Average();
        }

    }
}
