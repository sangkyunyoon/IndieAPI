namespace TestClient
{
    partial class FormService_Sheet
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
            this._tbFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this._lvSheets = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._lvData = new System.Windows.Forms.ListView();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _tbFilename
            // 
            this._tbFilename.Location = new System.Drawing.Point(66, 35);
            this._tbFilename.Name = "_tbFilename";
            this._tbFilename.Size = new System.Drawing.Size(127, 20);
            this._tbFilename.TabIndex = 1;
            this._tbFilename.Text = "Sample.xlsx";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filename";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(199, 35);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 20);
            this.button3.TabIndex = 2;
            this.button3.Text = "Refresh";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClick_RefreshSheet);
            // 
            // _lvSheets
            // 
            this._lvSheets.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this._lvSheets.FullRowSelect = true;
            this._lvSheets.GridLines = true;
            this._lvSheets.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvSheets.Location = new System.Drawing.Point(12, 87);
            this._lvSheets.MultiSelect = false;
            this._lvSheets.Name = "_lvSheets";
            this._lvSheets.Size = new System.Drawing.Size(223, 394);
            this._lvSheets.TabIndex = 3;
            this._lvSheets.UseCompatibleStateImageBehavior = false;
            this._lvSheets.View = System.Windows.Forms.View.Details;
            this._lvSheets.SelectedIndexChanged += new System.EventHandler(this.OnSelectChanged_Sheet);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sheet";
            this.columnHeader1.Width = 127;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Rows";
            this.columnHeader2.Width = 87;
            // 
            // _lvData
            // 
            this._lvData.FullRowSelect = true;
            this._lvData.GridLines = true;
            this._lvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvData.Location = new System.Drawing.Point(241, 87);
            this._lvData.MultiSelect = false;
            this._lvData.Name = "_lvData";
            this._lvData.Size = new System.Drawing.Size(531, 394);
            this._lvData.TabIndex = 4;
            this._lvData.UseCompatibleStateImageBehavior = false;
            this._lvData.View = System.Windows.Forms.View.Details;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(661, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 47);
            this.button1.TabIndex = 5;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_Back);
            // 
            // FormService_Sheet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._tbFilename);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this._lvSheets);
            this.Controls.Add(this._lvData);
            this.Name = "FormService_Sheet";
            this.Text = "FormService_Sheet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox _tbFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListView _lvSheets;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView _lvData;
        private System.Windows.Forms.Button button1;
    }
}