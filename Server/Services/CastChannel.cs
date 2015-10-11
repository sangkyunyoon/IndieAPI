using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Aegis;
using Aegis.Threading;
using Aegis.Network;
using IndieAPI.Server.UserManagement;



namespace IndieAPI.Server.Services
{
    public partial class CastChannel
    {
        private readonly RWLock _lock = new RWLock();
        public ReaderLock ReaderLock { get { return _lock.ReaderLock; } }
        public WriterLock WriterLock { get { return _lock.WriterLock; } }


        public static List<CastChannel> Channels { get; } = new List<CastChannel>();
        public List<User> Users { get; } = new List<User>();

        public Int32 ChannelNo { get; }
        public String Name { get; }





        private CastChannel(String name)
        {
            Name = name;


            lock (Channels)
            {
                ChannelNo = 1;
                foreach (CastChannel channel in Channels.OrderBy(v => v.ChannelNo).ToList())
                {
                    if (channel.ChannelNo != ChannelNo)
                        break;

                    ++ChannelNo;
                }
            }
        }


        public static CastChannel GetChannel(Int32 channelNo)
        {
            lock (Channels)
            {
                CastChannel channel = Channels.Find(v => v.ChannelNo == channelNo);
                if (channel == null)
                    throw new AegisException(ResultCode.IMC_InvalidChannelNo);

                return channel;
            }
        }


        public static CastChannel NewChannel(String name)
        {
            lock (Channels)
            {
                if (Channels.Where(v => v.Name == name).Count() > 0)
                    throw new AegisException(ResultCode.IMC_ExistsChannelName);

                CastChannel channel = new CastChannel(name);
                Channels.Add(channel);

                return channel;
            }
        }


        private static void RemoveChannel(CastChannel channel)
        {
            lock (Channels)
            {
                if (channel.Users.Count() > 0)
                    return;

                Channels.Remove(channel);
            }
        }


        public void Enter(User user)
        {
            using (WriterLock)
            {
                lock (Channels)
                {
                    if (Channels.Contains(this) == false)
                        throw new AegisException(ResultCode.IMC_InvalidChannelNo);
                }

                if (Users.Contains(user) == true)
                    throw new AegisException(ResultCode.IMC_ExistsUser);

                Users.Add(user);
            }
        }


        public void Leave(User user)
        {
            using (WriterLock)
            {
                Users.Remove(user);
                RemoveChannel(this);
            }
        }


        public User GetUser(Int32 userNo)
        {
            using (ReaderLock)
            {
                User user = Users.Find(v => v.UserNo == userNo);
                if (user == null)
                    throw new AegisException(ResultCode.IMC_NotExistsUser);

                return user;
            }
        }


        public void Broadcast(StreamBuffer packet, User except = null)
        {
            using (ReaderLock)
            {
                foreach (User user in Users)
                {
                    if (user != except)
                        user.SendPacket(packet.Clone());
                }
            }
        }
    }
}
