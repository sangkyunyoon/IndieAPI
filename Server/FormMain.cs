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



namespace Server
{
    public partial class FormMain : Form
    {
        private CancellationTokenSource _cts;





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

            _cts = new CancellationTokenSource();
            Aegis.Threading.AegisTask.RunPeriodically(1000, _cts.Token, Run).Name = "Statistics";
        }


        private void OnClick_Stop(object sender, EventArgs e)
        {
            _btnStart.Enabled = true;
            _btnStop.Enabled = false;


            if (_cts != null)
                _cts.Cancel();
            ServerMain.Instance.StopServer();
        }


        private void OnFormClosed(object sender, FormClosedEventArgs e)
        {
            _btnStart.Enabled = true;
            _btnStop.Enabled = false;


            if (_cts != null)
                _cts.Cancel();
            ServerMain.Instance.StopServer();
        }


        private Boolean Run()
        {
            try
            {
                if (InvokeRequired)
                    Invoke((MethodInvoker)delegate { UpdateStatistics(); });
                else
                    UpdateStatistics();
            }
            catch (Exception)
            {
            }

            return true;
        }


        private void UpdateStatistics()
        {
            _lbCachedUserCount.Text = Services.UserData.UserManager.Count.ToString();
            _lbCCU.Text = Services.UserData.UserManager.CCU.ToString();
            _lbCacheBoxItemCount.Text = Services.CacheBox.Count.ToString();
            _lbTaskCount.Text = Aegis.Threading.AegisTask.TaskCount.ToString();
        }
    }
}
