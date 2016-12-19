using System.Threading;
using System.Threading.Tasks;

namespace SampleBusinessCode
{
    public class LengthyStuff
    {
        public void DoItInSync(InfoObject info)
        {
            var lenghtyStuff = new LengthyStuff();

            info.Log($"TestCase: {nameof(DoItInSync)}");
            bool done = lenghtyStuff.DoLenghtyOperation(info);
        }

        public async Task<bool> DoItInAsync(InfoObject info, bool configureAwait = false)
        {
            var lenghtyStuff = new LengthyStuff();

            info.Log($"TestCase: {nameof(DoItInAsync)}", true);
            var task = lenghtyStuff.DoLenghtyOperationAsync(info, configureAwait);

            // u can do something here...

            // but now need result form task
            if (configureAwait)
            {
                await task.ConfigureAwait(false);
            }
            else
            {
                await task;
            }

            return task.Result;
        }

        public async Task DoItInAsyncInNewThread(InfoObject info)
        {
            var lenghtyStuff = new LengthyStuff();

            info.Log($"TestCase: {nameof(DoItInAsyncInNewThread)}", true);
            var task = Task.Run<bool>(() => lenghtyStuff.DoLenghtyOperationAsync(info));

            // u can do something here...

            // but now need result form task
            await task;
            bool result = task.Result;
        }

        public bool DoLenghtyOperation(InfoObject info)
        {
            info.Log($"In {nameof(DoLenghtyOperation)}, before Sleep");
            Thread.Sleep(info.MillisToSleep);
            info.Log($"In {nameof(DoLenghtyOperation)}, after Sleep");

            return true;
        }
        public async Task<bool> DoLenghtyOperationAsync(InfoObject info, bool configureAwait = false)
        {
            info.Log($"In {nameof(DoLenghtyOperationAsync)}, before Sleep");
            if (configureAwait)
            {
                await Task.Delay(info.MillisToSleep).ConfigureAwait(false);
            }
            else
            {
                await Task.Delay(info.MillisToSleep);
            }
            info.Log($"In {nameof(DoLenghtyOperationAsync)}, after Sleep");

            return true;
        }
    }
}
