using TrayNotifier.Business;

namespace TrayNotifier.SampleNotification
{
    public class SampleNotificationSystem : AbstractNotificationSystem
    {
        private int _numTimesAlerted;

        public override int NumberOfSecondsToCheck()
        {
            return 20;
        }

        public override void Check()
        {
            _numTimesAlerted++;
            if (_numTimesAlerted % 2 == 1)
                if (MessageReceived != null)
                    MessageReceived(5, "Sample Notification", string.Format("This is the {0} message..", _numTimesAlerted), Icon.Info);
        }
    }
}