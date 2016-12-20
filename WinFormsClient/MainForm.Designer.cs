namespace WinFormsClient
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnRunTest = new System.Windows.Forms.Button();
            this.tbInfo = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbTestCases = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbClearList = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnRunTest
            // 
            this.btnRunTest.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRunTest.Location = new System.Drawing.Point(713, 593);
            this.btnRunTest.Name = "btnRunTest";
            this.btnRunTest.Size = new System.Drawing.Size(149, 48);
            this.btnRunTest.TabIndex = 1;
            this.btnRunTest.Text = "&Run Test";
            this.btnRunTest.UseCompatibleTextRendering = true;
            this.btnRunTest.UseVisualStyleBackColor = true;
            this.btnRunTest.Click += new System.EventHandler(this.btnRunTest_Click);
            // 
            // tbInfo
            // 
            this.tbInfo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbInfo.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbInfo.Location = new System.Drawing.Point(13, 12);
            this.tbInfo.Multiline = true;
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.ReadOnly = true;
            this.tbInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbInfo.Size = new System.Drawing.Size(1200, 500);
            this.tbInfo.TabIndex = 2;
            this.tbInfo.TabStop = false;
            // 
            // btnExit
            // 
            this.btnExit.CausesValidation = false;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(1091, 593);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(122, 48);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Enabled = false;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(878, 593);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(149, 48);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel Test";
            this.btnCancel.UseCompatibleTextRendering = true;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbTestCases
            // 
            this.cbTestCases.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTestCases.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTestCases.FormattingEnabled = true;
            this.cbTestCases.Location = new System.Drawing.Point(55, 602);
            this.cbTestCases.Name = "cbTestCases";
            this.cbTestCases.Size = new System.Drawing.Size(587, 31);
            this.cbTestCases.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(51, 555);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "&Testfälle";
            // 
            // cbClearList
            // 
            this.cbClearList.AutoSize = true;
            this.cbClearList.Checked = true;
            this.cbClearList.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbClearList.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbClearList.Location = new System.Drawing.Point(327, 556);
            this.cbClearList.Name = "cbClearList";
            this.cbClearList.Size = new System.Drawing.Size(315, 25);
            this.cbClearList.TabIndex = 4;
            this.cbClearList.Text = "&Ausgabeliste vor jedem Test leeren";
            this.cbClearList.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AcceptButton = this.btnRunTest;
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnExit;
            this.ClientSize = new System.Drawing.Size(1258, 680);
            this.Controls.Add(this.cbClearList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbTestCases);
            this.Controls.Add(this.tbInfo);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRunTest);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "AsyncAndAwait Tests";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnRunTest;
        private System.Windows.Forms.TextBox tbInfo;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbTestCases;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbClearList;
    }
}

