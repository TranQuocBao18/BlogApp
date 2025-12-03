using System;
using System.Globalization;
using System.Runtime.InteropServices;
using TimeZoneConverter;

namespace Blog.Utilities.Extensions;

public static class DateTimeExtensions
{
    public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public static long ToUnixTime(this DateTime date)
        {
            if (date.ToUniversalTime() <= UnixEpoch)
            {
                return 0;
            }

            return Convert.ToInt64((date.ToUniversalTime() - UnixEpoch).TotalMilliseconds);
        }

        public static DateTime FromUnixTime(this long unixTime) => UnixEpoch.AddMilliseconds(unixTime);

        public static string AsTimestampString(this DateTime value) => value.ToString("yyyyMMddHHmmssffff");

        public static string AsTimestampShortString(this DateTime value) => value.ToString("yyyyMMddHHmmss");

        public static DateTime StartOfTheDay(this DateTime value) => new DateTime(value.Year, value.Month, value.Day, 0, 0, 0, 1, DateTimeKind.Utc);
        public static long StartOfTheDay(this long value) => value.FromUnixTime().StartOfTheDay().ToUnixTime();
        public static DateTime EndOfTheDay(this DateTime value) => value.Date.AddDays(1).AddTicks(-1);
        public static long EndOfTheDay(this long value) => value.FromUnixTime().AddDays(1).AddTicks(-1).ToUnixTime();

        public static DateTimeOffset ToDateTimeOffset(this DateTime currentUtc, string timeZoneId)
        {
            TimeZoneInfo cet = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            DateTimeOffset offset = TimeZoneInfo.ConvertTime(currentUtc, cet);
            return offset;
        }

        public static DateTime ToDateTime(this string text)
        {
            return text.ConvertTo<DateTime>();
        }

        public static bool IsFutureDate(this object value)
        {
            return value != null && DateTime.Compare(value.ToString().ConvertTo<long>().FromUnixTime(), DateTime.UtcNow) > 0;
        }

        public static DateTime? ToUtcDateTime(this long? value)
        {
            return value.HasValue ? value.ToString().ConvertTo<long>().FromUnixTime() : (DateTime?)null;
        }

        public static string ToUtcShortDateString(this long? value)
        {
            return value?.FromUnixTime().Date.ToShortDateString();
        }

        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static string ToLinuxTimezoneId(this string windowsTimezoneId)
        {
            return TZConvert.WindowsToIana(windowsTimezoneId);
        }

        public static string ToWindowsTimezoneId(this string linuxTimezoneId)
        {
            return TZConvert.IanaToWindows(linuxTimezoneId);
        }

        public static long SubtractMinutes(this long timeStamp, int minutes)
        {
            var currentTime = timeStamp.FromUnixTime();
            return currentTime.AddMinutes(-minutes).ToUnixTime();
        }

        public static long AddMinutes(this long timeStamp, int minutes)
        {
            var currentTime = timeStamp.FromUnixTime();
            return currentTime.AddMinutes(minutes).ToUnixTime();
        }

        public static string ToExportDateTime(this DateTime value, string culture)
        {
            var dateTimeFormatInfo = new CultureInfo(culture, false).DateTimeFormat;
            return value.ToString($"{dateTimeFormatInfo.ShortDatePattern} HH:mm");
        }

        public static string ToExportDateTime(this DateTime value)
        {
            return value.ToExportDateTime(System.Threading.Thread.CurrentThread.CurrentUICulture.Name);
        }
}
