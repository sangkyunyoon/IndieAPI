using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;



namespace IndieAPI
{
    public class ResponseData
    {
        public Int32 ResultCodeNo { get; private set; }
        public String ResultString { get; private set; }
        public SecurityPacket Packet { get; private set; }


        internal ResponseData(SecurityPacket packet)
        {
            Packet = packet;
            Packet.SkipHeader();

            ResultCodeNo = packet.GetInt32();
            ResultString = ResultCode.ToString(ResultCodeNo);
        }


        internal ResponseData(Int32 resultCodeNo)
        {
            ResultCodeNo = resultCodeNo;
        }
    }


    public class Response_Profile : ResponseData
    {
        public String Nickname { get; private set; }
        public Int16 Level { get; private set; }
        public Int16 Exp { get; private set; }

        public DateTime RegDate { get; private set; }
        public DateTime LastLoginDate { get; private set; }
        public Int16 LoginContinuousCount { get; private set; }
        public Int16 LoginDailyCount { get; private set; }



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
        public String TextData { get; private set; }



        internal Response_Profile_Text(SecurityPacket packet)
            : base(packet)
        {
            if (ResultCodeNo != ResultCode.Ok)
                return;

            TextData = packet.GetStringFromUtf16();
        }
    }
}
