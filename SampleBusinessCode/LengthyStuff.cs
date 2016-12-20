﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace SampleBusinessCode
{
    public class LengthyStuff
    {
        public void DoItInSync(InfoObject info)
        {
            string currentMethodName = nameof(DoItInSync);

            var lenghtyStuff = new LengthyStuff();

            info.Log($"TestCase: {currentMethodName}");
            bool done = lenghtyStuff.DoLenghtyOperation(info);
            info.Log($"TestCase: {currentMethodName} after calling DoLenghtyOperation");
        }

        public async Task<bool> DoItInAsync(InfoObject info, bool configureAwait = false)
        {
            string currentMethodName = nameof(DoItInAsync);
            var lenghtyStuff = new LengthyStuff();

            info.Log($"TestCase: {currentMethodName} before calling DoLengthyOperation...", true);
            var task = lenghtyStuff.DoLenghtyOperationAsync(info, configureAwait);
            info.Log($"TestCase: {currentMethodName} after calling DoLengthyOperation...");

            // perhaps you can do something before you need result from async-method

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
            string currentMethodName = nameof(DoItInAsyncInNewThread);
            var lenghtyStuff = new LengthyStuff();

            info.Log($"TestCase: {currentMethodName} before DoLenghty", true);
            var task = Task.Run(() => lenghtyStuff.DoLenghtyOperationAsync(info));

            // but now need result form task
            info.Log($"TestCase: {currentMethodName} after DoLenghty, but before await");
            await task;
            info.Log($"TestCase: {currentMethodName} after await");
            bool result = task.Result;
        }

        private bool DoLenghtyOperation(InfoObject info)
        {
            string currentMethodName = nameof(DoLenghtyOperation);

            info.Log($"In {currentMethodName}, before Sleep");
            Thread.Sleep(info.MillisToSleep);
            info.Log($"In {currentMethodName}, after Sleep");

            return true;
        }
        public async Task<bool> DoLenghtyOperationAsyncWithCancellationToken(InfoObject info, CancellationToken ct)
        {
            string currentMethodName = nameof(DoLenghtyOperationAsyncWithCancellationToken);

            if (ct.IsCancellationRequested)
            {
                info.Log($"In {currentMethodName}, at start - cancelling already requested...");

                if (info.ThrowIfCancellingRequesting)
                {
                    info.Log($"In {currentMethodName}, throwing ThrowIfCancellingRequested...");
                    ct.ThrowIfCancellationRequested();
                }
                else
                {
                    info.Log($"In {currentMethodName}, just leaving with false...");
                    return false;
                }
            }

            for (int i = 0; i < info.IterationsToSimulateWork; i++)
            {
                info.Log($"In {currentMethodName} - run <{i + 1}> before Sleep");

                // simulate work
                await Task.Delay(info.MillisToSleep);

                if (ct.IsCancellationRequested)
                {
                    info.Log($"In {currentMethodName}, in Loop - cancelling was requested...");

                    if (info.ThrowIfCancellingRequesting)
                    {
                        info.Log($"In {currentMethodName}, throwing ThrowIfCancellingRequested...");
                        ct.ThrowIfCancellationRequested();
                    }
                    else
                    {
                        info.Log($"In {currentMethodName}, cancelling, just leaving with false...");
                        return false;
                    }
                }

                info.Log($"In {currentMethodName} - run <{i + 1}> after Sleep");
            }

            info.Log($"In {currentMethodName}, reached end of method, returning true...");
            return true;
        }

        private async Task<bool> DoLenghtyOperationAsync(InfoObject info, bool configureAwait = false)
        {
            string currentMethodName = nameof(DoLenghtyOperationAsync);

            info.Log($"In {currentMethodName}, before Sleep");
            if (configureAwait)
            {
                await Task.Delay(info.MillisToSleep).ConfigureAwait(false);
            }
            else
            {
                await Task.Delay(info.MillisToSleep);
            }
            info.Log($"In {currentMethodName}, after Sleep");

            return true;
        }

        public async Task<bool> DoLenghtyOpAsyncWithCtInNewThread(InfoObject info, CancellationToken ct)
        {
            string currentMethodName = nameof(DoLenghtyOpAsyncWithCtInNewThread);
            Task<bool> task = null;

            info.Log($"In {currentMethodName}, before starting Task.Run");
            try
            {
                task = Task.Run(() => DoLenghtyOperationAsyncWithCancellationToken(info, ct), ct);
                await task;
            }
            catch (OperationCanceledException)
            {
                info.Log($"In {currentMethodName} received OperationCanceledException, Task.State <{task?.Status}>, rethrow");
                throw;
            }

            info.Log($"In {currentMethodName} Task.State <{task?.Status}>, Result: <{task.Result}>");
            return task.Result;
        }
    }
}
