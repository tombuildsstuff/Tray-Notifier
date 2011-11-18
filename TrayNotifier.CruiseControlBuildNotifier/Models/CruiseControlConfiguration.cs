using System.Xml.Linq;

namespace TrayNotifier.CruiseControlBuildNotifier.Models
{
    public class CruiseControlConfiguration
    {
        public string InstallDirectoryUrl { get; set; }

        public int? CheckInterval { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Domain { get; set; }

        public static CruiseControlConfiguration Parse(XElement element)
        {
            if (element == null)
                return null;

            var configuration = new CruiseControlConfiguration();

            var url = element.Attribute("url");
            if (url != null)
                configuration.InstallDirectoryUrl = url.Value;

            var interval = element.Attribute("interval");
            if (interval != null)
                configuration.CheckInterval = int.Parse(interval.Value);

            var username = element.Attribute("username");
            if (username != null)
                configuration.Username = username.Value;

            var password = element.Attribute("password");
            if (password != null)
                configuration.Password = password.Value;

            var domain = element.Attribute("domain");
            if (domain != null)
                configuration.Domain = domain.Value;

            return configuration;
        }
    }
}