using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SampleBusinessCode;
using static System.Console;

namespace TestConsole
{
    class Program
    {
        private static string separator = new StringBuilder().Append('-', 100).ToString();

        static void Pause(string msg = null)
        {
            WriteLine($"{msg ?? string.Empty}{Environment.NewLine}continue with NewLine");
            ReadLine();
        }

        static void AppendSeparator()
        {
            WriteLine(separator);
        }

        static void Main()
        {
            DoItInSync();
            AppendSeparator();

            var task = DoItWithAsyncAndAwait();
            task.Wait();
            AppendSeparator();

            DoItWithAsyncNoAwaitButWaitMayBlock().Wait();
            AppendSeparator();

            DoItWithAsyncNoAwaitButConfigureAwaitFalse().Wait();
            AppendSeparator();

            DoItInAsyncInNewThread().Wait();
            AppendSeparator();

            HandleLongRunningTask().Wait();
            AppendSeparator();

            Pause();
        }

        static void DoItInSync()
        {
            string currentMethodName = nameof(DoItInSync);

            var info = new InfoObject { MillisToSleep = 1234, TestCase = "Sync" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            lenghtyStuff.DoItInSync(info);
            info.Log($"End of <{currentMethodName}>");
        }

        static async Task DoItWithAsyncAndAwait()
        {
            string currentMethodName = nameof(DoItWithAsyncAndAwait);
            var info = new InfoObject { MillisToSleep = 1357, TestCase = "AsyncAndAwait" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            await lenghtyStuff.DoItInAsync(info);
            info.Log($"End of <{currentMethodName}>");
        }
        static async Task DoItWithAsyncNoAwaitButWaitMayBlock()
        {
            string currentMethodName = nameof(DoItWithAsyncNoAwaitButWaitMayBlock);
            var info = new InfoObject { MillisToSleep = 1246, TestCase = "AsyncNoAwaitMayBlock" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            lenghtyStuff.DoItInAsync(info).Wait();
            info.Log($"End of <{currentMethodName}>");
        }

        static async Task DoItWithAsyncNoAwaitButConfigureAwaitFalse()
        {
            string currentMethodName = nameof(DoItWithAsyncNoAwaitButConfigureAwaitFalse);
            var info = new InfoObject { MillisToSleep = 1333, TestCase = "AsyncNoAwaitButConfigureAwaitFalse" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            lenghtyStuff.DoItInAsync(info, true).Wait();
            info.Log($"End of <{currentMethodName}>");
        }


        static async Task DoItInAsyncInNewThread()
        {
            string currentMethodName = nameof(DoItInAsyncInNewThread);
            var info = new InfoObject { MillisToSleep = 1377, TestCase = "AsyncThread" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            await lenghtyStuff.DoItInAsyncInNewThread(info);
            info.Log($"End of <{currentMethodName}>");
        }

        static async Task HandleLongRunningTask()
        {
            string currentMethodName = nameof(HandleLongRunningTask);
            Task<bool> task = null;
            bool result = false;
            var _cts = new CancellationTokenSource();
            var info = new InfoObject { ThrowIfCancellingRequesting = true, TestCase = "CancellationToken", MillisToSleep = 2112 };
            var lenghtyStuff = new LengthyStuff();

            info.IncreaseIndentationLevel();

            // cancel the task after some time
            _cts.CancelAfter(4567);

            try
            {
                info.Log($"In {currentMethodName} before starting DoLengthy...");
                // there is really not much difference
                //task = lenghtyStuff.DoLengthyOperationAsyncWithCancellationToken(info, _cts.Token);
                //task = Task.Run(async () => await lenghtyStuff.DoLengthyOperationAsyncWithCancellationToken(info, _cts.Token), _cts.Token);
                task = lenghtyStuff.DoLengthyOpAsyncWithCtInNewThread(info, _cts.Token);

                result = await task;
                info.Log($"In {currentMethodName} after awaiting DoLengthy...");
            }
            catch (OperationCanceledException)
            {
                info.Log($"In {currentMethodName} received OperationCanceledException, return false");
                result = false;
            }

            info.Log($"In {currentMethodName} Task.State <{task?.Status}>, Result: <{result}>");
            info.DecreaseIndentationLevel();
        }
    }
}
