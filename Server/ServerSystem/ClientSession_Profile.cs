using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Network;
using Aegis.Converter;



namespace IndieAPI.Server.Routine
{
    public partial class ClientSession
    {
        private void OnCS_Profile_GetData_Req(SecurePacketRequest reqPacket)
        {
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket, ResultCode.Ok);
            resPacket.PutStringAsUtf16(_user.Profile.Nickname ?? "");
            resPacket.PutInt16(_user.Profile.Level);
            resPacket.PutInt16(_user.Profile.Exp);
            resPacket.PutDouble(_user.LoginCounter.RegDate.ToOADate());
            resPacket.PutDouble(_user.LoginCounter.LastLoginDate.ToOADate());
            resPacket.PutByte(_user.LoginCounter.ContinuousCount);
            resPacket.PutByte(_user.LoginCounter.DailyCount);

            SendPacket(resPacket);
        }


        private void OnCS_Profile_SetData_Req(SecurePacketRequest reqPacket)
        {
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket, ResultCode.Ok);


            _user.Profile.Nickname = reqPacket.GetStringFromUtf16();
            _user.Profile.Level = reqPacket.GetInt16();
            _user.Profile.Exp = reqPacket.GetInt16();
            _user.Profile.UpdateToDB();

            SendPacket(resPacket);
        }


        private void OnCS_Profile_Text_GetData_Req(SecurePacketRequest reqPacket)
        {
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket, ResultCode.Ok, 65535);
            resPacket.PutStringAsUtf16(_user.TextBox.TextData);

            SendPacket(resPacket);
        }


        private void OnCS_Profile_Text_SetData_Req(SecurePacketRequest reqPacket)
        {
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket, ResultCode.Ok);

            _user.TextBox.TextData = reqPacket.GetStringFromUtf16();
            SendPacket(resPacket);
        }
    }
}
