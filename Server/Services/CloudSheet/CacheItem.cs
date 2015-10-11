using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Aegis;



namespace IndieAPI.Server.Services.CloudSheet
{
    public static partial class Workbooks
    {
        private class CacheItem
        {
            private FileInfo _fileInfo;

            public Workbook TableCollection { get; private set; }
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
                    TableCollection = new Workbook(filename);
                    _fileInfo = new FileInfo(filename);


                    if (_fileInfo.Length > Global.MaxFileSize)
                        throw new AegisException(ResultCode.CloudSheet_TooBigFileSize, "File size too large({0}).", filename);
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
