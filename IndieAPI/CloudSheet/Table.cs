using System;
using System.Linq;
using System.Diagnostics;



namespace IndieAPI.CloudSheet
{
    [DebuggerDisplay("TableName={Name}, RecordCount={RecordCount}")]
    public class Table
    {
        private String _name;
        private ColumnInfo[] _columns;
        private Record[] _records;
        private Int32 _nextFieldIdx = 0;


        public String Name { get { return _name; } }
        public ColumnInfo[] Columns { get { return _columns; } }
        public Record[] Records { get { return _records; } }
        public Int32 RecordCount { get { return _records.Count(); } }



        internal Table(String name, Int32 recordCount, Int32 columnCount)
        {
            _name = name;
            _records = new Record[recordCount];
            _columns = new ColumnInfo[columnCount];
        }


        internal void AddField(FieldDataType type, String name)
        {
            _columns[_nextFieldIdx] = new ColumnInfo() { type = type, name = name };
            ++_nextFieldIdx;
        }


        internal void AddRowData(UInt32 rowNo, String[] values)
        {
            Int32 idx = 0;

            _records[rowNo] = new Record(rowNo);
            foreach (ColumnInfo fieldInfo in _columns)
            {
                _records[rowNo][fieldInfo.name] = values[idx];
                ++idx;
            }
        }
    }
}
