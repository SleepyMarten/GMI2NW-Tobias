using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Tobias.TestUtil.Load.WorkerThread;

namespace Tobias.TestUtil.Load
{
    /// <summary>
    /// Class that starts a number of parallel threads, all performing the same action.
    /// Useful for load tests and stress tests.
    /// </summary>
    public class WorkerThreadRunner
    {
        public WorkerThreadsExecutionSummary ExecuteThreads(Action action, int nOfThreads, int timeOutMs)
        {
            WorkerThreadsExecutionSummary workResult = new WorkerThreadsExecutionSummary();

            for (int i = 0; i < nOfThreads; i++)
            {
                var threadStart = new ThreadStart(() =>
                {
                    try
                    {
                        action.Invoke();
                    }
                    catch (Exception e)
                    {
                        workResult.SetException(Thread.CurrentThread.Name, e);
                    }
                    finally
                    {
                        workResult.SetCompletion(Thread.CurrentThread.Name);
                    }
                });

                var thread = new Thread(threadStart);
                thread.Name = $"Thread {i}";

                workResult.Add(new WorkerThreadExecutionInfo(thread.Name));
                thread.Start();
            }

            var stopwatch = Stopwatch.StartNew();

            while ((workResult.CompletedThreads.Count() < nOfThreads) && (stopwatch.ElapsedMilliseconds < timeOutMs))
            {
                Thread.Sleep(10);
            }

            workResult.ElapsedTimeMs = stopwatch.ElapsedMilliseconds;
            workResult.TimeOutOccurred = (workResult.ElapsedTimeMs >= timeOutMs);

            return workResult;
        }

    }
}
