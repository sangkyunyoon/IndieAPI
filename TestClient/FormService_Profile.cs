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
    public partial class FormService_Profile : Form
    {
        public FormService_Profile()
        {
            InitializeComponent();
        }


        public void OnInitView()
        {
            _tbTextData.Text = "";


            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_GetData'...");
            FormMain.API.Profile_GetData(OnResponse_Profile);
        }


        private void OnResponse_Profile(Response_Profile response)
        {
            _tbNickname.Text = response.Nickname;
            _tbLevel.Text = response.Level.ToString();
            _tbExp.Text = response.Exp.ToString();

            _tbRegDate.Text = response.RegDate.ToString();
            _tbLastLoginDate.Text = response.LastLoginDate.ToString();
            _tbContinuousCount.Text = response.LoginContinuousCount.ToString();
            _tbDailyCount.Text = response.LoginDailyCount.ToString();


            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_UpdateProfile(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_SetData'...");
            FormMain.API.Profile_SetData(
                _tbNickname.Text,
                Int16.Parse(_tbLevel.Text),
                Int16.Parse(_tbExp.Text),
                OnResponse_SetProfileData);
        }


        private void OnResponse_SetProfileData(ResponseData response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Black, "Ready");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        private void OnClick_GetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_GetTextData'...");
            FormMain.API.Profile_GetTextData(OnResponse_GetTextData);
        }


        private void OnResponse_GetTextData(Response_Profile_Text response)
        {
            _tbTextData.Text = response.TextData;
            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_SetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_SetTextData'...");
            FormMain.API.Profile_SetTextData(_tbTextData.Text, OnResponse_SetTextData);
        }


        private void OnResponse_SetTextData(ResponseData response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Black, "Ready");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        private void OnClick_CloudSheet(object sender, EventArgs e)
        {
            FormMain.ChangeView(FormMain.View_Service_Sheet);
        }


        private void OnClick_Chatting(object sender, EventArgs e)
        {
            FormMain.ChangeView(FormMain.View_Chat);
        }
    }
}
