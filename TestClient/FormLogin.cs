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
            _tbGuest_UUID.Text = "Device_1";
            _tbMember_UUID.Text = "Device_1";


            IDAPI.Initialize(_tbServerIp.Text, _tbServerPort.Text.ToInt32());
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Guest Register
        private void OnClick_GuestRegister(object sender, EventArgs e)
        {
            if (_tbGuest_UUID.Text.Length == 0)
            {
                FormMain.SetMessage(Color.Red, "Input Guest UUID value.");
                _tbGuest_UUID.Focus();
                return;
            }


            IDAPI.Request.Auth_RegisterGuest(_tbGuest_UUID.Text, OnResponse_Auth_RegisterGuest);
        }


        private void OnResponse_Auth_RegisterGuest(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Blue, "Guest registration completed.");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
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


            IDAPI.Request.Auth_LoginGuest(_tbGuest_UUID.Text, OnResponse_Auth_LoginGuest);
        }


        private void OnResponse_Auth_LoginGuest(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.ChangeView(FormMain.View_Service_Profile);
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
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


            IDAPI.Request.Auth_RegisterMember(
                _tbMember_UUID.Text,
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


            IDAPI.Request.Auth_LoginMember(
                _tbMember_UUID.Text,
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
