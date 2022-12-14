using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tobias.TestUtil.Load.WorkerThread
{
    public class WorkerThreadsExecutionSummary
    {
        private Dictionary<string, WorkerThreadExecutionInfo> ThreadExecutionInfoDictionary = new Dictionary<string, WorkerThreadExecutionInfo>();

        public long ElapsedTimeMs { get; internal set; }  = 0;
        public bool TimeOutOccurred { get; internal set; } = false;

        internal void Add(WorkerThreadExecutionInfo threadExecutionInfo)
        {
            ThreadExecutionInfoDictionary.Add(threadExecutionInfo.ThreadName, threadExecutionInfo);
        }
        internal void SetException(string threadName, Exception exception)
        {
            ThreadExecutionInfoDictionary[threadName].Exception = exception;
        }
        internal void SetCompletion(string threadName, bool completed = true)
        {
            ThreadExecutionInfoDictionary[threadName].Completed = completed;
        }
        public IEnumerable<WorkerThreadExecutionInfo> FailedThreads => ThreadExecutionInfoDictionary.Values.Where((threadExecutionInfo) => { return threadExecutionInfo.Exception is not null; });
        public int FailedThreadCount => FailedThreads.Count();
        public IEnumerable<WorkerThreadExecutionInfo> CompletedThreads => ThreadExecutionInfoDictionary.Values.Where((threadExecutionInfo) => { return threadExecutionInfo.Completed == true; });
        public int CompletedThreadCount => CompletedThreads.Count();

        public string ToString(bool verbose = false)
        {
            string FailedThreadsAsString()
            {
                var strb = new StringBuilder();
                foreach (var failedThread in FailedThreads)
                {
                    if (strb.Length > 0)
                    {
                        strb.Append(", ");
                    }
                    if (verbose)
                    {
                        var exceptionString = failedThread.Exception.ToString();
                        if (exceptionString.Length >= 80)
                        {
                            strb.AppendLine($"'{failedThread.ThreadName}': {exceptionString.Substring(0, 77)}...");
                        }
                    }
                    else
                    {
                        strb.Append($"'failedThread.ThreadName'");
                    }
                }
                return strb.ToString();
            }

            StringBuilder retStrBuilder = new StringBuilder();

            if (TimeOutOccurred)
            {
                retStrBuilder.AppendLine("Timeout occurred.");
            }
            if (FailedThreads.Count() > 0)
            {
                retStrBuilder.AppendLine($"The following threads failed: {FailedThreadsAsString()}");
            }
            return retStrBuilder.ToString();
        }
    }
}
