using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis.Configuration;



namespace Server
{
    public static class Version
    {
        public const Int32 BuildNo = 1;
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


        //  Storage
        public const UInt16 CS_Storage_Text_GetData_Req = 0x2101;
        public const UInt16 CS_Storage_Text_GetData_Res = 0x2102;
        public const UInt16 CS_Storage_Text_SetData_Req = 0x2103;
        public const UInt16 CS_Storage_Text_SetData_Res = 0x2104;

        public const UInt16 CS_Storage_Sheet_GetTableList_Req = 0x2111;
        public const UInt16 CS_Storage_Sheet_GetTableList_Res = 0x2112;
        public const UInt16 CS_Storage_Sheet_GetRecords_Req = 0x2113;
        public const UInt16 CS_Storage_Sheet_GetRecords_Res = 0x2114;


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
    }
}
