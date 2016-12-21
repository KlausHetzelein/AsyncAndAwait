using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleBusinessCode
{
    public class LengthyStuff
    {
        public void DoItInSync(InfoObject info)
        {
            string currentMethodName = nameof(DoItInSync);
            info.IncreaseIndentationLevel();

            var lenghtyStuff = new LengthyStuff();

            info.Log($"Begin of {currentMethodName} before calling DoLenghty-Sync");
            lenghtyStuff.DoLenghtyOperation(info);
            info.Log($"End of {currentMethodName} after calling DoLenghty-Sync");

            info.DecreaseIndentationLevel();
        }

        public async Task<bool> DoItInAsync(InfoObject info, bool configureAwait = false)
        {
            string currentMethodName = nameof(DoItInAsync);
            info.IncreaseIndentationLevel();
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Begin of {currentMethodName} before calling DoLengthy-Async");
            var task = lenghtyStuff.DoLenghtyOperationAsync(info, configureAwait);

            // perhaps you can do something before you need result from async-method
            info.Log($"In {currentMethodName} after calling DoLengthy-Async, but before awaiting it...");


            // but now need result form task
            if (configureAwait)
            {
                await task.ConfigureAwait(false);
            }
            else
            {
                await task;
            }

            info.Log($"End of {currentMethodName} after awaiting DoLengthy-Async...");
            info.DecreaseIndentationLevel();

            return task.Result;
        }

        public async Task DoItInAsyncInNewThread(InfoObject info)
        {
            string currentMethodName = nameof(DoItInAsyncInNewThread);
            info.IncreaseIndentationLevel();
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Begin of {currentMethodName} before DoLenghty-Async in new Thread");
            var task = Task.Run(() => lenghtyStuff.DoLenghtyOperationAsync(info));

            info.Log($"In {currentMethodName} after DoLenghty, but before await");
            await task;
            info.Log($"End of {currentMethodName} after await of DoLenghty-Async in new Thread");

            info.DecreaseIndentationLevel();
            bool result = task.Result;
        }

        private bool DoLenghtyOperation(InfoObject info)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLenghtyOperation);

            info.Log($"Begin of {currentMethodName}, before Thread.Sleep");
            Thread.Sleep(info.MillisToSleep);
            info.Log($"End of {currentMethodName}, after Thread.Sleep");

            info.DecreaseIndentationLevel();
            return true;
        }
        public async Task<bool> DoLenghtyOperationAsyncWithCancellationToken(InfoObject info, CancellationToken ct)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLenghtyOperationAsyncWithCancellationToken);

            info.Log($"Begin of {currentMethodName}, at start");

            if (ct.IsCancellationRequested)
            {
                info.Log($"In {currentMethodName}, at start - cancelling already requested...");

                if (info.ThrowIfCancellingRequesting)
                {
                    info.Log($"End of {currentMethodName}, throwing ThrowIfCancellingRequested...");
                    info.DecreaseIndentationLevel();
                    ct.ThrowIfCancellationRequested();
                }
                else
                {
                    info.Log($"End of {currentMethodName}, just leaving with false...");
                    info.DecreaseIndentationLevel();
                    return false;
                }
            }

            for (int i = 0; i < info.IterationsToSimulateWork; i++)
            {
                info.Log($"In {currentMethodName} - run <{i + 1}> before awaiting Task.Delay()");

                info.DecreaseIndentationLevel();
                // simulate work
                await Task.Delay(info.MillisToSleep);
                info.IncreaseIndentationLevel();

                if (ct.IsCancellationRequested)
                {
                    info.Log($"In {currentMethodName}, in Loop - cancelling was requested...");

                    if (info.ThrowIfCancellingRequesting)
                    {
                        info.Log($"End of {currentMethodName}, throwing ThrowIfCancellingRequested...");
                        info.DecreaseIndentationLevel();
                        ct.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        info.Log($"End of {currentMethodName}, cancelling, just leaving with false...");
                        info.DecreaseIndentationLevel();
                        return false;
                    }
                }

                info.Log($"In {currentMethodName} - run <{i + 1}> after Sleep");
            }

            info.Log($"End of {currentMethodName}, reached end of method, returning true...");
            info.DecreaseIndentationLevel();
            return true;
        }

        private async Task<bool> DoLenghtyOperationAsync(InfoObject info, bool configureAwait = false)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLenghtyOperationAsync);

            info.Log($"Begin of {currentMethodName}, before await Task.Delay");
            info.DecreaseIndentationLevel();
            if (configureAwait)
            {
                await Task.Delay(info.MillisToSleep).ConfigureAwait(false);
            }
            else
            {
                await Task.Delay(info.MillisToSleep);
            }
            info.IncreaseIndentationLevel();
            info.Log($"End of {currentMethodName}, after awaiting Task.Delay");

            info.DecreaseIndentationLevel();
            return true;
        }

        public async Task<bool> DoLenghtyOpAsyncWithCtInNewThread(InfoObject info, CancellationToken ct)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLenghtyOpAsyncWithCtInNewThread);
            Task<bool> task = null;

            info.Log($"Begin of {currentMethodName}, before starting Task.Run of DoLenghty-Async with CT");
            try
            {
                task = Task.Run(() => DoLenghtyOperationAsyncWithCancellationToken(info, ct), ct);
                info.Log($"In {currentMethodName}, before awaiting Task.Run of DoLenghty-Async with CT");
                await task;
                info.Log($"In {currentMethodName}, after awaiting Task.Run of DoLenghty-Async with CT");
            }
            catch (OperationCanceledException)
            {
                info.Log($"End of {currentMethodName} received OperationCanceledException, Task.State <{task?.Status}>, rethrow");
                info.DecreaseIndentationLevel();
                throw;
            }

            info.Log($"End of {currentMethodName} Task.State <{task?.Status}>, Result: <{task.Result}>");
            info.DecreaseIndentationLevel();
            return task.Result;
        }
    }
}
