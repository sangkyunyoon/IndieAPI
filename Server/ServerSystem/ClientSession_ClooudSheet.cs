﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Network;
using Aegis.Data.MySql;
using IndieAPI.Server.Services.CloudSheet;



namespace IndieAPI.Server.Routine
{
    public partial class ClientSession
    {
        private void OnCS_CloudSheet_GetSheetList_Req(SecurePacketRequest reqPacket)
        {
            String filename = reqPacket.GetStringFromUtf16();
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket, 65535);


            try
            {
                Workbook workbook = Workbooks.GetWorkbook(filename);

                resPacket.ResultCodeNo = ResultCode.Ok;
                resPacket.PutInt32(workbook.Items.Count());
                foreach (SheetData sheet in workbook.Items.Select(v => v.Value))
                {
                    //  Sheet information
                    resPacket.PutStringAsUtf16(sheet.Name);
                    resPacket.PutInt32(sheet.Records.Count());
                    resPacket.PutInt32(sheet.Fields.Count());


                    //  Field information
                    foreach (var fieldInfo in sheet.Fields)
                    {
                        resPacket.PutInt32((Int32)fieldInfo.DataType);
                        resPacket.PutStringAsUtf16(fieldInfo.Name);
                    }
                }
            }
            catch (AegisException e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());


                resPacket.Clear();
                resPacket.ResultCodeNo = e.ResultCodeNo;
            }

            SendPacket(resPacket);
        }


        private void OnCS_CloudSheet_GetRecords_Req(SecurePacketRequest reqPacket)
        {
            String filename = reqPacket.GetStringFromUtf16();
            String sheetName = reqPacket.GetStringFromUtf16();
            UInt32 startRowNo = reqPacket.GetUInt32();
            SecurePacketResponse resPacket = new SecurePacketResponse(reqPacket, 65535);


            try
            {
                resPacket.ResultCodeNo = ResultCode.Ok;

                Workbook workbook = Workbooks.GetWorkbook(filename);
                SheetData sheet = workbook.GetSheetData(sheetName);
                Int32 hasMoreIdx = resPacket.PutByte(0);
                Int32 rowCountIdx = resPacket.PutInt32(0);
                Int32 rowCount = 0;
                UInt32 lastRowIndex = 0;


                foreach (Record data in sheet.Records)
                {
                    if (data == null || data.RowNo < startRowNo)
                        continue;

                    ++rowCount;
                    lastRowIndex = data.RowNo;

                    resPacket.PutUInt32(data.RowNo);
                    foreach (String value in data.DataList)
                    {
                        if (value == null)
                            resPacket.PutStringAsUtf16("");
                        else
                            resPacket.PutStringAsUtf16(value.ToString());
                    }


                    //  #! 대략 이쯤...  패킷 크기를 초과하지 않도록 적당이 끊어준다.
                    if (resPacket.WrittenBytes > 50000)
                        break;
                }


                resPacket.OverwriteByte(hasMoreIdx, (Byte)(lastRowIndex < sheet.MaxRowNo ? 1 : 0));
                resPacket.OverwriteInt32(rowCountIdx, rowCount);
            }
            catch (AegisException e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());


                resPacket.Clear();
                resPacket.ResultCodeNo = e.ResultCodeNo;
            }

            SendPacket(resPacket);
        }
    }
}
