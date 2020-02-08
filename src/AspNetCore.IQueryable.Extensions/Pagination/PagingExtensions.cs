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
        where TModel : IQueryPaging
        {
            var attr = Attribute.GetCustomAttributes(PrimitiveExtensions.GetProperty<TModel>("Limit")).FirstOrDefault();
            // Check for the AnimalType attribute.
            if (attr?.GetType() == typeof(QueryOperatorAttribute))
            {
                var data = (QueryOperatorAttribute)attr;
                if (data.Max > 0)
                    options.Limit = data.Max;
            }

            return result.Skip(options.Offset).Take(options.Limit);
        }
    }
}
