namespace TestClient
{
    partial class FormServiceMain
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
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this._tbTextData = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this._tbFilename = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this._lvTables = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._lvData = new System.Windows.Forms.ListView();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(19, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 31);
            this.button1.TabIndex = 5;
            this.button1.Text = "Get Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_GetTextData);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._tbTextData);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 211);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Text Storage";
            // 
            // _tbTextData
            // 
            this._tbTextData.Location = new System.Drawing.Point(19, 70);
            this._tbTextData.Multiline = true;
            this._tbTextData.Name = "_tbTextData";
            this._tbTextData.Size = new System.Drawing.Size(408, 113);
            this._tbTextData.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(126, 33);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 6;
            this.button2.Text = "Set Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClick_SetTextData);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this._tbFilename);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this._lvTables);
            this.groupBox2.Controls.Add(this._lvData);
            this.groupBox2.Location = new System.Drawing.Point(12, 229);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(708, 320);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Text Storage";
            // 
            // _tbFilename
            // 
            this._tbFilename.Location = new System.Drawing.Point(441, 294);
            this._tbFilename.Name = "_tbFilename";
            this._tbFilename.Size = new System.Drawing.Size(127, 20);
            this._tbFilename.TabIndex = 7;
            this._tbFilename.Text = "Sample.xlsx";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(387, 298);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Filename";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(574, 294);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(101, 20);
            this.button3.TabIndex = 5;
            this.button3.Text = "Refresh";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClick_RefreshCloudSheet);
            // 
            // _lvTables
            // 
            this._lvTables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this._lvTables.FullRowSelect = true;
            this._lvTables.GridLines = true;
            this._lvTables.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvTables.Location = new System.Drawing.Point(19, 31);
            this._lvTables.MultiSelect = false;
            this._lvTables.Name = "_lvTables";
            this._lvTables.Size = new System.Drawing.Size(208, 257);
            this._lvTables.TabIndex = 4;
            this._lvTables.UseCompatibleStateImageBehavior = false;
            this._lvTables.View = System.Windows.Forms.View.Details;
            this._lvTables.SelectedIndexChanged += new System.EventHandler(this.OnSelected_Table);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Table";
            this.columnHeader1.Width = 127;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Rows";
            this.columnHeader2.Width = 77;
            // 
            // _lvData
            // 
            this._lvData.FullRowSelect = true;
            this._lvData.GridLines = true;
            this._lvData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvData.Location = new System.Drawing.Point(242, 31);
            this._lvData.MultiSelect = false;
            this._lvData.Name = "_lvData";
            this._lvData.Size = new System.Drawing.Size(433, 257);
            this._lvData.TabIndex = 3;
            this._lvData.UseCompatibleStateImageBehavior = false;
            this._lvData.View = System.Windows.Forms.View.Details;
            // 
            // FormServiceMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormServiceMain";
            this.Text = "FormServiceMain";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox _tbTextData;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox _tbFilename;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ListView _lvTables;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ListView _lvData;
    }
}