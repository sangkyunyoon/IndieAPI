using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;
using IndieAPI.CloudSheet;



namespace IndieAPI
{
    public partial class Requester
    {
        ////////////////////////////////////////////////////////////////////////////////
        //  Authentication
        public void Auth_RegisterGuest(String udid, APICallbackHandler<ResponseData> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_RegisterGuest_Req);
            reqPacket.PutStringAsUtf16(udid);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new ResponseData(resPacket));
                });
        }


        public void Auth_RegisterMember(String udid, String userId, String userPwd, APICallbackHandler<ResponseData> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_RegisterMember_Req);
            reqPacket.PutStringAsUtf16(udid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new ResponseData(resPacket));
                });
        }


        public void Auth_LoginGuest(String udid, APICallbackHandler<ResponseData> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_LoginGuest_Req);
            reqPacket.PutStringAsUtf16(udid);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new ResponseData(resPacket));
                });
        }


        public void Auth_LoginMember(String udid, String userId, String userPwd, APICallbackHandler<ResponseData> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_LoginMember_Req);
            reqPacket.PutStringAsUtf16(udid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new ResponseData(resPacket));
                });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Profile
        public void Profile_GetData(APICallbackHandler<Response_Profile> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new Response_Profile(resPacket));
                });
        }


        public void Profile_SetData(String nickname, Int16 level, Int16 exp, APICallbackHandler<ResponseData> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(nickname);
            reqPacket.PutInt16(level);
            reqPacket.PutInt16(exp);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new ResponseData(resPacket));
                });
        }


        public void Profile_GetTextData(APICallbackHandler<Response_Profile_Text> callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_Text_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new Response_Profile_Text(resPacket));
                });
        }


        public void Profile_SetTextData(String text, APICallbackHandler<ResponseData> callback)
        {
            if (text.Length > 32500)
                throw new AegisException("The 'text' length must be less than 32500.");


            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_Text_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(text);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    callback(new ResponseData(resPacket));
                });
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  CloudSheet
        private Int32 _sheetRequestedNo;
        private String _sheetFilename, _sheetName;
        public Workbook Workbook { get; private set; }





        public void Storage_Sheet_Refresh(String filename, APICallbackHandler<ResponseData> callback)
        {
            _sheetFilename = filename;

            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_CloudSheet_GetSheetList_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    ResponseData response = new ResponseData(resPacket);
                    if (response.ResultCodeNo != ResultCode.Ok)
                    {
                        callback(response);
                        return;
                    }

                    Workbook = new Workbook();
                    OnRecv_Storage_Sheet_GetSheetList(resPacket, callback);
                });
        }


        private void OnRecv_Storage_Sheet_GetSheetList(SecurityPacket packet, APICallbackHandler<ResponseData> callback)
        {
            Int32 sheetCount = packet.GetInt32();
            while (sheetCount-- > 0)
            {
                String sheetName = packet.GetStringFromUtf16();
                Int32 recordCount = packet.GetInt32();
                Int32 columnCount = packet.GetInt32();


                Sheet sheet = Workbook.AddSheet(sheetName, recordCount, columnCount);
                while (columnCount-- > 0)
                {
                    FieldDataType type = (FieldDataType)packet.GetInt32();
                    String fieldname = packet.GetStringFromUtf16();

                    sheet.AddField(type, fieldname);
                }
            }


            if (Workbook.Sheets.Count() == 0)
            {
                callback(new ResponseData(ResultCode.Ok));
                return;
            }


            _sheetRequestedNo = 0;
            _sheetName = Workbook.Sheets[_sheetRequestedNo].Name;


            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_CloudSheet_GetRecords_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);
            reqPacket.PutStringAsUtf16(_sheetName);
            reqPacket.PutUInt32(0);

            SendPacket(reqPacket,
                (resPacket) =>
                {
                    ResponseData response = new ResponseData(resPacket);
                    if (response.ResultCodeNo != ResultCode.Ok)
                    {
                        callback(response);
                        return;
                    }

                    OnRecv_Storage_Sheet_GetRecords(resPacket, callback);
                });
        }


        private void OnRecv_Storage_Sheet_GetRecords(SecurityPacket packet, APICallbackHandler<ResponseData> callback)
        {
            try
            {
                Boolean hasMore = (packet.GetByte() == 1);
                Int32 rowCount = packet.GetInt32();
                UInt32 rowNo = 0;
                Sheet table = Workbook.GetSheet(_sheetName);


                while (rowCount-- > 0)
                {
                    String[] values = new String[table.Fields.Count()];

                    rowNo = packet.GetUInt32();
                    for (Int32 i = 0; i < table.Fields.Count(); ++i)
                        values[i] = packet.GetStringFromUtf16();

                    table.AddRowData(rowNo, values);
                }

                if (hasMore)
                {
                    SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_CloudSheet_GetRecords_Req);
                    reqPacket.PutInt32(_userNo);
                    reqPacket.PutStringAsUtf16(_sheetFilename);
                    reqPacket.PutStringAsUtf16(_sheetName);
                    reqPacket.PutUInt32(rowNo + 1);

                    SendPacket(reqPacket,
                        (resPacket) =>
                        {
                            ResponseData response = new ResponseData(resPacket);
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

                        SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_CloudSheet_GetRecords_Req);
                        reqPacket.PutInt32(_userNo);
                        reqPacket.PutStringAsUtf16(_sheetFilename);
                        reqPacket.PutStringAsUtf16(_sheetName);
                        reqPacket.PutUInt32(0);

                        SendPacket(reqPacket,
                            (resPacket) =>
                            {
                                ResponseData response = new ResponseData(resPacket);
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
                        callback(new ResponseData(packet));
                    }
                }
            }
            catch (Exception)
            {
                callback(new ResponseData(ResultCode.UnknownError));
            }
        }
    }
}
