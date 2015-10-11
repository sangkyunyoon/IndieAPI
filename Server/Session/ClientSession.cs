using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using Aegis.Network;
using IndieAPI.Server.UserManagement;



namespace IndieAPI.Server.Session
{
    public partial class ClientSession : AsyncResultSession
    {
        private User _user;
        private String _aesIV, _aesKey;





        public ClientSession()
        {
            NetworkEvent_Accepted += OnAccepted;
            NetworkEvent_Closed += OnClosed;
            NetworkEvent_Received += OnReceived;
            PacketValidator += PacketRequest.IsValidPacket;
        }


        private void OnAccepted(NetworkSession session)
        {
            SecurePacket ntfPacket = new SecurePacket(Protocol.CS_Hello_Ntf);
            Int32 seed = 0;


            //  각 8비트마다 0이 나오지 않는 임의 숫자 생성
            seed |= Randomizer.NextNumber(1, 255) << 24;
            seed |= Randomizer.NextNumber(1, 255) << 16;
            seed |= Randomizer.NextNumber(1, 255) << 8;
            seed |= Randomizer.NextNumber(1, 255);


            //  패킷 암호화 키 생성
            {
                String characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
                char[] ascii = new char[16];


                for (Int32 i = 0; i < 16; ++i)
                {
                    Int32 val = seed & (0x6E << i);
                    ascii[i] = characterSet[(val % characterSet.Length)];
                }
                _aesIV = new string(ascii);


                for (Int32 i = 0; i < 16; ++i)
                {
                    Int32 val = seed & (0xF4 << i);
                    ascii[i] = characterSet[(val % characterSet.Length)];
                }
                _aesKey = new string(ascii);
            }


            ntfPacket.PutInt32(seed);
            ntfPacket.Encrypt(Global.AES_IV, Global.AES_Key);
            base.SendPacket(ntfPacket);
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
            reqPacket.Decrypt(_aesIV, _aesKey);
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

                            default:
                                Logger.Write(LogType.Err, 2, "Invalid pakcet received(PID=0x{0:X}).", reqPacket.PacketId);
                                break;
                        }
                    }
                    catch (AegisException e) when (e.ResultCodeNo == AegisResult.BufferUnderflow)
                    {
                        Logger.Write(LogType.Err, 2, "Packet buffer underflow(PID=0x{0:X}).", reqPacket.PacketId);
                    }
                });
            }
        }


        public override void SendPacket(StreamBuffer buffer, Action<StreamBuffer> onSent = null)
        {
            SecurePacket packet = (SecurePacket)buffer;
            packet.Encrypt(_aesIV, _aesKey);
            base.SendPacket(buffer, onSent);
        }
    }
}
