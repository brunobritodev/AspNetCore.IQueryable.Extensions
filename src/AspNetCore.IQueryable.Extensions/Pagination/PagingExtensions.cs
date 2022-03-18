using AspNetCore.IQueryable.Extensions.Attributes;
using System;
using System.Linq;

namespace AspNetCore.IQueryable.Extensions.Pagination
{
    public static class PagingExtensions
    {
        public static IQueryable<TEntity> Paginate<TEntity>(this
            IQueryable<TEntity> result,
            int limit = 10,
            int offset = 0)
        {
            if (limit <= 0)
                limit = 10;
            return result.Skip(offset).Take(limit);
        }
        public static IQueryable<TEntity> Paginate<TEntity, TModel>(this
                    IQueryable<TEntity> result,
                    TModel options)
        where TModel : class, IQueryPaging
        {
            var attr = Attribute.GetCustomAttribute(PrimitiveExtensions.GetProperty(options.GetType(), "Limit"), typeof(QueryOperatorAttribute));

            if (attr?.GetType() == typeof(QueryOperatorAttribute))
            {
                var data = (QueryOperatorAttribute)attr;
                if (options.Limit is null || options.Limit < 0 || options.Limit > data.Max && data.Max >= 0)
                    options.Limit = data.Max;
            }

            if (options.Offset.HasValue)
                result = result.Skip(options.Offset.Value);

            if (options.Limit.HasValue)
                result = result.Take(options.Limit.Value);

            return result;
        }
    }
}
