using System;
using System.Linq;
using System.Reflection;

namespace AspNetCore.IQueryable.Extensions
{
    internal static class PrimitiveExtensions
    {
        internal static PropertyInfo GetProperty<TEntity>(string name)
        {
            return typeof(TEntity)
                .GetProperties()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static bool HasProperty(this Type type, string propertyName)
        {
            return type.GetProperties().Any(a => a.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Comma separated string: a,b,c
        /// </summary>
        public static string[] Fields(this string fields)
        {
            return fields.Split(",");
        }

        public static string FieldName(this string field)
        {
            return field.StartsWith("-", "+") ? field.Substring(1).Trim() : field.Trim();
        }

        public static bool StartsWith(this string text, params string[] with)
        {
            return with.Any(text.StartsWith);
        }

        public static bool IsDescending(this string field)
        {
            return field.StartsWith("-");
        }
    }
}
