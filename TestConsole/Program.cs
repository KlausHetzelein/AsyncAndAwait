using System;
using System.Threading.Tasks;
using SampleBusinessCode;
using static System.Console;

namespace TestConsole
{
    class Program
    {
        static void Pause(string msg = null)
        {
            WriteLine($"{msg ?? string.Empty}{Environment.NewLine}continue with NewLine");
            ReadLine();
        }

        static void Main()
        {
            DoItInSync();
            var task = DoItWithAsyncAndAwait();
            task.Wait();
            DoItWithAsyncNoAwaitButWaitMayBlock().Wait();
            DoItWithAsyncNoAwaitButConfigureAwaitFalse().Wait();
            DoItInAsyncInNewThread().Wait();
            Pause();
        }

        static void DoItInSync()
        {
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "Sync" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInSync(info);
        }

        static async Task DoItWithAsyncAndAwait()
        {
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncAndAwait" };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsync(info);
        }
        static async Task DoItWithAsyncNoAwaitButWaitMayBlock()
        {
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncNoAwaitMayBlock" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInAsync(info).Wait();
        }

        static async Task DoItWithAsyncNoAwaitButConfigureAwaitFalse()
        {
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncNoAwaitButConfigureAwaitFalse" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInAsync(info, true).Wait();
        }


        static async Task DoItInAsyncInNewThread()
        {
            var info = new InfoObject { MillisToSleep = 3000, TestCase = "AsyncThread" };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsyncInNewThread(info);
        }
    }
}
