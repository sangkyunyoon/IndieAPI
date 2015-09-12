using System;
using System.Collections.Generic;
using System.Linq;



namespace IndieAPI.CloudSheet
{
    public class Workbook
    {
        private Dictionary<String, Sheet> _sheets = new Dictionary<String, Sheet>();

        public List<Sheet> Sheets { get { return _sheets.Select(v => v.Value).ToList(); } }





        public Sheet AddSheet(String name, Int32 recordCount, Int32 fieldCount)
        {
            Sheet sheet = new Sheet(name, recordCount, fieldCount);
            _sheets[name] = sheet;

            return sheet;
        }


        public Sheet GetSheet(String name)
        {
            return _sheets[name];
        }


        public void Clear()
        {
            _sheets.Clear();
        }
    }
}
