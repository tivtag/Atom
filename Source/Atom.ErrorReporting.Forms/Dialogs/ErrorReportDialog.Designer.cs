namespace Atom.ErrorReporting.Dialogs
{
    partial class ErrorReportDialog
    {
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSendReport = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxErrorInfo = new System.Windows.Forms.TextBox();
            this.buttonShowStackTrace = new System.Windows.Forms.Button();
            this.buttonCopyReport = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Unhandled Exception";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonSendReport
            // 
            this.buttonSendReport.BackColor = System.Drawing.SystemColors.Control;
            this.buttonSendReport.Location = new System.Drawing.Point(12, 116);
            this.buttonSendReport.Name = "buttonSendReport";
            this.buttonSendReport.Size = new System.Drawing.Size(86, 23);
            this.buttonSendReport.TabIndex = 1;
            this.buttonSendReport.Text = "Send Report";
            this.buttonSendReport.UseVisualStyleBackColor = false;
            this.buttonSendReport.Click += new System.EventHandler(this.OnSendReportButtonClicked);
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.SystemColors.Control;
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.Location = new System.Drawing.Point(288, 116);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(79, 23);
            this.buttonClose.TabIndex = 2;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = false;
            this.buttonClose.Click += new System.EventHandler(this.OnCloseButtonClicked);
            // 
            // textBoxErrorInfo
            // 
            this.textBoxErrorInfo.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxErrorInfo.Location = new System.Drawing.Point(13, 40);
            this.textBoxErrorInfo.Multiline = true;
            this.textBoxErrorInfo.Name = "textBoxErrorInfo";
            this.textBoxErrorInfo.ReadOnly = true;
            this.textBoxErrorInfo.Size = new System.Drawing.Size(354, 70);
            this.textBoxErrorInfo.TabIndex = 3;
            // 
            // buttonShowStackTrace
            // 
            this.buttonShowStackTrace.BackColor = System.Drawing.SystemColors.Control;
            this.buttonShowStackTrace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonShowStackTrace.Location = new System.Drawing.Point(196, 116);
            this.buttonShowStackTrace.Name = "buttonShowStackTrace";
            this.buttonShowStackTrace.Size = new System.Drawing.Size(86, 23);
            this.buttonShowStackTrace.TabIndex = 4;
            this.buttonShowStackTrace.Text = "Details..";
            this.buttonShowStackTrace.UseVisualStyleBackColor = false;
            this.buttonShowStackTrace.Click += new System.EventHandler(this.OnShowDetailsButtonClicked);
            // 
            // buttonCopyReport
            // 
            this.buttonCopyReport.BackColor = System.Drawing.SystemColors.Control;
            this.buttonCopyReport.Location = new System.Drawing.Point(104, 116);
            this.buttonCopyReport.Name = "buttonCopyReport";
            this.buttonCopyReport.Size = new System.Drawing.Size(86, 23);
            this.buttonCopyReport.TabIndex = 5;
            this.buttonCopyReport.Text = "Copy Report";
            this.buttonCopyReport.UseVisualStyleBackColor = false;
            this.buttonCopyReport.Click += new System.EventHandler(this.OnCopyReportButtonClicked);
            // 
            // ErrorReportDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(381, 144);
            this.Controls.Add(this.buttonCopyReport);
            this.Controls.Add(this.buttonShowStackTrace);
            this.Controls.Add(this.textBoxErrorInfo);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonSendReport);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorReportDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Error Report";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSendReport;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxErrorInfo;
        private System.Windows.Forms.Button buttonShowStackTrace;
        private System.Windows.Forms.Button buttonCopyReport;
    }
}