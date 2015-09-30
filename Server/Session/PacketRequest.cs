using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;



namespace Server.Session
{
    public class PacketRequest : SecurePacket
    {
        public new const Int32 HeaderSize = SecurePacket.HeaderSize + 4;
        public Int32 UserNo { get { return GetInt32(SecurePacket.HeaderSize); } }





        public PacketRequest(StreamBuffer source)
            : base(source)
        {
        }


        public PacketRequest(UInt16 packetId, Int32 userNo)
            : base(packetId)
        {
            PutInt32(userNo);
        }


        public PacketRequest(UInt16 packetId, Int32 userNo, UInt16 capacity)
            : base(packetId, capacity)
        {
            PutInt32(userNo);
        }


        public override StreamBuffer Clone()
        {
            PacketRequest packet = new PacketRequest(this);
            packet.ResetReadIndex();
            packet.ResetWriteIndex();
            packet.Read(ReadBytes);

            return packet;
        }


        public new static Boolean IsValidPacket(StreamBuffer buffer, out Int32 packetSize)
        {
            if (buffer.WrittenBytes < HeaderSize)
            {
                packetSize = 0;
                return false;
            }

            //  최초 2바이트를 수신할 패킷의 크기로 처리
            packetSize = buffer.GetUInt16(0);
            return (packetSize > 0 && buffer.WrittenBytes >= packetSize);
        }


        public override void SkipHeader()
        {
            base.SkipHeader();
            GetInt32();         //  UserNo
        }
    }
}
