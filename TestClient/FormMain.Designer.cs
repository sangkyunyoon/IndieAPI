namespace TestClient
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
            this._tbMesssage = new System.Windows.Forms.Label();
            this._panelContent = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _tbMesssage
            // 
            this._tbMesssage.ForeColor = System.Drawing.SystemColors.HotTrack;
            this._tbMesssage.Location = new System.Drawing.Point(12, 615);
            this._tbMesssage.Name = "_tbMesssage";
            this._tbMesssage.Size = new System.Drawing.Size(688, 13);
            this._tbMesssage.TabIndex = 16;
            this._tbMesssage.Text = "Ready";
            this._tbMesssage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _panelContent
            // 
            this._panelContent.Location = new System.Drawing.Point(12, 12);
            this._panelContent.Name = "_panelContent";
            this._panelContent.Size = new System.Drawing.Size(800, 600);
            this._panelContent.TabIndex = 15;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(737, 615);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 17;
            this.button1.Text = "Disconnect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_Disconnect);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 642);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._tbMesssage);
            this.Controls.Add(this._panelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TestClient";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label _tbMesssage;
        private System.Windows.Forms.Panel _panelContent;
        private System.Windows.Forms.Button button1;
    }
}

