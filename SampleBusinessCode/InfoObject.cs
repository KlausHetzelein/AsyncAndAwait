using System;
using System.Text;
using System.Threading;

namespace SampleBusinessCode
{
    public delegate void LogInfo(string info);

    public class InfoObject
    {
        const int _COUNT_OF_BLANKS_PER_INDENTATION_ = 6;
        private int _indentationLevel = 0;

        public LogInfo Logger { get; set; }

        public int MillisToSleep { get; set; } = 3000;

        public int IterationsToSimulateWork { get; set; } = 10;

        public bool ThrowIfCancellingRequesting { get; set; } = false;

        public string TestCase { get; set; } = string.Empty;

        public void IncreaseIndentationLevel()
        {
            _indentationLevel += _COUNT_OF_BLANKS_PER_INDENTATION_;
        }

        public void DecreaseIndentationLevel()
        {
            _indentationLevel -= _COUNT_OF_BLANKS_PER_INDENTATION_;
            if (_indentationLevel < 0)
            {
                _indentationLevel = 0;
            }
        }

        public void Log(string info)
        {
            StringBuilder indentation = new StringBuilder();
            indentation.Append(' ', _indentationLevel);

            LogInfo logger = Logger ?? Console.WriteLine;
            logger($"{indentation.ToString()}<{DateTime.Now:T}> <ThreadId: {Thread.CurrentThread.ManagedThreadId}>: {info}");
        }
    }
}