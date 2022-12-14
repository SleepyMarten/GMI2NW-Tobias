using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tobias.TestUtil.Load.WorkerThread

{
    public class WorkerThreadExecutionInfo
    {
        public string ThreadName { get; private set; } = String.Empty;
        public Exception Exception = null;
        public bool Completed = false;

        public WorkerThreadExecutionInfo(string threadName)
        {
            ThreadName = threadName;
        }
    }
}
