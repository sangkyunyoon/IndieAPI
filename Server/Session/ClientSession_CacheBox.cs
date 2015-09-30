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
        private void OnCS_Cache_SetValue_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Cache_SetValue_Res);
            resPacket.SeqNo = reqPacket.SeqNo;

            String key = reqPacket.GetStringFromUtf16();
            String value = reqPacket.GetStringFromUtf16();
            Int32 durationMinutes = reqPacket.GetInt32();
            Double expireTime = reqPacket.GetDouble();


            try
            {
                if (durationMinutes > 0)
                    expireTime = DateTime.Now.AddMinutes(durationMinutes).ToOADate();
                else
                    expireTime = DateTime.FromOADate(expireTime).ToLocalTime().ToOADate();


                CacheBox.Instance.Set(key, value, expireTime);
                resPacket.PutInt32(ResultCode.Ok);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }

            SendPacket(resPacket);
        }


        private void OnCS_Cache_SetExpireTime_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Cache_SetExpireTime_Res);
            resPacket.SeqNo = reqPacket.SeqNo;

            String key = reqPacket.GetStringFromUtf16();
            Int32 durationMinutes = reqPacket.GetInt32();
            Double expireTime = reqPacket.GetDouble();


            try
            {
                if (durationMinutes > 0)
                    expireTime = DateTime.Now.AddMinutes(durationMinutes).ToOADate();
                else
                    expireTime = DateTime.FromOADate(expireTime).ToLocalTime().ToOADate();


                CacheBox.Instance.SetExpireTime(key, expireTime);
                resPacket.PutInt32(ResultCode.Ok);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }

            SendPacket(resPacket);
        }


        private void OnCS_Cache_GetValue_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Cache_GetValue_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;

            String key = reqPacket.GetStringFromUtf16();


            try
            {
                String value;
                Int32 durationMunites;
                CacheBox.Instance.Get(key, out value, out durationMunites);

                resPacket.PutInt32(ResultCode.Ok);
                resPacket.PutStringAsUtf16(value);
                resPacket.PutInt32(durationMunites);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }

            SendPacket(resPacket);
        }
    }
}
