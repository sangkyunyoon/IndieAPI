using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;



namespace Server.Services
{
    public partial class CacheBox
    {
        private class CacheItem
        {
            public String Key { get; }
            public String Value { get; set; }

            public Double ExpireTime { get; set; }





            public CacheItem(String key, String value, Double expire)
            {
                if (key.Length > Global.CacheBox_MaxKeyLength)
                    throw new AegisException(ResultCode.CacheBox_TooLongKey);
                if (value.Length > Global.CacheBox_MaxValueLength)
                    throw new AegisException(ResultCode.CacheBox_TooLongValue);


                Key = key;
                Value = value;
                ExpireTime = expire;
            }
        }
    }
}
