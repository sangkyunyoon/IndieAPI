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
        private void OnCS_Sheet_GetTableList_Req(SecurityPacket reqPacket)
        {
            String filename = reqPacket.GetStringFromUtf16();
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Sheet_GetTableList_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                resPacket.PutInt32(ResultCode.Ok);
                Services.SheetPackage.CloudSheet.Instance.GetTableList(filename, resPacket);
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


        private void OnCS_Sheet_GetRecords_Req(SecurityPacket reqPacket)
        {
            String filename = reqPacket.GetStringFromUtf16();
            String tableName = reqPacket.GetStringFromUtf16();
            UInt32 startRowNo = reqPacket.GetUInt32();
            SecurityPacket resPacket = new SecurityPacket(Protocol.CS_Sheet_GetRecords_Res, 65535);
            resPacket.SeqNo = reqPacket.SeqNo;


            try
            {
                resPacket.PutInt32(ResultCode.Ok);
                Services.SheetPackage.CloudSheet.Instance.GetRowList(filename, tableName, startRowNo, resPacket);
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
