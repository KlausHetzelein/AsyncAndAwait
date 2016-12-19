using System;
using System.Threading;

namespace SampleBusinessCode
{
    public delegate void LogInfo(string info);

    public class InfoObject
    {
        public LogInfo Logger { get; set; }

        public int MillisToSleep { get; set; } = 3000;

        public void Log(string info)
        {
            LogInfo logger = Logger ?? Console.WriteLine;

            logger($"<{DateTime.Now:T}> <ThreadId: {Thread.CurrentThread.ManagedThreadId}>: {info}");
        }
    }
}