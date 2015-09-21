using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Network;



namespace Server.Session
{
    public partial class ClientSession
    {
        private void OnCS_Auth_RegisterGuest_Req(SecurePacket reqPacket)
        {
            String udid = reqPacket.GetStringFromUtf16();
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Auth_RegisterGuest_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            Services.Membership.Instance.RegisterGuest(udid, (result) =>
            {
                resPacket.PutInt32(result);
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_RegisterMember_Req(SecurePacket reqPacket)
        {
            String udid = reqPacket.GetStringFromUtf16();
            String userId = reqPacket.GetStringFromUtf16();
            String userPwd = reqPacket.GetStringFromUtf16();
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Auth_RegisterMember_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            Services.Membership.Instance.RegisterMember(udid, userId, userPwd, (result) =>
            {
                resPacket.PutInt32(result);
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_LoginGuest_Req(SecurePacket reqPacket)
        {
            String udid = reqPacket.GetStringFromUtf16();
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Auth_LoginGuest_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            Services.Membership.Instance.LoginGuest(udid, async (result, userNo) =>
            {
                if (result == ResultCode.Ok)
                {
                    _user = Services.UserData.UserManager.Instance.GetUser(userNo);
                    _user.LastSeqNo = reqPacket.SeqNo;
                    await _user.LoadFromDB();

                    _user.LoginCounter.OnLoggedIn();
                }


                resPacket.PutInt32(result);
                resPacket.PutInt32(userNo);
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_LoginMember_Req(SecurePacket reqPacket)
        {
            String udid = reqPacket.GetStringFromUtf16();
            String userId = reqPacket.GetStringFromUtf16();
            String userPwd = reqPacket.GetStringFromUtf16();
            SecurePacket resPacket = new SecurePacket(Protocol.CS_Auth_LoginMember_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            Services.Membership.Instance.LoginMember(udid, userId, userPwd, async (result, userNo) =>
            {
                if (result == ResultCode.Ok)
                {
                    _user = Services.UserData.UserManager.Instance.GetUser(userNo);
                    _user.LastSeqNo = reqPacket.SeqNo;
                    await _user.LoadFromDB();

                    _user.LoginCounter.OnLoggedIn();
                }


                resPacket.PutInt32(result);
                resPacket.PutInt32(userNo);
                SendPacket(resPacket);
            });
        }
    }
}
