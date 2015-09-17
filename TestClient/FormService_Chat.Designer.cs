namespace TestClient
{
    partial class FormService_Chat
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
            this._lvChannel = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this._lbRoomTitle = new System.Windows.Forms.Label();
            this._lvUser = new System.Windows.Forms.ListView();
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this._tbChatLog = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this._tbChat = new System.Windows.Forms.TextBox();
            this._tbCreateChannelName = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // _lvChannel
            // 
            this._lvChannel.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this._lvChannel.FullRowSelect = true;
            this._lvChannel.GridLines = true;
            this._lvChannel.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvChannel.HideSelection = false;
            this._lvChannel.Location = new System.Drawing.Point(12, 142);
            this._lvChannel.MultiSelect = false;
            this._lvChannel.Name = "_lvChannel";
            this._lvChannel.Size = new System.Drawing.Size(185, 300);
            this._lvChannel.TabIndex = 1;
            this._lvChannel.TabStop = false;
            this._lvChannel.UseCompatibleStateImageBehavior = false;
            this._lvChannel.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "No";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Name";
            this.columnHeader2.Width = 118;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(12, 116);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 23);
            this.label1.TabIndex = 0;
            this.label1.Text = "Channels";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 448);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(185, 39);
            this.button1.TabIndex = 4;
            this.button1.TabStop = false;
            this.button1.Text = "Enter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnClick_Enter);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(203, 448);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 39);
            this.button2.TabIndex = 5;
            this.button2.TabStop = false;
            this.button2.Text = "Leave";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.OnClick_Leave);
            // 
            // _lbRoomTitle
            // 
            this._lbRoomTitle.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._lbRoomTitle.Location = new System.Drawing.Point(203, 12);
            this._lbRoomTitle.Name = "_lbRoomTitle";
            this._lbRoomTitle.Size = new System.Drawing.Size(165, 23);
            this._lbRoomTitle.TabIndex = 2;
            this._lbRoomTitle.Text = "-";
            this._lbRoomTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _lvUser
            // 
            this._lvUser.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this._lvUser.FullRowSelect = true;
            this._lvUser.GridLines = true;
            this._lvUser.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this._lvUser.HideSelection = false;
            this._lvUser.Location = new System.Drawing.Point(203, 38);
            this._lvUser.MultiSelect = false;
            this._lvUser.Name = "_lvUser";
            this._lvUser.Size = new System.Drawing.Size(165, 404);
            this._lvUser.TabIndex = 3;
            this._lvUser.TabStop = false;
            this._lvUser.UseCompatibleStateImageBehavior = false;
            this._lvUser.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "UserNo";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Nickname";
            this.columnHeader4.Width = 96;
            // 
            // _tbChatLog
            // 
            this._tbChatLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this._tbChatLog.Location = new System.Drawing.Point(374, 12);
            this._tbChatLog.MaxLength = 1048576;
            this._tbChatLog.Multiline = true;
            this._tbChatLog.Name = "_tbChatLog";
            this._tbChatLog.ReadOnly = true;
            this._tbChatLog.Size = new System.Drawing.Size(398, 430);
            this._tbChatLog.TabIndex = 6;
            this._tbChatLog.TabStop = false;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(607, 448);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(165, 39);
            this.button3.TabIndex = 8;
            this.button3.Text = "Send";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.OnClick_SendMessage);
            // 
            // _tbChat
            // 
            this._tbChat.Location = new System.Drawing.Point(374, 458);
            this._tbChat.Name = "_tbChat";
            this._tbChat.Size = new System.Drawing.Size(227, 20);
            this._tbChat.TabIndex = 7;
            // 
            // _tbCreateChannelName
            // 
            this._tbCreateChannelName.Location = new System.Drawing.Point(12, 12);
            this._tbCreateChannelName.Name = "_tbCreateChannelName";
            this._tbCreateChannelName.Size = new System.Drawing.Size(185, 20);
            this._tbCreateChannelName.TabIndex = 9;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 38);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(185, 31);
            this.button4.TabIndex = 10;
            this.button4.TabStop = false;
            this.button4.Text = "Create";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.OnClick_CreateChannel);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 75);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(185, 31);
            this.button5.TabIndex = 11;
            this.button5.TabStop = false;
            this.button5.Text = "Refresh Channel";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.OnClick_RefreshChannel);
            // 
            // FormService_Chat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this._tbCreateChannelName);
            this.Controls.Add(this._tbChat);
            this.Controls.Add(this.button3);
            this.Controls.Add(this._tbChatLog);
            this.Controls.Add(this._lbRoomTitle);
            this.Controls.Add(this._lvUser);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._lvChannel);
            this.Name = "FormService_Chat";
            this.Text = "FormService_Chat";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView _lvChannel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label _lbRoomTitle;
        private System.Windows.Forms.ListView _lvUser;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TextBox _tbChatLog;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox _tbChat;
        private System.Windows.Forms.TextBox _tbCreateChannelName;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}