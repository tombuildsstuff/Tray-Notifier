using System.Collections.Generic;

namespace TrayNotifier.Business
{
    public interface INotificationRegistration
    {
        IEnumerable<AbstractNotificationSystem> RegisterComponents();
    }
}