using System;
using System.Collections.Generic;
using System.Linq;



namespace IndieAPI.Sheet
{
    public class Tables
    {
        private Dictionary<String, Table> _tables = new Dictionary<String, Table>();

        public List<Table> Items { get { return _tables.Select(v => v.Value).ToList(); } }





        public Table CreateTable(String name, Int32 recordCount, Int32 fieldCount)
        {
            Table table = new Table(name, recordCount, fieldCount);
            _tables[name] = table;

            return table;
        }


        public Table GetTable(String name)
        {
            return _tables[name];
        }


        public void Clear()
        {
            _tables.Clear();
        }
    }
}
