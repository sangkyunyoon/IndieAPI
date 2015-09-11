namespace TestClient
{
    partial class FormService_Profile
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
            this._tbTextData = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._tbNickname = new System.Windows.Forms.TextBox();
            this._tbLevel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._tbExp = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this._tbContinuousCount = new System.Windows.Forms.TextBox();
            this._tbRegDate = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this._tbLastLoginDate = new System.Windows.Forms.TextBox();
            this._tbDailyCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(242, 294);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 31);
            this.button1.TabIndex = 5;
            this.button1.Text = "Get Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_GetTextData);
            // 
            // _tbTextData
            // 
            this._tbTextData.Location = new System.Drawing.Point(42, 175);
            this._tbTextData.Multiline = true;
            this._tbTextData.Name = "_tbTextData";
            this._tbTextData.Size = new System.Drawing.Size(408, 113);
            this._tbTextData.TabIndex = 7;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(349, 294);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 31);
            this.button2.TabIndex = 6;
            this.button2.Text = "Set Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClick_SetTextData);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 432);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(128, 54);
            this.button3.TabIndex = 7;
            this.button3.Text = "CloudSheet";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClick_CloudSheet);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(42, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 8;
            this.label1.Text = "Nickname";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbNickname
            // 
            this._tbNickname.Location = new System.Drawing.Point(148, 42);
            this._tbNickname.Name = "_tbNickname";
            this._tbNickname.Size = new System.Drawing.Size(100, 20);
            this._tbNickname.TabIndex = 9;
            // 
            // _tbLevel
            // 
            this._tbLevel.Location = new System.Drawing.Point(148, 70);
            this._tbLevel.Name = "_tbLevel";
            this._tbLevel.Size = new System.Drawing.Size(100, 20);
            this._tbLevel.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(42, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 23);
            this.label2.TabIndex = 10;
            this.label2.Text = "Level";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbExp
            // 
            this._tbExp.Location = new System.Drawing.Point(148, 98);
            this._tbExp.Name = "_tbExp";
            this._tbExp.Size = new System.Drawing.Size(100, 20);
            this._tbExp.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(42, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 23);
            this.label3.TabIndex = 12;
            this.label3.Text = "Exp";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Controls.Add(this._tbTextData);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.button2);
            this.groupBox2.Controls.Add(this._tbExp);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this._tbNickname);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this._tbLevel);
            this.groupBox2.Location = new System.Drawing.Point(12, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(481, 347);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Profiles";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(42, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(408, 23);
            this.label4.TabIndex = 16;
            this.label4.Text = "Text Storage";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(270, 40);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 79);
            this.button4.TabIndex = 15;
            this.button4.Text = "Update Profile";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnClick_UpdateProfile);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this._tbDailyCount);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this._tbContinuousCount);
            this.groupBox1.Controls.Add(this._tbRegDate);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this._tbLastLoginDate);
            this.groupBox1.Location = new System.Drawing.Point(499, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 347);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login Count";
            // 
            // label6
            // 
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Location = new System.Drawing.Point(32, 40);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 23);
            this.label6.TabIndex = 14;
            this.label6.Text = "Register Date";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbContinuousCount
            // 
            this._tbContinuousCount.Location = new System.Drawing.Point(32, 211);
            this._tbContinuousCount.Name = "_tbContinuousCount";
            this._tbContinuousCount.ReadOnly = true;
            this._tbContinuousCount.Size = new System.Drawing.Size(166, 20);
            this._tbContinuousCount.TabIndex = 19;
            this._tbContinuousCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _tbRegDate
            // 
            this._tbRegDate.Location = new System.Drawing.Point(32, 66);
            this._tbRegDate.Name = "_tbRegDate";
            this._tbRegDate.ReadOnly = true;
            this._tbRegDate.Size = new System.Drawing.Size(166, 20);
            this._tbRegDate.TabIndex = 15;
            this._tbRegDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label7.Location = new System.Drawing.Point(32, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(166, 23);
            this.label7.TabIndex = 18;
            this.label7.Text = "Continuous Count";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label8.Location = new System.Drawing.Point(32, 109);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(166, 23);
            this.label8.TabIndex = 16;
            this.label8.Text = "Last Login Date";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbLastLoginDate
            // 
            this._tbLastLoginDate.Location = new System.Drawing.Point(32, 135);
            this._tbLastLoginDate.Name = "_tbLastLoginDate";
            this._tbLastLoginDate.ReadOnly = true;
            this._tbLastLoginDate.Size = new System.Drawing.Size(166, 20);
            this._tbLastLoginDate.TabIndex = 17;
            this._tbLastLoginDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _tbDailyCount
            // 
            this._tbDailyCount.Location = new System.Drawing.Point(32, 280);
            this._tbDailyCount.Name = "_tbDailyCount";
            this._tbDailyCount.ReadOnly = true;
            this._tbDailyCount.Size = new System.Drawing.Size(166, 20);
            this._tbDailyCount.TabIndex = 21;
            this._tbDailyCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(32, 254);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 23);
            this.label5.TabIndex = 20;
            this.label5.Text = "Daily Count";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormServiceMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button3);
            this.Name = "FormServiceMain";
            this.Text = "FormServiceMain";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _tbTextData;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _tbNickname;
        private System.Windows.Forms.TextBox _tbLevel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _tbExp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox _tbContinuousCount;
        private System.Windows.Forms.TextBox _tbRegDate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox _tbLastLoginDate;
        private System.Windows.Forms.TextBox _tbDailyCount;
        private System.Windows.Forms.Label label5;
    }
}