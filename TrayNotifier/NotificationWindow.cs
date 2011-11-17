using System;
using TrayNotifier.Business;

namespace TrayNotifier
{
    using System.Windows.Forms;

    public partial class NotificationWindow : Form
    {
        private readonly AbstractNotificationSystem[] _notificationSystems;

        public NotificationWindow(AbstractNotificationSystem[] notificationSystems)
        {
            _notificationSystems = notificationSystems;
            InitializeComponent();

            GoGoMagicStealthMode();
            Load += Window_Loaded;
        }

        private void Window_Loaded(object sender, EventArgs e)
        {
            if (_notificationSystems.Length == 0)
            {
                DisplayMessage(30, "No Notifications Configured", "Are there any in the BIN directory?", AbstractNotificationSystem.Icon.Error);
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
                    timer.Tick += delegate { notificationService.Check(); };
                    notificationService.MessageReceived += DisplayMessage;
                    notificationService.Check(); // go start now!
                }
                catch (Exception ex)
                {
                    DisplayMessage(30, "Errors Launching Notifier", ex.Message, AbstractNotificationSystem.Icon.Error);
                }
            }
        }

        private void DisplayMessage(int secondsToNotifyFor, string title, string message, AbstractNotificationSystem.Icon icon)
        {
            notificationIcon.ShowBalloonTip(secondsToNotifyFor, title, message, GetToolTipIcon(icon));
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