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
    public partial class FormService_Profile : Form
    {
        public FormService_Profile()
        {
            InitializeComponent();
        }


        public void OnViewEntered()
        {
            _tbTextData.Text = "";


            FormMain.SetMessageBlue("Requesting 'Profile_GetData'...");
            NetworkAPI.Profile_GetData(OnResponse_Profile);
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


            FormMain.SetMessageReady();
        }


        private void OnClick_UpdateProfile(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_SetData'...");
            NetworkAPI.Profile_SetData(
                _tbNickname.Text,
                Int16.Parse(_tbLevel.Text),
                Int16.Parse(_tbExp.Text),
                (response) =>
                {
                    if (response.ResultCodeNo == ResultCode.Ok)
                        FormMain.SetMessageReady();
                    else
                        FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
                });
        }


        private void OnClick_GetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_GetTextData'...");
            NetworkAPI.Profile_GetTextData((response) =>
            {
                _tbTextData.Text = response.TextData;
                FormMain.SetMessageReady();
            });
        }


        private void OnClick_SetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_SetTextData'...");
            NetworkAPI.Profile_SetTextData(_tbTextData.Text, (response) =>
            {
                if (response.ResultCodeNo == ResultCode.Ok)
                    FormMain.SetMessageReady();
                else
                    FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
            });
        }


        private void OnClick_CloudSheet(object sender, EventArgs e)
        {
            UIViews.ChangeView<FormService_Sheet>();
        }


        private void OnClick_Chatting(object sender, EventArgs e)
        {
            UIViews.ChangeView<FormService_Chat>();
        }


        private void OnClick_CacheBox(object sender, EventArgs e)
        {
            UIViews.ChangeView<FormService_CacheBox>();
        }
    }
}
