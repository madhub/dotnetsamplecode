using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Session;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace ProcessTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            if (TraceEventSession.IsElevated() != true)
            {
                Console.WriteLine("Must be elevated (Admin) to run this program.");
                Debugger.Break();
                return;
            }
            Console.WriteLine("Monitoring Process Starts/Stops system wide");
            Console.WriteLine("Press Ctrl-C to stop monitoring early.");
            Console.WriteLine("Start a program to see some events!");

            // Start the session as a real time monitoring session,  
            using (TraceEventSession session = new TraceEventSession(KernelTraceEventParser.KernelSessionName))
            {
                Console.CancelKeyPress += new ConsoleCancelEventHandler((object sender, ConsoleCancelEventArgs cancelArgs) =>
                {
                    Console.WriteLine("Control C pressed");     // Note that if you hit Ctrl-C twice rapidly you may be called concurrently.  
                    session.Dispose();                          // Note that this causes Process() to return.  
                    cancelArgs.Cancel = true;                   // This says don't abort, since Process() will return we can terminate nicely.   
                });
                session.EnableKernelProvider(KernelTraceEventParser.Keywords.Process);


                //  Subscribe to more events (process start) 
                session.Source.Kernel.ProcessStart += delegate (ProcessTraceData data)
                {
                    Console.WriteLine("Process Started {0,6} Parent {1,6} Name {2,8} Cmd: {3}",
                        data.ProcessID, data.ParentID, data.ProcessName, data.CommandLine);
                };
                //  Subscribe to more events (process end)
                session.Source.Kernel.ProcessStop += delegate (ProcessTraceData data)
                {
                    Console.WriteLine("Process Ending {0,6} ,Name {1} Cmd: {2}, ImageFile: {3}",
                        data.ProcessID, data.ProcessName, data.CommandLine,data.ImageFileName);
                };
                // Start listening for events, will end if session.Source.StopProcessing() is called or session.Dispose() is called.
                session.Source.Process();
            }
        }
    }
}
