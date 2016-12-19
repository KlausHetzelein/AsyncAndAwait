using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using SampleBusinessCode;

namespace WinFormsClient
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private async void btnRunTest_Click(object sender, EventArgs e)
        {
            btnRunTest.Enabled = false;
            DoItInSync();
            await DoItInAsync();
            await DoItInAsyncInNewThread();
            btnRunTest.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DoItInSync()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 5000 };
            var lenghtyStuff = new LengthyStuff();

            lenghtyStuff.DoItInSync(info);
        }

        private async Task DoItInAsync()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 5000 };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsync(info);
        }

        private async Task DoItInAsyncInNewThread()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 5000 };
            var lenghtyStuff = new LengthyStuff();

            await lenghtyStuff.DoItInAsyncInNewThread(info);
        }

        private void LogIt(string msg)
        {
            var newText = string.IsNullOrWhiteSpace(tbInfo.Text) ? msg : $"{tbInfo.Text}{Environment.NewLine}{msg}";
            if (tbInfo.InvokeRequired)
            {
                tbInfo.BeginInvoke((MethodInvoker)delegate () { tbInfo.Text = newText; });
            }
            else
            {
                tbInfo.Text = newText;
                tbInfo.Update();
            }
        }
    }
}
