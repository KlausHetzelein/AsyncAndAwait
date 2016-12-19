using System;
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
            var info = new InfoObject();
            var lenghtyStuff = new LengthyStuff();

            info.Log("Before LengthyOperation...");

            bool done = lenghtyStuff.DoLenghtyOperation(info);

            info.Log("After LengthyOperation...");

            Pause();
        }
    }
}
