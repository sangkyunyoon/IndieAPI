using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Aegis.Configuration;



namespace Server
{
    public class ServerMain
    {
        public static ServerMain Instance { get { return Singleton<ServerMain>.Instance; } }





        private ServerMain()
        {
        }


        public void StartServer(System.Windows.Forms.TextBox ctrl)
        {
            Logger.AddLogger(new LogTextBox(ctrl));


            try
            {
                Logger.Write(LogType.Info, 2, $"INDIE-API Server ({Aegis.Configuration.Environment.ExecutingVersion})");
                Logger.Write(LogType.Info, 2, $"Powered by AegisNetwork ({Aegis.Configuration.Environment.AegisVersion})");

                Starter.Initialize("./Config.xml");
                {
                    Services.GameDB.Initialize();
                    Global.Refresh();
                    Services.CloudSheet.Workbooks.Initialize();
                }
                Starter.StartNetwork();
            }
            catch (Exception e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());
            }
        }


        public void StopServer()
        {
            Services.CloudSheet.Workbooks.Release();
            Services.GameDB.Release();
            Starter.Release();
            Logger.Release();
        }


        public Int32 GetActiveSessionCount()
        {
            return NetworkChannel.FindChannel("NetworkClient")?.SessionManager.ActiveSessionCount ?? 0;
        }
    }
}
