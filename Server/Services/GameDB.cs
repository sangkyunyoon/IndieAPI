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
    public static class GameDB
    {
        private static MySqlDatabase _mysql;





        public static void Initialize()
        {
            _mysql = new MySqlDatabase(
                Starter.CustomData.GetValue("GameDB/ipaddress"),
                Starter.CustomData.GetValue("GameDB/port").ToInt32(),
                "",
                Starter.CustomData.GetValue("GameDB/dbname"),
                Starter.CustomData.GetValue("GameDB/userid"),
                Starter.CustomData.GetValue("GameDB/userpwd"));
            _mysql.SetWorketQueue(4);
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
            return DBCommand.NewCommand(_mysql);
        }
    }
}
