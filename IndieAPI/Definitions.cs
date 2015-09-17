using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace IndieAPI
{
    public delegate void NetworkStatusChanged(NetworkStatus status);
    public delegate void NotifyPacketHandler<T>(T response);



    public enum NetworkStatus
    {
        Connected = 1,
        ConnectionFailed,
        Disconnected,
        SessionForceClosed
    }





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
        public const UInt16 CS_IMC_ChannelStatus_Ntf = 0x2305;
        public const UInt16 CS_IMC_Enter_Req = 0x2306;
        public const UInt16 CS_IMC_Enter_Res = 0x2307;
        public const UInt16 CS_IMC_EnteredUser_Ntf = 0x2308;
        public const UInt16 CS_IMC_Leave_Req = 0x2309;
        public const UInt16 CS_IMC_Leave_Res = 0x230A;
        public const UInt16 CS_IMC_LeavedUser_Ntf = 0x230B;

        public const UInt16 CS_IMC_UserList_Req = 0x230C;
        public const UInt16 CS_IMC_UserList_Res = 0x230D;
        public const UInt16 CS_IMC_SendToAny_Req = 0x230E;
        public const UInt16 CS_IMC_SendToAny_Res = 0x230F;
        public const UInt16 CS_IMC_SendToOne_Req = 0x2310;
        public const UInt16 CS_IMC_SendToOne_Res = 0x2311;
        public const UInt16 CS_IMC_Message_Ntf = 0x2312;
    }


    public static class ResultCode
    {
        public const Int32 Ok = 0;
        public const Int32 UnknownError = 1;

        public const Int32 AlreadyExistsUDID = 1001;
        public const Int32 AlreadyExistsUserId = 1002;

        public const Int32 InvalidUDID = 1003;
        public const Int32 InvalidUserId = 1004;


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

        public const Int32 CastChannel_InvalidChannelNo = 1301;
        public const Int32 CastChannel_ExistsName = 1302;
        public const Int32 CastChannel_ExistsUser = 1303;
        public const Int32 CastChannel_NotExistsUser = 1304;
        public const Int32 CastChannel_InChannel = 1305;
        public const Int32 CastChannel_NotInChannel = 1306;





        public static String ToString(Int32 resultCode)
        {
            switch (resultCode)
            {
                case Ok: return "Ok";
                case AlreadyExistsUDID: return "Already exists UDID";
                case AlreadyExistsUserId: return "Already exists UserId";
                case InvalidUDID: return "Invalid UDID";
                case InvalidUserId: return "Invalid UserId";

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

                case CastChannel_InvalidChannelNo: return "Invalid ChannelNo.";
                case CastChannel_ExistsName: return "Already exists channel name.";
                case CastChannel_ExistsUser: return "Already exists user.";
                case CastChannel_NotExistsUser: return "Not exists user.";
                case CastChannel_InChannel: return "Cannot process because you're in channel.";
                case CastChannel_NotInChannel: return "Cannot process because you're not in channel.";
            }

            return String.Format($"Unknown ResultCode(0x:{resultCode:X})");
        }
    }
}
