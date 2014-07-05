using System;
using TrayNotifier.Business;

namespace TrayNotifier
{
    using System.Windows.Forms;

    public partial class NotificationWindow : Form
    {
        private readonly AbstractNotificationSystem[] _notificationSystems;
        private readonly INotificationManager _notificationManager;

        public NotificationWindow(AbstractNotificationSystem[] notificationSystems, INotificationManager notificationManager)
        {
            _notificationSystems = notificationSystems;
            _notificationManager = notificationManager;
            InitializeComponent();

            _notificationManager.NotificationAvailable += DisplayNotification;
            GoGoMagicStealthMode();
            Load += Window_Loaded;
        }

        private void DisplayNotification(QueuedNotificationDetails notification)
        {
            notificationIcon.ShowBalloonTip(notification.NumSecondsToDisplayFor, notification.Title, notification.Message, GetToolTipIcon(notification.Icon));
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (_notificationSystems.Length == 0)
            {
                DisplayNotification(new QueuedNotificationDetails
                                        {
                                            Icon = AbstractNotificationSystem.Icon.Error,
                                            Title = "No Notifications Configured",
                                            Message = "Are there any in the Plugin directory?",
                                            NumSecondsToDisplayFor = 30,
                                            DisplayAt = DateTime.Now
                                        });
                return;
            }

            foreach (var notificationService in _notificationSystems)
            {
                try
                {
                    var timer = new Timer
                                    {
                                        Enabled = true,
                                        Interval = notificationService.NumberOfSecondsToCheck() * 1000
                                    };
                    AbstractNotificationSystem service = notificationService;
                    timer.Tick += delegate { service.Check(); };
                    notificationService.MessageReceived += MessageReceived;
                    notificationService.Check(); // go start now!
                }
                catch (Exception ex)
                {
                    DisplayNotification(new QueuedNotificationDetails
                                        {
                                            Icon = AbstractNotificationSystem.Icon.Error,
                                            Title = "Error Launching Notifier",
                                            Message = ex.Message,
                                            NumSecondsToDisplayFor = 30,
                                            DisplayAt = DateTime.Now
                                        });
                }
            }
        }

        private void MessageReceived(int secondstonotifyfor, string title, string message, AbstractNotificationSystem.Icon icon)
        {
            _notificationManager.AddNotification(icon, title, message, secondstonotifyfor);
        }

        /// <summary>
        /// Hides the window..
        /// </summary>
        private void GoGoMagicStealthMode()
        {
            Hide();
            WindowState = FormWindowState.Minimized;
        }

        private static ToolTipIcon GetToolTipIcon(AbstractNotificationSystem.Icon icon)
        {
            switch (icon)
            {
                case AbstractNotificationSystem.Icon.Warning:
                    return ToolTipIcon.Warning;

                case AbstractNotificationSystem.Icon.Info:
                    return ToolTipIcon.Info;

                default:
                    return ToolTipIcon.Error;
            }
        }

    }
}