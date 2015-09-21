using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Server.Services;
using Server.Services.UserData;



namespace Server.Session
{
    public partial class ClientSession
    {
        private void OnCS_IMC_ChannelList_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_IMC_ChannelList_Res, 4096);
            resPacket.SeqNo = reqPacket.SeqNo;


            lock (CastChannel.Channels)
            {
                Int32 count = 0, idxCount;


                resPacket.PutInt32(ResultCode.Ok);
                idxCount = resPacket.PutInt32(count);
                foreach (var channel in CastChannel.Channels)
                {
                    resPacket.PutInt32(channel.ChannelNo);
                    resPacket.PutStringAsUtf16(channel.Name);
                    ++count;
                }

                resPacket.OverwriteInt32(idxCount, count);
            }

            SendPacket(resPacket);
        }


        private void OnCS_IMC_Create_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_IMC_Create_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                if (_user.CastChannel != null)
                    throw new AegisException(ResultCode.IMC_InChannel);


                String channelName = reqPacket.GetStringFromUtf16();
                _user.CastChannel = CastChannel.NewChannel(channelName);
                _user.CastChannel.Enter(_user);


                resPacket.PutInt32(ResultCode.Ok);
                resPacket.PutInt32(_user.CastChannel.ChannelNo);
                resPacket.PutStringAsUtf16(_user.CastChannel.Name);
            }
            catch (AegisException e)
            {
                resPacket.Clear();
                resPacket.PutInt32(e.ResultCodeNo);
            }


            SendPacket(resPacket);
        }


        private void OnCS_IMC_Enter_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_IMC_Enter_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                if (_user.CastChannel != null)
                    throw new AegisException(ResultCode.IMC_InChannel);


                Int32 channelNo = reqPacket.GetInt32();
                _user.CastChannel = CastChannel.GetChannel(channelNo);


                _user.CastChannel.Enter(_user);
                {
                    SecurePacket ntfPacket = new SecurePacket(Protocol.CS_IMC_EnteredUser_Ntf);
                    ntfPacket.PutInt32(ResultCode.Ok);
                    ntfPacket.PutInt32(_user.UserNo);
                    ntfPacket.PutStringAsUtf16(_user.Profile.Nickname);

                    _user.CastChannel.Broadcast(ntfPacket, _user);
                }


                resPacket.PutInt32(ResultCode.Ok);
                resPacket.PutInt32(_user.CastChannel.ChannelNo);
                resPacket.PutStringAsUtf16(_user.CastChannel.Name);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }


            SendPacket(resPacket);
        }


        private void OnCS_IMC_Leave_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_IMC_Leave_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                if (_user.CastChannel == null)
                    throw new AegisException(ResultCode.IMC_NotInChannel);


                _user.CastChannel.Leave(_user);
                {
                    SecurePacket ntfPacket = new SecurePacket(Protocol.CS_IMC_LeavedUser_Ntf);
                    ntfPacket.PutInt32(ResultCode.Ok);
                    ntfPacket.PutInt32(_user.UserNo);

                    _user.CastChannel.Broadcast(ntfPacket, _user);
                }


                _user.CastChannel = null;
                resPacket.PutInt32(ResultCode.Ok);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }


            SendPacket(resPacket);
        }


        private void OnCS_IMC_UserList_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_IMC_UserList_Res, 4096);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                Int32 count = 0, idxCount;


                resPacket.PutInt32(ResultCode.Ok);
                idxCount = resPacket.PutInt32(count);

                using (_user.CastChannel.ReaderLock)
                {
                    foreach (var user in _user.CastChannel.Users)
                    {
                        resPacket.PutInt32(user.UserNo);
                        resPacket.PutStringAsUtf16(user.Profile.Nickname);
                        ++count;
                    }
                }


                resPacket.OverwriteInt32(idxCount, count);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }


            SendPacket(resPacket);
        }


        private void OnCS_IMC_SendMessage_Req(SecurePacket reqPacket)
        {
            SecurePacket resPacket = new SecurePacket(Protocol.CS_IMC_SendMessage_Res);
            Int32 targetUserNo = reqPacket.GetInt32();
            resPacket.SeqNo = reqPacket.SeqNo;


            if (_user.CastChannel == null)
            {
                resPacket.PutInt32(ResultCode.IMC_NotInChannel);
                SendPacket(resPacket);
                return;
            }


            try
            {
                SecurePacket ntfPacket = new SecurePacket(Protocol.CS_IMC_Message_Ntf, (UInt16)reqPacket.ReadableSize);
                ntfPacket.PutInt32(ResultCode.Ok);
                ntfPacket.PutInt32(_user.UserNo);
                ntfPacket.Write(reqPacket.Buffer, reqPacket.ReadBytes, reqPacket.ReadableSize);


                if (targetUserNo != 0)
                {
                    User targetUser = _user.CastChannel.GetUser(targetUserNo);
                    targetUser.SendPacket(ntfPacket);
                }
                else
                    _user.CastChannel.Broadcast(ntfPacket);


                resPacket.PutInt32(ResultCode.Ok);
            }
            catch (AegisException e)
            {
                resPacket.PutInt32(e.ResultCodeNo);
            }


            SendPacket(resPacket);
        }
    }
}
