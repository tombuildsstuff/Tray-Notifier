namespace TrayNotifier.Business
{
    public abstract class AbstractNotificationRegistration
    {
        public enum Icon
        {
            Error,
            Warning,
            Info
        }

        public delegate void NotificationMessageReceived(string title, string message, Icon icon);

        public NotificationMessageReceived MessageReceived;

        public abstract int NumberOfSecondsToCheck();

        public abstract void Check();
    }
}