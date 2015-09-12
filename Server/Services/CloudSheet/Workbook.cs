using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aegis;
using Aegis.Threading;



namespace Server.Services.CloudSheet
{
    public class Workbook
    {
        public Dictionary<String, SheetData> Items { get; } = new Dictionary<String, SheetData>();





        public Workbook(String filename)
        {
            using (ExcelLoader excel = new ExcelLoader(filename, 2, 3, 4))
            {
                foreach (ExcelSheetReader reader in excel.GetReaderEnumerator())
                {
                    if (reader.SheetName.Length == 0 || reader.SheetName[0] == '#')
                        continue;

                    Items.Add(reader.SheetName, new SheetData(reader));
                    if (Items.Count() >= Global.MaxTableCount)
                        break;
                }
            }
        }


        public SheetData GetSheetData(String tableName)
        {
            try
            {
                return Items[tableName];
            }
            catch (Exception)
            {
                throw new AegisException(ResultCode.InvalidTableName, "Invalid table name({0}).", tableName);
            }
        }
    }
}
