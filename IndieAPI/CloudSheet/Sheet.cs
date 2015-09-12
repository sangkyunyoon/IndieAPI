using System;
using System.Linq;
using System.Diagnostics;



namespace IndieAPI.CloudSheet
{
    [DebuggerDisplay("Name={Name}, RecordCount={RecordCount}")]
    public class Sheet
    {
        private String _name;
        private FieldInfo[] _fields;
        private Record[] _records;
        private Int32 _nextFieldIdx = 0;


        public String Name { get { return _name; } }
        public FieldInfo[] Fields { get { return _fields; } }
        public Record[] Records { get { return _records; } }
        public Int32 RecordCount { get { return _records.Count(); } }



        internal Sheet(String name, Int32 recordCount, Int32 fieldCount)
        {
            _name = name;
            _records = new Record[recordCount];
            _fields = new FieldInfo[fieldCount];
        }


        internal void AddField(FieldDataType type, String name)
        {
            _fields[_nextFieldIdx] = new FieldInfo() { type = type, name = name };
            ++_nextFieldIdx;
        }


        internal void AddRowData(UInt32 rowNo, String[] values)
        {
            Int32 idx = 0;

            _records[rowNo] = new Record(rowNo);
            foreach (FieldInfo fieldInfo in _fields)
            {
                _records[rowNo][fieldInfo.name] = values[idx];
                ++idx;
            }
        }
    }
}
