using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;



namespace Server.Services.UserData
{
    public class UserManager
    {
        public static UserManager Instance { get { return Singleton<UserManager>.Instance; } }
        private Dictionary<Int32, User> _users = new Dictionary<Int32, User>();
        private RWLock _lock = new RWLock();



        private UserManager()
        {
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
    }
}
