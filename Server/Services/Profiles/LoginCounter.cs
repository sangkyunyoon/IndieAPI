using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data.MySql;
using IndieAPI.Server.UserManagement;




namespace IndieAPI.Server.Services.Profiles
{
    public class LoginCounter
    {
        private User _user;
        public DateTime RegDate { get; private set; }
        public DateTime LastLoginDate { get; private set; }
        public Byte ContinuousCount { get; private set; }
        public Byte DailyCount { get; private set; }





        public LoginCounter(User user)
        {
            _user = user;
        }


        public void LoadFromDB(DataReader reader)
        {
            if (reader.Read())
            {
                RegDate = reader.GetDateTime(0);
                LastLoginDate = reader.GetDateTime(1);
                ContinuousCount = reader.GetByte(2);
                DailyCount = reader.GetByte(3);
            }
        }


        public void OnLoggedIn()
        {
            if (DateTime.Now.Date == RegDate.Date)
            {
                ++ContinuousCount;
                ++DailyCount;
            }
            else
            {
                if ((DateTime.Now.Date - LastLoginDate.Date).Days == 1)
                    ++ContinuousCount;

                if (DateTime.Now.Day != LastLoginDate.Day)
                    ++DailyCount;
            }


            LastLoginDate = DateTime.Now;

            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("update t_logincounts set lastlogindate=@_0, continuous_count=@_1, daily_count=@_2");
                cmd.CommandText.Append(" where userno=@_3;");
                cmd.BindParameter("@_0", LastLoginDate);
                cmd.BindParameter("@_1", ContinuousCount);
                cmd.BindParameter("@_2", DailyCount);
                cmd.BindParameter("@_3", _user.UserNo);
                cmd.PostQueryNoReader();
            }
        }
    }
}
