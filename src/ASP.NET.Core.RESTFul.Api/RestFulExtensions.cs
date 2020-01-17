using System;
using System.Linq;
using System.Reflection;
using AspNetCore.RESTFul.Extensions.Filter;
using AspNetCore.RESTFul.Extensions.Pagination;
using AspNetCore.RESTFul.Extensions.Sort;

namespace AspNetCore.RESTFul.Extensions
{
    public static class RestFulExtensions
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

        public static IQueryable<TEntity> Apply<TEntity, TModel>(this IQueryable<TEntity> result, TModel model) where TModel : new()
        {
            result = result.Filter(model);

            if (model is IRestSort sort)
                result = result.Sort(sort);

            if (model is IRestPagination pagination)
                result = result.Paginate(pagination);

            return result;
        }

    }
}
