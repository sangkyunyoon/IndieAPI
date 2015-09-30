using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Server.Services;



namespace Server.Session
{
    public partial class ClientSession
    {
        private void OnCS_CacheBox_SetValue_Req(PacketRequest reqPacket)
        {
            PacketResponse resPacket = new PacketResponse(reqPacket, ResultCode.Ok);
            String key = reqPacket.GetStringFromUtf16();
            String value = reqPacket.GetStringFromUtf16();
            Int32 durationMinutes = reqPacket.GetInt32();
            Double expireTime = reqPacket.GetDouble();


            try
            {
                if (durationMinutes == -1 && expireTime == -1)
                    expireTime = -1;
                else if (durationMinutes >= 0)
                    expireTime = DateTime.Now.AddMinutes(durationMinutes).ToOADate();
                else
                    expireTime = DateTime.FromOADate(expireTime).ToLocalTime().ToOADate();


                CacheBox.Instance.Set(key, value, expireTime);
            }
            catch (AegisException e)
            {
                resPacket.ResultCodeNo = e.ResultCodeNo;
            }

            SendPacket(resPacket);
        }


        private void OnCS_CacheBox_SetExpireTime_Req(PacketRequest reqPacket)
        {
            PacketResponse resPacket = new PacketResponse(reqPacket, ResultCode.Ok);
            String key = reqPacket.GetStringFromUtf16();
            Int32 durationMinutes = reqPacket.GetInt32();
            Double expireTime = reqPacket.GetDouble();


            try
            {
                if (durationMinutes == -1 && expireTime == -1)
                    expireTime = -1;
                else if (durationMinutes >= 0)
                    expireTime = DateTime.Now.AddMinutes(durationMinutes).ToOADate();
                else
                    expireTime = DateTime.FromOADate(expireTime).ToLocalTime().ToOADate();


                CacheBox.Instance.SetExpireTime(key, expireTime);
            }
            catch (AegisException e)
            {
                resPacket.ResultCodeNo = e.ResultCodeNo;
            }

            SendPacket(resPacket);
        }


        private void OnCS_CacheBox_GetValue_Req(PacketRequest reqPacket)
        {
            PacketResponse resPacket = new PacketResponse(reqPacket, 65535);
            String key = reqPacket.GetStringFromUtf16();


            try
            {
                String value;
                Int32 durationMunites;
                CacheBox.Instance.Get(key, out value, out durationMunites);

                resPacket.ResultCodeNo = ResultCode.Ok;
                resPacket.PutStringAsUtf16(value);
                resPacket.PutInt32(durationMunites);
            }
            catch (AegisException e)
            {
                resPacket.ResultCodeNo = e.ResultCodeNo;
            }

            SendPacket(resPacket);
        }
    }
}
