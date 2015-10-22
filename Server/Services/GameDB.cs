using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Aegis;
using Aegis.Data.MySql;
using Aegis.Converter;



namespace IndieAPI.Server.Services
{
    public static class GameDB
    {
        private static MySqlDatabase _mysql;
        //private static Process _processMySqld;





        public static void Initialize()
        {
            /*
            _processMySqld = new Process();
            _processMySqld.StartInfo = new ProcessStartInfo()
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                WorkingDirectory = Starter.CustomData.GetValue("GameDB/path"),
                FileName = "cmd.exe",
                Arguments = String.Format("/c {0}\\mysql_start.cmd", Starter.CustomData.GetValue("GameDB/path")),
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            _processMySqld.Start();
            */


            _mysql = new MySqlDatabase(
                Starter.CustomData.GetValue("GameDB/ipAddress"),
                Starter.CustomData.GetValue("GameDB/port").ToInt32(),
                "",
                Starter.CustomData.GetValue("GameDB/dbName"),
                Starter.CustomData.GetValue("GameDB/userId"),
                Starter.CustomData.GetValue("GameDB/userPwd"));
        }


        public static void Release()
        {
            if (_mysql != null)
            {
                _mysql.Release();
                _mysql = null;
            }
        }


        public static DBCommand NewCommand()
        {
            return new DBCommand(_mysql);
        }
    }
}
