namespace TrayNotifier.CruiseControlBuildNotifier
{
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Business;
    using Business.Helpers;
    using Models;
    using OpenFileSystem.IO;

    public class CruiseControlNotificationRegistration : INotificationRegistration
    {
        private const string ConfigurationFileName = "cruisecontrol.config";
        private const string DefaultFileContents = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n<projects>\n <!-- <project url=\"http://mycorp.com/ccnet\" interval=\"60\" username=\"foo\" password=\"foobar\" /> -->\n</projects>";
        private readonly AbstractConfigurationDetails _configuration;
        private readonly IFileSystem _fileSystem;

        public CruiseControlNotificationRegistration(AbstractConfigurationDetails configuration, IFileSystem fileSystem)
        {
            _configuration = configuration;
            _fileSystem = fileSystem;
        }

        public IEnumerable<AbstractNotificationSystem> RegisterComponents()
        {
            var filePath = System.IO.Path.Combine(_configuration.UserDataDirectory, ConfigurationFileName);
            var file = _fileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                CreateFile(_configuration, _fileSystem);
                throw new ConfigurationErrorsException(string.Format("Go Configure {0}!", ConfigurationFileName));
            }

            string fileContents;
            using (var fileData = file.OpenRead())
                fileContents = new StreamReader(fileData).ReadToEnd();

            var configuration = ParseConfiguration(fileContents);
            if (configuration != null)
                return configuration.Select(c => new CruiseControlBuildNotificationSystem(c));

            throw new ConfigurationErrorsException(string.Format("Go Configure {0}!", ConfigurationFileName));
        }

        private static void CreateFile(AbstractConfigurationDetails configuration, IFileSystem fileSystem)
        {
            var filePath = System.IO.Path.Combine(configuration.UserDataDirectory, ConfigurationFileName);
            using (var file = fileSystem.GetFile(filePath).MustExist().OpenWrite())
                file.Write(DefaultFileContents.ToBytes());
        }

        private static IEnumerable<CruiseControlConfiguration> ParseConfiguration(string fileContents)
        {
            if (!string.IsNullOrWhiteSpace(fileContents))
            {
                var xml = XDocument.Parse(fileContents);
                var config = xml.Descendants("projects").ToList();
                if (config.Count() > 0)
                {
                    var projects = config.Descendants("project").ToList();
                    if (projects.Count() > 0)
                        return projects.Select(CruiseControlConfiguration.Parse).Where(c => c != null).ToList();
                }
            }

            return null;
        }
    }
}