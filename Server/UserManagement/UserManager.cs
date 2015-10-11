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
        private RWLock _lock = new RWLock();
        private Dictionary<Int32, User> _users = new Dictionary<Int32, User>();
        private Thread _thread;
        private Int32 _ccu;





        private UserManager()
        {
        }


        public void Initialize()
        {
            _ccu = 0;
            _thread = ThreadFactory.CallPeriodically(1000, Run);
            _thread.Name = "UserManager";
        }


        public void Release()
        {
            ThreadFactory.Stop(_thread);
            _thread = null;
            _ccu = 0;
        }


        public User FindUser(Int32 userNo)
        {
            using (_lock.ReaderLock)
            {
                User user;
                if (_users.TryGetValue(userNo, out user) == true)
                    return user;
            }

            return null;
        }


        public User GetUser(Int32 userNo)
        {
            User user = FindUser(userNo);
            if (user != null)
                return user;


            using (_lock.WriterLock)
            {
                user = FindUser(userNo);
                if (user == null)
                {
                    user = new User(userNo);
                    _users.Add(userNo, user);
                }
            }

            return user;
        }


        public void RemoveUser(User user)
        {
            using (_lock.WriterLock)
            {
                _users.Remove(user.UserNo);
            }
        }


        private Boolean Run()
        {
            //  Calculate CCU
            using (_lock.ReaderLock)
            {
                _ccu = _users.Values
                             .Where(v => v.LastAliveTick.ElapsedMilliseconds / 1000 < Global.UserManager_CCUMaxTime)
                             .Count();
            }


            //  Check Expired User
            List<User> expiredUsers;
            using (_lock.ReaderLock)
            {
                expiredUsers = _users.Values
                                     .Where(v => v.LastAliveTick.ElapsedMilliseconds / 1000 >= Global.UserManager_MaxAliveTime)
                                     .ToList();
            }

            using (_lock.WriterLock)
            {
                foreach (User user in expiredUsers)
                    _users.Remove(user.UserNo);
            }


            return true;
        }
    }
}
