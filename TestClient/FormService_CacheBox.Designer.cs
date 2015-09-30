namespace TestClient
{
    partial class FormService_CacheBox
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
            this._tbKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this._tbValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this._tbDuration = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this._dtExpireTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this._cbImmortal = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(661, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(111, 47);
            this.button1.TabIndex = 13;
            this.button1.Text = "Back";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_Back);
            // 
            // _tbKey
            // 
            this._tbKey.Location = new System.Drawing.Point(213, 146);
            this._tbKey.Name = "_tbKey";
            this._tbKey.Size = new System.Drawing.Size(306, 20);
            this._tbKey.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(159, 150);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(542, 177);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(117, 56);
            this.button3.TabIndex = 10;
            this.button3.Text = "Get Value";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClick_GetValue);
            // 
            // _tbValue
            // 
            this._tbValue.Location = new System.Drawing.Point(159, 202);
            this._tbValue.Multiline = true;
            this._tbValue.Name = "_tbValue";
            this._tbValue.Size = new System.Drawing.Size(360, 104);
            this._tbValue.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(159, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(360, 23);
            this.label2.TabIndex = 2;
            this.label2.Text = "Value";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _tbDuration
            // 
            this._tbDuration.Location = new System.Drawing.Point(238, 362);
            this._tbDuration.Name = "_tbDuration";
            this._tbDuration.Size = new System.Drawing.Size(127, 20);
            this._tbDuration.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(171, 365);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Duration";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(371, 366);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(28, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "min.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _dtExpireTime
            // 
            this._dtExpireTime.Location = new System.Drawing.Point(238, 388);
            this._dtExpireTime.Name = "_dtExpireTime";
            this._dtExpireTime.Size = new System.Drawing.Size(161, 20);
            this._dtExpireTime.TabIndex = 9;
            this._dtExpireTime.ValueChanged += new System.EventHandler(this.OnSelect_ExpireTime);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(171, 391);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Expire Time";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(542, 250);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(117, 56);
            this.button2.TabIndex = 11;
            this.button2.Text = "Set Value";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClick_SetValue);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(542, 323);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(117, 56);
            this.button4.TabIndex = 12;
            this.button4.Text = "Set Expire Time";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnClick_SetExpireTime);
            // 
            // _cbImmortal
            // 
            this._cbImmortal.AutoSize = true;
            this._cbImmortal.Location = new System.Drawing.Point(174, 339);
            this._cbImmortal.Name = "_cbImmortal";
            this._cbImmortal.Size = new System.Drawing.Size(65, 17);
            this._cbImmortal.TabIndex = 4;
            this._cbImmortal.Text = "Immortal";
            this._cbImmortal.UseVisualStyleBackColor = true;
            this._cbImmortal.CheckedChanged += new System.EventHandler(this.OnChange_Immortal);
            // 
            // FormService_CacheBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this._cbImmortal);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this._dtExpireTime);
            this.Controls.Add(this.label4);
            this.Controls.Add(this._tbDuration);
            this.Controls.Add(this.label3);
            this.Controls.Add(this._tbValue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this._tbKey);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Name = "FormService_CacheBox";
            this.Text = "FormService_CacheBox";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox _tbKey;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox _tbValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox _tbDuration;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker _dtExpireTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.CheckBox _cbImmortal;
    }
}