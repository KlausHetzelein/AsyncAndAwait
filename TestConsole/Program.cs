using System;
using System.Reflection;
using System.Text;
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

            Pause();
        }

        static void DoItInSync()
        {
            string currentMethodName = nameof(DoItInSync);

            var info = new InfoObject { MillisToSleep = 3000, TestCase = "Sync" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            lenghtyStuff.DoItInSync(info);
            info.Log($"End of <{currentMethodName}>");
        }

        static async Task DoItWithAsyncAndAwait()
        {
            string currentMethodName = nameof(DoItWithAsyncAndAwait);
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncAndAwait" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            await lenghtyStuff.DoItInAsync(info);
            info.Log($"End of <{currentMethodName}>");
        }
        static async Task DoItWithAsyncNoAwaitButWaitMayBlock()
        {
            string currentMethodName = nameof(DoItWithAsyncNoAwaitButWaitMayBlock);
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncNoAwaitMayBlock" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            lenghtyStuff.DoItInAsync(info).Wait();
            info.Log($"End of <{currentMethodName}>");
        }

        static async Task DoItWithAsyncNoAwaitButConfigureAwaitFalse()
        {
            string currentMethodName = nameof(DoItWithAsyncNoAwaitButConfigureAwaitFalse);
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncNoAwaitButConfigureAwaitFalse" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            lenghtyStuff.DoItInAsync(info, true).Wait();
            info.Log($"End of <{currentMethodName}>");
        }


        static async Task DoItInAsyncInNewThread()
        {
            string currentMethodName = nameof(DoItInAsyncInNewThread);
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncThread" };
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Start of <{currentMethodName}>");
            await lenghtyStuff.DoItInAsyncInNewThread(info);
            info.Log($"End of <{currentMethodName}>");
        }
    }
}
