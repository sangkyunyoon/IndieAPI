using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Network;



namespace IndieAPI.Server.Routine
{
    public partial class ClientSession
    {
        private void OnCS_Auth_RegisterGuest_Req(SecurePacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket);


            Services.Membership.Instance.RegisterGuest(uuid, (result) =>
            {
                resPacket.ResultCodeNo = result;
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_RegisterMember_Req(SecurePacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            String userId = reqPacket.GetStringFromUtf16();
            String userPwd = reqPacket.GetStringFromUtf16();
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket);


            Services.Membership.Instance.RegisterMember(uuid, userId, userPwd, (result) =>
            {
                resPacket.ResultCodeNo = result;
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_LoginGuest_Req(SecurePacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket);


            Services.Membership.Instance.LoginGuest(uuid, (result, userNo) =>
            {
                if (result == ResultCode.Ok)
                {
                    _user = UserManagement.UserManager.Instance.GetUser(userNo);
                    _user.LastSeqNo = reqPacket.SeqNo;

                    _user.LoadFromDB(() =>
                    {
                        _user.LoginCounter.OnLoggedIn();

                        resPacket.ResultCodeNo = result;
                        resPacket.PutInt32(userNo);
                        SendPacket(resPacket);
                    });
                }
                else
                {
                    resPacket.ResultCodeNo = result;
                    resPacket.PutInt32(userNo);
                    SendPacket(resPacket);
                }
            });
        }


        private void OnCS_Auth_LoginMember_Req(SecurePacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            String userId = reqPacket.GetStringFromUtf16();
            String userPwd = reqPacket.GetStringFromUtf16();
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket);


            Services.Membership.Instance.LoginMember(uuid, userId, userPwd, (result, userNo) =>
            {
                if (result == ResultCode.Ok)
                {
                    _user = UserManagement.UserManager.Instance.GetUser(userNo);
                    _user.LastSeqNo = reqPacket.SeqNo;

                    _user.LoadFromDB(() =>
                    {
                        _user.LoginCounter.OnLoggedIn();

                        resPacket.ResultCodeNo = result;
                        resPacket.PutInt32(userNo);
                        SendPacket(resPacket);
                    });
                }
                else
                {
                    resPacket.ResultCodeNo = result;
                    resPacket.PutInt32(userNo);
                    SendPacket(resPacket);
                }
            });
        }
    }
}
