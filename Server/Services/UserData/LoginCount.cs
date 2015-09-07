using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data.MySql;





namespace Server.Services.UserData
{
    public class LoginCount
    {
        public DateTime FirstLoginTime { get; set; }
        public DateTime LastLoginTime { get; set; }
        public SByte ContinuousCount { get; set; }





        public void LoadFromDB(User user)
        {
            using (DBCommand cmd = SyncZoneDB.NewCommand())
            {
                cmd.CommandText.Append("select firstlogintime, lastlogintime, continuous_count");
                cmd.CommandText.Append($" from t_logincounts where userno={user.UserNo};");

                DataReader reader = cmd.Query();
                if (reader.Read())
                {
                    FirstLoginTime = reader.GetDateTime(0);
                    LastLoginTime = reader.GetDateTime(1);
                    ContinuousCount = reader.GetSByte(2);
                }
            }
        }
    }
}
