using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;
using Aegis.Network;
using IndieAPI.Server.UserManagement;



namespace IndieAPI.Server.Routine
{
    public partial class ClientSession : Session
    {
        private User _user;
        private String _aesIV, _aesKey;





        public ClientSession()
        {
            NetworkEvent_Accepted += OnAccepted;
            NetworkEvent_Closed += OnClosed;
            NetworkEvent_Received += OnReceived;
            PacketValidator += SecurePacketRequest.IsValidPacket;
        }


        private void OnAccepted(Session session)
        {
            SecurePacket ntfPacket = new SecurePacket(Protocol.GetID("CS_Hello_Ntf"));
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


        private void OnClosed(Session session)
        {
            _user?.Logout();
            _user = null;
        }


        private void OnReceived(Session session, StreamBuffer buffer)
        {
            SecurePacketRequest reqPacket = new SecurePacketRequest(buffer);
            reqPacket.Decrypt(_aesIV, _aesKey);
            reqPacket.SkipHeader();



            try
            {
                //  Authentication Packets
                if ((reqPacket.PacketId >> 8) == 0x20)
                {
                    reqPacket.Dispatch(this, "On" + Protocol.GetName(reqPacket.PacketId));
                }

                //  Contents Packets
                else
                {
                    _user = UserManager.Instance.FindUser(reqPacket.UserNo);
                    if (_user == null)
                    {
                        ForceClose("Invalid UserNo.");
                        return;
                    }
                    if (_user.LastSeqNo + 1 != reqPacket.SeqNo)
                    {
                        ForceClose("Invalid Sequence Number.");
                        return;
                    }

                    _user.LastSeqNo = reqPacket.SeqNo;
                    _user.Session = this;
                    _user.LastPulse.Restart();

                    reqPacket.Dispatch(this, "On" + Protocol.GetName(reqPacket.PacketId));
                }
            }
            catch (AegisException e) when (e.ResultCodeNo == AegisResult.BufferUnderflow)
            {
                Logger.Write(LogType.Err, 2, "Packet buffer underflow(PID=0x{0:X}).", reqPacket.PacketId);
            }
        }


        public void ForceClose(String message)
        {
            SecurePacket ntfPacket = new SecurePacket(Protocol.GetID("CS_ForceClosing_Ntf"));
            ntfPacket.PutStringAsUtf16(message);

            SendPacket(ntfPacket, (sentPacket) =>
            {
                Close();
            });
        }


        public override void SendPacket(StreamBuffer buffer, Action<StreamBuffer> onSent = null)
        {
            SecurePacket packet = (SecurePacket)buffer;
            packet.Encrypt(_aesIV, _aesKey);
            base.SendPacket(buffer, onSent);
        }
    }
}
