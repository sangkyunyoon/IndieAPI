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
    public partial class FormService_CacheBox : Form
    {
        public FormService_CacheBox()
        {
            InitializeComponent();
        }


        public void OnInitView()
        {
            _tbDuration.Text = "0";
            FormMain.SetMessage(Color.Black, "Ready");
        }


        private void OnClick_Back(object sender, EventArgs e)
        {
            FormMain.ChangeView(FormMain.View_Service_Profile);
        }


        private void OnChange_Immortal(object sender, EventArgs e)
        {
            _tbDuration.Enabled = !_cbImmortal.Checked;
            _dtExpireTime.Enabled = !_cbImmortal.Checked;
        }


        private void OnSelect_ExpireTime(object sender, EventArgs e)
        {
            _tbDuration.Text = "-1";
        }


        private void OnClick_SetValue(object sender, EventArgs e)
        {
            if (_cbImmortal.Checked == true)
            {
                IDAPI.Request.CacheBox_SetValue(_tbKey.Text, _tbValue.Text, -1, OnResponse_SetValue);
            }
            else
            {
                Int32 duration = Int32.Parse(_tbDuration.Text);
                if (duration == -1)
                    IDAPI.Request.CacheBox_SetValue(_tbKey.Text, _tbValue.Text, _dtExpireTime.Value, OnResponse_SetValue);
                else
                    IDAPI.Request.CacheBox_SetValue(_tbKey.Text, _tbValue.Text, duration, OnResponse_SetValue);
            }
        }


        private void OnResponse_SetValue(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Blue, "Ok");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        private void OnClick_SetExpireTime(object sender, EventArgs e)
        {
            Int32 duration;
            if (Int32.TryParse(_tbDuration.Text, out duration) == false ||
                duration == 0)
            {
                IDAPI.Request.CacheBox_SetExpireTime(_tbKey.Text, _dtExpireTime.Value, OnResponse_SetExpireTime);
            }
            else
            {
                IDAPI.Request.CacheBox_SetExpireTime(_tbKey.Text, duration, OnResponse_SetExpireTime);
            }
        }


        private void OnResponse_SetExpireTime(Response response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessage(Color.Blue, "Ok");
            else
                FormMain.SetMessage(Color.Red, response.ResultString);
        }


        private void OnClick_GetValue(object sender, EventArgs e)
        {
            IDAPI.Request.CacheBox_GetValue(_tbKey.Text,
                (response) =>
                {
                    if (response.ResultCodeNo == ResultCode.Ok)
                        FormMain.SetMessage(Color.Blue, "Ok");
                    else
                    {
                        FormMain.SetMessage(Color.Red, response.ResultString);
                        return;
                    }


                    if (response.DurationMinutes == -1)
                    {
                        _cbImmortal.Checked = true;
                        _tbDuration.Text = "-1";
                        _tbValue.Text = response.Value;
                    }
                    else
                    {
                        _cbImmortal.Checked = false;
                        _tbValue.Text = response.Value;
                        _dtExpireTime.Value = DateTime.Now.AddMinutes(response.DurationMinutes);
                        _tbDuration.Text = response.DurationMinutes.ToString();
                    }
                });
        }
    }
}
