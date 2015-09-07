using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Aegis.Data.MySql;



namespace Server.Session
{
    public partial class ClientSession
    {
        private void OnCS_Storage_Text_GetData_Req(SecurityPacket reqPacket)
        {
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Storage_Text_GetData_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;


            resPacket.PutInt32(ResultCode.Ok);
            resPacket.PutStringAsUtf16(_user.TextBox.TextData);
            SendPacket(resPacket);
        }


        private void OnCS_Storage_Text_SetData_Req(SecurityPacket reqPacket)
        {
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Storage_Text_SetData_Res);
            resPacket.SeqNo = reqPacket.SeqNo;


            _user.TextBox.TextData = reqPacket.GetStringFromUtf16();


            resPacket.PutInt32(ResultCode.Ok);
            SendPacket(resPacket);
        }


        private void OnCS_Storage_Sheet_GetTableList_Req(SecurityPacket reqPacket)
        {
            String filename = reqPacket.GetStringFromUtf16();
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Storage_Sheet_GetTableList_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                resPacket.PutInt32(ResultCode.Ok);
                Services.CloudSheetPackage.CloudSheet.Instance.GetTableList(filename, resPacket);
            }
            catch (AegisException e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());


                resPacket.Clear();
                resPacket.SeqNo = reqPacket.SeqNo;
                resPacket.PutInt32(e.ResultCodeNo);
            }

            SendPacket(resPacket);
        }


        private void OnCS_Storage_Sheet_GetRecords_Req(SecurityPacket reqPacket)
        {
            String filename = reqPacket.GetStringFromUtf16();
            String tableName = reqPacket.GetStringFromUtf16();
            UInt32 startRowNo = reqPacket.GetUInt32();
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Storage_Sheet_GetRecords_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                resPacket.PutInt32(ResultCode.Ok);
                Services.CloudSheetPackage.CloudSheet.Instance.GetRowList(filename, tableName, startRowNo, resPacket);
            }
            catch (AegisException e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());


                resPacket.Clear();
                resPacket.SeqNo = reqPacket.SeqNo;
                resPacket.PutInt32(e.ResultCodeNo);
            }

            SendPacket(resPacket);
        }
    }
}
