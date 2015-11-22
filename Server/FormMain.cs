using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace IndieAPI.Server
{
    public partial class FormMain : Form
    {
        private Timer _timer;





        public FormMain()
        {
            InitializeComponent();

            _btnStart.Enabled = true;
            _btnStop.Enabled = false;

            this.Text = String.Format("IndieAPI Server v{0}", Aegis.Configuration.Environment.ExecutingVersion.ToString());
        }


        private void OnClick_Start(object sender, EventArgs e)
        {
            _btnStart.Enabled = false;
            _btnStop.Enabled = true;
            _tbLog.Text = "";


            ServerSystem.ServerMain.Instance.StartServer(_tbLog);

            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 1000;
            _timer.Tick += OnTimer;
            _timer.Start();
        }


        private void OnClick_Stop(object sender, EventArgs e)
        {
            _timer.Stop();
            _btnStart.Enabled = true;
            _btnStop.Enabled = false;

            ServerSystem.ServerMain.Instance.StopServer();
        }


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _btnStart.Enabled = true;
            _btnStop.Enabled = false;

            ServerSystem.ServerMain.Instance.StopServer();
        }


        private void OnTimer(object sender, EventArgs e)
        {
            _lbCachedUserCount.Text = UserManagement.UserManager.Count.ToString();
            _lbCCU.Text = UserManagement.UserManager.CCU.ToString();
            _lbCacheBoxItemCount.Text = Services.CacheBox.Count.ToString();
            _lbTaskCount.Text = Aegis.Threading.AegisTask.TaskCount.ToString();
        }
    }
}
