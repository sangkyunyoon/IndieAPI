using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis.Client;
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


            FormMain.SetMessage(Color.Blue, "Requesting 'Storage_GetTextData'...");
            FormMain.API.Profile_GetData(OnRecv_Profile);
        }


        private void OnRecv_Profile(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            _tbNickname.Text = resPacket.GetStringFromUtf16();
            _tbLevel.Text = resPacket.GetInt16().ToString();
            _tbExp.Text = resPacket.GetInt16().ToString();

            _tbRegDate.Text = DateTime.FromOADate(resPacket.GetDouble()).ToString();
            _tbLastLoginDate.Text = DateTime.FromOADate(resPacket.GetDouble()).ToString();
            _tbContinuousCount.Text = resPacket.GetByte().ToString();
            _tbDailyCount.Text = resPacket.GetByte().ToString();


            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_UpdateProfile(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_SetData'...");
            FormMain.API.Profile_SetData(
                _tbNickname.Text,
                Int16.Parse(_tbLevel.Text),
                Int16.Parse(_tbExp.Text),
                OnRecv_SetProfileData);
        }


        private void OnRecv_SetProfileData(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            if (result != ResultCode.Ok)
                FormMain.SetMessage(Color.Red, ResultCode.ToString(result));
            else
                FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_GetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_GetTextData'...");
            FormMain.API.Profile_GetTextData(OnRecv_GetTextData);
        }


        private void OnRecv_GetTextData(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            String textData = resPacket.GetStringFromUtf16();

            _tbTextData.Text = textData;

            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_SetTextData(object sender, EventArgs e)
        {
            FormMain.SetMessage(Color.Blue, "Requesting 'Profile_Text_SetData'...");
            FormMain.API.Profile_SetTextData(_tbTextData.Text, OnRecv_SetTextData);
        }


        private void OnRecv_SetTextData(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            if (result != ResultCode.Ok)
                FormMain.SetMessage(Color.Red, ResultCode.ToString(result));

            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_CloudSheet(object sender, EventArgs e)
        {
            FormMain.ChangeView(FormMain.View_Service_Sheet);
        }
    }
}
