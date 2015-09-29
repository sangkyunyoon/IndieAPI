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

            public DateTime? ExpireTime { get; set; }





            public CacheItem(String key, String value, DateTime? expire)
            {
                if (key.Length > 32)
                    throw new AegisException(ResultCode.Cache_TooLongKey);
                if (value.Length > 1024 * 1024)
                    throw new AegisException(ResultCode.Cache_TooLongValue);


                Key = key;
                Value = value;
                ExpireTime = expire;
            }
        }
    }
}
