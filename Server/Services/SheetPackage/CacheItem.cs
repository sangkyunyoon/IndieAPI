using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Aegis;



namespace Server.Services.SheetPackage
{
    public static partial class Cache
    {
        private class CacheItem
        {
            private FileInfo _fileInfo;

            public TableCollection TableCollection { get; private set; }
            public DateTime LastAccessTime { get; set; }





            public CacheItem(String filename)
            {
                Renew(filename);
            }


            public void Renew(String filename)
            {
                LastAccessTime = DateTime.Now;
                if (HasChanged() == false)
                    return;


                lock (this)
                {
                    TableCollection = new TableCollection(filename);
                    _fileInfo = new FileInfo(filename);


                    if (_fileInfo.Length > Global.MaxFileSize)
                        throw new AegisException(ResultCode.TooBigFileSize, "File size too large({0}).", filename);
                }
            }


            private Boolean HasChanged()
            {
                if (_fileInfo == null)
                    return true;

                FileInfo newInfo = new FileInfo(_fileInfo.FullName);
                return (newInfo.CreationTime != _fileInfo.CreationTime ||
                        newInfo.Length != _fileInfo.Length);
            }
        }
    }
}
