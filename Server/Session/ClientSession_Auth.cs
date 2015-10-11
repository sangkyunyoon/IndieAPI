﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Network;



namespace IndieAPI.Server.Session
{
    public partial class ClientSession
    {
        private void OnCS_Auth_RegisterGuest_Req(PacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            PacketResponse resPacket = new PacketResponse(reqPacket);


            Services.Membership.Instance.RegisterGuest(uuid, (result) =>
            {
                resPacket.PutInt32(result);
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_RegisterMember_Req(PacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            String userId = reqPacket.GetStringFromUtf16();
            String userPwd = reqPacket.GetStringFromUtf16();
            PacketResponse resPacket = new PacketResponse(reqPacket);


            Services.Membership.Instance.RegisterMember(uuid, userId, userPwd, (result) =>
            {
                resPacket.PutInt32(result);
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_LoginGuest_Req(PacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            PacketResponse resPacket = new PacketResponse(reqPacket);


            Services.Membership.Instance.LoginGuest(uuid, async (result, userNo) =>
            {
                if (result == ResultCode.Ok)
                {
                    _user = UserManagement.UserManager.Instance.GetUser(userNo);
                    _user.LastSeqNo = reqPacket.SeqNo;
                    await _user.LoadFromDB();

                    _user.LoginCounter.OnLoggedIn();
                }


                resPacket.PutInt32(result);
                resPacket.PutInt32(userNo);
                SendPacket(resPacket);
            });
        }


        private void OnCS_Auth_LoginMember_Req(PacketRequest reqPacket)
        {
            String uuid = reqPacket.GetStringFromUtf16();
            String userId = reqPacket.GetStringFromUtf16();
            String userPwd = reqPacket.GetStringFromUtf16();
            PacketResponse resPacket = new PacketResponse(reqPacket);


            Services.Membership.Instance.LoginMember(uuid, userId, userPwd, async (result, userNo) =>
            {
                if (result == ResultCode.Ok)
                {
                    _user = UserManagement.UserManager.Instance.GetUser(userNo);
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
