using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace IndieAPI.Server
{
    public partial class FormMain : Form
    {
        private Thread _thread;





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

            ServerMain.Instance.StartServer(_tbLog);
            _thread = Aegis.Threading.ThreadExtend.CallPeriodically(1000, UpdateStatistics);
            _thread.Name = "Statistics";
        }


        private void OnClick_Stop(object sender, EventArgs e)
        {
            _btnStart.Enabled = true;
            _btnStop.Enabled = false;

            ServerMain.Instance.StopServer();
        }


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _btnStart.Enabled = true;
            _btnStop.Enabled = false;

            ServerMain.Instance.StopServer();
        }


        private Boolean UpdateStatistics()
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { UpdateStatistics(); });
            else
            {
                _lbCachedUserCount.Text = UserManagement.UserManager.Count.ToString();
                _lbCCU.Text = UserManagement.UserManager.CCU.ToString();
                _lbCacheBoxItemCount.Text = Services.CacheBox.Count.ToString();
                _lbTaskCount.Text = Aegis.Threading.AegisTask.TaskCount.ToString();
            }

            return true;
        }
    }
}
