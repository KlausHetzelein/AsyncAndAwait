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
            DoItInAsync();
            DoItInAsyncInNewThread();
            Pause();
        }

        static void DoItInSync()
        {
            var info = new InfoObject { MillisToSleep = 3000 };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInSync(info);
        }

        static async Task DoItInAsync()
        {
            var info = new InfoObject { MillisToSleep = 3000 };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsync(info);
        }

        static async Task DoItInAsyncInNewThread()
        {
            var info = new InfoObject { MillisToSleep = 3000 };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsyncInNewThread(info);
        }
    }
}
