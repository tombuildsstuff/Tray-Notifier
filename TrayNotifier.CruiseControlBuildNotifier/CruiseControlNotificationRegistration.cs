using System.Collections.Generic;
using OpenFileSystem.IO;
using TrayNotifier.Business;

namespace TrayNotifier.CruiseControlBuildNotifier
{
    public class CruiseControlNotificationRegistration : INotificationRegistration
    {
        private readonly IFileSystem _fileSystem;
        public CruiseControlNotificationRegistration(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IEnumerable<AbstractNotificationSystem> RegisterComponents()
        {
            throw new System.NotImplementedException();
        }
    }
}