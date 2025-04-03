using System.Globalization;

namespace Com.Weather.Task1.Domain.Helpers
{
    public static class TableHelper
    {
        private const string ParitionKeyFormat = "yyyy-MM";
        private const string RowKeyFormat = "yyyy-MM-dd-HH-MM";

        public static string GetParitionKey(DateTimeOffset dateTime)
        {
            return dateTime.ToString(ParitionKeyFormat);
        }

        public static string GetParitionKey(string rowKey)
        {
            var date = DateTimeOffset.ParseExact(rowKey, RowKeyFormat, CultureInfo.InvariantCulture);
            return GetParitionKey(date);
        }

        public static string GetRowKey(DateTimeOffset dateTime)
        {
            return dateTime.ToString(RowKeyFormat);
        }

        public static string GetBlobNameByRowKey(string rowKey)
        {
            return $"{rowKey}.json";
        }
    }
}
