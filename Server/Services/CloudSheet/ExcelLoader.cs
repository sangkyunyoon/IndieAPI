using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Aegis;



namespace IndieAPI.Server.Services.CloudSheet
{
    public class ExcelLoader : IDisposable
    {
        private String _filename;

        public Int32 RowIndex_FieldName { get; private set; }
        public Int32 RowIndex_DataType { get; private set; }
        public Int32 RowIndex_DataRow { get; private set; }
        public Int32 SheetCount { get; private set; }
        internal SpreadsheetDocument SSDocument { get; private set; }





        public ExcelLoader(String filename, Int32 fieldNameIndex = 2, Int32 dataTypeIndex = 3, Int32 dataRowStartIndex = 4)
        {
            _filename = filename;
            RowIndex_FieldName = fieldNameIndex;
            RowIndex_DataType = dataTypeIndex;
            RowIndex_DataRow = dataRowStartIndex;
            SSDocument = SpreadsheetDocument.Open(_filename, false);


            Sheets sheets = SSDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            SheetCount = (sheets == null ? 0 : sheets.Count());
        }


        public ExcelSheetReader GetReader(String sheetName)
        {
            Sheets sheets = SSDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            foreach (Sheet sheet in sheets)
            {
                if (sheet.Name.Value.ToLower() == sheetName.ToLower())
                    return new ExcelSheetReader(this, sheet);
            }

            throw new AegisException("'{0}' is not exists in '{1}'", sheetName, _filename);
        }


        public IEnumerable<ExcelSheetReader> GetReaderEnumerator()
        {
            Sheets sheets = SSDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>();
            foreach (Sheet sheet in sheets)
                yield return new ExcelSheetReader(this, sheet);
        }


        public void Dispose()
        {
            SSDocument.Close();
            SSDocument = null;
        }
    }


    public struct FieldInfo
    {
        private String _name;
        private DataType _type;


        public String Name
        {
            get { return _name; }
            internal set { _name = value; }
        }
        public DataType DataType
        {
            get { return _type; }
            internal set { _type = value; }
        }


        public FieldInfo(String name, DataType type)
        {
            _name = name;
            _type = type;
        }
    }


    public struct CellValue
    {
        private FieldInfo _field;
        private dynamic _value;


        public FieldInfo FieldInfo { get { return _field; } }
        public dynamic Value { get { return _value; } }


        internal CellValue(FieldInfo field, dynamic value)
        {
            _field = field;
            _value = value;
        }
    }
}
