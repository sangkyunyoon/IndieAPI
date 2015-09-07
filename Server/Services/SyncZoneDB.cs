using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Configuration;
using Aegis.Data.MySql;
using Aegis.Converter;



namespace Server.Services
{
    public static class SyncZoneDB
    {
        private static MySqlDatabase _mysql;





        public static void Initialize()
        {
            _mysql = new MySqlDatabase(
                Starter.CustomData.GetValue("SyncZoneDB/ipaddress"),
                Starter.CustomData.GetValue("SyncZoneDB/port").ToInt32(),
                "",
                Starter.CustomData.GetValue("SyncZoneDB/dbname"),
                Starter.CustomData.GetValue("SyncZoneDB/userid"),
                Starter.CustomData.GetValue("SyncZoneDB/userpwd"));
            _mysql.SetWorketQueue(4);
        }


        public static void Release()
        {
            _mysql.Release();
            _mysql = null;
        }


        public static DBCommand NewCommand()
        {
            return DBCommand.NewCommand(_mysql);
        }
    }
}
