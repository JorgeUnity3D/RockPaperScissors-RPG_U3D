using System;
namespace Kapibara.RPS
{
    public static class RPSTimestamp
    {
        public static string GetTimestamp()
        {
            long timestamp = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return timestamp.ToString();
        }
        
        public static DateTime ConvertTimestampToDateTime(string unixTimestamp)
        {
            long timestamp = long.Parse(unixTimestamp);
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(timestamp);
            return dateTime;
        }
    }
}