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
        public void Set(String key, String value, Double expire)
        {
            using (_lock.WriterLock)
            {
                CacheItem item;
                if (_cached.TryGetValue(key, out item) == false)
                {
                    item = new CacheItem(key, value, expire);
                    _cached.Add(item.Key, item);
                }
                else
                {
                    item.Value = value;
                    item.ExpireTime = expire;
                }
            }
        }


        public void SetExpireTime(String key, Double expire)
        {
            using (_lock.ReaderLock)
            {
                CacheItem item;
                if (_cached.TryGetValue(key, out item) == false)
                    throw new AegisException(ResultCode.CacheBox_InvalidKey);


                item.ExpireTime = expire;
            }
        }


        public String Get(String key)
        {
            using (_lock.ReaderLock)
            {
                CacheItem item;
                Double now = DateTime.Now.ToOADate();
                if (_cached.TryGetValue(key, out item) == false ||
                    (item.ExpireTime != 0 && item.ExpireTime >= now))
                    throw new AegisException(ResultCode.CacheBox_InvalidKey);

                return item.Value;
            }
        }


        public void Delete(String key)
        {
            using (_lock.WriterLock)
            {
                if (_cached.ContainsKey(key) == false)
                    return;

                _cached.Remove(key);
            }
        }
    }
}
