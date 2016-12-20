using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SampleBusinessCode;

namespace WinFormsClient
{
    public partial class frmMain : Form
    {
        private CancellationTokenSource _cts;


        public frmMain()
        {
            InitializeComponent();
        }

        public bool UiLoggingEnabled { get; set; } = true;

        private async void btnRunTest_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnRunTest.Enabled = false;

            //DoItInSync();
            //await DoItWithAsyncAndAwait();
            ////await DoItWithAsyncNoAwaitMayBlock();
            //await DoItWithAsyncNoAwaitButConfigureAwaitFalse();
            //await DoItInAsyncInNewThread();

            await HandleLongRunningTask();

            btnRunTest.Enabled = true;
            btnCancel.Enabled = false;
        }

        private async Task<bool> HandleLongRunningTask()
        {
            string currentMethodName = "HandleLongRunningTask";
            Task<bool> task = null;
            bool result = false;
            _cts = new CancellationTokenSource();
            var info = new InfoObject { Logger = LogIt, ThrowIfCancellingRequesting = true }; //, TestCase = "CancellingLongRunningTask" };
            var lenghtyStuff = new LengthyStuff();

            // cancel the task right away, will throw TaskCanceledException, if ct is provided for Task and not only for Method
            //_cts.Cancel();

            try
            {
                info.Log($"In {currentMethodName} before starting DoLengthy...");
                //task = lenghtyStuff.DoLenghtyOperationAsyncWithCancellationToken(info, _cts.Token);
                //task = Task.Run(async () => await lenghtyStuff.DoLenghtyOperationAsyncWithCancellationToken(info, _cts.Token), _cts.Token);
                task = lenghtyStuff.DoLenghtyOpAsyncWithCtInNewThread(info, _cts.Token);
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DoItInSync()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 2000, TestCase = "Sync" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInSync(info);
        }

        private async Task DoItWithAsyncAndAwait()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 2000, TestCase = "AsyncAndAwait" };
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
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 2000, TestCase = "AsyncNoAwaitButConfigureAwaitFalse" };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInAsync(info, true).Wait();
        }

        private async Task DoItInAsyncInNewThread()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 2000, TestCase = "AsyncInNewThread" };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsyncInNewThread(info);
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
    }
}
