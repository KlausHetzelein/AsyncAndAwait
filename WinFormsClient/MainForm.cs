using System;
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

        private void btnRunTest_Click(object sender, EventArgs e)
        {
            btnRunTest.Enabled = false;
            DoIt();
            btnRunTest.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void DoIt()
        {
            var info = new InfoObject { Logger = LogIt, MillisToSleep = 5000 };
            var lenghtyStuff = new LengthyStuff();

            bool done = lenghtyStuff.DoLenghtyOperation(info);
        }

        private void LogIt(string msg)
        {
            tbInfo.Text = string.IsNullOrWhiteSpace(tbInfo.Text) ? msg : $"{tbInfo.Text}{Environment.NewLine}{msg}";
            tbInfo.Update();
        }
    }
}
