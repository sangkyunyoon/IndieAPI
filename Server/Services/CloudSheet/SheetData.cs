using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;



namespace Server.Services.CloudSheet
{
    public class SheetData
    {
        public String Name { get; private set; }
        public FieldInfo[] Fields { get; private set; }
        public Record[] Records { get; private set; }
        public UInt32 MaxRowNo { get; private set; }





        internal SheetData(ExcelSheetReader reader)
        {
            reader.Load();

            Name = reader.SheetName;
            Fields = reader.Fields;


            //  Check Data
            {
                if (reader.RowCount < reader.ExcelLoader.RowIndex_DataRow)
                    throw new AegisException(ResultCode.NoDataInSheet, "No data in '{0}' sheet.", Name);

                if (reader.Fields.Count() == 0)
                    throw new AegisException(ResultCode.NoDataInSheet, "No data in '{0}' sheet.", Name);

                if (reader.Fields.Count() > Global.MaxColumnCount)
                    throw new AegisException(ResultCode.TooManyColumns, "Too many columns in '{0}' sheet.", Name);

                if (reader.RowCount > Global.MaxRecordCount)
                    throw new AegisException(ResultCode.TooManyRecords, "Too many records in '{0}' sheet.", Name);
            }


            //  Check Field Name
            for (Int32 i = 0; i < reader.Fields.Count(); ++i)
            {
                if (reader.Fields[i].Name.Length == 0)
                    throw new AegisException(ResultCode.EmptyColumnName, "Empty column name in '{1}' sheet.", Name);

                for (Int32 k = 0; k < reader.Fields.Count(); ++k)
                {
                    if (i == k)
                        continue;

                    if (reader.Fields[k].Name == reader.Fields[i].Name)
                        throw new AegisException(ResultCode.DuplicateColumnName, "Duplicate column name({0}) in '{1}' sheet.", reader.Fields[k].Name, Name);
                }
            }


            Int32 idx = 0;
            Records = new Record[reader.RowCount - (reader.ExcelLoader.RowIndex_DataRow - 1)];
            MaxRowNo = 0;


            //  Read by line
            try
            {
                while (reader.NextRow())
                {
                    Record data = new Record(reader.CurrentRow.RowIndex.Value - (UInt32)reader.ExcelLoader.RowIndex_DataRow);
                    Records[idx++] = data;

                    if (MaxRowNo < data.RowNo)
                        MaxRowNo = data.RowNo;


                    foreach (CellValue cellValue in reader.GetCellValues())
                    {
                        if (cellValue.Value == null)
                            data.Add(cellValue.FieldInfo.Name, null);

                        else if (cellValue.FieldInfo.DataType == DataType.DateTime)
                            data.Add(cellValue.FieldInfo.Name, cellValue.Value.ToOADate().ToString());

                        else if (cellValue.FieldInfo.DataType == DataType.Int)
                            data.Add(cellValue.FieldInfo.Name, cellValue.Value.ToString());

                        else if (cellValue.FieldInfo.DataType == DataType.Double)
                            data.Add(cellValue.FieldInfo.Name, cellValue.Value.ToString());

                        else
                            data.Add(cellValue.FieldInfo.Name, cellValue.Value);
                    }

                    if (data.DataList.Count() != reader.Fields.Count())
                        throw new AegisException(ResultCode.ColumnCountIsNotMatch, "Column count is not match at {0}({1} row).", Name, reader.CurrentRow.RowIndex.Value);
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new AegisException(ResultCode.ColumnCountIsNotMatch, "Column count is not match at {0}({1} row).", Name, reader.CurrentRow.RowIndex.Value);
            }
        }
    }
}
