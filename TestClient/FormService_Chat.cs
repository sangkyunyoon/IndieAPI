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
using TestClient.WinFormHelper;



namespace TestClient
{
    public partial class FormService_Chat : Form
    {
        public FormService_Chat()
        {
            InitializeComponent();

            NetworkAPI.IMC_EnteredUser += OnEnteredUser;
            NetworkAPI.IMC_LeavedUser += OnLeavedUser;
            NetworkAPI.IMC_Message += OnMessage;
        }


        public void OnViewEntered()
        {
            FormMain.SetMessageBlue("Requesting 'IMC_ChannelList'...");
            NetworkAPI.IMC_ChannelList(OnResponse_IMC_ChannelList);
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


            FormMain.SetMessageReady();
        }


        private void OnClick_CreateChannel(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'IMC_Create'...");

            NetworkAPI.IMC_Create(_tbCreateChannelName.Text, (response) =>
            {
                if (response.ResultCodeNo != ResultCode.Ok)
                    FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
                else
                {
                    _lbRoomTitle.Text = String.Format("{0} - {1}", response.ChannelNo, response.ChannelName);

                    FormMain.SetMessageBlue( "Requesting 'IMC_UserList'...");
                    NetworkAPI.IMC_UserList(OnResponse_IMC_UserList);
                }
            });
        }


        private void OnClick_RefreshChannel(object sender, EventArgs e)
        {
            FormMain.SetMessageBlue("Requesting 'IMC_ChannelList'...");
            NetworkAPI.IMC_ChannelList(OnResponse_IMC_ChannelList);
        }


        private void OnClick_Enter(object sender, EventArgs e)
        {
            if (_lvChannel.SelectedItems.Count == 0)
                return;


            FormMain.SetMessageBlue("Requesting 'IMC_Enter'...");

            Int32 channelNo = Int32.Parse(_lvChannel.SelectedItems[0].Text);
            NetworkAPI.IMC_Enter(channelNo, (response) =>
            {
                if (response.ResultCodeNo != ResultCode.Ok)
                {
                    FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
                    return;
                }
                _lbRoomTitle.Text = String.Format("{0} - {1}", response.ChannelNo, response.ChannelName);


                FormMain.SetMessageBlue("Requesting 'IMC_UserList'...");
                NetworkAPI.IMC_UserList(OnResponse_IMC_UserList);
            });
        }


        private void OnClick_Leave(object sender, EventArgs e)
        {
            FormMain.SetMessageBlue("Requesting 'IMC_Leave'...");

            NetworkAPI.IMC_Leave((response) =>
            {
                if (response.ResultCodeNo != ResultCode.Ok)
                {
                    FormMain.SetMessage(Color.Red, ResultCode.ToString(response.ResultCodeNo));
                    return;
                }

                _lbRoomTitle.Text = "-";
                _lvUser.Items.Clear();
                _tbChatLog.Text = "";

                UIViews.ChangeView<FormService_Profile>();
            });
        }


        private void OnClick_SendMessage(object sender, EventArgs e)
        {
            var message = new Aegis.Client.Network.StreamBuffer();
            message.PutStringAsUtf16(_tbChat.Text);


            //  Send a message to all users (Broadcasting)
            if (_lvUser.SelectedItems.Count == 0)
            {
                NetworkAPI.IMC_SendMessage(0, message, (response) =>
                {
                    if (response.ResultCodeNo != ResultCode.Ok)
                        FormMain.SetMessage(Color.Red, ResultCode.ToString(response.ResultCodeNo));
                    else
                        FormMain.SetMessageReady();
                });
            }
            //  Send a message to one user (Unicasting)
            else
            {
                Int32 targetUserNo = Int32.Parse(_lvUser.SelectedItems[0].Text);
                NetworkAPI.IMC_SendMessage(targetUserNo, message, (response) =>
                {
                    if (response.ResultCodeNo != ResultCode.Ok)
                        FormMain.SetMessage(Color.Red, ResultCode.ToString(response.ResultCodeNo));
                    else
                        FormMain.SetMessageReady();
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

            FormMain.SetMessageReady();
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
