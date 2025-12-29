namespace OmniGenerator.Lib.Infrastructure
{
    public class Notifier<TNotificationData> where TNotificationData : notnull
    {
        private DateTime? _lastNotification;

        public bool IsEnabled { get; init; }
        public int Resolution { get; init; }
        public bool UseRegulation { get; set; } = false;
        public IProgress<TNotificationData>? Progress { get; set; }

        public Notifier(int resolution, bool isEnabled = true)
        {
            Resolution = resolution;
            IsEnabled = isEnabled;
        }

        public void SendNotification(TNotificationData data, bool force = false)
        {
            if (IsEnabled)
            {
                var now = DateTime.Now;
                if (Progress is not null &&
                    (!UseRegulation || force || (now - (_lastNotification ?? DateTime.MinValue)).TotalMilliseconds >= Resolution))
                {
                    Progress.Report(data);
                    _lastNotification = now;
                }
            }
        }
    }
}
