using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aegis;
using Aegis.Threading;



namespace Server.Services
{
    public partial class CacheBox
    {
        public static CacheBox Instance { get { return Singleton<CacheBox>.Instance; } }
        public static Int32 Count { get { return Instance._cached.Count(); } }
        private RWLock _lock = new RWLock();
        private CancellationTokenSource _cts;
        private Dictionary<String, CacheItem> _cached = new Dictionary<String, CacheItem>();





        private CacheBox()
        {
        }


        public void Initialize()
        {
            _cts = new CancellationTokenSource();
            AegisTask.RunPeriodically(1000, _cts.Token, CheckExpiredItem).Name = "CacheBox";
        }


        public void Release()
        {
            _cts.Cancel();
        }


        private Boolean CheckExpiredItem()
        {
            List<CacheItem> expiredItems;
            Double now = DateTime.Now.ToOADate();


            using (_lock.ReaderLock)
            {
                expiredItems = _cached.Values
                                      .Where(v => v.ExpireTime != 0 && v.ExpireTime >= now)
                                      .ToList();
            }

            using (_lock.WriterLock)
            {
                foreach (CacheItem item in expiredItems)
                    _cached.Remove(item.Key);
            }

            return true;
        }
    }
}
