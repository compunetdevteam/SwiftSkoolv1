using System;
using System.Configuration;

namespace SwiftSkoolv1.WebUI.Services
{
    public class ConfigService
    {
        public ConfigService() { }

        /// <summary>
        /// Get the SMS gateway url
        /// </summary>
        public string SmsUrl => getAppSetting(typeof(string), "SmsUrl").ToString();

        /// <summary>
        /// Get teh gateway account to use in sending sms message
        /// </summary>
        public string SmsAccount => getAppSetting(typeof(string), "SmsAccount").ToString();

        /// <summary>
        /// Get the sub account to use for the sending of sms
        /// </summary>
        public string SubAccount => getAppSetting(typeof(string), "SubAccount").ToString();

        /// <summary>
        /// Get the sub account password for the sms gateway
        /// </summary>
        public string SubAccountPwd => getAppSetting(typeof(string), "SubAccountPass").ToString();

        private static object getAppSetting(Type expectedType, string key)
        {
            string value = ConfigurationManager.AppSettings[key]; //.Get(key);

            if (value == null)
            {
                throw new Exception(
                    $"The config file does not have the key '{key}' defined in the AppSetting section.");
            }

            if (expectedType == typeof(int))
            {
                return int.Parse(value);
            }

            if (expectedType == typeof(string))
            {
                return value;
            }
            else
                throw new Exception("Type not supported.");
        }
    }
}