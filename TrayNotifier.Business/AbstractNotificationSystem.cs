namespace TrayNotifier.Business
{
    public abstract class AbstractNotificationSystem
    {
        public enum Icon
        {
            Error,
            Warning,
            Info
        }

        public delegate void NotificationMessageReceived(int secondsToNotifyFor, string title, string message, Icon icon);

        public NotificationMessageReceived MessageReceived;

        public abstract int NumberOfSecondsToCheck();

        public abstract void Check();
    }
}