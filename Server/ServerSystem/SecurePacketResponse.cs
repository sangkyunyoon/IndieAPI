using System;
using System.Threading;
using Aegis;
using Aegis.Network;



namespace IndieAPI.Server.Routine
{
    public class SecurePacketResponse : SecurePacket
    {
        public new const Int32 HeaderSize = SecurePacket.HeaderSize + 4;
        public Int32 ResultCodeNo
        {
            get { return GetInt32(SecurePacket.HeaderSize); }
            set { OverwriteInt32(SecurePacket.HeaderSize, value); }
        }





        private SecurePacketResponse()
        {
        }


        public SecurePacketResponse(SecurePacketRequest requestPacket, UInt16 capacity = 0)
        {
            if (capacity > 0)
                Capacity(capacity);

            PacketId = (UInt16)(requestPacket.PacketId + 1);
            SeqNo = requestPacket.SeqNo;
            ResultCodeNo = 0;
        }


        public SecurePacketResponse(SecurePacketRequest requestPacket, Int32 resultCode, UInt16 capacity = 0)
        {
            if (capacity > 0)
                Capacity(capacity);

            PacketId = (UInt16)(requestPacket.PacketId + 1);
            SeqNo = requestPacket.SeqNo;
            ResultCodeNo = resultCode;
        }


        public override StreamBuffer Clone()
        {
            SecurePacketResponse packet = new SecurePacketResponse();
            packet.Write(Buffer);

            packet.ResetReadIndex();
            packet.ResetWriteIndex();
            packet.Read(ReadBytes);

            return packet;
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
