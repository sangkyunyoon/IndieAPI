﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;



namespace IndieAPI
{
    public class Response
    {
        public readonly int ResultCodeNo;
        public readonly string ResultString;
        public readonly StreamBuffer Packet;


        internal Response(SecurePacket packet)
        {
            packet.SkipHeader();
            Packet = packet;

            ResultCodeNo = packet.GetInt32();
            ResultString = ResultCode.ToString(ResultCodeNo);
        }


        internal Response(int resultCodeNo)
        {
            ResultCodeNo = resultCodeNo;
        }
    }


    public class Response_Profile : Response
    {
        public readonly string Nickname;
        public readonly Int16 Level;
        public readonly Int16 Exp;

        public readonly DateTime RegDate;
        public readonly DateTime LastLoginDate;
        public readonly Int16 LoginContinuousCount;
        public readonly Int16 LoginDailyCount;



        internal Response_Profile(SecurePacket packet)
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


    public class Response_Profile_Text : Response
    {
        public readonly string TextData;



        internal Response_Profile_Text(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            TextData = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_ChannelList : Response
    {
        public struct ChannelInfo
        {
            public int ChannelNo;
            public string ChannelName;
        }
        public readonly List<ChannelInfo> Channels = new List<ChannelInfo>();



        internal Response_IMC_ChannelList(SecurePacket packet)
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


    public class Response_IMC_Create : Response
    {
        public readonly int ChannelNo;
        public readonly string ChannelName;



        internal Response_IMC_Create(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            ChannelNo = packet.GetInt32();
            ChannelName = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_Enter : Response
    {
        public readonly int ChannelNo;
        public readonly string ChannelName;



        internal Response_IMC_Enter(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            ChannelNo = packet.GetInt32();
            ChannelName = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_EnteredUser : Response
    {
        public readonly int UserNo;
        public readonly string Nickname;



        internal Response_IMC_EnteredUser(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            UserNo = packet.GetInt32();
            Nickname = packet.GetStringFromUtf16();
        }
    }


    public class Response_IMC_LeavedUser : Response
    {
        public readonly int UserNo;



        internal Response_IMC_LeavedUser(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            UserNo = packet.GetInt32();
        }
    }


    public class Response_IMC_UserList : Response
    {
        public struct UserData
        {
            public int UserNo;
            public string Nickname;
        }
        public readonly List<UserData> Users = new List<UserData>();



        internal Response_IMC_UserList(SecurePacket packet)
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


    public class Response_IMC_Message : Response
    {
        public readonly int SenderUserNo;
        public readonly StreamBuffer Message;



        internal Response_IMC_Message(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            SenderUserNo = packet.GetInt32();
            Message = new StreamBuffer(packet.Buffer, packet.ReadBytes, packet.ReadableSize);
        }
    }


    public class Response_CacheBox_Value : Response
    {
        public readonly string Value;
        public readonly int DurationMinutes;



        internal Response_CacheBox_Value(SecurePacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            Value = packet.GetStringFromUtf16();
            DurationMinutes = packet.GetInt32();
        }
    }
}
