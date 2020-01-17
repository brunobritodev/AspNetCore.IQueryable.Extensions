using System.Linq;

namespace AspNetCore.RESTFul.Extensions.Pagination
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
        where TModel : IRestPagination
        {
            return result.Skip(options.Offset).Take(options.Limit);
        }
    }
}
