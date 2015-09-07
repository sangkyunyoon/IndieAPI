﻿using System;
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
        public String this[String fieldName] { get { return _data[fieldName]; } set { _data[fieldName] = value; } }





        public Record(UInt32 rowNo)
        {
            _rowNo = rowNo;
            _data = new Dictionary<String, String>();
        }
    }
}
