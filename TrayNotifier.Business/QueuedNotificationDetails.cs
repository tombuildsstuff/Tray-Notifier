using System;

namespace TrayNotifier.Business
{
    public class QueuedNotificationDetails
    {
        public string Title { get; set; }

        public string Message { get; set; }

        public DateTime DisplayAt { get; set; }

        public int NumSecondsToDisplayFor { get; set; }

        public AbstractNotificationSystem.Icon Icon { get; set; }

        public delegate void SendNotification(QueuedNotificationDetails notification);
    }
}