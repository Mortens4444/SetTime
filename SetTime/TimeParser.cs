using System;

namespace SetTime
{
    class TimeParser
    {
        public SystemTime Get(string siteContent)
        {
            const string timeStartSearchPattern = "<div class=\"time\" id=\"clock\">";
            const string dateStartSearchPattern = "<div class=\"date\">";
            const string endSearchPattern = "</div>";

            var timeStartIndex = siteContent.IndexOf(timeStartSearchPattern) + timeStartSearchPattern.Length;
            var timeEndIndex = siteContent.IndexOf(endSearchPattern, timeStartIndex);
            var dateStartIndex = siteContent.IndexOf(dateStartSearchPattern) + dateStartSearchPattern.Length;
            var dateEndIndex = siteContent.IndexOf(endSearchPattern, dateStartIndex);

            var time = siteContent.Substring(timeStartIndex, timeEndIndex - timeStartIndex);
            var date = siteContent.Substring(dateStartIndex, dateEndIndex - dateStartIndex);

            var timeParts = time.Split(':');
            var dateParts = date.Split('.');

            var hour = Convert.ToUInt16(timeParts[0]);
            var minute = Convert.ToUInt16(timeParts[1]);
            var second = Convert.ToUInt16(timeParts[2]);

            var year = Convert.ToUInt16(dateParts[0]);
            var month = Convert.ToUInt16(dateParts[1]);
            var day = Convert.ToUInt16(dateParts[2]);

            var dateTimeToSystemTimeConverter = new DateTimeToSystemTimeConverter();
            var dateTime = new DateTime(year, month, day, hour, minute, second);
            return dateTimeToSystemTimeConverter.Convert(dateTime);
        }
    }
}
