using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using IndieAPI.Server.Routine;
using IndieAPI.Server.Services;
using IndieAPI.Server.Services.Profiles;



namespace IndieAPI.Server.UserManagement
{
    public class User
    {
        public Int32 LastSeqNo { get; set; }
        public Stopwatch LastAliveTick { get; set; }
        public ClientSession Session { get; set; }

        public Int32 UserNo { get; }
        public UserProfile Profile { get; }
        public LoginCounter LoginCounter { get; }
        public TextBox TextBox { get; }
        public CastChannel CastChannel { get; set; }





        public User(Int32 userNo)
        {
            UserNo = userNo;

            Profile = new UserProfile(this);
            LoginCounter = new LoginCounter(this);
            TextBox = new TextBox(this);

            LastAliveTick = Stopwatch.StartNew();
        }


        public void SendPacket(StreamBuffer buffer, Action<StreamBuffer> onSent = null)
        {
            Session?.SendPacket(buffer, onSent);
        }


        public void LoadFromDB(Action actionOnLoaded)
        {
            using (var cmd = GameDB.NewCommand())
            {
                cmd.CommandText.Append("select nickname, level, exp");
                cmd.CommandText.Append($" from t_profiles where userno={UserNo};");

                cmd.CommandText.Append("select regdate, lastlogindate, continuous_count, daily_count");
                cmd.CommandText.Append($" from t_logincounts where userno={UserNo};");

                cmd.CommandText.Append($"select textdata from t_textbox where userno={UserNo};");
                cmd.PostQuery(() =>
                {
                    Profile.LoadFromDB(cmd.Reader);

                    cmd.Reader.NextResult();
                    LoginCounter.LoadFromDB(cmd.Reader);

                    cmd.Reader.NextResult();
                    TextBox.LoadFromDB(cmd.Reader);


                    actionOnLoaded();
                });
            }
        }
    }
}
