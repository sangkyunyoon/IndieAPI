using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;



namespace IndieAPI.Server.Services
{
    public partial class CacheBox
    {
        public void Set(String key, String value, Double expire)
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


        public void SetExpireTime(String key, Double expire)
        {
            CacheItem item;
            if (_cached.TryGetValue(key, out item) == false)
                throw new AegisException(ResultCode.CacheBox_InvalidKey);


            item.ExpireTime = expire;
        }


        public void Get(String key, out String value, out Int32 durationMinutes)
        {
            CacheItem item;
            DateTime now = DateTime.Now;

            if (_cached.TryGetValue(key, out item) == false ||
                (item.ExpireTime != -1 && now > DateTime.FromOADate(item.ExpireTime)))
                throw new AegisException(ResultCode.CacheBox_InvalidKey);


            value = item.Value;
            if (item.ExpireTime == -1)
                durationMinutes = -1;
            else
                durationMinutes = (DateTime.FromOADate(item.ExpireTime) - DateTime.Now).Minutes;
        }


        public void Delete(String key)
        {
            if (_cached.ContainsKey(key) == false)
                return;

            _cached.Remove(key);
        }
    }
}
