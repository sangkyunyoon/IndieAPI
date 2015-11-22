using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TestClient.WinFormHelper;
using IndieAPI;



namespace TestClient
{
    public partial class FormMain : Form
    {
        private static FormMain _instance;





        public FormMain()
        {
            InitializeComponent();

            _instance = this;
            NetworkAPI.Initialize();
            NetworkAPI.NetworkStatusChanged += OnNetworkStatusChanged;
            this.CreateTimer(10, () =>
            {
                NetworkAPI.Update();
            }).Start();

            UIViews.Initialize(this, _panelContent);
            UIViews.ChangeView<FormLogin>();


            SetMessageReady();
        }


        public static void OnNetworkStatusChanged(Aegis.Client.NetworkStatus status)
        {
            _instance.PerformOnMainThread(() =>
            {
                if (status == Aegis.Client.NetworkStatus.Connected)
                    _instance._lbConnectionStatus.Text = "Connected";

                else if (status == Aegis.Client.NetworkStatus.ConnectionFailed)
                    _instance._lbConnectionStatus.Text = "Connection failed";

                else if (status == Aegis.Client.NetworkStatus.Disconnected)
                    _instance._lbConnectionStatus.Text = "Disconnected";

                else if (status == Aegis.Client.NetworkStatus.SessionForceClosed)
                    _instance._lbConnectionStatus.Text = "Force closed";
            });
        }


        private void OnClick_LoginPage(object sender, EventArgs e)
        {
            UIViews.ChangeView<FormLogin>();
        }


        private void OnClick_Disconnect(object sender, EventArgs e)
        {
            NetworkAPI.Disconnect();
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            NetworkAPI.Release();
        }


        #region Methods for SetMessage
        public static void SetMessage(String message, params object[] args)
        {
            _instance.PerformOnMainThread(() =>
            {
                _instance._lbMessage.Text = String.Format(message, args);
            });
        }


        public static void SetMessage(Color foreColor, String message, params object[] args)
        {
            _instance.PerformOnMainThread(() =>
            {
                _instance._lbMessage.ForeColor = foreColor;
                _instance._lbMessage.Text = String.Format(message, args);
            });
        }


        public static void SetMessageRed(String message, params object[] args)
        {
            SetMessage(Color.Red, message, args);
        }


        public static void SetMessageBlue(String message, params object[] args)
        {
            SetMessage(Color.Blue, message, args);
        }


        public static void SetMessageReady()
        {
            SetMessage(Color.Black, "Ready");
        }
        #endregion
    }
}
