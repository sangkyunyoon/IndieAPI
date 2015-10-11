using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;



namespace IndieAPI
{
    public partial class Request
    {
        private AegisClient _aegisClient = new AegisClient();
        private Queue<SecurePacket> _queueSendPacket = new Queue<SecurePacket>();
        private Queue<SecurePacket> _queueReceivedPacket = new Queue<SecurePacket>();
        private CallbackQueue _callbackQueue = new CallbackQueue();
        private NetworkStatusChanged _handlerNetworkStatus;

        private Int32 _nextSeqNo, _userNo;
        private String _initAESIV, _initAESKey;
        private String _aesIV, _aesKey;

        public Int32 ConnectionAliveTime
        {
            get { return _aegisClient.ConnectionAliveTime; }
            set { _aegisClient.ConnectionAliveTime = value; }
        }





        public void Initialize(String hostAddress, Int32 hostPortNo, NetworkStatusChanged handler)
        {
            _aegisClient.NetworkEvent_Connected += OnConnect;
            _aegisClient.NetworkEvent_Disconnected += OnDisconnect;
            _aegisClient.NetworkEvent_Received += OnReceive;
            _aegisClient.PacketValidator = IsValidPacket;
            _aegisClient.Initialize();

            _aegisClient.HostAddress = hostAddress;
            _aegisClient.HostPortNo = hostPortNo;
            _aegisClient.EnableSend = false;

            _initAESIV = _aesIV = "1sf_@skKKw#ns(*s";
            _initAESKey = _aesKey = "nt_som5slm1NBB3 ";
            _handlerNetworkStatus = handler;

            _callbackQueue.NoMatchesPacket = OnNoMatchesPacket;

            _nextSeqNo = 1;
            _userNo = 0;

            ConnectionAliveTime = 0;
        }


        public void Release()
        {
            _aegisClient.Release();
            _callbackQueue.Clear();
            _queueSendPacket.Clear();
        }


        public void Disconnect()
        {
            _aegisClient.Close();
        }


        public void Update()
        {
            ProcessSendQueue();
            _callbackQueue.DoCallback();
        }


        private void ProcessSendQueue()
        {
            lock (_aegisClient)
            {
                if (_queueSendPacket.Count() == 0)
                    return;

                if (_aegisClient.ConnectionStatus == ConnectionStatus.Closed)
                    _aegisClient.Connect();

                if (_aegisClient.ConnectionStatus == ConnectionStatus.Connected &&
                    _aegisClient.EnableSend == true)
                {
                    SecurePacket packet = _queueSendPacket.Dequeue();
                    packet.Encrypt(_aesIV, _aesKey);
                    _aegisClient.SendPacket(packet);
                }
            }
        }


        private void OnNetworkStatusChanged(NetworkStatus status)
        {
            if (_handlerNetworkStatus != null)
                _handlerNetworkStatus(status);
        }


        private bool IsValidPacket(StreamBuffer buffer, out int packetSize)
        {
            if (buffer.WrittenBytes < 4)
            {
                packetSize = 0;
                return false;
            }

            //  최초 2바이트를 수신할 패킷의 크기로 처리
            packetSize = buffer.GetUInt16();
            return (packetSize > 0 && buffer.WrittenBytes >= packetSize);
        }


        private void OnConnect(bool connected)
        {
            if (connected == true)
                OnNetworkStatusChanged(NetworkStatus.Connected);
            else
                OnNetworkStatusChanged(NetworkStatus.ConnectionFailed);
        }


        private void OnDisconnect()
        {
            _aegisClient.EnableSend = false;
            OnNetworkStatusChanged(NetworkStatus.Disconnected);

            _aesIV = _initAESIV;
            _aesKey = _initAESKey;
        }


        private void OnReceive(StreamBuffer buffer)
        {
            SecurePacket packet = new SecurePacket(buffer);
            packet.Decrypt(_aesIV, _aesKey);
            packet.SkipHeader();


            if (packet.PacketId == Protocol.CS_Hello_Ntf)
            {
                OnHello(packet);
                return;
            }

            if (packet.PacketId == Protocol.CS_ForceClosing_Ntf)
                OnNetworkStatusChanged(NetworkStatus.SessionForceClosed);

            else
            {
                if (packet.PacketId == Protocol.CS_Auth_LoginGuest_Res ||
                    packet.PacketId == Protocol.CS_Auth_LoginMember_Res)
                {
                    Int32 result = packet.GetInt32();
                    if (result == ResultCode.Ok)
                        _userNo = packet.GetInt32();
                }

                _callbackQueue.AddPacket(packet);
            }
        }


        private void OnHello(SecurePacket packet)
        {
            Int32 seed = packet.GetInt32();
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


            _aegisClient.EnableSend = true;
        }


        private void OnNoMatchesPacket(SecurePacket packet)
        {
            switch (packet.PacketId)
            {
                case Protocol.CS_IMC_EnteredUser_Ntf:
                    if (IMC_EnteredUser != null)
                        IMC_EnteredUser(new Response_IMC_EnteredUser(packet));
                    break;


                case Protocol.CS_IMC_LeavedUser_Ntf:
                    if (IMC_LeavedUser != null)
                        IMC_LeavedUser(new Response_IMC_LeavedUser(packet));
                    break;


                case Protocol.CS_IMC_Message_Ntf:
                    if (IMC_Message != null)
                        IMC_Message(new Response_IMC_Message(packet));
                    break;
            }
        }


        private void SendPacket(SecurePacket packet, Action<SecurePacket> responseAction)
        {
            lock (_aegisClient)
            {
                packet.SeqNo = _nextSeqNo++;
                if (_nextSeqNo == Int32.MaxValue)
                    _nextSeqNo = 0;

                _queueSendPacket.Enqueue(packet);
                _callbackQueue.AddCallback(packet.SeqNo, responseAction);
            }
        }
    }
}
