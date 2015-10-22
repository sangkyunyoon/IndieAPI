using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;



namespace IndieAPI.Server.Services
{
    public partial class CacheBox
    {
        public static CacheBox Instance { get { return Singleton<CacheBox>.Instance; } }
        public static Int32 Count { get { return Instance._cached.Count(); } }
        private Dictionary<String, CacheItem> _cached = new Dictionary<String, CacheItem>();
        private Thread _thread;






        private CacheBox()
        {
        }


        public void Initialize()
        {
            _thread = ThreadExtend.CallPeriodically(1000, CheckExpiredItem);
            _thread.Name = "CacheBox";
        }


        public void Release()
        {
            ThreadExtend.Cancel(_thread);
        }


        private Boolean CheckExpiredItem()
        {
            SpinWorker.Dispatch(() =>
            {
                List<CacheItem> expiredItems;
                DateTime now = DateTime.Now;


                expiredItems = _cached.Values
                                      .Where(v => v.ExpireTime != -1 && now > DateTime.FromOADate(v.ExpireTime))
                                      .ToList();

                foreach (CacheItem item in expiredItems)
                    _cached.Remove(item.Key);
            });

            return true;
        }
    }
}
