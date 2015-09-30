using System;
using System.Reflection;



namespace Server
{
    public static class Protocol
    {
        public const UInt16 CS_Hello_Ntf = 0x1000;
        public const UInt16 CS_ForceClosing_Ntf = 0x1001;


        //  Authentication
        public const UInt16 CS_Auth_RegisterGuest_Req = 0x2001;
        public const UInt16 CS_Auth_RegisterGuest_Res = 0x2002;
        public const UInt16 CS_Auth_RegisterMember_Req = 0x2003;
        public const UInt16 CS_Auth_RegisterMember_Res = 0x2004;

        public const UInt16 CS_Auth_LoginGuest_Req = 0x2005;
        public const UInt16 CS_Auth_LoginGuest_Res = 0x2006;
        public const UInt16 CS_Auth_LoginMember_Req = 0x2007;
        public const UInt16 CS_Auth_LoginMember_Res = 0x2008;


        //  User Profile
        public const UInt16 CS_Profile_GetData_Req = 0x2101;
        public const UInt16 CS_Profile_GetData_Res = 0x2102;
        public const UInt16 CS_Profile_SetData_Req = 0x2103;
        public const UInt16 CS_Profile_SetData_Res = 0x2104;
        public const UInt16 CS_Profile_Text_GetData_Req = 0x2105;
        public const UInt16 CS_Profile_Text_GetData_Res = 0x2106;
        public const UInt16 CS_Profile_Text_SetData_Req = 0x2107;
        public const UInt16 CS_Profile_Text_SetData_Res = 0x2108;


        //  CloudSheet
        public const UInt16 CS_CloudSheet_GetSheetList_Req = 0x2211;
        public const UInt16 CS_CloudSheet_GetSheetList_Res = 0x2212;
        public const UInt16 CS_CloudSheet_GetRecords_Req = 0x2213;
        public const UInt16 CS_CloudSheet_GetRecords_Res = 0x2214;


        //  Instant Messaging Channel
        public const UInt16 CS_IMC_ChannelList_Req = 0x2301;
        public const UInt16 CS_IMC_ChannelList_Res = 0x2302;
        public const UInt16 CS_IMC_Create_Req = 0x2303;
        public const UInt16 CS_IMC_Create_Res = 0x2304;
        public const UInt16 CS_IMC_Enter_Req = 0x2305;
        public const UInt16 CS_IMC_Enter_Res = 0x2306;
        public const UInt16 CS_IMC_EnteredUser_Ntf = 0x2307;
        public const UInt16 CS_IMC_Leave_Req = 0x2308;
        public const UInt16 CS_IMC_Leave_Res = 0x2309;
        public const UInt16 CS_IMC_LeavedUser_Ntf = 0x230A;

        public const UInt16 CS_IMC_UserList_Req = 0x230B;
        public const UInt16 CS_IMC_UserList_Res = 0x230C;
        public const UInt16 CS_IMC_SendMessage_Req = 0x230D;
        public const UInt16 CS_IMC_SendMessage_Res = 0x230E;
        public const UInt16 CS_IMC_Message_Ntf = 0x230F;


        //  CacheBox
        public const UInt16 CS_CacheBox_SetValue_Req = 0x2401;
        public const UInt16 CS_CacheBox_SetValue_Res = 0x2402;
        public const UInt16 CS_CacheBox_SetExpireTime_Req = 0x2403;
        public const UInt16 CS_CacheBox_SetExpireTime_Res = 0x2404;
        public const UInt16 CS_CacheBox_GetValue_Req = 0x2405;
        public const UInt16 CS_CacheBox_GetValue_Res = 0x2406;
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
    }
}
