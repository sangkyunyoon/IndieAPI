using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;
using IndieAPI.CloudSheet;



namespace IndieAPI
{
    public partial class Request
    {
        ////////////////////////////////////////////////////////////////////////////////
        //  Authentication
        public void Auth_RegisterGuest(string uuid, APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Auth_RegisterGuest_Req);
            reqPacket.PutStringAsUtf16(uuid);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        public void Auth_RegisterMember(string uuid, string userId, string userPwd, APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Auth_RegisterMember_Req);
            reqPacket.PutStringAsUtf16(uuid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        public void Auth_LoginGuest(string uuid, APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Auth_LoginGuest_Req);
            reqPacket.PutStringAsUtf16(uuid);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        public void Auth_LoginMember(string uuid, string userId, string userPwd, APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Auth_LoginMember_Req);
            reqPacket.PutStringAsUtf16(uuid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Profile
        public void Profile_GetData(APICallbackHandler<Response_Profile> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Profile_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_Profile(resPacket)); });
        }


        public void Profile_SetData(string nickname, Int16 level, Int16 exp, APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Profile_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(nickname);
            reqPacket.PutInt16(level);
            reqPacket.PutInt16(exp);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        public void Profile_GetTextData(APICallbackHandler<Response_Profile_Text> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Profile_Text_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_Profile_Text(resPacket)); });
        }


        public void Profile_SetTextData(string text, APICallbackHandler<Response> callback)
        {
            if (text.Length > 32500)
                throw new AegisException("The 'text' length must be less than 32500.");


            SecurePacket reqPacket = new SecurePacket(Protocol.CS_Profile_Text_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(text);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  CloudSheet
        private int _sheetRequestedNo;
        private string _sheetFilename, _sheetName;
        public Workbook Workbook { get; private set; }





        public void Storage_Sheet_Refresh(string filename, APICallbackHandler<Response> callback)
        {
            _sheetFilename = filename;

            SecurePacket reqPacket = new SecurePacket(Protocol.CS_CloudSheet_GetSheetList_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    Response response = new Response(resPacket);
                    if (response.ResultCodeNo != ResultCode.Ok)
                    {
                        callback(response);
                        return;
                    }

                    Workbook = new Workbook();
                    OnRecv_Storage_Sheet_GetSheetList(resPacket, callback);
                });
        }


        private void OnRecv_Storage_Sheet_GetSheetList(SecurePacket packet, APICallbackHandler<Response> callback)
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
                callback(new Response(ResultCode.Ok));
                return;
            }


            _sheetRequestedNo = 0;
            _sheetName = Workbook.Sheets[_sheetRequestedNo].Name;


            SecurePacket reqPacket = new SecurePacket(Protocol.CS_CloudSheet_GetRecords_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);
            reqPacket.PutStringAsUtf16(_sheetName);
            reqPacket.PutUInt32(0);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    Response response = new Response(resPacket);
                    if (response.ResultCodeNo != ResultCode.Ok)
                    {
                        callback(response);
                        return;
                    }

                    OnRecv_Storage_Sheet_GetRecords(resPacket, callback);
                });
        }


        private void OnRecv_Storage_Sheet_GetRecords(SecurePacket packet, APICallbackHandler<Response> callback)
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
                    SecurePacket reqPacket = new SecurePacket(Protocol.CS_CloudSheet_GetRecords_Req);
                    reqPacket.PutInt32(_userNo);
                    reqPacket.PutStringAsUtf16(_sheetFilename);
                    reqPacket.PutStringAsUtf16(_sheetName);
                    reqPacket.PutUInt32(rowNo + 1);

                    SendPacket(reqPacket,
                        (resPacket) =>
                        {
                            Response response = new Response(resPacket);
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

                        SecurePacket reqPacket = new SecurePacket(Protocol.CS_CloudSheet_GetRecords_Req);
                        reqPacket.PutInt32(_userNo);
                        reqPacket.PutStringAsUtf16(_sheetFilename);
                        reqPacket.PutStringAsUtf16(_sheetName);
                        reqPacket.PutUInt32(0);

                        SendPacket(reqPacket,
                            (resPacket) =>
                            {
                                Response response = new Response(resPacket);
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
                        callback(new Response(packet));
                    }
                }
            }
            catch (Exception)
            {
                callback(new Response(ResultCode.UnknownError));
            }
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Instant Messaging Channel
        public NotifyPacketHandler<Response_IMC_EnteredUser> IMC_EnteredUser { get; set; }
        public NotifyPacketHandler<Response_IMC_LeavedUser> IMC_LeavedUser { get; set; }
        public NotifyPacketHandler<Response_IMC_Message> IMC_Message { get; set; }





        public void IMC_ChannelList(APICallbackHandler<Response_IMC_ChannelList> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_IMC_ChannelList_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_ChannelList(resPacket)); });
        }


        public void IMC_Create(string channelName, APICallbackHandler<Response_IMC_Create> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_IMC_Create_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(channelName);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_Create(resPacket)); });
        }


        public void IMC_Enter(int channelNo, APICallbackHandler<Response_IMC_Enter> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_IMC_Enter_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutInt32(channelNo);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_Enter(resPacket)); });
        }


        public void IMC_Leave(APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_IMC_Leave_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }


        public void IMC_UserList(APICallbackHandler<Response_IMC_UserList> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_IMC_UserList_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response_IMC_UserList(resPacket)); });
        }


        public void IMC_SendMessage(int targetUserNo, StreamBuffer data, APICallbackHandler<Response> callback)
        {
            SecurePacket reqPacket = new SecurePacket(Protocol.CS_IMC_SendMessage_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutInt32(targetUserNo);
            reqPacket.Write(data.Buffer);

            SendPacket(reqPacket,
                       (resPacket) => { callback(new Response(resPacket)); });
        }
    }
}
