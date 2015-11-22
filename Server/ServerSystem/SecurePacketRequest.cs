using System;
using System.Threading;
using System.Reflection;
using Aegis;
using Aegis.Network;



namespace IndieAPI.Server.Routine
{
    public class SecurePacketRequest : SecurePacket
    {
        public new const Int32 HeaderSize = SecurePacket.HeaderSize + 4;
        public Int32 UserNo { get { return GetInt32(SecurePacket.HeaderSize); } }





        public SecurePacketRequest(StreamBuffer source)
            : base(source)
        {
        }


        public SecurePacketRequest(UInt16 packetId, Int32 userNo)
            : base(packetId)
        {
            PutInt32(userNo);
        }


        public SecurePacketRequest(UInt16 packetId, Int32 userNo, UInt16 capacity)
            : base(packetId, capacity)
        {
            PutInt32(userNo);
        }


        public override StreamBuffer Clone()
        {
            SecurePacketRequest packet = new SecurePacketRequest(this);
            packet.ResetReadIndex();
            packet.ResetWriteIndex();
            packet.Read(ReadBytes);

            return packet;
        }


        public override void SkipHeader()
        {
            base.SkipHeader();
            GetInt32();         //  UserNo
        }


        public void Dispatch(Object instance, String methodName)
        {
            MethodInfo method = instance.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (method == null)
                throw new AegisException("No {0} method in {1}.", methodName, instance.GetType().Name);

            method.Invoke(instance, new object[] { this });
        }
    }
}
