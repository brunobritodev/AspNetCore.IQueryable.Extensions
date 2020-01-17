using System.Linq;
using AspNetCore.IQueryable.Extensions.Filter;
using AspNetCore.IQueryable.Extensions.Pagination;
using AspNetCore.IQueryable.Extensions.Sort;

namespace AspNetCore.IQueryable.Extensions
{
    public static class RestFulExtensions
    {


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
