using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetCore.IQueryable.Extensions
{
    internal static class PrimitiveExtensions
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> MemoryObjects;
        static PrimitiveExtensions()
        {
            MemoryObjects = new ConcurrentDictionary<Type, List<PropertyInfo>>();
        }
        public static bool IsPropertyACollection(this PropertyInfo property)
        {
            return IsGenericEnumerable(property.PropertyType) || property.PropertyType.IsArray;
        }
        public static bool IsPropertyObject(this PropertyInfo property, object value)
        {
            return Convert.GetTypeCode(property.GetValue(value, null)) == TypeCode.Object;
        }
        private static bool IsGenericEnumerable(Type type)
        {
            return type.IsGenericType &&
                   type.GetInterfaces().Any(
                       ti => (ti == typeof(IEnumerable<>) || ti.Name == "IEnumerable"));
        }


        internal static List<PropertyInfo> GetAllProperties(this Type type)
        {
            if (MemoryObjects.ContainsKey(type))
                return MemoryObjects[type];

            var properties = type.GetProperties().ToList();
            MemoryObjects.TryAdd(type, properties);
            return properties;
        }
        internal static PropertyInfo GetProperty(Type type, string name)
        {
            return type
                .GetAllProperties()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        internal static PropertyInfo GetProperty<TEntity>(string name)
        {
            return typeof(TEntity)
                .GetAllProperties()
                .FirstOrDefault(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }
        public static bool HasProperty(this Type type, string propertyName)
        {
            return type.GetAllProperties().Any(a => a.Name.Equals(propertyName, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Comma separated string: a,b,c
        /// </summary>
        public static string[] Fields(this string fields)
        {
            return fields.Split(',');
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
