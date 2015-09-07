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
        //  Storage
        private ResultHandler _cloudsheetResultHandler;
        private Int32 _cloudsheetRequestedTableNo;
        private String _cloudsheetFilename;
        public CloudSheet.Tables CloudSheetTables { get; private set; }





        public void Storage_Text_GetData(PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Storage_Text_GetData_Req);
            reqPacket.PutInt32(_userNo);

            SendPacket(reqPacket, callback);
        }


        public void Storage_Test_SetData(String text, PacketCallbackHandler callback)
        {
            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Storage_Text_SetData_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(text);

            SendPacket(reqPacket, callback);
        }


        public void Storage_Sheet_Refresh(String filename, ResultHandler handler)
        {
            _cloudsheetResultHandler = handler;
            _cloudsheetFilename = filename;

            SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Storage_Sheet_GetTableList_Req);
            reqPacket.PutInt32(_userNo);
            reqPacket.PutStringAsUtf16(_cloudsheetFilename);

            SendPacket(reqPacket, OnRecv_Storage_Sheet_GetTableList);
        }


        private void OnRecv_Storage_Sheet_GetTableList(SecurityPacket resPacket)
        {
            Int32 result = resPacket.GetInt32();
            if (result != ResultCode.Ok)
            {
                if (_cloudsheetResultHandler != null)
                    _cloudsheetResultHandler(result);
                return;
            }


            CloudSheetTables = new Tables();

            Int32 tableCount = resPacket.GetInt32();
            while (tableCount-- > 0)
            {
                String tableName = resPacket.GetStringFromUtf16();
                Int32 recordCount = resPacket.GetInt32();
                Int32 columnCount = resPacket.GetInt32();


                Table table = CloudSheetTables.CreateTable(tableName, recordCount, columnCount);
                while (columnCount-- > 0)
                {
                    FieldDataType type = (FieldDataType)resPacket.GetInt32();
                    String fieldname = resPacket.GetStringFromUtf16();

                    table.AddField(type, fieldname);
                }
            }


            _cloudsheetRequestedTableNo = 0;
            if (CloudSheetTables.Items.Count() > _cloudsheetRequestedTableNo)
            {
                Table table = CloudSheetTables.Items[_cloudsheetRequestedTableNo];
                SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Storage_Sheet_GetRecords_Req);


                reqPacket.PutInt32(_userNo);
                reqPacket.PutStringAsUtf16(_cloudsheetFilename);
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
                if (_cloudsheetResultHandler != null)
                    _cloudsheetResultHandler(result);
                return;
            }


            try
            {
                String tableName = resPacket.GetStringFromUtf16();
                Boolean hasMore = (resPacket.GetByte() == 1);
                Int32 rowCount = resPacket.GetInt32();
                UInt32 lastRowNo = 0;
                Table table = CloudSheetTables.GetTable(tableName);


                while (rowCount-- > 0)
                {
                    String[] values = new String[table.Columns.Count()];

                    lastRowNo = resPacket.GetUInt32();
                    for (Int32 i = 0; i < table.Columns.Count(); ++i)
                        values[i] = resPacket.GetStringFromUtf16();

                    table.AddRowData(lastRowNo, values);
                }

                if (hasMore)
                {
                    SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Storage_Sheet_GetRecords_Req);
                    reqPacket.PutInt32(_userNo);
                    reqPacket.PutStringAsUtf16(_cloudsheetFilename);
                    reqPacket.PutStringAsUtf16(tableName);
                    reqPacket.PutUInt32(lastRowNo + 1);

                    SendPacket(reqPacket, OnRecv_Storage_Sheet_GetRecords);
                }
                else
                {
                    ++_cloudsheetRequestedTableNo;
                    if (CloudSheetTables.Items.Count() > _cloudsheetRequestedTableNo)
                    {
                        SecurityPacket reqPacket = new SecurityPacket(Protocol.CS_Storage_Sheet_GetRecords_Req);

                        table = CloudSheetTables.Items[_cloudsheetRequestedTableNo];

                        reqPacket.PutInt32(_userNo);
                        reqPacket.PutStringAsUtf16(_cloudsheetFilename);
                        reqPacket.PutStringAsUtf16(table.Name);
                        reqPacket.PutUInt32(0);
                        SendPacket(reqPacket, OnRecv_Storage_Sheet_GetRecords);
                    }
                    else
                    {
                        if (_cloudsheetResultHandler != null)
                            _cloudsheetResultHandler(ResultCode.Ok);
                    }
                }
            }
            catch (Exception)
            {
                if (_cloudsheetResultHandler != null)
                    _cloudsheetResultHandler(ResultCode.UnknownError);
            }
        }
    }
}
