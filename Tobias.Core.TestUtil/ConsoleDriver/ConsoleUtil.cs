using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tobias.Core.TestUtil.ConsoleDriver
{
    public static class ConsoleUtil
    {
        #region Constants
        private const uint DefaultTimeoutMs = 10000;
        #endregion

        #region Public methods
        public static bool ExecuteTobiasConsole(IEnumerable<string> arguments, uint timeOutMs = DefaultTimeoutMs)
        {
            // 1. Collect/construct various folder paths and file names, 
            //    in order to call Tobias.Console in the correct directory
            //    and with the correct parameters.

            string curDir = Directory.GetCurrentDirectory();
            string fourStepsUp = string.Format(@"..\..\..\..\");
            string[] directories = curDir.Split(@"\");
            int lastIndex = directories.Length - 1;
            string consoleBinaryPath = Path.Combine(directories[lastIndex - 2],
                                                     directories[lastIndex - 1],
                                                     directories[lastIndex]);
            string consoleProjectFolder = @"Tobias.Console";
            string consoleExeFileName = @"Tobias.Console.exe";
            string consoleExeFilePath = Path.Combine(curDir,
                                                     fourStepsUp,
                                                     consoleProjectFolder,
                                                     consoleBinaryPath,
                                                     consoleExeFileName);

            StringBuilder argsStringBuilder = new StringBuilder();
            foreach (var arg in arguments)
            {
                argsStringBuilder.Append(arg);
                argsStringBuilder.Append(" ");
            }

            Process tobiasConsoleProcess = new Process();
            tobiasConsoleProcess.StartInfo.WorkingDirectory = curDir;
            tobiasConsoleProcess.StartInfo.CreateNoWindow = false;
            tobiasConsoleProcess.StartInfo.FileName = consoleExeFilePath;
            tobiasConsoleProcess.StartInfo.Arguments = argsStringBuilder.ToString();

            tobiasConsoleProcess.Start();

            //4. Timeout loop
            var watch = Stopwatch.StartNew();

            while (!tobiasConsoleProcess.HasExited && watch.ElapsedMilliseconds<timeOutMs)
            {
                Thread.Sleep((int) timeOutMs / 100);
            }

            if (tobiasConsoleProcess.HasExited)
            {
                Console.WriteLine($"Tobias.Console exited with code: {tobiasConsoleProcess.ExitCode}");
            }
            return (tobiasConsoleProcess.HasExited && (tobiasConsoleProcess.ExitCode == 0));
        }
        #endregion
    }
}
