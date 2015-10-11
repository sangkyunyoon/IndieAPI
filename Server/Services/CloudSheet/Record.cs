using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace IndieAPI.Server.Services.CloudSheet
{
    public class Record
    {
        private Dictionary<String, String> _itemDictionary = new Dictionary<String, String>();
        private List<String> _items = new List<String>();

        public UInt32 RowNo { get; private set; }
        public List<String> DataList { get { return _items; } }
        public String this[String key] { get { return _itemDictionary[key]; } }
        public String this[Int32 idx] { get { return _items[idx]; } }





        internal Record(UInt32 rowNo)
        {
            RowNo = rowNo;
        }


        public void Add(String key, String val)
        {
            _itemDictionary.Add(key, val);
            _items.Add(val);
        }
    }
}
