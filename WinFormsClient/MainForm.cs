using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SampleBusinessCode;

namespace WinFormsClient
{
    public partial class frmMain : Form
    {
        private const int TV_LONG_RUNNING_JUST_AWAIT = 1;
        private const int TV_LONG_RUNNING_AWAIT_AND_TASK_RUN = 2;
        private const int TV_LONG_RUNNING_AWAIT_AND_TASK_RUN_WITH_EXCEPTION_HANDLING = 3;
        private const int TV_LONG_RUNNING_CANCELED_BEFORE_START = 10;

        private CancellationTokenSource _cts;
        private delegate Task TestMethod();
        private class TestCase
        {
            public string Description { get; set; }

            public TestMethod Method { get; set; }

            public int VariantInTest { get; set; }

            public int SimulatedWorkInMillis { get; set; } = 5000;
        }
        private List<TestCase> _testCases = new List<TestCase>();

        private void FillTestCases()
        {
            _testCases.Add(new TestCase { Description = "Synchronous Call, UI not responsive", Method = DoItInSync });
            _testCases.Add(new TestCase { Description = "Async and Await, UI responsive", Method = DoItWithAsyncAndAwait });
            _testCases.Add(new TestCase { Description = "Async No Await - Will Block", Method = DoItWithAsyncNoAwaitButWaitMayBlock });
            _testCases.Add(new TestCase { Description = "Async No Await, but ConfigureAwait False", Method = DoItWithAsyncNoAwaitButConfigureAwaitFalse });
            _testCases.Add(new TestCase { Description = "Async with new Thread", Method = DoItInAsyncInNewThread });
            _testCases.Add(new TestCase { Description = "Cancelable longrunning Async", Method = HandleLongRunningTask, VariantInTest = TV_LONG_RUNNING_JUST_AWAIT, SimulatedWorkInMillis = 2000 });
            _testCases.Add(new TestCase { Description = "Cancelable longrunning new Thread", Method = HandleLongRunningTask, VariantInTest = TV_LONG_RUNNING_AWAIT_AND_TASK_RUN, SimulatedWorkInMillis = 2000 });
            _testCases.Add(new TestCase { Description = "Cancelable longrunning new Thread and exc-handling", Method = HandleLongRunningTask, VariantInTest = TV_LONG_RUNNING_AWAIT_AND_TASK_RUN_WITH_EXCEPTION_HANDLING, SimulatedWorkInMillis = 2000 });
            _testCases.Add(new TestCase { Description = "Already canceled longrunning async", Method = HandleLongRunningTask, VariantInTest = TV_LONG_RUNNING_CANCELED_BEFORE_START });

            foreach (var tc in _testCases)
            {
                cbTestCases.Items.Add(tc.Description);
            }

            cbTestCases.SelectedIndex = 0;
        }

