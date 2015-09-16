using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;



namespace IndieAPI.CloudSheet
{
    [DebuggerDisplay("RowNo={RowNo}")]
    public class Record
    {
        private UInt32 _rowNo;
        private Dictionary<String, String> _data;

        public UInt32 RowNo { get { return _rowNo; } }
        public List<String> Values { get { return _data.Values.ToList(); } }
        public String this[String fieldName]
        {
            get { return _data[fieldName]; }
            set { _data[fieldName] = value; }
        }
        public String this[Int32 index]
        {
            get { return _data.ElementAt(index).Value; }
            set { _data[this[index]] = value; }
        }





        public Record(UInt32 rowNo)
        {
            _rowNo = rowNo;
            _data = new Dictionary<String, String>();
        }
    }
}
