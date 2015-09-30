using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace TestClient
{
    public partial class FormMain : Form
    {
        private struct FormData
        {
            public Int32 ID;
            public Form Form;
            public FormData(Int32 id, Form form)
            {
                ID = id;
                Form = form;
            }
        }
        public const Int32 View_Login = 0;
        public const Int32 View_Service_Profile = 1;
        public const Int32 View_Service_Sheet = 2;
        public const Int32 View_Chat = 3;
        public const Int32 View_CacheBox = 4;


        public static FormMain Instance { get; private set; }
        private Form _curForm;
        private FormData[] _forms =
        {
            new FormData(View_Login, new FormLogin()),
            new FormData(View_Service_Profile, new FormService_Profile()),
            new FormData(View_Service_Sheet, new FormService_Sheet()),
            new FormData(View_Chat, new FormService_Chat()),
            new FormData(View_CacheBox, new FormService_CacheBox()),
        };





        public FormMain()
        {
            InitializeComponent();
            Instance = this;


            foreach (FormData item in _forms)
            {
                item.Form.Text = "";
                item.Form.TopLevel = false;
                item.Form.ControlBox = false;
                item.Form.FormBorderStyle = FormBorderStyle.None;
                this.Controls.Add(item.Form);
            }

            ChangeView(View_Login);
        }


        public static void SetMessage(Color foreColor, String format, params object[] args)
        {
            if (Instance._tbMesssage.InvokeRequired)
            {
                Instance.Invoke((MethodInvoker)delegate { SetMessage(foreColor, format, args); });
            }
            else
            {
                Instance._tbMesssage.ForeColor = foreColor;
                Instance._tbMesssage.Text = String.Format(format, args);
            }
        }


        public static void ChangeView(Int32 viewType)
        {
            if (Instance.InvokeRequired)
            {
                Instance.Invoke((MethodInvoker)delegate { ChangeView(viewType); });
            }
            else
            {
                if (Instance._curForm != null)
                {
                    Instance._curForm.Parent = null;
                    Instance._curForm.Hide();
                }


                Instance._curForm = Instance._forms[viewType].Form;
                Instance._curForm.Parent = Instance._panelContent;
                Instance._curForm.Show();


                System.Reflection.MethodInfo method = Instance._curForm.GetType().GetMethod("OnInitView");
                if (method != null)
                    method.Invoke(Instance._curForm, null);
            }
        }


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            IDAPI.Release();
        }


        private void OnClick_Disconnect(object sender, EventArgs e)
        {
            IDAPI.Request.Disconnect();
        }
    }
}
