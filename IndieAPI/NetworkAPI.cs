using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;
using Aegis.Client.Network;



namespace IndieAPI
{
    public static partial class NetworkAPI
    {
        private static Request _request = new Request();
        private static String _aesIV = "1sf_@skKKw#ns(*s";
        private static String _aesKey = "nt_som5slm1NBB3 ";


        public static Int32 ConnectionAliveTime
        {
            get { return _request.ConnectionAliveTime; }
            set { _request.ConnectionAliveTime = value; }
        }
        public static ConnectionStatus ConnectionStatus { get { return _request.ConnectionStatus; } }
        public static event NetworkStatusHandler NetworkStatusChanged;





        public static void Initialize()
        {
            _request.EnableSend = false;
            _request.AESIV = _aesIV;
            _request.AESKey = _aesKey;
            _request.ConnectionAliveTime = 0;
            _request.PacketPreprocessing += OnPacketPreprocessing;
            _request.NetworkStatusChanged += OnNetworkStatusChanged;
            _request.Initialize();
        }


        public static void SetServer(String ipAddress, Int32 portNo)
        {
            _request.HostAddress = ipAddress;
            _request.HostPortNo = portNo;
        }


        public static void Update()
        {
            _request.Update();
        }


        public static void Disconnect()
        {
            _request.Disconnect();
        }


        public static void Release()
        {
            _request.Release();
        }


        private static void OnNetworkStatusChanged(NetworkStatus status)
        {
            if (status == NetworkStatus.Disconnected)
            {
                _request.AESIV = _aesIV;
                _request.AESKey = _aesKey;
                _request.EnableSend = false;
            }

            if (NetworkStatusChanged != null)
                NetworkStatusChanged(status);
        }


        private static bool OnPacketPreprocessing(SecurePacket packet)
        {
            if (packet.PacketId == Protocol.GetID("CS_Hello_Ntf"))
            {
                OnHello(packet);
                return true;
            }
            if (packet.PacketId == Protocol.GetID("CS_ForceClosing_Ntf"))
            {
                OnNetworkStatusChanged(NetworkStatus.SessionForceClosed);
                return true;
            }
            if (packet.PacketId == Protocol.GetID("CS_IMC_EnteredUser_Ntf"))
            {
                if (IMC_EnteredUser != null)
                    IMC_EnteredUser(new Response_IMC_EnteredUser(packet));
            }
            if (packet.PacketId == Protocol.GetID("CS_IMC_LeavedUser_Ntf"))
            {
                if (IMC_LeavedUser != null)
                    IMC_LeavedUser(new Response_IMC_LeavedUser(packet));
            }
            if (packet.PacketId == Protocol.GetID("CS_IMC_Message_Ntf"))
            {
                if (IMC_Message != null)
                    IMC_Message(new Response_IMC_Message(packet));
            }

            return false;
        }


        private static void OnHello(SecurePacket packet)
        {
            Int32 seed = packet.GetInt32();
            String characterSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            char[] ascii = new char[16];


            for (Int32 i = 0; i < 16; ++i)
            {
                Int32 val = seed & (0x6E << i);
                ascii[i] = characterSet[(val % characterSet.Length)];
            }
            _request.AESIV = new String(ascii);


            for (Int32 i = 0; i < 16; ++i)
            {
                Int32 val = seed & (0xF4 << i);
                ascii[i] = characterSet[(val % characterSet.Length)];
            }
            _request.AESKey = new String(ascii);

            _request.EnableSend = true;
        }
    }
}
