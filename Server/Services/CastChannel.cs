using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Aegis;
using Aegis.Threading;
using Aegis.Network;
using Server.Services.UserData;



namespace Server.Services
{
    public partial class CastChannel
    {
        public static readonly CastChannel Root = new CastChannel(null, "root", true);


        private readonly RWLock _lock = new RWLock();
        public ReaderLock ReaderLock { get { return _lock.ReaderLock; } }
        public WriterLock WriterLock { get { return _lock.WriterLock; } }


        public readonly CastChannel Parent;
        public readonly List<CastChannel> ChildChannels = new List<CastChannel>();
        public readonly List<User> Users = new List<User>();

        public readonly Int32 ChannelNo;
        public readonly String Name;
        public readonly Boolean IsStatic;

        private Boolean _isRemoved = false;





        private CastChannel(CastChannel parent, String name, Boolean isStatic)
        {
            Parent = parent;
            Name = name;
            IsStatic = isStatic;


            using (ReaderLock)
            {
                ChannelNo = 1;
                foreach (CastChannel channel in ChildChannels.OrderBy(v => v.ChannelNo).ToList())
                {
                    if (channel.ChannelNo != ChannelNo)
                        break;

                    ++ChannelNo;
                }
            }
        }


        public CastChannel GetChannel(Int32 channelNo)
        {
            using (ReaderLock)
            {
                CastChannel channel = ChildChannels.Find(v => v.ChannelNo == channelNo);
                if (channel == null)
                    throw new AegisException(ResultCode.CastChannel_InvalidChannelNo);

                return channel;
            }
        }


        public CastChannel NewChannel(String name, Boolean isStatic)
        {
            using (WriterLock)
            {
                if (ChildChannels.Where(v => v.Name == name).Count() > 0)
                    throw new AegisException(ResultCode.CastChannel_ExistsName);

                CastChannel channel = new CastChannel(this, name, isStatic);
                ChildChannels.Add(channel);

                return channel;
            }
        }


        private void RemoveChannel()
        {
            using (WriterLock)
            {
                if (IsStatic == true || Parent == null ||
                    Users.Count() > 0 || ChildChannels.Count() > 0)
                    return;


                _isRemoved = true;
                Parent.ChildChannels.Remove(this);
            }

            Parent.RemoveChannel();
        }


        public void Enter(User user)
        {
            using (WriterLock)
            {
                if (_isRemoved == true)
                    throw new AegisException(ResultCode.CastChannel_InvalidChannelNo);

                if (Users.Contains(user) == true)
                    throw new AegisException(ResultCode.CastChannel_ExistsUser);

                Users.Add(user);
            }
        }


        public void Leave(User user)
        {
            using (WriterLock)
            {
                Users.Remove(user);
                RemoveChannel();
            }
        }


        public User GetUser(Int32 userNo)
        {
            using (ReaderLock)
            {
                User user = Users.Find(v => v.UserNo == userNo);
                if (user == null)
                    throw new AegisException(ResultCode.CastChannel_NotExistsUser);

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
