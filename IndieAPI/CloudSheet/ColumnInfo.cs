using System;
using System.Diagnostics;



namespace IndieAPI.CloudSheet
{
    public enum FieldDataType
    {
        Int,
        Double,
        DateTime,
        String
    }





    [DebuggerDisplay("Type={type}, Name={name}")]
    public struct ColumnInfo
    {
        public FieldDataType type;
        public String name;
    }
}
