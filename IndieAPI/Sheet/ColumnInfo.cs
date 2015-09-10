using System;
using System.Diagnostics;



namespace IndieAPI.Sheet
{
    public enum FieldDataType
    {
        Int,
        Double,
        DateTime,
        String
    }





    [DebuggerDisplay("Type={type}, Name={name}")]
    public struct FieldInfo
    {
        public FieldDataType type;
        public String name;
    }
}
