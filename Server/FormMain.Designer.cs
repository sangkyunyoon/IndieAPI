namespace IndieAPI.Server
{
    partial class FormMain
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
            this._lbTaskCount = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this._lbCCU = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this._tbLog = new System.Windows.Forms.TextBox();
            this._btnStart = new System.Windows.Forms.Button();
            this._btnStop = new System.Windows.Forms.Button();
            this._lbCachedUserCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._lbCacheBoxItemCount = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _lbTaskCount
            // 
            this._lbTaskCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lbTaskCount.Location = new System.Drawing.Point(340, 122);
            this._lbTaskCount.Name = "_lbTaskCount";
            this._lbTaskCount.Size = new System.Drawing.Size(106, 23);
            this._lbTaskCount.TabIndex = 84;
            this._lbTaskCount.Text = "0";
            this._lbTaskCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(237, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(99, 23);
            this.label3.TabIndex = 83;
            this.label3.Text = "Task Count";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _lbCCU
            // 
            this._lbCCU.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lbCCU.Location = new System.Drawing.Point(340, 89);
            this._lbCCU.Name = "_lbCCU";
            this._lbCCU.Size = new System.Drawing.Size(106, 23);
            this._lbCCU.TabIndex = 82;
            this._lbCCU.Text = "0";
            this._lbCCU.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(236, 89);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(99, 23);
            this.label6.TabIndex = 81;
            this.label6.Text = "CCU";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbLog
            // 
            this._tbLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tbLog.Location = new System.Drawing.Point(15, 161);
            this._tbLog.MaxLength = 1048576;
            this._tbLog.Multiline = true;
            this._tbLog.Name = "_tbLog";
            this._tbLog.ReadOnly = true;
            this._tbLog.Size = new System.Drawing.Size(431, 218);
            this._tbLog.TabIndex = 76;
            this._tbLog.TabStop = false;
            // 
            // _btnStart
            // 
            this._btnStart.Location = new System.Drawing.Point(12, 12);
            this._btnStart.Name = "_btnStart";
            this._btnStart.Size = new System.Drawing.Size(99, 56);
            this._btnStart.TabIndex = 74;
            this._btnStart.Text = "Start";
            this._btnStart.UseVisualStyleBackColor = true;
            this._btnStart.Click += new System.EventHandler(this.OnClick_Start);
            // 
            // _btnStop
            // 
            this._btnStop.Location = new System.Drawing.Point(116, 12);
            this._btnStop.Name = "_btnStop";
            this._btnStop.Size = new System.Drawing.Size(99, 56);
            this._btnStop.TabIndex = 75;
            this._btnStop.Text = "Stop";
            this._btnStop.UseVisualStyleBackColor = true;
            this._btnStop.Click += new System.EventHandler(this.OnClick_Stop);
            // 
            // _lbCachedUserCount
            // 
            this._lbCachedUserCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lbCachedUserCount.Location = new System.Drawing.Point(116, 89);
            this._lbCachedUserCount.Name = "_lbCachedUserCount";
            this._lbCachedUserCount.Size = new System.Drawing.Size(106, 23);
            this._lbCachedUserCount.TabIndex = 86;
            this._lbCachedUserCount.Text = "0";
            this._lbCachedUserCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(99, 23);
            this.label2.TabIndex = 85;
            this.label2.Text = "Cached Users";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _lbCacheBoxItemCount
            // 
            this._lbCacheBoxItemCount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lbCacheBoxItemCount.Location = new System.Drawing.Point(116, 122);
            this._lbCacheBoxItemCount.Name = "_lbCacheBoxItemCount";
            this._lbCacheBoxItemCount.Size = new System.Drawing.Size(106, 23);
            this._lbCacheBoxItemCount.TabIndex = 88;
            this._lbCacheBoxItemCount.Text = "0";
            this._lbCacheBoxItemCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(12, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 23);
            this.label4.TabIndex = 87;
            this.label4.Text = "CacheBox";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(470, 397);
            this.Controls.Add(this._lbCacheBoxItemCount);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._lbCachedUserCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._lbTaskCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._lbCCU);
            this.Controls.Add(this.label6);
            this.Controls.Add(this._tbLog);
            this.Controls.Add(this._btnStart);
            this.Controls.Add(this._btnStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IndieAPI Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label _lbTaskCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label _lbCCU;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _tbLog;
        private System.Windows.Forms.Button _btnStart;
        private System.Windows.Forms.Button _btnStop;
        private System.Windows.Forms.Label _lbCachedUserCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label _lbCacheBoxItemCount;
        private System.Windows.Forms.Label label4;
    }
}

