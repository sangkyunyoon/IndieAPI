using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;



namespace IndieAPI
{
    public class ResponseData
    {
        public readonly int ResultCodeNo;
        public readonly string ResultString;
        public readonly StreamBuffer Packet;


        internal ResponseData(SecurityPacket packet)
        {
            packet.SkipHeader();
            Packet = packet;

            ResultCodeNo = packet.GetInt32();
            ResultString = ResultCode.ToString(ResultCodeNo);
        }


        internal ResponseData(int resultCodeNo)
        {
            ResultCodeNo = resultCodeNo;
        }
    }


    public class Response_Profile : ResponseData
    {
        public readonly string Nickname;
        public readonly Int16 Level;
        public readonly Int16 Exp;

        public readonly DateTime RegDate;
        public readonly DateTime LastLoginDate;
        public readonly Int16 LoginContinuousCount;
        public readonly Int16 LoginDailyCount;



        internal Response_Profile(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            Nickname = packet.GetStringFromUtf16();
            Level = packet.GetInt16();
            Exp = packet.GetInt16();

            RegDate = DateTime.FromOADate(packet.GetDouble());
            LastLoginDate = DateTime.FromOADate(packet.GetDouble());
            LoginContinuousCount = packet.GetByte();
            LoginDailyCount = packet.GetByte();
        }
    }


    public class Response_Profile_Text : ResponseData
    {
        public readonly string TextData;



        internal Response_Profile_Text(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            TextData = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_ChannelList : ResponseData
    {
        public struct ChannelInfo
        {
            public int ChannelNo;
            public string ChannelName;
        }
        public readonly List<ChannelInfo> Channels = new List<ChannelInfo>();



        internal Response_IMC_ChannelList(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;


            int count = packet.GetInt32();
            while (count-- > 0)
            {
                Channels.Add(new ChannelInfo()
                {
                    ChannelNo = packet.GetInt32(),
                    ChannelName = packet.GetStringFromUtf16()
                });
            }
        }
    }


    public class Response_IMC_Create : ResponseData
    {
        public readonly int ChannelNo;
        public readonly string ChannelName;



        internal Response_IMC_Create(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            ChannelNo = packet.GetInt32();
            ChannelName = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_ChannelStatus : ResponseData
    {
        public readonly int ChannelNo;
        public readonly string ChannelName;
        public readonly int UserCount;



        internal Response_IMC_ChannelStatus(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            ChannelNo = packet.GetInt32();
            ChannelName = packet.GetStringFromUtf16();
            UserCount = packet.GetInt32();
        }
    }


    public class Response_IMC_Enter : ResponseData
    {
        public readonly int ChannelNo;
        public readonly string ChannelName;



        internal Response_IMC_Enter(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            ChannelNo = packet.GetInt32();
            ChannelName = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_EnteredUser : ResponseData
    {
        public readonly int UserNo;
        public readonly string Nickname;



        internal Response_IMC_EnteredUser(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            UserNo = packet.GetInt32();
            Nickname = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_LeavedUser : ResponseData
    {
        public readonly int UserNo;



        internal Response_IMC_LeavedUser(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            UserNo = packet.GetInt32();
        }
    }


    public class Response_IMC_UserList : ResponseData
    {
        public struct UserData
        {
            public int UserNo;
            public string Nickname;
        }
        public readonly List<UserData> Users = new List<UserData>();



        internal Response_IMC_UserList(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;


            int count = packet.GetInt32();
            while (count-- > 0)
            {
                Users.Add(new UserData()
                {
                    UserNo = packet.GetInt32(),
                    Nickname = packet.GetStringFromUtf16()
                });
            }
        }
    }


    public class Response_IMC_Message : ResponseData
    {
        public readonly int SenderUserNo;
        public readonly StreamBuffer Message;



        internal Response_IMC_Message(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            SenderUserNo = packet.GetInt32();
            Message = new StreamBuffer(packet.Buffer, packet.ReadBytes, packet.ReadableSize);
        }
    }
}
