using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;



namespace IndieAPI.Server.UserManagement
{
    public class UserManager
    {
        public static UserManager Instance { get { return Singleton<UserManager>.Instance; } }
        public static Int32 Count { get { return Instance._users.Count(); } }
        public static Int32 CCU { get { return Instance._ccu; } }
        private Dictionary<Int32, User> _users = new Dictionary<Int32, User>();
        private Thread _thread;
        private Int32 _ccu;





        private UserManager()
        {
        }


        public void Initialize()
        {
            _ccu = 0;
            _thread = ThreadExtend.CallPeriodically(1000, Run);
            _thread.Name = "UserManager";
        }


        public void Release()
        {
            ThreadExtend.Cancel(_thread);
            _thread = null;
            _ccu = 0;
        }


        public User FindUser(Int32 userNo)
        {
            User user;
            if (_users.TryGetValue(userNo, out user) == true)
                return user;

            return null;
        }


        public User GetUser(Int32 userNo)
        {
            User user = FindUser(userNo);
            if (user != null)
                return user;


            user = FindUser(userNo);
            if (user == null)
            {
                user = new User(userNo);
                _users.Add(userNo, user);
            }

            return user;
        }


        public void RemoveUser(User user)
        {
           _users.Remove(user.UserNo);
        }


        private Boolean Run()
        {
            SpinWorker.Dispatch(() =>
            {
                //  Calculate CCU
                _ccu = _users.Values
                             .Where(v => v.LastAliveTick.ElapsedMilliseconds / 1000 < Global.UserManager_CCUMaxTime)
                             .Count();


                //  Check Expired User
                List<User> expiredUsers;
                expiredUsers = _users.Values
                                     .Where(v => v.LastAliveTick.ElapsedMilliseconds / 1000 >= Global.UserManager_MaxAliveTime)
                                     .ToList();

                foreach (User user in expiredUsers)
                    _users.Remove(user.UserNo);
            });

            return true;
        }
    }
}
