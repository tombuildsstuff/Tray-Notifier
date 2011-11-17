using System.Collections.Generic;
using OpenFileSystem.IO;
using TrayNotifier.Business;

namespace TrayNotifier.CruiseControlBuildNotifier
{
    public class CruiseControlNotificationRegistration : INotificationRegistration
    {
        private readonly INotificationRegistration _notificationRegistration;
        private readonly IFileSystem _fileSystem;
        public CruiseControlNotificationRegistration(INotificationRegistration notificationRegistration, IFileSystem fileSystem)
        {
            _notificationRegistration = notificationRegistration;
            _fileSystem = fileSystem;
        }

        public IEnumerable<AbstractNotificationSystem> RegisterComponents()
        {
            throw new System.NotImplementedException();
        }
    }
}