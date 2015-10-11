using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using IndieAPI.Server.Services;
using IndieAPI.Server.Services.Profiles;



namespace IndieAPI.Server.UserManagement
{
    public class User
    {
        public Int32 LastSeqNo { get; set; }
        public Stopwatch LastAliveTick { get; set; }
        public Session.ClientSession Session { get; set; }

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


        public async Task LoadFromDB()
        {
            await AegisTask.Run(() =>
            {
                Profile.LoadFromDB();
                LoginCounter.LoadFromDB();
                TextBox.LoadFromDB();
            });
        }
    }
}
