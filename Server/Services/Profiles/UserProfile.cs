using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Data.MySql;
using IndieAPI.Server.UserManagement;



namespace IndieAPI.Server.Services.Profiles
{
    public class UserProfile
    {
        private User _user;
        public String Nickname { get; set; }
        public Int16 Level { get; set; }
        public Int16 Exp { get; set; }





        public UserProfile(User user)
        {
            _user = user;
        }


        public void LoadFromDB()
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("select nickname, level, exp");
                cmd.CommandText.Append($" from t_profiles where userno={_user.UserNo};");

                DataReader reader = cmd.Query();
                if (reader.Read())
                {
                    Nickname = (reader.IsDBNull(0) ? null : reader.GetString(0));
                    Level = reader.GetInt16(1);
                    Exp = reader.GetInt16(2);
                }
            }
        }


        public void UpdateToDB()
        {
            using (DBCommand cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("update t_profiles set nickname=@_0, level=@_1, exp=@_2");
                cmd.CommandText.Append($" where userno={_user.UserNo};");
                cmd.BindParameter("@_0", Nickname);
                cmd.BindParameter("@_1", Level);
                cmd.BindParameter("@_2", Exp);
                cmd.PostQuery();
            }
        }
    }
}
