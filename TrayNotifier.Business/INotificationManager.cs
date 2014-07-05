namespace TrayNotifier.Business
{
    public interface INotificationManager
    {
        event QueuedNotificationDetails.SendNotification NotificationAvailable;
        void AddNotification(AbstractNotificationSystem.Icon icon, string title, string message, int displayFor = 10);
    }
}