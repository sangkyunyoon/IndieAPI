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



namespace TestClient
{
    public partial class FormLogin : Form
    {
        public FormLogin()
        {
            InitializeComponent();
        }


        public void OnInitView()
        {
            _tbServerIp.Text = "192.168.0.100";
            _tbServerPort.Text = "10100";
            _tbGuest_UDID.Text = "Device_1";
            _tbMember_UDID.Text = "Device_1";


            FormMain.API.Initialize(
                _tbServerIp.Text, _tbServerPort.Text.ToInt32(),
                "AEGIS For Indie!", "AEGIS Indie APIs",
                OnNetworkStatusChanged);
        }


        private void OnNetworkStatusChanged(NetworkStatus status)
        {
            switch (status)
            {
                case NetworkStatus.Connected:
                    FormMain.SetMessage(Color.Black, "Connected to server.");
                    break;

                case NetworkStatus.ConnectionFailed:
                    FormMain.SetMessage(Color.Red, "Connection failed.");
                    break;

                case NetworkStatus.Disconnected:
                    FormMain.SetMessage(Color.Black, "Disconnected from server.");
                    break;

                case NetworkStatus.SessionForceClosed:
                    FormMain.SetMessage(Color.Red, "This session closed by force.");
                    break;
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Guest Register
        private void OnClick_GuestRegister(object sender, EventArgs e)
        {
            if (_tbGuest_UDID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Guest UDID value.");
                _tbGuest_UDID.Focus();
                return;
            }


            FormMain.API.Auth_RegisterGuest(_tbGuest_UDID.Text, OnResponse_Auth_RegisterGuest);
        }


        private void OnResponse_Auth_RegisterGuest(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Blue, "Guest registration completed.");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Guest Login
        private void OnClick_GuestLogin(object sender, EventArgs e)
        {
            if (_tbGuest_UDID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Guest UDID value.");
                _tbGuest_UDID.Focus();
                return;
            }


            FormMain.API.Auth_LoginGuest(_tbGuest_UDID.Text, OnResponse_Auth_LoginGuest);
        }


        private void OnResponse_Auth_LoginGuest(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.ChangeView(FormMain.View_Service_Profile);
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Member Register
        private void OnClick_MemberRegister(object sender, EventArgs e)
        {
            if (_tbMember_UDID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member UDID value.");
                _tbMember_UDID.Focus();
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


            FormMain.API.Auth_RegisterMember(
                _tbMember_UDID.Text,
                _tbMember_UserId.Text,
                _tbMember_UserPwd.Text,
                OnResponse_Auth_RegisterMember);
        }


        private void OnResponse_Auth_RegisterMember(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Blue, "Member registration completed.");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////
        //  Member Login
        private void OnClick_MemberLogin(object sender, EventArgs e)
        {
            if (_tbMember_UDID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Member UDID value.");
                _tbMember_UDID.Focus();
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


            FormMain.API.Auth_LoginMember(
                _tbMember_UDID.Text,
                _tbMember_UserId.Text,
                _tbMember_UserPwd.Text,
                OnResponse_Auth_LoginMember);
        }


        private void OnResponse_Auth_LoginMember(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.ChangeView(FormMain.View_Service_Profile);
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }
    }
}
