using System;
using System.Globalization;

namespace net.roomful.api
{
    /// <summary>
    /// Time Utility methods.
    /// </summary>
    public static class RoomfulTime
    {
        private const string RFC3339_FORMAT = "yyyy-MM-dd'T'HH:mm:ssK";
        private const string MIN_RFC339_VALUE = "0001-01-01T00:00:00Z";

        /// <summary>
        /// Converts DateTime to Rfc3339 formatted string
        /// </summary>
        public static string DateTimeToRfc3339(DateTime dateTime) {
            return dateTime == DateTime.MinValue 
                ? MIN_RFC339_VALUE 
                : dateTime.ToString(RFC3339_FORMAT, DateTimeFormatInfo.InvariantInfo);
        }
    }
}