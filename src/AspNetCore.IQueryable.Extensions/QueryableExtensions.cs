using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;
using System.Linq;

namespace AspNetCore.IQueryable.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Apply<TEntity>(this IQueryable<TEntity> result, ICustomQueryable model)
        {
            result = result.Filter(model);

            if (model is IQuerySort sort)
                result = result.Sort(sort);

            if (model is IQueryPaging pagination)
                result = result.Paginate(pagination);

            return result;
        }

    }
}
