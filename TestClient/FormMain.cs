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
        public static Int32 View_Login = 0;
        public static Int32 View_Service_Profile = 1;
        public static Int32 View_Service_Sheet = 2;
        public static Int32 View_Chat = 3;
        public static Int32 View_Count = 4;


        public static FormMain Instance { get; private set; }
        private Form _curForm;
        private Form[] _forms;





        public FormMain()
        {
            InitializeComponent();
            Instance = this;


            //  Forms setting
            _forms = new Form[View_Count];
            _forms[View_Login] = new FormLogin();
            _forms[View_Service_Profile] = new FormService_Profile();
            _forms[View_Service_Sheet] = new FormService_Sheet();
            _forms[View_Chat] = new FormService_Chat();


            foreach (Form item in _forms)
            {
                if (item == null)
                    continue;

                item.Text = "";
                item.TopLevel = false;
                item.ControlBox = false;
                item.FormBorderStyle = FormBorderStyle.None;
                this.Controls.Add(item);
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


                Instance._curForm = Instance._forms[viewType];
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
