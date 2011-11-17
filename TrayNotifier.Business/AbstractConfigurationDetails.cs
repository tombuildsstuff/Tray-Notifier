namespace TrayNotifier.Business
{
    public abstract class AbstractConfigurationDetails
    {
        public abstract string PluginDirectory { get; }

        public abstract string UserDataDirectory { get; }
    }
}