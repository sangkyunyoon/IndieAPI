using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;



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
                Logger.Write(LogType.Info, 2, $"INDIE-API Server Build {Version.BuildNo}");

                Aegis.Configuration.Starter.Initialize(System.Reflection.Assembly.GetExecutingAssembly(), "./Config.xml");
                {
                    Services.SyncZoneDB.Initialize();
                    Global.Refresh();
                }
                Aegis.Configuration.Starter.StartNetwork();
            }
            catch (Exception e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());
            }
        }


        public void StopServer()
        {
            Services.SyncZoneDB.Release();
            Aegis.Configuration.Starter.Release();
            Logger.Release();
        }


        public Int32 GetActiveSessionCount()
        {
            lock (NetworkChannel.Channels)
            {
                NetworkChannel channel = NetworkChannel.Channels.Find(v => v.Name == "NetworkClient");
                if (channel == null)
                    return 0;

                return channel.SessionManager.ActiveSessionCount;
            }
        }
    }
}
