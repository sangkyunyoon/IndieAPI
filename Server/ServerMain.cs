using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Aegis.Configuration;



namespace IndieAPI.Server
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
                    Global.Refresh();
                    Services.GameDB.Initialize();
                    Services.CacheBox.Instance.Initialize();
                    UserManagement.UserManager.Instance.Initialize();
                    Services.CloudSheet.Workbooks.Initialize();
                }
                Starter.StartNetwork();
            }
            catch (AegisException e) when (e.ResultCodeNo == AegisResult.MySqlConnectionFailed)
            {
                Logger.Write(LogType.Err, 2, "Cannot connect to MySQL Database.");
                Logger.Write(LogType.Err, 2, "Server cannot be initialized.");
            }
            catch (Exception e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());
                Logger.Write(LogType.Err, 2, "Server cannot be initialized.");
            }
        }


        public void StopServer()
        {
            Starter.Release();
            Services.CloudSheet.Workbooks.Release();
            UserManagement.UserManager.Instance.Release();
            Services.CacheBox.Instance.Release();
            Services.GameDB.Release();

            Logger.Write(LogType.Info, 2, "Server stopped.");
            Logger.Release();
        }
    }
}
