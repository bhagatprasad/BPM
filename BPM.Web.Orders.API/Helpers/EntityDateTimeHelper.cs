namespace BPM.Web.Orders.API.Helpers
{
    public static class EntityDateTimeHelper
    {
        /// <summary>
        /// Ensures all DateTime properties in an entity are in UTC
        /// </summary>
        public static void EnsureAllDateTimesUtc(this object entity)
        {
            if (entity == null) return;

            var properties = entity.GetType().GetProperties()
                .Where(p => p.PropertyType == typeof(DateTime) || p.PropertyType == typeof(DateTime?));

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    var value = (DateTime)property.GetValue(entity);
                    if (value.Kind != DateTimeKind.Utc)
                    {
                        property.SetValue(entity, DateTime.SpecifyKind(value, DateTimeKind.Utc));
                    }
                }
                else if (property.PropertyType == typeof(DateTime?))
                {
                    var value = (DateTime?)property.GetValue(entity);
                    if (value.HasValue && value.Value.Kind != DateTimeKind.Utc)
                    {
                        property.SetValue(entity, DateTime.SpecifyKind(value.Value, DateTimeKind.Utc));
                    }
                }
            }
        }

        /// <summary>
        /// Ensures all DateTime properties in a collection of entities are in UTC
        /// </summary>
        public static void EnsureAllDateTimesUtc<T>(this IEnumerable<T> entities) where T : class
        {
            if (entities == null) return;

            foreach (var entity in entities)
            {
                entity.EnsureAllDateTimesUtc();
            }
        }
    }
}
