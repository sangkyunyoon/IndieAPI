using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Aegis;



namespace Server.Services.CloudSheetPackage
{
    public enum DataType
    {
        Int,
        Double,
        DateTime,
        String
    }



    public class ExcelSheet
    {
        private Sheet _sheet;
        private SharedStringTablePart _sstp;
        private IEnumerator<Row> _iter;


        public ExcelLoader ExcelLoader { get; private set; }
        public UInt32 MaxRowIndex { get; private set; }
        public Int32 RowCount { get; private set; }
        public String Name { get { return _sheet.Name; } }
        public FieldInfo[] Fields { get; private set; }
        public Row CurrentRow { get { return _iter.Current; } }





        internal ExcelSheet(ExcelLoader parent, Sheet sheet)
        {
            ExcelLoader = parent;
            _sheet = sheet;
        }


        public ExcelSheet Load()
        {
            WorkbookPart wbp = ExcelLoader.Workbook.WorkbookPart;
            WorksheetPart wsp = (WorksheetPart)wbp.GetPartById(_sheet.Id.Value);
            SheetData sheetData = wsp.Worksheet.GetFirstChild<SheetData>();


            _iter = sheetData.Elements<Row>().GetEnumerator();
            _sstp = wbp.GetPartsOfType<SharedStringTablePart>().First();
            RowCount = sheetData.Elements<Row>().Count();
            MaxRowIndex = sheetData.Elements<Row>().Max(v => v.RowIndex.Value);


            //  Field data
            if (ExcelLoader.RowIndex_FieldName >= 0)
            {
                Row row = sheetData.Elements<Row>().Where(v => v.RowIndex == ExcelLoader.RowIndex_FieldName).FirstOrDefault();
                Int32 idx = 0;


                //  field name
                if (row == null)
                    throw new AegisException("FieldName index is {0}, but {1} has no {0} row index.", ExcelLoader.RowIndex_FieldName, _sheet.Name);
                Fields = new FieldInfo[row.LongCount()];


                foreach (Cell cell in row.Elements<Cell>())
                {
                    Fields[idx] = new FieldInfo();
                    Fields[idx].Name = GetTextInCell(cell);
                    ++idx;
                }


                //  data type
                idx = 0;
                row = sheetData.Elements<Row>().Where(v => v.RowIndex == ExcelLoader.RowIndex_DataType).First();
                foreach (Cell cell in row.Elements<Cell>())
                {
                    String text = GetTextInCell(cell).ToLower();

                    if (text == "int" || text == "integer")
                        Fields[idx++].DataType = DataType.Int;

                    else if (text == "double")
                        Fields[idx++].DataType = DataType.Double;

                    else if (text == "datetime")
                        Fields[idx++].DataType = DataType.DateTime;

                    else if (text == "string")
                        Fields[idx++].DataType = DataType.String;

                    else
                        throw new AegisException("Invalid field type at {0}.{1}", _sheet.Name, cell.CellReference);
                }
            }

            return this;
        }


        private String GetTextInCell(Cell cell)
        {
            String text = "";
            String cellRef = cell.CellReference;


            //  Getting Text value of the Cell.
            if (cell.DataType != null && cell.DataType == CellValues.SharedString)
            {
                Int32 ssid = Int32.Parse(cell.CellValue.Text);
                text = _sstp.SharedStringTable.ChildElements[ssid].InnerText;
            }
            else if (cell.CellValue != null)
                text = cell.CellValue.Text;

            return text;
        }


        public FieldInfo GetFieldInfo(Int32 index)
        {
            if (Fields == null || Fields.Count() <= index)
                throw new AegisException("Field index out of range.");

            return Fields[index];
        }


        public Boolean NextRow()
        {
            if (_iter.Current == null)
            {
                //  Enumerator를 DataRowIndex까지 이동
                while (_iter.Current == null || _iter.Current.RowIndex != ExcelLoader.RowIndex_DataRow)
                {
                    if (_iter.MoveNext() == false)
                        return false;
                }

                return true;
            }

            return _iter.MoveNext();
        }


        public CellValue GetCellValue(String fieldName)
        {
            if (Fields == null)
                throw new AegisException("'{0}' sheet has no field information.", _sheet.Name);


            //  필드 위치 찾기
            Int32 fieldIdx = 0;
            foreach (FieldInfo fieldInfo in Fields)
            {
                if (StringComparer.OrdinalIgnoreCase.Equals(fieldInfo.Name, fieldName) == true)
                    break;
                ++fieldIdx;
            }

            if (fieldIdx >= Fields.Count())
                throw new AegisException("There is no field name('{0}') in '{1}' sheet.", fieldName, _sheet.Name);

            if (fieldIdx >= _iter.Current.Elements<Cell>().Count())
                throw new AegisException("Out of cell index({0}) at '{1}' {2} row", fieldIdx, _sheet.Name, _iter.Current.RowIndex);


            Cell cell = _iter.Current.Elements<Cell>().Where(v => GetColumnIndex(v) == fieldIdx).FirstOrDefault();
            return GetCellValue(cell, Fields[fieldIdx]);
        }


        public IEnumerable<CellValue> GetCellValues()
        {
            Int32 idx = 0;


            if (Fields == null)
                throw new AegisException("'{0}' sheet has no field information.", _sheet.Name);

            foreach (Cell cell in _iter.Current.Elements<Cell>())
                yield return GetCellValue(cell, Fields[idx++]);
        }


        private CellValue GetCellValue(Cell cell, FieldInfo fieldInfo)
        {
            String text = GetTextInCell(cell);


            if (text == "")
            {
                if (fieldInfo.DataType == DataType.Int)
                    return new CellValue(fieldInfo, (Int32)0);

                if (fieldInfo.DataType == DataType.Double)
                    return new CellValue(fieldInfo, (Double)0);

                return new CellValue(fieldInfo, null);
            }

            else if (fieldInfo.DataType == DataType.DateTime)
            {
                DateTime value;
                Double dblValue;

                if (DateTime.TryParse(text, out value) == true)
                    return new CellValue(fieldInfo, value);

                else if (Double.TryParse(text, out dblValue) == true)
                    return new CellValue(fieldInfo, DateTime.FromOADate(dblValue));

                else
                    throw new AegisException("Invalid data at {0}.{1}", _sheet.Name, cell.CellReference);
            }

            else if (fieldInfo.DataType == DataType.Int)
            {
                Int32 value;
                if (Int32.TryParse(text, out value) == true)
                    return new CellValue(fieldInfo, value);

                else
                    throw new AegisException("Invalid data at {0}.{1}", _sheet.Name, cell.CellReference);
            }

            else if (fieldInfo.DataType == DataType.Double)
            {
                Double value;
                if (Double.TryParse(text, out value) == true)
                    return new CellValue(fieldInfo, value);

                else
                    throw new AegisException("Invalid data at {0}.{1}", _sheet.Name, cell.CellReference);
            }

            else
                return new CellValue(fieldInfo, text);
        }


        private Int32 GetColumnIndex(Cell cell)
        {
            Int32 startIdx = cell.CellReference.Value.IndexOfAny("0123456789".ToCharArray());
            String column = cell.CellReference.Value.Substring(0, startIdx);


            //  26진수 -> 10진수
            {
                Int32 dec = 0;
                Int32 powVal = column.Length - 1;

                foreach (char c in column)
                    dec += (c - 'A' + 1) * (Int32)Math.Pow(26, powVal--);

                return dec - 1;
            }
        }
    }
}
