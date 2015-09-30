using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;



namespace Server.Session
{
    public class PacketResponse : SecurePacket
    {
        public new const Int32 HeaderSize = SecurePacket.HeaderSize + 4;
        public Int32 ResultCodeNo
        {
            get { return GetInt32(SecurePacket.HeaderSize); }
            set { OverwriteInt32(SecurePacket.HeaderSize, value); }
        }





        private PacketResponse()
        {
        }


        public PacketResponse(PacketRequest requestPacket, UInt16 capacity = 0)
        {
            if (capacity > 0)
                Capacity(capacity);

            PacketId = (UInt16)(requestPacket.PacketId + 1);
            SeqNo = requestPacket.SeqNo;
        }


        public PacketResponse(PacketRequest requestPacket, Int32 resultCode, UInt16 capacity = 0)
        {
            if (capacity > 0)
                Capacity(capacity);

            PacketId = (UInt16)(requestPacket.PacketId + 1);
            SeqNo = requestPacket.SeqNo;
            ResultCodeNo = resultCode;
        }


        public PacketResponse(UInt16 packetId, UInt16 capacity = 0)
        {
            if (capacity > 0)
                Capacity(capacity);

            PacketId = packetId;
        }


        public PacketResponse(UInt16 packetId, Int32 resultCode, UInt16 capacity = 0)
        {
            if (capacity > 0)
                Capacity(capacity);

            PacketId = packetId;
            ResultCodeNo = resultCode;
        }


        public override StreamBuffer Clone()
        {
            PacketResponse packet = new PacketResponse();
            packet.Write(Buffer);

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
            GetInt32();         //  ResultCode
        }


        public override void Clear()
        {
            Int32 seqNo = SeqNo;

            base.Clear();
            SeqNo = seqNo;
        }


        public override void Clear(byte[] source, int index, int size)
        {
            Int32 seqNo = SeqNo;

            base.Clear(source, index, size);
            SeqNo = seqNo;
        }


        public override void Clear(StreamBuffer source)
        {
            Int32 seqNo = SeqNo;

            base.Clear(source);
            SeqNo = seqNo;
        }
    }
}
