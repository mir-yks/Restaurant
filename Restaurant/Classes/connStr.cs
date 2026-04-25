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
        public static string GetConnectionString(string databaseName)
        {
            Configuration currentConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            string host = currentConfig.AppSettings.Settings["host"]?.Value;
            string uid = currentConfig.AppSettings.Settings["uid"]?.Value;
            string pwd = currentConfig.AppSettings.Settings["pwd"]?.Value;

            if (string.IsNullOrEmpty(host) || string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(pwd))
            {
                throw new Exception("Не все параметры подключения настроены в конфигурационном файле.");
            }

            return $"host={host};uid={uid};pwd={pwd};database={databaseName};";
        }
    }
}