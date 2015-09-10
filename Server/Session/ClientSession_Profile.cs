using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Network;
using Aegis.Converter;



namespace Server.Session
{
    public partial class ClientSession
    {
        private void OnCS_Profile_GetData_Req(SecurityPacket reqPacket)
        {
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Profile_GetData_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            resPacket.PutInt32(ResultCode.Ok);
            resPacket.PutStringAsUtf16(_user.Profile.Nickname ?? "");
            resPacket.PutInt16(_user.Profile.Level);
            resPacket.PutInt16(_user.Profile.Exp);
            resPacket.PutDouble(_user.LoginCounter.RegDate.ToOADate());
            resPacket.PutDouble(_user.LoginCounter.LastLoginDate.ToOADate());
            resPacket.PutByte(_user.LoginCounter.ContinuousCount);
            resPacket.PutByte(_user.LoginCounter.DailyCount);
            SendPacket(resPacket);
        }


        private void OnCS_Profile_SetData_Req(SecurityPacket reqPacket)
        {
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Profile_SetData_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            _user.Profile.Nickname = reqPacket.GetStringFromUtf16();
            _user.Profile.Level = reqPacket.GetInt16();
            _user.Profile.Exp = reqPacket.GetInt16();
            _user.Profile.UpdateToDB();


            resPacket.PutInt32(ResultCode.Ok);
            SendPacket(resPacket);
        }


        private void OnCS_Profile_Text_GetData_Req(SecurityPacket reqPacket)
        {
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Profile_Text_GetData_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;


            resPacket.PutInt32(ResultCode.Ok);
            resPacket.PutStringAsUtf16(_user.TextBox.TextData);
            SendPacket(resPacket);
        }


        private void OnCS_Profile_Text_SetData_Req(SecurityPacket reqPacket)
        {
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Profile_Text_SetData_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            _user.TextBox.TextData = reqPacket.GetStringFromUtf16();


            resPacket.PutInt32(ResultCode.Ok);
            SendPacket(resPacket);
        }
    }
}
