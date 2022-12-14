using System;
using System.Threading;

namespace Tobias.ConsoleApp
{
    public class Program
    {
        private static readonly Mutex SingleInstanceMutex = new Mutex(true, "{E89AEE17-4399-4733-850B-4113B87DC55D}"); 

        private static void Main(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;

            if (!SingleInstanceMutex.WaitOne(TimeSpan.Zero, true))
            {
                Console.Error.WriteLine("An instance of this application is already running.");
                return;
            }

            Parameters parameters;
            if (!Parameters.TryParse(args, out parameters))
            {
                return;
            }

            Console.WriteLine("Command executed successfully.");
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
            var exception = args.ExceptionObject as Exception ?? new Exception("Unable to determine unhandled exception!");
            var message = string.Format("An unhandled exception occurred in {0}.\n\n{1}", typeof(Program).Assembly.Location, exception);
            Console.Error.WriteLine(message);
            
            // TODO: Customize the source if you wish, or remove this bit if you don't want to write exceptions to the event log.
            //try
            //{
            //    EventLog.WriteEntry(AppDomain.CurrentDomain.FriendlyName, message, EventLogEntryType.Error);
            //}
            //catch (Exception e)
            //{
            //    Console.Error.WriteLine("Failed to write exception to event log:\n\n" + e);
            //}
            Console.Error.WriteLine("Exiting.");
        }
    }
}