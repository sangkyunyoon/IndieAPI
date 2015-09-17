using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IndieAPI;
using Aegis.Client;



namespace TestClient
{
    public partial class FormService_Chat : Form
    {
        public FormService_Chat()
        {
            InitializeComponent();

            FormMain.API.IMC_EnteredUser += OnEnteredUser;
            FormMain.API.IMC_LeavedUser += OnLeavedUser;
            FormMain.API.IMC_Message += OnMessage;
        }


        public void OnInitView()
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'IMC_ChannelList'...");
            FormMain.API.IMC_ChannelList(OnResponse_IMC_ChannelList);
        }


        private void OnResponse_IMC_ChannelList(Response_IMC_ChannelList response)
        {
            _lvChannel.Items.Clear();
            foreach (var channel in response.Channels)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = channel.ChannelNo.ToString();
                lvi.SubItems.Add(channel.ChannelName);

                _lvChannel.Items.Add(lvi);
            }


            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_CreateChannel(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'IMC_Create'...");

            FormMain.API.IMC_Create(_tbCreateChannelName.Text, (response) =>
            {
                if (response.ResultCodeNo != ResultCode.Ok)
                    FormMain.SetMessage(Color.Red, response.ResultString);
                else
                {
                    _lbRoomTitle.Text = String.Format("{0} - {1}", response.ChannelNo, response.ChannelName);

                    FormMain.SetMessage(Color.Blue, "Requesting 'IMC_UserList'...");
                    FormMain.API.IMC_UserList(OnResponse_IMC_UserList);
                }
            });
        }


        private void OnClick_RefreshChannel(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'IMC_ChannelList'...");
            FormMain.API.IMC_ChannelList(OnResponse_IMC_ChannelList);
        }


        private void OnClick_Enter(object sender, EventArgs e)
        {
            if (_lvChannel.SelectedItems.Count == 0)
                return;


            FormMain.SetMessage(Color.Blue, "Requesting 'IMC_Enter'...");

            Int32 channelNo = Int32.Parse(_lvChannel.SelectedItems[0].Text);
            FormMain.API.IMC_Enter(channelNo, (response) =>
            {
                if (response.ResultCodeNo != ResultCode.Ok)
                {
                    FormMain.SetMessage(Color.Red, response.ResultString);
                    return;
                }
                _lbRoomTitle.Text = String.Format("{0} - {1}", response.ChannelNo, response.ChannelName);


                FormMain.SetMessage(Color.Blue, "Requesting 'IMC_UserList'...");
                FormMain.API.IMC_UserList(OnResponse_IMC_UserList);
            });
        }


        private void OnClick_Leave(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'IMC_Leave'...");

            FormMain.API.IMC_Leave((response) =>
            {
                if (response.ResultCodeNo != ResultCode.Ok)
                {
                    FormMain.SetMessage(Color.Red, response.ResultString);
                    return;
                }

                _lbRoomTitle.Text = "-";
                _lvUser.Items.Clear();
                _tbChatLog.Text = "";

                FormMain.ChangeView(FormMain.View_Service_Profile);
            });
        }


        private void OnClick_SendMessage(object sender, EventArgs e)
        {
            StreamBuffer message = new StreamBuffer();
            message.PutStringAsUtf16(_tbChat.Text);


            if (_lvUser.SelectedItems.Count == 0)
            {
                FormMain.API.IMC_SendToAny(message, (response) =>
                {
                    if (response.ResultCodeNo != ResultCode.Ok)
                        FormMain.SetMessage(Color.Red, response.ResultString);
                    else
                        FormMain.SetMessage(Color.Black, "Ready");
                });
            }
            else
            {
                String targetNickname = _lvUser.SelectedItems[0].SubItems[1].Text;
                FormMain.API.IMC_SendToOne(targetNickname, message, (response) =>
                {
                    if (response.ResultCodeNo != ResultCode.Ok)
                        FormMain.SetMessage(Color.Red, response.ResultString);
                    else
                        FormMain.SetMessage(Color.Black, "Ready");
                });
            }

            _tbChat.Text = "";
            _tbChat.Focus();
        }


        private void OnResponse_IMC_UserList(Response_IMC_UserList response)
        {
            _lvUser.Items.Clear();

            foreach (var user in response.Users)
            {
                ListViewItem lvi = new ListViewItem();
                lvi.Text = user.UserNo.ToString();
                lvi.SubItems.Add(user.Nickname);

                _lvUser.Items.Add(lvi);
            }

            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnEnteredUser(Response_IMC_EnteredUser response)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Text = response.UserNo.ToString();
            lvi.SubItems.Add(response.Nickname);
            _lvUser.Items.Add(lvi);


            String msg = String.Format("{0} entered.\r\n", response.Nickname);
            _tbChatLog.Text += msg;
            _tbChatLog.SelectionStart = _tbChatLog.TextLength;
            _tbChatLog.ScrollToCaret();
        }


        private void OnLeavedUser(Response_IMC_LeavedUser response)
        {
            String nickname = "";
            foreach (ListViewItem lvi in _lvUser.Items)
            {
                if (lvi.Text == response.UserNo.ToString())
                {
                    nickname = lvi.SubItems[1].Text;
                    _lvUser.Items.Remove(lvi);
                    break;
                }
            }


            String msg = String.Format("{0} leaved.\r\n", nickname);
            _tbChatLog.Text += msg;
            _tbChatLog.SelectionStart = _tbChatLog.TextLength;
            _tbChatLog.ScrollToCaret();



            foreach (ListViewItem lvi in _lvUser.Items)
            {
                if (lvi.Text == response.UserNo.ToString())
                {
                    _lvUser.Items.Remove(lvi);
                    break;
                }
            }
        }


        private void OnMessage(Response_IMC_Message response)
        {
            String nickname = "";
            foreach (ListViewItem lvi in _lvUser.Items)
            {
                if (lvi.Text == response.SenderUserNo.ToString())
                {
                    nickname = lvi.SubItems[1].Text;
                    break;
                }
            }


            String chatMessage = response.Message.GetStringFromUtf16();
            String msg = String.Format("{0} > {1}\r\n", nickname, chatMessage);
            _tbChatLog.Text += msg;
            _tbChatLog.SelectionStart = _tbChatLog.TextLength;
            _tbChatLog.ScrollToCaret();
        }
    }
}
