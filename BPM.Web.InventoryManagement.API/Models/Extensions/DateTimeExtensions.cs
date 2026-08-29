namespace BPM.Web.InventoryManagement.API.Models.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Ensures DateTime is in UTC format for PostgreSQL
        /// </summary>
        public static DateTime EnsureUtc(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            if (dateTime.Kind == DateTimeKind.Local)
            {
                return dateTime.ToUniversalTime();
            }
            return dateTime; // Already UTC
        }

        /// <summary>
        /// Ensures nullable DateTime is in UTC format for PostgreSQL
        /// </summary>
        public static DateTime? EnsureUtc(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.EnsureUtc();
            }
            return null;
        }

        /// <summary>
        /// Converts to UTC and ensures it's not null
        /// </summary>
        public static DateTime EnsureUtcWithDefault(this DateTime? dateTime, DateTime defaultValue)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.EnsureUtc();
            }
            return defaultValue.EnsureUtc();
        }

        /// <summary>
        /// Safely convert to UTC for database operations
        /// </summary>
        public static DateTime ToDatabaseUtc(this DateTime dateTime)
        {
            // If unspecified, treat as UTC
            if (dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            return dateTime.ToUniversalTime();
        }

        /// <summary>
        /// Safely convert nullable to UTC for database operations
        /// </summary>
        public static DateTime? ToDatabaseUtc(this DateTime? dateTime)
        {
            return dateTime?.ToDatabaseUtc();
        }
    }
}
