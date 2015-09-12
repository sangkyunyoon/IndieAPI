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
            SecurityPacket packet = new SecurityPacket(Protocol.CS_Hello_Ntf);
            SendPacket(packet);
        }


        private void OnClosed(NetworkSession session)
        {
            _user = null;
        }


        private void OnReceived(NetworkSession session, StreamBuffer buffer)
        {
            SecurityPacket packet = new SecurityPacket(buffer);
            AegisTask.Run(() =>
            {
                packet.Decrypt(Global.AES_IV, Global.AES_Key);
                packet.SkipHeader();


                //  Authentication Packets
                if (packet.PID == Protocol.CS_Auth_RegisterGuest_Req) OnCS_Auth_RegisterGuest_Req(packet);
                else if (packet.PID == Protocol.CS_Auth_RegisterMember_Req) OnCS_Auth_RegisterMember_Req(packet);
                else if (packet.PID == Protocol.CS_Auth_LoginGuest_Req) OnCS_Auth_LoginGuest_Req(packet);
                else if (packet.PID == Protocol.CS_Auth_LoginMember_Req) OnCS_Auth_LoginMember_Req(packet);

                //  Service Packets
                else
                {
                    Int32 userNo = packet.GetInt32();
                    _user = UserManager.Instance.FindUser(userNo);
                    if (_user == null)
                    {
                        //  Force close
                        SendPacket(new SecurityPacket(Protocol.CS_ForceClosing_Ntf), () => { Close(); });
                        return;
                    }


                    switch (packet.PID)
                    {
                        case Protocol.CS_Profile_GetData_Req: OnCS_Profile_GetData_Req(packet); break;
                        case Protocol.CS_Profile_SetData_Req: OnCS_Profile_SetData_Req(packet); break;
                        case Protocol.CS_Profile_Text_GetData_Req: OnCS_Profile_Text_GetData_Req(packet); break;
                        case Protocol.CS_Profile_Text_SetData_Req: OnCS_Profile_Text_SetData_Req(packet); break;

                        case Protocol.CS_CloudSheet_GetSheetList_Req: OnCS_CloudSheet_GetSheetList_Req(packet); break;
                        case Protocol.CS_CloudSheet_GetRecords_Req: OnCS_CloudSheet_GetRecords_Req(packet); break;
                    }
                }
            });
        }


        public void SendPacket(SecurityPacket buffer, Action onSent = null)
        {
            buffer.Encrypt(Global.AES_IV, Global.AES_Key);
            base.SendPacket(buffer, onSent);
        }
    }
}
