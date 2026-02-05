using AspNetCore.IQueryable.Extensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AspNetCore.IQueryable.Extensions.Sort
{
    public static class SortingExtensions
    {
        public static IQueryable<TEntity> Sort<TEntity>(this IQueryable<TEntity> result, string fields)
        {
            if (string.IsNullOrEmpty(fields))
            {
                return result;
            }

            var useThenBy = false;
            foreach (var sortTerm in fields.Fields())
            {
                var fieldName = sortTerm.FieldName();

                var parameter = Expression.Parameter(typeof(TEntity), "p");
                if (TryBuildPropertyAccess(parameter, typeof(TEntity), fieldName, out var propertyAccess))
                {
                    var command = useThenBy ? "ThenBy" : "OrderBy";
                    command += sortTerm.IsDescending() ? "Descending" : string.Empty;

                    result = result.OrderBy(parameter, propertyAccess, command);
                }

                useThenBy = true;
            }

            return result;
        }

        private static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, ParameterExpression parameter, Expression propertyAccess, string command)
        {
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { typeof(TEntity), propertyAccess.Type },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> Sort<TEntity, TModel>(this IQueryable<TEntity> result, TModel fields) where TModel : IQuerySort
        {
            return Sort(result, MapSortFields<TModel>(fields.Sort));
        }

        private static string MapSortFields<TModel>(string fields) where TModel : ICustomQueryable
        {
            if (string.IsNullOrWhiteSpace(fields))
                return fields;

            var aliases = GetAliases<TModel>();
            if (aliases.Count == 0)
                return fields;

            var mapped = fields.Fields().Select(term =>
            {
                var prefix = term.StartsWith("-", "+") ? term.Substring(0, 1) : string.Empty;
                var name = term.FieldName();
                if (aliases.TryGetValue(name, out var mappedName))
                {
                    name = mappedName;
                }
                return $"{prefix}{name}";
            });

            return string.Join(",", mapped);
        }

        private static Dictionary<string, string> GetAliases<TModel>() where TModel : ICustomQueryable
        {
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var property in typeof(TModel).GetProperties())
            {
                var attr = Attribute.GetCustomAttributes(property).FirstOrDefault(a => a is QueryOperatorAttribute) as QueryOperatorAttribute;
                if (attr == null)
                    continue;

                if (string.IsNullOrWhiteSpace(attr.HasName))
                    continue;

                dict[property.Name] = attr.HasName;
            }

            return dict;
        }

        private static bool TryBuildPropertyAccess(ParameterExpression parameter, Type entityType, string path, out Expression propertyAccess)
        {
            propertyAccess = parameter;
            var currentType = entityType;

            foreach (var part in path.Split('.'))
            {
                var property = PrimitiveExtensions.GetProperty(currentType, part);
                if (property == null)
                {
                    propertyAccess = null;
                    return false;
                }

                propertyAccess = Expression.PropertyOrField(propertyAccess, property.Name);
                currentType = property.PropertyType;
            }

            return true;
        }
    }
}
