using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;
using Aegis.Client.Network;
using IndieAPI.CloudSheet;



namespace IndieAPI
{
    public static partial class NetworkAPI
    {
        ////////////////////////////////////////////////////////////////////////////////
        //  Authentication
        private static Int32 _userNo;


        public static void Auth_RegisterGuest(string uuid, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Auth_RegisterGuest_Req"));
            reqPacket.PutInt32(0);
            reqPacket.PutStringAsUtf16(uuid);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void Auth_RegisterMember(string uuid, string userId, string userPwd, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Auth_RegisterMember_Req"));
            reqPacket.PutInt32(0);
            reqPacket.PutStringAsUtf16(uuid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void Auth_LoginGuest(string uuid, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Auth_LoginGuest_Req"));
            reqPacket.PutInt32(0);
            reqPacket.PutStringAsUtf16(uuid);

            _request.SendPacket(reqPacket,
                               (resPacket) =>
                               {
                                   Int32 ret = resPacket.GetInt32();
                                   if (ret == ResultCode.Ok)
                                       _userNo = resPacket.GetInt32();

                                   callback(new ResponseBase(resPacket));
                               });
        }


        public static void Auth_LoginMember(string uuid, string userId, string userPwd, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Auth_LoginMember_Req"));
            reqPacket.PutInt32(0);
            reqPacket.PutStringAsUtf16(uuid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            _request.SendPacket(reqPacket,
                               (resPacket) =>
                               {
                                   Int32 ret = resPacket.GetInt32();
                                   if (ret == ResultCode.Ok)
                                       _userNo = resPacket.GetInt32();

                                   callback(new ResponseBase(resPacket));
                               });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Profile
        public static void Profile_GetData(APICallbackHandler<Response_Profile> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Profile_GetData_Req"));
            reqPacket.PutInt32(_userNo);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_Profile(resPacket)); });
        }


        public static void Profile_SetData(string nickname, Int16 level, Int16 exp, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Profile_SetData_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(nickname);
            reqPacket.PutInt16(level);
            reqPacket.PutInt16(exp);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void Profile_GetTextData(APICallbackHandler<Response_Profile_Text> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Profile_Text_GetData_Req"));
            reqPacket.PutInt32(_userNo);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_Profile_Text(resPacket)); });
        }


        public static void Profile_SetTextData(string text, APICallbackHandler<ResponseBase> callback)
        {
            if (text.Length > 32500)
                throw new AegisException("The 'text' length must be less than 32500.");


            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_Profile_Text_SetData_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(text);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  CloudSheet
        private static int _sheetRequestedNo;
        private static string _sheetFilename, _sheetName;
        public static Workbook Workbook { get; private set; }





        public static void Storage_Sheet_Refresh(string filename, APICallbackHandler<ResponseBase> callback)
        {
            _sheetFilename = filename;

            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CloudSheet_GetSheetList_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);

            _request.SendPacket(reqPacket,
                (resPacket) =>
                {
                    ResponseBase response = new ResponseBase(resPacket);
                    if (response.ResultCodeNo != ResultCode.Ok)
                    {
                        callback(response);
                        return;
                    }

                    Workbook = new Workbook();
                    OnRecv_Storage_Sheet_GetSheetList(resPacket, callback);
                });
        }


        private static void OnRecv_Storage_Sheet_GetSheetList(SecurePacket packet, APICallbackHandler<ResponseBase> callback)
        {
            int sheetCount = packet.GetInt32();
            while (sheetCount-- > 0)
            {
                string sheetName = packet.GetStringFromUtf16();
                int recordCount = packet.GetInt32();
                int columnCount = packet.GetInt32();


                Sheet sheet = Workbook.AddSheet(sheetName, recordCount, columnCount);
                while (columnCount-- > 0)
                {
                    FieldDataType type = (FieldDataType)packet.GetInt32();
                    string fieldname = packet.GetStringFromUtf16();

                    sheet.AddField(type, fieldname);
                }
            }


            if (Workbook.Sheets.Count() == 0)
            {
                callback(new ResponseBase(ResultCode.Ok));
                return;
            }


            _sheetRequestedNo = 0;
            _sheetName = Workbook.Sheets[_sheetRequestedNo].Name;


            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CloudSheet_GetRecords_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);
            reqPacket.PutStringAsUtf16(_sheetName);
            reqPacket.PutUInt32(0);

            _request.SendPacket(reqPacket,
                (resPacket) =>
                {
                    ResponseBase response = new ResponseBase(resPacket);
                    if (response.ResultCodeNo != ResultCode.Ok)
                    {
                        callback(response);
                        return;
                    }

                    OnRecv_Storage_Sheet_GetRecords(resPacket, callback);
                });
        }


