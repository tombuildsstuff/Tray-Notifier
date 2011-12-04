using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TrayNotifier.Business;

namespace TrayNotifier.Domain
{
    public class NotificationManager : INotificationManager
    {
        public event QueuedNotificationDetails.SendNotification NotificationAvailable;
        private readonly Timer _timer;
        private readonly List<QueuedNotificationDetails> _queuedNotifications;

        public NotificationManager()
        {
            _queuedNotifications = new List<QueuedNotificationDetails>();
            _timer = new Timer { Interval = 1000 };
            _timer.Elapsed += TimerTicked;
            _timer.Start();
        }

        private void TimerTicked(object sender, ElapsedEventArgs e)
        {
            // any notifications to display? add them
            var numSecsNow = (int) DateTime.Now.TimeOfDay.TotalSeconds;
            var notificationToDisplay = _queuedNotifications.Where(n => numSecsNow == (int)n.DisplayAt.TimeOfDay.TotalSeconds).FirstOrDefault();
            if (notificationToDisplay != null)
                if (NotificationAvailable != null)
                {
                    NotificationAvailable(notificationToDisplay);
                    _queuedNotifications.Remove(notificationToDisplay);
                }
        }

        public void AddNotification(AbstractNotificationSystem.Icon icon, string title, string message, int displayFor = 10)
        {
            // determine what time it should be displayed at
            var lastNotification = _queuedNotifications.Where(n => n.DisplayAt >= DateTime.Now).OrderByDescending(n => n.DisplayAt).FirstOrDefault();
            var displayAt = lastNotification == null
                                ? DateTime.Now.AddSeconds(2)
                                : lastNotification.DisplayAt.AddSeconds(lastNotification.NumSecondsToDisplayFor);
            _queuedNotifications.Add(new QueuedNotificationDetails
                                         {
                                             Title = title,
                                             Message = message,
                                             DisplayAt = displayAt,
                                             NumSecondsToDisplayFor = displayFor,
                                             Icon = icon
                                         });
        }
    }
}