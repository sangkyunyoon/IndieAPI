using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aegis;
using Aegis.Configuration;
using Aegis.Converter;



namespace Server
{
    public static class Global
    {
        public static String StoragePath { get; set; }
        public static Int32 UserManager_CCUMaxTime { get; set; }
        public static Int32 UserManager_MaxAliveTime { get; set; }
        public static Int32 MaxTableCount { get; set; }
        public static Int32 MaxColumnCount { get; set; }
        public static Int32 MaxRecordCount { get; set; }
        public static Int32 MaxFileSize { get; set; }
        public static Int32 SessionTimeout { get; set; }
        public static Int32 DataCacheTime { get; set; }
        public static String AES_IV { get; private set; }
        public static String AES_Key { get; private set; }
        public static Int32 CacheBox_MaxKeyLength { get; private set; }
        public static Int32 CacheBox_MaxValueLength { get; private set; }





        public static void Refresh()
        {
            try
            {
                AES_IV = Starter.CustomData.GetValue("AES/IV");
                AES_Key = Starter.CustomData.GetValue("AES/Key");

                UserManager_CCUMaxTime = Starter.CustomData.GetValue("UserManager/ccuMaxTime", "300").ToInt32();
                UserManager_MaxAliveTime = Starter.CustomData.GetValue("UserManager/maxAliveTime", "1800").ToInt32();

                StoragePath = Starter.CustomData.GetValue("CloudSheet/storagePath", @".\");
                MaxTableCount = Starter.CustomData.GetValue("CloudSheet/maxTableCount", "4").ToInt32();
                MaxColumnCount = Starter.CustomData.GetValue("CloudSheet/maxColumnCount", "20").ToInt32();
                MaxRecordCount = Starter.CustomData.GetValue("CloudSheet/maxRecordCount", "30000").ToInt32();
                MaxFileSize = Starter.CustomData.GetValue("CloudSheet/maxFileSize", "65000").ToInt32();
                SessionTimeout = Starter.CustomData.GetValue("CloudSheet/sessionTimeout", "30").ToInt32();
                DataCacheTime = Starter.CustomData.GetValue("CloudSheet/dataCacheTime", "30").ToInt32();

                CacheBox_MaxKeyLength = Starter.CustomData.GetValue("CacheBox/maxKeyLength", "32").ToInt32();
                CacheBox_MaxValueLength = Starter.CustomData.GetValue("CacheBox/maxValueLength", "32500").ToInt32();
            }
            catch (Exception e)
            {
                Logger.Write(LogType.Err, 2, e.ToString());
            }
        }
    }
}