        private void LogIt(string msg)
        {
            if (!UiLoggingEnabled)
            {
                return;
            }

            var newText = $"{msg}{Environment.NewLine}";
            if (tbInfo.InvokeRequired)
            {
                tbInfo.BeginInvoke((MethodInvoker)delegate ()
                {
                    tbInfo.AppendText(newText);
                });
            }
            else
            {
                tbInfo.AppendText(newText);
                tbInfo.Update();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = false;
            if (null == _cts || _cts.IsCancellationRequested)
            {
                LogIt($"Cancelling requested..., but no CTS or already requested...");
            }
            else
            {
                LogIt($"Cancelling requested...");
                _cts?.Cancel();
            }
        }


        private int GetTestVariant() => _testCases[cbTestCases.SelectedIndex]?.VariantInTest ?? 0;

        private int GetSimulatedWorkInMillis() => _testCases[cbTestCases.SelectedIndex]?.SimulatedWorkInMillis ?? 5000;

        public bool UiLoggingEnabled { get; set; } = true;

        public frmMain()
        {
            InitializeComponent();
            FillTestCases();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void btnRunTest_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnRunTest.Enabled = false;

            if (cbClearList.CheckState == CheckState.Checked)
            {
                tbInfo.Clear();
            }

            int index = cbTestCases.SelectedIndex;
            TestCase tc = _testCases[index];
            LogIt($"Starting <{tc.Description}> with <{tc.SimulatedWorkInMillis}>ms of simulated work...");
            await tc.Method();
            LogIt($"Back from <{tc.Description}>");

            btnRunTest.Enabled = true;
            btnCancel.Enabled = false;
        }

        private async Task<bool> HandleLongRunningTask()
        {
            string currentMethodName = "HandleLongRunningTask";
            Task<bool> task = null;
            bool result = false;
            _cts = new CancellationTokenSource();
            var info = new InfoObject { Logger = LogIt, ThrowIfCancellingRequesting = true, TestCase = "CancellationToken", MillisToSleep = GetSimulatedWorkInMillis() };
            var lenghtyStuff = new LengthyStuff();

            int testVariant = GetTestVariant();
            if (testVariant == TV_LONG_RUNNING_CANCELED_BEFORE_START)
            {
                // cancel the task right away, will throw TaskCanceledException, if ct is provided for Task and not only for Method
                _cts.Cancel();
            }

            try
            {
                info.Log($"In {currentMethodName} before starting DoLengthy...");
                if (testVariant == TV_LONG_RUNNING_JUST_AWAIT)
                {
                    task = lenghtyStuff.DoLenghtyOperationAsyncWithCancellationToken(info, _cts.Token);
                }
                else if (testVariant == TV_LONG_RUNNING_AWAIT_AND_TASK_RUN)
                {
                    task = Task.Run(async () => await lenghtyStuff.DoLenghtyOperationAsyncWithCancellationToken(info, _cts.Token), _cts.Token);

                }
                else if (testVariant == TV_LONG_RUNNING_AWAIT_AND_TASK_RUN_WITH_EXCEPTION_HANDLING)
                {
                    task = lenghtyStuff.DoLenghtyOpAsyncWithCtInNewThread(info, _cts.Token);
                }

                if (null == task)
                {
                    // to see a status of a task
                    task = Task.Run(async () => { await Task.Delay(1); return false; }, _cts.Token);
                }

                result = await task;
                info.Log($"In {currentMethodName} after awaiting DoLengthy...");
            }
            // unnecessary, cos TaskCanceledException is derived from OperationCanceledExceoption
            //catch (TaskCanceledException)
            //{
            //    info.Log($"In {currentMethodName} received TaskCanceledException, return false");
            //    result = false;
            //}
            catch (OperationCanceledException)
            {
                info.Log($"In {currentMethodName} received OperationCanceledException, return false");
                result = false;
            }

            info.Log($"In {currentMethodName} Task.State <{task?.Status}>, Result: <{result}>");
            return result;
        }

        private Task DoItInSync()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = GetSimulatedWorkInMillis(), TestCase = "Sync" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInSync(info);
            return Task.Delay(1);
        }

        private async Task DoItWithAsyncAndAwait()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = GetSimulatedWorkInMillis(), TestCase = "AsyncAndAwait" };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsync(info);
        }

        // that blocks ui-thread
        private async Task DoItWithAsyncNoAwaitButWaitMayBlock()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 2000, TestCase = "AsyncNoAwaitMayBlock" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInAsync(info).Wait();
        }

        private async Task DoItWithAsyncNoAwaitButConfigureAwaitFalse()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = GetSimulatedWorkInMillis(), TestCase = "AsyncNoAwaitButConfigureAwaitFalse" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInAsync(info, true).Wait();
        }

        private async Task DoItInAsyncInNewThread()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = GetSimulatedWorkInMillis(), TestCase = "AsyncInNewThread" };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsyncInNewThread(info);
        }
    }
}
