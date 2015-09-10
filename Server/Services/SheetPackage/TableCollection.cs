using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Aegis;
using Aegis.Threading;



namespace Server.Services.SheetPackage
{
    public class TableCollection
    {
        public Dictionary<String, Table> Tables { get; } = new Dictionary<String, Table>();





        public TableCollection(String filename)
        {
            using (ExcelLoader excel = new ExcelLoader(filename, 2, 3, 4))
            {
                foreach (ExcelSheet sheet in excel.GetSheets())
                {
                    if (sheet.Name.Length == 0 || sheet.Name[0] == '#')
                        continue;

                    Tables.Add(sheet.Name, new Table(sheet));
                    if (Tables.Count() >= Global.MaxTableCount)
                        break;
                }
            }
        }


        public Table GetTable(String tableName)
        {
            try
            {
                return Tables[tableName];
            }
            catch (Exception)
            {
                throw new AegisException(ResultCode.InvalidTableName, "Invalid table name({0}).", tableName);
            }
        }
    }
}
