using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using IndieAPI;



namespace TestClient
{
    public static class IDAPI
    {
        public static Request Request { get; } = new Request();



        public static void Initialize(String ipAddress, Int32 portNo)
        {
            Request.Initialize(ipAddress, portNo,
                               "AEGIS For Indie!", "AEGIS Indie APIs",
                               OnNetworkStatusChanged);

            Timer timer = new Timer();
            timer.Tick += delegate (object sender, EventArgs e) { Request.Update(); };
            timer.Interval = 10;
            timer.Start();
        }


        public static void Release()
        {
            Request.Release();
        }


        private static void OnNetworkStatusChanged(NetworkStatus status)
        {
            switch (status)
            {
                case NetworkStatus.Connected:
                    FormMain.SetMessage(Color.Black, "Connected to server.");
                    break;

                case NetworkStatus.ConnectionFailed:
                    FormMain.SetMessage(Color.Red, "Connection failed.");
                    break;

                case NetworkStatus.Disconnected:
                    FormMain.SetMessage(Color.Black, "Disconnected from server.");
                    break;

                case NetworkStatus.SessionForceClosed:
                    FormMain.SetMessage(Color.Red, "This session closed by force.");
                    break;
            }
        }
    }
}
