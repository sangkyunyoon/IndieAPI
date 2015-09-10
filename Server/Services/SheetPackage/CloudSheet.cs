using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Data.MySql;



namespace Server.Services.SheetPackage
{
    public class CloudSheet
    {
        public static CloudSheet Instance { get { return Singleton<CloudSheet>.Instance; } }





        private CloudSheet()
        {
        }


        public void GetTableList(String filename, StreamBuffer destination)
        {
            TableCollection tables = Cache.GetTableCollection(filename);
            lock (tables)
            {
                destination.PutInt32(tables.Tables.Count());
                foreach (Table table in tables.Tables.Select(v => v.Value))
                {
                    //  Table information
                    destination.PutStringAsUtf16(table.Name);
                    destination.PutInt32(table.Records.Count());
                    destination.PutInt32(table.Fields.Count());


                    //  Field information
                    foreach (var fieldInfo in table.Fields)
                    {
                        destination.PutInt32((Int32)fieldInfo.DataType);
                        destination.PutStringAsUtf16(fieldInfo.Name);
                    }
                }
            }
        }


        public void GetRowList(String filename, String tableName, UInt32 startRowNo, StreamBuffer destination)
        {
            TableCollection tables = Cache.GetTableCollection(filename);
            Table table = tables.GetTable(tableName);


            destination.PutStringAsUtf16(tableName);

            Int32 hasMoreIdx = destination.PutByte(0);
            Int32 rowCountIdx = destination.PutInt32(0);
            Int32 rowCount = 0;
            UInt32 lastRowIndex = 0;


            foreach (Record data in table.Records)
            {
                if (data == null || data.RowNo < startRowNo)
                    continue;

                ++rowCount;
                lastRowIndex = data.RowNo;

                destination.PutUInt32(data.RowNo);
                foreach (String value in data.DataList)
                {
                    if (value == null)
                        destination.PutStringAsUtf16("");
                    else
                        destination.PutStringAsUtf16(value.ToString());
                }


                //  대략 이쯤...  패킷 크기를 초과하지 않도록 적당이 끊어준다.
                if (destination.WrittenBytes > 65535 - 1024)
                    break;
            }


            destination.OverwriteByte(hasMoreIdx, (Byte)(lastRowIndex < table.MaxRowNo ? 1 : 0));
            destination.OverwriteInt32(rowCountIdx, rowCount);
        }
    }
}
