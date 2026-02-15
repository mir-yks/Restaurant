using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant
{
    public static class connStr
    {
        public static string ConnectionString
        {
            get
            {
                Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                string host = currentConfig.AppSettings.Settings["host"]?.Value;
                string uid = currentConfig.AppSettings.Settings["uid"]?.Value;
                string pwd = currentConfig.AppSettings.Settings["pwd"]?.Value;
                string database = currentConfig.AppSettings.Settings["database"]?.Value;

                if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(uid) ||
                    string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(database))
                {
                    throw new Exception("Не все параметры подключения настроены в конфигурационном файле.");
                }

                return $"host={host};uid={uid};pwd={pwd};database={database};";
            }
        }
    }
}