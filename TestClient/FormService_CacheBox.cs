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
    public partial class FormService_CacheBox : Form
    {
        public FormService_CacheBox()
        {
            InitializeComponent();
        }


        public void OnViewEntered()
        {
            _tbDuration.Text = "0";
            FormMain.SetMessageReady();
        }


        private void OnClick_Back(object sender, EventArgs e)
        {
            UIViews.ChangeView<FormService_Profile>();
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
                NetworkAPI.CacheBox_SetValue(_tbKey.Text, _tbValue.Text, -1, OnResponse_SetValue);
            }
            else
            {
                Int32 duration = Int32.Parse(_tbDuration.Text);
                if (duration == -1)
                    NetworkAPI.CacheBox_SetValue(_tbKey.Text, _tbValue.Text, _dtExpireTime.Value, OnResponse_SetValue);
                else
                    NetworkAPI.CacheBox_SetValue(_tbKey.Text, _tbValue.Text, duration, OnResponse_SetValue);
            }
        }


        private void OnResponse_SetValue(ResponseBase response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessageBlue("Ok");
            else
                FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
        }


        private void OnClick_SetExpireTime(object sender, EventArgs e)
        {
            Int32 duration;
            if (Int32.TryParse(_tbDuration.Text, out duration) == false ||
                duration == 0)
            {
                NetworkAPI.CacheBox_SetExpireTime(_tbKey.Text, _dtExpireTime.Value, OnResponse_SetExpireTime);
            }
            else
            {
                NetworkAPI.CacheBox_SetExpireTime(_tbKey.Text, duration, OnResponse_SetExpireTime);
            }
        }


        private void OnResponse_SetExpireTime(ResponseBase response)
        {
            if (response.ResultCodeNo == ResultCode.Ok)
                FormMain.SetMessageBlue("Ok");
            else
                FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
        }


        private void OnClick_GetValue(object sender, EventArgs e)
        {
            NetworkAPI.CacheBox_GetValue(_tbKey.Text,
                (response) =>
                {
                    if (response.ResultCodeNo == ResultCode.Ok)
                        FormMain.SetMessageBlue("Ok");
                    else
                    {
                        FormMain.SetMessageRed(ResultCode.ToString(response.ResultCodeNo));
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
