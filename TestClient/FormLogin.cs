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
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }


        public void OnViewEntered()
        {
            _tbServerIp.Text = "192.168.0.100";
            _tbServerPort.Text = "10100";
            _tbGuest_UUID.Text = "Device_1";
            _tbMember_UUID.Text = "Device_1";
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Guest Register
        private void OnClick_GuestRegister(object sender, EventArgs e)
        {
            if (_tbGuest_UUID.Text.Length == 0)
            {
                FormMain.SetMessageRed("Input Guest UUID value.");
                _tbGuest_UUID.Focus();
                return;
            }


            NetworkAPI.SetServer(_tbServerIp.Text, _tbServerPort.Text.ToInt32());
            NetworkAPI.Auth_RegisterGuest(_tbGuest_UUID.Text, (response) =>
            {
                if (response.ResultCodeNo == ResultCode.Ok)
                    FormMain.SetMessageBlue("Guest registration completed.");
                else
                    FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
            });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Guest Login
        private void OnClick_GuestLogin(object sender, EventArgs e)
        {
            if (_tbGuest_UUID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Guest UUID value.");
                _tbGuest_UUID.Focus();
                return;
            }


            NetworkAPI.SetServer(_tbServerIp.Text, _tbServerPort.Text.ToInt32());
            NetworkAPI.Auth_LoginGuest(_tbGuest_UUID.Text, (response) =>
            {
                if (response.ResultCodeNo == ResultCode.Ok)
                    UIViews.ChangeView<FormService_Profile>();
                else
                    FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
            });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Member Register
        private void OnClick_MemberRegister(object sender, EventArgs e)
        {
            if (_tbMember_UUID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member UUID value.");
                _tbMember_UUID.Focus();
                return;
            }
            if (_tbMember_UserId.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member UserId value.");
                _tbMember_UserId.Focus();
                return;
            }
            if (_tbMember_UserPwd.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member Password value.");
                _tbMember_UserPwd.Focus();
                return;
            }


            NetworkAPI.SetServer(_tbServerIp.Text, _tbServerPort.Text.ToInt32());
            NetworkAPI.Auth_RegisterMember(
                _tbMember_UUID.Text,
                _tbMember_UserId.Text,
                _tbMember_UserPwd.Text,
                (response) =>
                {
                    if (response.ResultCodeNo == ResultCode.Ok)
                        FormMain.SetMessageRed("Member registration completed.");
                    else
                        FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
                });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Member Login
        private void OnClick_MemberLogin(object sender, EventArgs e)
        {
            if (_tbMember_UUID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member UUID value.");
                _tbMember_UUID.Focus();
                return;
            }
            if (_tbMember_UserId.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member UserId value.");
                _tbMember_UserId.Focus();
                return;
            }
            if (_tbMember_UserPwd.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member Password value.");
                _tbMember_UserPwd.Focus();
                return;
            }


            NetworkAPI.SetServer(_tbServerIp.Text, _tbServerPort.Text.ToInt32());
            NetworkAPI.Auth_LoginMember(
                _tbMember_UUID.Text,
                _tbMember_UserId.Text,
                _tbMember_UserPwd.Text,
                (response) =>
                {
                    if (response.ResultCodeNo == ResultCode.Ok)
                        UIViews.ChangeView<FormService_Profile>();
                    else
                        FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
                });
        }
    }
}
