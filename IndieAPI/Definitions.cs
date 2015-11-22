using System;
using System.Collections.Generic;



namespace IndieAPI
{
    public delegate void NotifyPacketHandler<T>(T response);





    public static class Protocol
    {
        private static readonly Dictionary<UInt16, String> _ids = new Dictionary<UInt16, String>()
        {
            {0x1000, "CS_Hello_Ntf"},
            {0x1001, "CS_ForceClosing_Ntf"},


            //  Authentication
            {0x2001, "CS_Auth_RegisterGuest_Req"}, {0x2002, "CS_Auth_RegisterGuest_Res"},
            {0x2003, "CS_Auth_RegisterMember_Req"}, {0x2004, "CS_Auth_RegisterMember_Res"},
            {0x2005, "CS_Auth_LoginGuest_Req"}, {0x2006, "CS_Auth_LoginGuest_Res"},
            {0x2007, "CS_Auth_LoginMember_Req"}, {0x2008, "CS_Auth_LoginMember_Res"},


            //  User Profile
            {0x2101, "CS_Profile_GetData_Req"}, {0x2102, "CS_Profile_GetData_Res"},
            {0x2103, "CS_Profile_SetData_Req"}, {0x2104, "CS_Profile_SetData_Res"},
            {0x2105, "CS_Profile_Text_GetData_Req"}, {0x2106, "CS_Profile_Text_GetData_Res"},
            {0x2107, "CS_Profile_Text_SetData_Req"}, {0x2108, "CS_Profile_Text_SetData_Res"},


            //  CloudSheet
            {0x2211, "CS_CloudSheet_GetSheetList_Req"}, {0x2212, "CS_CloudSheet_GetSheetList_Res"},
            {0x2213, "CS_CloudSheet_GetRecords_Req"}, {0x2214, "CS_CloudSheet_GetRecords_Res"},


            //  Instant Messaging Channel
            {0x2301, "CS_IMC_ChannelList_Req"}, {0x2302, "CS_IMC_ChannelList_Res"},
            {0x2303, "CS_IMC_Create_Req"}, {0x2304, "CS_IMC_Create_Res"},
            {0x2305, "CS_IMC_Enter_Req"}, {0x2306, "CS_IMC_Enter_Res"},
            {0x2307, "CS_IMC_EnteredUser_Ntf"},
            {0x2308, "CS_IMC_Leave_Req"}, {0x2309, "CS_IMC_Leave_Res"},
            {0x230A, "CS_IMC_LeavedUser_Ntf"},
            {0x230B, "CS_IMC_UserList_Req"}, {0x230C, "CS_IMC_UserList_Res"},
            {0x230D, "CS_IMC_SendMessage_Req"}, {0x230E, "CS_IMC_SendMessage_Res"},
            {0x230F, "CS_IMC_Message_Ntf"},


            //  CacheBox
            {0x2401, "CS_CacheBox_SetValue_Req"}, {0x2402, "CS_CacheBox_SetValue_Res"},
            {0x2403, "CS_CacheBox_SetExpireTime_Req"}, {0x2404, "CS_CacheBox_SetExpireTime_Res"},
            {0x2405, "CS_CacheBox_GetValue_Req"}, {0x2406, "CS_CacheBox_GetValue_Res"},
        };
        public static UInt16 GetID(String name)
        {
            foreach (var id in _ids)
            {
                if (id.Value == name)
                    return id.Key;
            }

            throw new Exception(String.Format("Invalid protocol name({0}).", name));
        }
        public static String GetName(UInt16 id)
        {
            String name;
            if (_ids.TryGetValue(id, out name) == true)
                return name;

            throw new Exception(String.Format("Invalid protocol id(0x{0:X}).", id));
        }
    }


    public static class ResultCode
    {
        public const Int32 Ok = 0;
        public const Int32 UnknownError = 1;

        public const Int32 InvalidPacketSeqNo = 1001;
        public const Int32 InvalidUserNo = 1002;
        public const Int32 AlreadyExistsUUID = 1011;
        public const Int32 AlreadyExistsUserId = 1012;
        public const Int32 InvalidUUID = 1013;
        public const Int32 InvalidUserId = 1014;


        public const Int32 InvalidFileType = 1101;
        public const Int32 InvalidFileName = 1102;
        public const Int32 InvalidTableName = 1103;

        public const Int32 CloudSheet_NoDataInSheet = 1201;
        public const Int32 CloudSheet_ColumnCountIsNotMatch = 1202;
        public const Int32 CloudSheet_EmptyColumnName = 1203;
        public const Int32 CloudSheet_DuplicateColumnName = 1204;
        public const Int32 CloudSheet_TooManyColumns = 1205;
        public const Int32 CloudSheet_TooManyRecords = 1206;
        public const Int32 CloudSheet_TooBigFileSize = 1207;

        public const Int32 IMC_InvalidChannelNo = 1301;
        public const Int32 IMC_ExistsChannelName = 1302;
        public const Int32 IMC_ExistsUser = 1303;
        public const Int32 IMC_NotExistsUser = 1304;
        public const Int32 IMC_InChannel = 1305;
        public const Int32 IMC_NotInChannel = 1306;

        public const Int32 CacheBox_TooLongKey = 1401;
        public const Int32 CacheBox_TooLongValue = 1402;
        public const Int32 CacheBox_InvalidKey = 1403;





        public static String ToString(Int32 resultCode)
        {
            switch (resultCode)
            {
                case Ok: return "Ok";

                case InvalidPacketSeqNo: return "Invalid Packet SequenceNo.";
                case InvalidUserNo: return "Invalid UserNo.";
                case AlreadyExistsUUID: return "Already exists UUID.";
                case AlreadyExistsUserId: return "Already exists UserId.";
                case InvalidUUID: return "Invalid UUID.";
                case InvalidUserId: return "Invalid UserId.";

                case InvalidFileType: return "The file is not xlsx file.";
                case InvalidFileName: return "Invalid filename.";
                case InvalidTableName: return "Invalid table name.";

                case CloudSheet_NoDataInSheet: return "There is no data in sheet.";
                case CloudSheet_ColumnCountIsNotMatch: return "Column count is not match.";
                case CloudSheet_EmptyColumnName: return "There is empty column name.";
                case CloudSheet_DuplicateColumnName: return "There is same column names.";
                case CloudSheet_TooManyColumns: return "Too many columns defined.";
                case CloudSheet_TooManyRecords: return "Too many records in sheet.";
                case CloudSheet_TooBigFileSize: return "File size too big.";

                case IMC_InvalidChannelNo: return "Invalid ChannelNo.";
                case IMC_ExistsChannelName: return "Already exists channel name.";
                case IMC_ExistsUser: return "Already exists user.";
                case IMC_NotExistsUser: return "Not exists user.";
                case IMC_InChannel: return "Cannot process because you're in channel.";
                case IMC_NotInChannel: return "Cannot process because you're not in channel.";

                case CacheBox_TooLongKey: return "Key is too long.";
                case CacheBox_TooLongValue: return "Value is too long.";
                case CacheBox_InvalidKey: return "Invalid Key.";
            }

            return String.Format($"Unknown ResultCode(0x:{resultCode:X})");
        }
    }
}
