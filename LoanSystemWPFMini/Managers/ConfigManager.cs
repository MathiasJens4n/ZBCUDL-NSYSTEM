using System;
using System.Collections.Specialized;
using System.Configuration;

namespace LoanSystemWPFMini.Managers
{
    public class ConfigManager
    {
        public string GetAppSetting(string key)
        {
            try
            {
                NameValueCollection appSettings = ConfigurationManager.AppSettings;
                return appSettings.Get(key);
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
        }

        public void UpdateAppSettings(string sectionName, string key, string value)
        {
            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(sectionName);
            }
            catch (ConfigurationErrorsException)
            {
                throw;
            }
        }
    }
}
