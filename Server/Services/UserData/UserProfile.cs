using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data.MySql;



namespace Server.Services.UserData
{
    public class UserProfile
    {
        public String Nickname { get; set; }
        public Int16 Level { get; set; }
        public Int16 Exp { get; set; }





        public void LoadFromDB(User user)
        {
            using (DBCommand cmd = SyncZoneDB.NewCommand())
            {
                cmd.CommandText.Append("select nickname, level, exp");
                cmd.CommandText.Append($" from t_profiles where userno={user.UserNo};");

                DataReader reader = cmd.Query();
                if (reader.Read())
                {
                    Nickname = reader.GetString(0);
                    Level = reader.GetInt16(1);
                    Exp = reader.GetInt16(2);
                }
            }
        }
    }
}
