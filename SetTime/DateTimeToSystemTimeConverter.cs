using System;

namespace SetTime
{
    class DateTimeToSystemTimeConverter
    {
        public SystemTime Convert(DateTime dateTime)
        {
            var utcDateTime = dateTime.ToUniversalTime();

            return new SystemTime
            {
                Year = (ushort)utcDateTime.Year,
                Month = (ushort)utcDateTime.Month,
                Day = (ushort)utcDateTime.Day,
                Hour = (ushort)utcDateTime.Hour,
                Minute = (ushort)utcDateTime.Minute,
                Second = (ushort)utcDateTime.Second
            };
        }
    }
}
