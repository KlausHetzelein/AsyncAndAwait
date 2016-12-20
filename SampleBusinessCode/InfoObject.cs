using System;
using System.Threading;

namespace SampleBusinessCode
{
    public delegate void LogInfo(string info);

    public class InfoObject
    {
        public LogInfo Logger { get; set; }

        public int MillisToSleep { get; set; } = 3000;

        public int IterationsToSimulateWork { get; set; } = 10;

        public bool ThrowIfCancellingRequesting { get; set; } = false;

        public string TestCase { get; set; } = string.Empty;

        public void Log(string info, bool startWithNewline = false)
        {
            LogInfo logger = Logger ?? Console.WriteLine;
            logger((startWithNewline ? Environment.NewLine : string.Empty) + 
                $"<{TestCase}><{DateTime.Now:T}> <ThreadId: {Thread.CurrentThread.ManagedThreadId}>: {info}");
        }
    }
}