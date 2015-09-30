using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using Aegis.Network;
using Server.Services.UserData;



namespace Server.Session
{
    public partial class ClientSession : AsyncResultSession
    {
        private User _user;





        public ClientSession()
        {
            NetworkEvent_Accepted += OnAcceptd;
            NetworkEvent_Closed += OnClosed;
            NetworkEvent_Received += OnReceived;
            PacketValidator += IsValidPacket;
        }


        private Boolean IsValidPacket(NetworkSession session, StreamBuffer buffer, out int packetSize)
        {
            if (buffer.WrittenBytes < 8)
            {
                packetSize = 0;
                return false;
            }

            //  최초 2바이트를 수신할 패킷의 크기로 처리
            packetSize = buffer.GetUInt16();
            return (packetSize > 0 && buffer.WrittenBytes >= packetSize);
        }


        private void OnAcceptd(NetworkSession session)
        {
            SendPacket(new SecurePacket(Protocol.CS_Hello_Ntf));
        }


        private void OnClosed(NetworkSession session)
        {
            if (_user != null)
            {
                _user.CastChannel?.Leave(_user);
                _user.CastChannel = null;
                _user.Session = null;
            }

            _user = null;
        }


        private void OnReceived(NetworkSession session, StreamBuffer buffer)
        {
            PacketRequest reqPacket = new PacketRequest(buffer);
            reqPacket.Decrypt(Global.AES_IV, Global.AES_Key);
            reqPacket.SkipHeader();


            //  Authentication Packets
            if ((reqPacket.PacketId >> 8) == 0x20)
            {
                AegisTask.Run(() =>
                {
                    if (reqPacket.PacketId == Protocol.CS_Auth_RegisterGuest_Req) OnCS_Auth_RegisterGuest_Req(reqPacket);
                    else if (reqPacket.PacketId == Protocol.CS_Auth_RegisterMember_Req) OnCS_Auth_RegisterMember_Req(reqPacket);
                    else if (reqPacket.PacketId == Protocol.CS_Auth_LoginGuest_Req) OnCS_Auth_LoginGuest_Req(reqPacket);
                    else if (reqPacket.PacketId == Protocol.CS_Auth_LoginMember_Req) OnCS_Auth_LoginMember_Req(reqPacket);
                });
            }

            //  Contents Packets
            else
            {
                //  Validate User object
                {
                    _user = UserManager.Instance.FindUser(reqPacket.UserNo);
                    if (_user == null)
                    {
                        SendPacket(new SecurePacket(Protocol.CS_ForceClosing_Ntf), (sentPacket) => { Close(); });
                        return;
                    }
                    if (_user.LastSeqNo + 1 != reqPacket.SeqNo)
                    {
                        Logger.Write(LogType.Info, 2, "Invalid SequenceNo(UserNo={0}).", _user.UserNo);
                        SendPacket(new SecurePacket(Protocol.CS_ForceClosing_Ntf), (sentPacket) => { Close(); });
                        return;
                    }

                    _user.LastSeqNo = reqPacket.SeqNo;
                    _user.Session = this;
                    _user.LastAliveTick.Restart();
                }


                AegisTask.Run(() =>
                {
                    try
                    {
                        switch (reqPacket.PacketId)
                        {
                            case Protocol.CS_Profile_GetData_Req: OnCS_Profile_GetData_Req(reqPacket); break;
                            case Protocol.CS_Profile_SetData_Req: OnCS_Profile_SetData_Req(reqPacket); break;
                            case Protocol.CS_Profile_Text_GetData_Req: OnCS_Profile_Text_GetData_Req(reqPacket); break;
                            case Protocol.CS_Profile_Text_SetData_Req: OnCS_Profile_Text_SetData_Req(reqPacket); break;

                            case Protocol.CS_CloudSheet_GetSheetList_Req: OnCS_CloudSheet_GetSheetList_Req(reqPacket); break;
                            case Protocol.CS_CloudSheet_GetRecords_Req: OnCS_CloudSheet_GetRecords_Req(reqPacket); break;

                            case Protocol.CS_IMC_ChannelList_Req: OnCS_IMC_ChannelList_Req(reqPacket); break;
                            case Protocol.CS_IMC_Create_Req: OnCS_IMC_Create_Req(reqPacket); break;
                            case Protocol.CS_IMC_Enter_Req: OnCS_IMC_Enter_Req(reqPacket); break;
                            case Protocol.CS_IMC_Leave_Req: OnCS_IMC_Leave_Req(reqPacket); break;
                            case Protocol.CS_IMC_UserList_Req: OnCS_IMC_UserList_Req(reqPacket); break;
                            case Protocol.CS_IMC_SendMessage_Req: OnCS_IMC_SendMessage_Req(reqPacket); break;

                            case Protocol.CS_CacheBox_SetValue_Req: OnCS_CacheBox_SetValue_Req(reqPacket); break;
                            case Protocol.CS_CacheBox_SetExpireTime_Req: OnCS_CacheBox_SetExpireTime_Req(reqPacket); break;
                            case Protocol.CS_CacheBox_GetValue_Req: OnCS_CacheBox_GetValue_Req(reqPacket); break;
                        }
                    }
                    catch (AegisException e) when (e.ResultCodeNo == AegisResult.BufferUnderflow)
                    {
                        Logger.Write(LogType.Err, 2, "Invalid pakcet received(PID=0x{0:X}).", reqPacket.PacketId);
                    }
                });
            }
        }


        public override void SendPacket(StreamBuffer buffer, Action<StreamBuffer> onSent = null)
        {
            SecurePacket packet = (SecurePacket)buffer;
            packet.Encrypt(Global.AES_IV, Global.AES_Key);
            base.SendPacket(buffer, onSent);
        }
    }
}
