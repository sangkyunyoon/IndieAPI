﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;



namespace IndieAPI
{
    public partial class Request
    {
        private AegisClient _aegisClient = new AegisClient();
        private Queue<SecurePacket> _queueReceivedPacket = new Queue<SecurePacket>();
        private CallbackQueue _callbackQueue = new CallbackQueue();
        private NetworkStatusChanged _handlerNetworkStatus;

        private Int32 _nextSeqNo, _userNo;
        private String _aesIV, _aesKey;

        public Int32 ConnectionAliveTime
        {
            get { return _aegisClient.ConnectionAliveTime; }
            set { _aegisClient.ConnectionAliveTime = value; }
        }





        public void Initialize(String hostAddress, Int32 hostPortNo, String aesIV, String aesKey, NetworkStatusChanged handler)
        {
            _aegisClient.NetworkEvent_Connected += OnConnect;
            _aegisClient.NetworkEvent_Disconnected += OnDisconnect;
            _aegisClient.NetworkEvent_Received += OnReceive;
            _aegisClient.PacketValidator = IsValidPacket;
            _aegisClient.Initialize();

            _aegisClient.HostAddress = hostAddress;
            _aegisClient.HostPortNo = hostPortNo;
            _aegisClient.EnableSend = false;

            _aesIV = aesIV;
            _aesKey = aesKey;
            _handlerNetworkStatus = handler;

            _callbackQueue.NoMatchesPacket = OnNoMatchesPacket;

            _nextSeqNo = 1;
            _userNo = 0;

            //ConnectionAliveTime = 3000;
        }


        public void Release()
        {
            _aegisClient.Release();
            _callbackQueue.Clear();
        }


        public void Disconnect()
        {
            _aegisClient.Close();
        }


        public void Update()
        {
            _callbackQueue.DoCallback();
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
            OnNetworkStatusChanged(NetworkStatus.Disconnected);
            _aegisClient.EnableSend = false;
        }


        private void OnReceive(StreamBuffer buffer)
        {
            SecurePacket packet = new SecurePacket(buffer);
            packet.Decrypt(_aesIV, _aesKey);


            if (packet.PacketId == Protocol.CS_Hello_Ntf)
                _aegisClient.EnableSend = true;

            if (packet.PacketId == Protocol.CS_ForceClosing_Ntf)
                OnNetworkStatusChanged(NetworkStatus.SessionForceClosed);

            else
            {
                if (packet.PacketId == Protocol.CS_Auth_LoginGuest_Res ||
                    packet.PacketId == Protocol.CS_Auth_LoginMember_Res)
                {
                    packet.SkipHeader();

                    Int32 result = packet.GetInt32();
                    if (result == ResultCode.Ok)
                        _userNo = packet.GetInt32();
                }

                _callbackQueue.AddPacket(packet);
            }
        }


        private void OnNoMatchesPacket(SecurePacket packet)
        {
            packet.SkipHeader();

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
                Int32 seqNo = _nextSeqNo++;
                packet.SeqNo = seqNo;
                packet.Encrypt(_aesIV, _aesKey);

                _callbackQueue.AddCallback(seqNo, responseAction);
                _aegisClient.SendPacket(packet);

                if (_nextSeqNo == Int32.MaxValue)
                    _nextSeqNo = 0;
            }
        }
    }
}