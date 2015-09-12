using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace IndieAPI
{
    public delegate void NetworkStatusChanged(NetworkStatus status);



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
    }


    public static class ResultCode
    {
        public const Int32 Ok = 0;
        public const Int32 UnknownError = 1;

        public const Int32 AlreadyExistsUDID = 11;
        public const Int32 AlreadyExistsUserId = 12;

        public const Int32 InvalidUDID = 13;
        public const Int32 InvalidUserId = 14;


        public const Int32 InvalidFileType = 101;
        public const Int32 InvalidFileName = 102;
        public const Int32 InvalidTableName = 103;

        public const Int32 NoDataInSheet = 106;
        public const Int32 ColumnCountIsNotMatch = 107;
        public const Int32 EmptyColumnName = 108;
        public const Int32 DuplicateColumnName = 109;
        public const Int32 TooManyColumns = 111;
        public const Int32 TooManyRecords = 112;
        public const Int32 TooBigFileSize = 113;





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

                case NoDataInSheet: return "There is no data in sheet.";
                case ColumnCountIsNotMatch: return "Column count is not match.";
                case EmptyColumnName: return "There is empty column name.";
                case DuplicateColumnName: return "There is same column names.";
                case TooManyColumns: return "Too many columns defined.";
                case TooManyRecords: return "Too many records in sheet.";
                case TooBigFileSize: return "File size too big.";
            }

            return String.Format($"Unknown ResultCode(0x:{resultCode:X})");
        }
    }
}
