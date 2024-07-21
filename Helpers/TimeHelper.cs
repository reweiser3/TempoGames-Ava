namespace Ava.Helpers
{
    public static class TimeHelper
    {
        public static string GetLastPlayedText(DateTime? lastPlayed)
        {
            if (lastPlayed == null) return "Never";

            var timeSpan = DateTime.Now - lastPlayed.Value;

            if (timeSpan.TotalMinutes < 60)
            {
                return $"{timeSpan.TotalMinutes:F0} minutes ago";
            }
            if (timeSpan.TotalDays < 1)
            {
                return $"{timeSpan.TotalHours:F0} hours ago";
            }
            if (timeSpan.TotalDays < 30)
            {
                return $"{timeSpan.TotalDays:F0} days ago";
            }
            if (timeSpan.TotalDays < 365)
            {
                return $"{timeSpan.TotalDays / 30:F0} months ago";
            }
            return $"{timeSpan.TotalDays / 365:F1} years ago";
        }
    }
}