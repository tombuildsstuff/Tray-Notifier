using System.Configuration;
using TrayNotifier.Business;

namespace TrayNotifier.Domain
{
    public class ConfigurationBasedNotificationConfigurationDetails : AbstractConfigurationDetails
    {
        public override string PluginDirectory
        {
            get { return ConfigurationManager.AppSettings["PluginDirectory"]; }
        }

        public override string UserDataDirectory
        {
            get { return ConfigurationManager.AppSettings["UserDataDirectory"]; }
        }
    }
}