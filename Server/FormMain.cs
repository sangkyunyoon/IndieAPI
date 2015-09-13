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
            (new Thread(Run)).Start();
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


        private async void Run()
        {
            while (_btnStart.Enabled == false)
            {
                await Task.Run(() =>
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
                }, _cts.Token);


                await Task.Delay(100);
            }
        }


        private void UpdateStatistics()
        {
            Int32 sessionCount = ServerMain.Instance.GetActiveSessionCount();


            _lbActiveSession.Text = String.Format("{0:N0}", sessionCount);
            _lbTaskCount.Text = Aegis.Threading.AegisTask.TaskCount.ToString();
        }
    }
}
