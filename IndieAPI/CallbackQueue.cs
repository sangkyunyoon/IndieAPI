using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;



namespace IndieAPI
{
    public delegate void PacketCallbackHandler(SecurityPacket resPacket);
    public delegate void ResultHandler(Int32 result);





    internal class CallbackQueue
    {
        private Dictionary<Int32, PacketCallbackHandler> _callbacks = new Dictionary<Int32, PacketCallbackHandler>();
        private Queue<SecurityPacket> _receivedPackets = new Queue<SecurityPacket>();





        public CallbackQueue()
        {
        }


        public void Clear()
        {
            lock (this)
            {
                _callbacks.Clear();
                _receivedPackets.Clear();
            }
        }


        public void AddCallback(Int32 key, PacketCallbackHandler callback)
        {
            lock (this)
            {
                if (_callbacks.ContainsKey(key) == true)
                    _callbacks[key] = callback;
                else
                    _callbacks.Add(key, callback);
            }
        }


        public void AddPacket(SecurityPacket packet)
        {
            lock (this)
            {
                _receivedPackets.Enqueue(packet);
            }
        }


        public void DoCallback()
        {
            lock (this)
            {
                while (_receivedPackets.Count() > 0)
                {
                    SecurityPacket packet = _receivedPackets.Dequeue();
                    Int32 key = packet.SeqNo;
                    PacketCallbackHandler callback;


                    if (_callbacks.TryGetValue(key, out callback) == true)
                    {
                        packet.SkipHeader();

                        _callbacks.Remove(key);
                        callback(packet);
                    }
                }
            }
        }
    }
}
