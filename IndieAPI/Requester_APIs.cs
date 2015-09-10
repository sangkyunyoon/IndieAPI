using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aegis.Client;
using IndieAPI.Sheet;



namespace IndieAPI
{
    public partial class Requester
    {
        ////////////////////////////////////////////////////////////////////////////////
        //  Authentication
        public void Auth_RegisterGuest(String udid, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_RegisterGuest_Req);
            reqPacket.PutStringAsUtf16(udid);

            SendPacket(reqPacket, callback);
        }


        public void Auth_RegisterMember(String udid, String userId, String userPwd, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_RegisterMember_Req);
            reqPacket.PutStringAsUtf16(udid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            SendPacket(reqPacket, callback);
        }


        public void Auth_LoginGuest(String udid, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_LoginGuest_Req);
            reqPacket.PutStringAsUtf16(udid);

            SendPacket(reqPacket, callback);
        }


        public void Auth_LoginMember(String udid, String userId, String userPwd, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Auth_LoginMember_Req);
            reqPacket.PutStringAsUtf16(udid);
            reqPacket.PutStringAsUtf16(userId);
            reqPacket.PutStringAsUtf16(userPwd);

            SendPacket(reqPacket, callback);
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Profile
        public void Profile_GetData(PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket, callback);
        }


        public void Profile_SetData(String nickname, Int16 level, Int16 exp, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(nickname);
            reqPacket.PutInt16(level);
            reqPacket.PutInt16(exp);

            SendPacket(reqPacket, callback);
        }


        public void Profile_GetTextData(PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_Text_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket, callback);
        }


        public void Profile_SetTextData(String text, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Profile_Text_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(text);

            SendPacket(reqPacket, callback);
        }


        ////////////////////////////////////////////////////////////////////////////////
        //  Sheet
        private ResultHandler _sheetResultHandler;
        private Int32 _sheetRequestedTableNo;
        private String _sheetFilename;
        public Tables SheetTables { get; private set; }





        public void Storage_Sheet_Refresh(String filename, ResultHandler handler)
        {
            _sheetResultHandler = handler;
            _sheetFilename = filename;

            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Sheet_GetTableList_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_sheetFilename);

            SendPacket(reqPacket, OnRecv_Storage_Sheet_GetTableList);
        }


        private void OnRecv_Storage_Sheet_GetTableList(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            if (result != ResultCode.Ok)
            {
                if (_sheetResultHandler != null)
                    _sheetResultHandler(result);
                return;
            }


            SheetTables = new Tables();

            Int32 tableCount = resPacket.GetInt32();
            while (tableCount-- > 0)
            {
                String tableName = resPacket.GetStringFromUtf16();
                Int32 recordCount = resPacket.GetInt32();
                Int32 columnCount = resPacket.GetInt32();


                Table table = SheetTables.CreateTable(tableName, recordCount, columnCount);
                while (columnCount-- > 0)
                {
                    FieldDataType type = (FieldDataType)resPacket.GetInt32();
                    String fieldname = resPacket.GetStringFromUtf16();

                    table.AddField(type, fieldname);
                }
            }


            _sheetRequestedTableNo = 0;
            if (SheetTables.Items.Count() > _sheetRequestedTableNo)
            {
                Table table = SheetTables.Items[_sheetRequestedTableNo];
                SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Sheet_GetRecords_Req);


                reqPacket.PutInt32(_userNo);
                reqPacket.PutStringAsUtf16(_sheetFilename);
                reqPacket.PutStringAsUtf16(table.Name);
                reqPacket.PutUInt32(0);
                SendPacket(reqPacket, OnRecv_Storage_Sheet_GetRecords);
            }
        }


        private void OnRecv_Storage_Sheet_GetRecords(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            if (result != ResultCode.Ok)
            {
                if (_sheetResultHandler != null)
                    _sheetResultHandler(result);
                return;
            }


            try
            {
                String tableName = resPacket.GetStringFromUtf16();
                Boolean hasMore = (resPacket.GetByte() == 1);
                Int32 rowCount = resPacket.GetInt32();
                UInt32 lastRowNo = 0;
                Table table = SheetTables.GetTable(tableName);


                while (rowCount-- > 0)
                {
                    String[] values = new String[table.Fields.Count()];

                    lastRowNo = resPacket.GetUInt32();
                    for (Int32 i = 0; i < table.Fields.Count(); ++i)
                        values[i] = resPacket.GetStringFromUtf16();

                    table.AddRowData(lastRowNo, values);
                }

                if (hasMore)
                {
                    SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Sheet_GetRecords_Req);
                    reqPacket.PutInt32(_userNo);
                    reqPacket.PutStringAsUtf16(_sheetFilename);
                    reqPacket.PutStringAsUtf16(tableName);
                    reqPacket.PutUInt32(lastRowNo + 1);

                    SendPacket(reqPacket, OnRecv_Storage_Sheet_GetRecords);
                }
                else
                {
                    ++_sheetRequestedTableNo;
                    if (SheetTables.Items.Count() > _sheetRequestedTableNo)
                    {
                        SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Sheet_GetRecords_Req);

                        table = SheetTables.Items[_sheetRequestedTableNo];

                        reqPacket.PutInt32(_userNo);
                        reqPacket.PutStringAsUtf16(_sheetFilename);
                        reqPacket.PutStringAsUtf16(table.Name);
                        reqPacket.PutUInt32(0);
                        SendPacket(reqPacket, OnRecv_Storage_Sheet_GetRecords);
                    }
                    else
                    {
                        if (_sheetResultHandler != null)
                            _sheetResultHandler(ResultCode.Ok);
                    }
                }
            }
            catch (Exception)
            {
                if (_sheetResultHandler != null)
                    _sheetResultHandler(ResultCode.UnknownError);
            }
        }
    }
}
