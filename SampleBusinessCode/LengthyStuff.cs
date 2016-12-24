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

            info.Log($"Begin of {currentMethodName} before calling DoLengthy-Sync");
            lenghtyStuff.DoLengthyOperation(info);
            info.Log($"End of {currentMethodName} after calling DoLengthy-Sync");

            info.DecreaseIndentationLevel();
        }

        public async Task<bool> DoItInAsync(InfoObject info, bool configureAwait = true)
        {
            string currentMethodName = nameof(DoItInAsync);
            info.IncreaseIndentationLevel();
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Begin of {currentMethodName} before calling DoLengthy-Async");
            var task = lenghtyStuff.DoLengthyOperationAsync(info, configureAwait).ConfigureAwait(continueOnCapturedContext: configureAwait);

            // perhaps you can do something before you need result from async-method
            info.Log($"In {currentMethodName} after calling DoLengthy-Async, but before awaiting it...");

            // but now need result form task
            bool result = await task;

            info.Log($"End of {currentMethodName} after awaiting DoLengthy-Async...");
            info.DecreaseIndentationLevel();

            return result;
        }

        public async Task TaskDelay(InfoObject info, bool configureAwait = true)
        {
            string currentMethodName = nameof(TaskDelay);
            var lenghtyStuff = new LengthyStuff();

            info.IncreaseIndentationLevel();
            info.Log($"Begin of {currentMethodName} before awaiting Task.Delay with ContinueOnCapturedContext {configureAwait}");
            var task = Task.Delay(info.MillisToSleep).ConfigureAwait(configureAwait);
            info.Log($"In {currentMethodName} before awaiting Task.Delay");
            await task;
            info.Log($"In {currentMethodName} after awaiting Task.Delay");

            info.DecreaseIndentationLevel();
        }

        public async Task<bool> DoItInAsyncInNewThread(InfoObject info, bool configureAwait = true)
        {
            string currentMethodName = nameof(DoItInAsyncInNewThread);
            info.IncreaseIndentationLevel();
            var lenghtyStuff = new LengthyStuff();

            info.Log($"Begin of {currentMethodName} before DoLengthy-Async");
            var task = Task.Run(async() => await lenghtyStuff.DoLengthyOperationAsync(info, configureAwait)).ConfigureAwait(configureAwait);

            info.Log($"In {currentMethodName} after DoLengthy, but before await");
            bool result = await task;
            info.Log($"End of {currentMethodName} after awaiting of DoLengthy-Async");

            info.DecreaseIndentationLevel();
            return result;
        }

        public bool DoLengthyOperation(InfoObject info)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLengthyOperation);

            info.Log($"Begin of {currentMethodName}, before Thread.Sleep");
            Thread.Sleep(info.MillisToSleep);
            info.Log($"End of {currentMethodName}, after Thread.Sleep");

            info.DecreaseIndentationLevel();
            return true;
        }

        public async Task<bool> DoLengthyOperationAsyncWithCancellationToken(InfoObject info, CancellationToken ct)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLengthyOperationAsyncWithCancellationToken);

            info.Log($"Begin of {currentMethodName}, at start");

            // maybe check cancellation before starting work
            //if (ct.IsCancellationRequested)

            for (int i = 0; i < info.IterationsToSimulateWork; i++)
            {
                info.Log($"In {currentMethodName} - run <{i + 1}> ");

                // first of all check for requested cancelling
                if (ct.IsCancellationRequested)
                {
                    info.Log($"In {currentMethodName}, cancelling was requested...");

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
                info.Log($"In {currentMethodName} - run <{i + 1}> before awaiting Task.Delay()");

                info.DecreaseIndentationLevel();
                // simulate work
                await Task.Delay(info.MillisToSleep);
                info.IncreaseIndentationLevel();

                info.Log($"In {currentMethodName} - run <{i + 1}> after awaiting Task.Delay()");
            }

            info.Log($"End of {currentMethodName}, reached end of method, returning true...");
            info.DecreaseIndentationLevel();
            return true;
        }

        private async Task<bool> DoLengthyOperationAsync(InfoObject info, bool configureAwait = true)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLengthyOperationAsync);

            info.Log($"in {currentMethodName}, before awaiting Task.Delay");
            info.DecreaseIndentationLevel();
            await Task.Delay(info.MillisToSleep).ConfigureAwait(configureAwait);
            info.IncreaseIndentationLevel();
            info.Log($"in {currentMethodName}, after awaiting Task.Delay");

            info.DecreaseIndentationLevel();
            return true;
        }

        public async Task<bool> DoLengthyOpAsyncWithCtInNewThread(InfoObject info, CancellationToken ct)
        {
            info.IncreaseIndentationLevel();
            string currentMethodName = nameof(DoLengthyOpAsyncWithCtInNewThread);
            Task<bool> task = null;

            info.Log($"Begin of {currentMethodName}, before starting Task.Run of DoLengthy-Async with CT");
            try
            {
                task = Task.Run(() => DoLengthyOperationAsyncWithCancellationToken(info, ct), ct);
                info.Log($"In {currentMethodName}, before awaiting Task.Run of DoLengthy-Async with CT");
                await task;
                info.Log($"In {currentMethodName}, after awaiting Task.Run of DoLengthy-Async with CT");
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
