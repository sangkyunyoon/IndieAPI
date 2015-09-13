using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.IO;
using Aegis;
using Aegis.Threading;



namespace Server.Services.CloudSheet
{
    public static partial class Workbooks
    {
        private static RWLock _lock = new RWLock();
        private static Dictionary<String, CacheItem> _cache = new Dictionary<String, CacheItem>();
        private static CancellationTokenSource _cancelCleaner;


        public static Int32 CachedCount
        {
            get
            {
                using (_lock.ReaderLock)
                {
                    return _cache.Count();
                }
            }
        }





        public static void Initialize()
        {
            _cancelCleaner = new CancellationTokenSource();
            ClearUnusedCache();
        }


        public static void Release()
        {
            using (_lock.WriterLock)
            {
                _cache.Clear();
            }

            _cancelCleaner?.Cancel();
        }


        public static Workbook GetWorkbook(String filename)
        {
            CacheItem item;


            try
            {
                filename = String.Format("{0}\\{1}", Global.StoragePath, filename).ToLower();

                using (_lock.WriterLock)
                {
                    if (_cache.TryGetValue(filename, out item) == false)
                    {
                        item = new CacheItem(filename);
                        _cache.Add(filename, item);
                    }
                }
            }
            catch (DirectoryNotFoundException)
            {
                throw new AegisException(ResultCode.InvalidFileName, "'{0}' file not found.", filename);
            }
            catch (FileNotFoundException)
            {
                throw new AegisException(ResultCode.InvalidFileName, "'{0}' file not found.", filename);
            }
            catch (FileFormatException)
            {
                throw new AegisException(ResultCode.InvalidFileType, "'{0}' is not valid xlsx.", filename);
            }
            catch (AegisException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new AegisException(e, e.ToString());
            }


            item.Renew(filename);
            return item.TableCollection;
        }


        private static async void ClearUnusedCache()
        {
            while (_cancelCleaner.IsCancellationRequested == false)
            {
                try
                {
                    await System.Threading.Tasks.Task.Delay(60 * 1000, _cancelCleaner.Token);
                }
                catch (Exception)
                {
                    _cancelCleaner = null;
                    break;
                }


                List<String> items;
                using (_lock.ReaderLock)
                {
                    items = _cache
                        .Where(v => DateTime.Now.Subtract(v.Value.LastAccessTime).Minutes > Global.DataCacheTime)
                        .Select(v => v.Key)
                        .ToList();
                }

                using (_lock.WriterLock)
                {
                    foreach (String filename in items)
                    {
                        _cache.Remove(filename);
                    }
                }
            }
        }
    }
}
