using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public class AppSettingsHelper
    {
        /// <summary>
        /// Get app setting value based on his key
        /// </summary>
        /// <param name="settingKey"></param>
        /// <returns></returns>
        public static string GetSettingValue(string settingKey)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            return config[settingKey];
        }

        public static string GetConnectionString(string connectionStringName)
        {
            string connectionStringKey = $"ConnectionStrings:{connectionStringName}";
            return GetSettingValue(connectionStringKey);
        }
    }
}