        private static void OnRecv_Storage_Sheet_GetRecords(SecurePacket packet, APICallbackHandler<ResponseBase> callback)
        {
            try
            {
                Boolean hasMore = (packet.GetByte() == 1);
                int rowCount = packet.GetInt32();
                UInt32 rowNo = 0;
                Sheet table = Workbook.GetSheet(_sheetName);


                while (rowCount-- > 0)
                {
                    string[] values = new string[table.Fields.Count()];

                    rowNo = packet.GetUInt32();
                    for (int i = 0; i < table.Fields.Count(); ++i)
                        values[i] = packet.GetStringFromUtf16();

                    table.AddRowData(rowNo, values);
                }

                if (hasMore)
                {
                    SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CloudSheet_GetRecords_Req"));
                    reqPacket.PutInt32(_userNo);
                    reqPacket.PutStringAsUtf16(_sheetFilename);
                    reqPacket.PutStringAsUtf16(_sheetName);
                    reqPacket.PutUInt32(rowNo + 1);

                    _request.SendPacket(reqPacket,
                        (resPacket) =>
                        {
                            ResponseBase response = new ResponseBase(resPacket);
                            if (response.ResultCodeNo != ResultCode.Ok)
                            {
                                callback(response);
                                return;
                            }

                            OnRecv_Storage_Sheet_GetRecords(resPacket, callback);
                        });
                }
                else
                {
                    ++_sheetRequestedNo;
                    if (Workbook.Sheets.Count() > _sheetRequestedNo)
                    {
                        table = Workbook.Sheets[_sheetRequestedNo];
                        _sheetName = table.Name;

                        SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CloudSheet_GetRecords_Req"));
                        reqPacket.PutInt32(_userNo);
                        reqPacket.PutStringAsUtf16(_sheetFilename);
                        reqPacket.PutStringAsUtf16(_sheetName);
                        reqPacket.PutUInt32(0);

                        _request.SendPacket(reqPacket,
                            (resPacket) =>
                            {
                                ResponseBase response = new ResponseBase(resPacket);
                                if (response.ResultCodeNo != ResultCode.Ok)
                                {
                                    callback(response);
                                    return;
                                }

                                OnRecv_Storage_Sheet_GetRecords(resPacket, callback);
                            });
                    }
                    else
                    {
                        callback(new ResponseBase(packet));
                    }
                }
            }
            catch (Exception)
            {
                callback(new ResponseBase(ResultCode.UnknownError));
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Instant Messaging Channel
        public static NotifyPacketHandler<Response_IMC_EnteredUser> IMC_EnteredUser { get; set; }
        public static NotifyPacketHandler<Response_IMC_LeavedUser> IMC_LeavedUser { get; set; }
        public static NotifyPacketHandler<Response_IMC_Message> IMC_Message { get; set; }





        public static void IMC_ChannelList(APICallbackHandler<Response_IMC_ChannelList> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_IMC_ChannelList_Req"));
            reqPacket.PutInt32(_userNo);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_ChannelList(resPacket)); });
        }


        public static void IMC_Create(string channelName, APICallbackHandler<Response_IMC_Create> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_IMC_Create_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(channelName);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_Create(resPacket)); });
        }


        public static void IMC_Enter(int channelNo, APICallbackHandler<Response_IMC_Enter> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_IMC_Enter_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutInt32(channelNo);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_Enter(resPacket)); });
        }


        public static void IMC_Leave(APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_IMC_Leave_Req"));
            reqPacket.PutInt32(_userNo);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void IMC_UserList(APICallbackHandler<Response_IMC_UserList> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_IMC_UserList_Req"));
            reqPacket.PutInt32(_userNo);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_UserList(resPacket)); });
        }


        public static void IMC_SendMessage(int targetUserNo, StreamBuffer data, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_IMC_SendMessage_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutInt32(targetUserNo);
            reqPacket.Write(data.Buffer);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  CacheBox
        public static void CacheBox_SetValue(string key, string value, int durationMinutes, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CacheBox_SetValue_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(key);
            reqPacket.PutStringAsUtf16(value);
            reqPacket.PutInt32(durationMinutes);
            reqPacket.PutDouble(-1);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void CacheBox_SetValue(string key, string value, DateTime expireTime, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CacheBox_SetValue_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(key);
            reqPacket.PutStringAsUtf16(value);
            reqPacket.PutInt32(-1);
            reqPacket.PutDouble(expireTime.ToUniversalTime().ToOADate());

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void CacheBox_SetExpireTime(string key, int durationMinutes, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CacheBox_SetExpireTime_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(key);
            reqPacket.PutInt32(durationMinutes);
            reqPacket.PutDouble(-1);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void CacheBox_SetExpireTime(string key, DateTime expireTime, APICallbackHandler<ResponseBase> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CacheBox_SetExpireTime_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(key);
            reqPacket.PutInt32(-1);
            reqPacket.PutDouble(expireTime.ToUniversalTime().ToOADate());

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new ResponseBase(resPacket)); });
        }


        public static void CacheBox_GetValue(string key, APICallbackHandler<Response_CacheBox_Value> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.GetID("CS_CacheBox_GetValue_Req"));
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(key);

            _request.SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_CacheBox_Value(resPacket)); });
        }
    }
}
