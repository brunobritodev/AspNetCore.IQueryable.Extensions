using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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
                var property = PrimitiveExtensions.GetProperty<TEntity>(sortTerm.FieldName());

                if (property != null)
                {
                    var command = useThenBy ? "ThenBy" : "OrderBy";
                    command += sortTerm.IsDescending() ? "Descending" : string.Empty;

                    result = result.OrderBy(property, command);
                }

                useThenBy = true;
            }

            return result;
        }

        private static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, PropertyInfo propertyInfo, string command)
        {
            var type = typeof(TEntity);
            var parameter = Expression.Parameter(type, "p");

            dynamic propertyValue = parameter;
            if (propertyInfo.Name.Contains("."))
            {
                var parts = propertyInfo.Name.Split('.');
                for (var i = 0; i < parts.Length - 1; i++)
                {
                    propertyValue = Expression.PropertyOrField(propertyValue, parts[i]);
                }
            }

            var propertyAccess = Expression.MakeMemberAccess(propertyValue, propertyInfo);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, propertyInfo.PropertyType },
                source.Expression, Expression.Quote(orderByExpression));
            return source.Provider.CreateQuery<TEntity>(resultExpression);
        }

        public static IQueryable<TEntity> Sort<TEntity, TModel>(this IQueryable<TEntity> result, TModel fields) where TModel : IQuerySort
        {
            return Sort(result, fields.Sort);
        }
    }
}
