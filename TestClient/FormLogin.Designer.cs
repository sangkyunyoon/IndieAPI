namespace TestClient
{
    partial class FormLogin
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
            this._tbServerIp = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this._tbServerPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._tbGuest_UUID = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this._tbMember_UserPwd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this._tbMember_UserId = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this._tbMember_UUID = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(20, 157);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 41);
            this.button1.TabIndex = 2;
            this.button1.Text = "Register";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_GuestRegister);
            // 
            // _tbServerIp
            // 
            this._tbServerIp.Location = new System.Drawing.Point(318, 49);
            this._tbServerIp.Name = "_tbServerIp";
            this._tbServerIp.Size = new System.Drawing.Size(100, 20);
            this._tbServerIp.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(212, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Server Address";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbServerPort
            // 
            this._tbServerPort.Location = new System.Drawing.Point(424, 49);
            this._tbServerPort.Name = "_tbServerPort";
            this._tbServerPort.Size = new System.Drawing.Size(56, 20);
            this._tbServerPort.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(78, 76);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "UUID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbGuest_UUID
            // 
            this._tbGuest_UUID.Location = new System.Drawing.Point(184, 76);
            this._tbGuest_UUID.Name = "_tbGuest_UUID";
            this._tbGuest_UUID.Size = new System.Drawing.Size(100, 20);
            this._tbGuest_UUID.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this._tbGuest_UUID);
            this.groupBox1.Location = new System.Drawing.Point(18, 135);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(368, 227);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Login as Guest";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 157);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 41);
            this.button2.TabIndex = 3;
            this.button2.Text = "Login";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClick_GuestLogin);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this._tbMember_UserPwd);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this._tbMember_UserId);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this._tbMember_UUID);
            this.groupBox2.Controls.Add(this.button4);
            this.groupBox2.Location = new System.Drawing.Point(392, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(380, 227);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Login as Member";
            // 
            // label5
            // 
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Location = new System.Drawing.Point(81, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 20);
            this.label5.TabIndex = 4;
            this.label5.Text = "User Password";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbMember_UserPwd
            // 
            this._tbMember_UserPwd.Location = new System.Drawing.Point(187, 102);
            this._tbMember_UserPwd.Name = "_tbMember_UserPwd";
            this._tbMember_UserPwd.Size = new System.Drawing.Size(100, 20);
            this._tbMember_UserPwd.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Location = new System.Drawing.Point(81, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "User ID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbMember_UserId
            // 
            this._tbMember_UserId.Location = new System.Drawing.Point(187, 76);
            this._tbMember_UserId.Name = "_tbMember_UserId";
            this._tbMember_UserId.Size = new System.Drawing.Size(100, 20);
            this._tbMember_UserId.TabIndex = 3;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(187, 157);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(158, 41);
            this.button3.TabIndex = 7;
            this.button3.Text = "Login";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClick_MemberLogin);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(81, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 20);
            this.label3.TabIndex = 0;
            this.label3.Text = "UUID";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbMember_UUID
            // 
            this._tbMember_UUID.Location = new System.Drawing.Point(187, 50);
            this._tbMember_UUID.Name = "_tbMember_UUID";
            this._tbMember_UUID.Size = new System.Drawing.Size(100, 20);
            this._tbMember_UUID.TabIndex = 1;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(23, 157);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(158, 41);
            this.button4.TabIndex = 6;
            this.button4.Text = "Register";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnClick_MemberRegister);
            // 
            // FormLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this._tbServerPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._tbServerIp);
            this.Name = "FormLogin";
            this.Text = "FormLogin";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _tbServerIp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _tbServerPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _tbGuest_UUID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox _tbMember_UUID;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox _tbMember_UserPwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox _tbMember_UserId;
    }
}